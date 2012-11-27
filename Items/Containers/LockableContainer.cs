using System;
using Server.Network;
using Server.Items;
using Server.Engines.Craft;
using Server.Mobiles;
using Khaeros.Scripts.Timers;

namespace Server.Items
{
	public abstract class LockableContainer : TrapableContainer, ILockable, ILockpickable, ICraftable//, ITelekinesisable
	{
		private bool m_Locked;
		private int m_LockLevel, m_MaxLockLevel, m_RequiredSkill;
		private uint m_KeyValue;
		private Mobile m_Picker;
		private bool m_TrapOnLockpick;
        private ContainerRelockTimer relockTimer;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Picker
		{
			get
			{
				return m_Picker;
			}
			set
			{
				m_Picker = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxLockLevel
		{
			get
			{
				return m_MaxLockLevel;
			}
			set
			{
				m_MaxLockLevel = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int LockLevel
		{
			get
			{
				return m_LockLevel;
			}
			set
			{
				m_LockLevel = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int RequiredSkill
		{
			get
			{
				return m_RequiredSkill;
			}
			set
			{
				m_RequiredSkill = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public virtual bool Locked
		{
			get
			{
				return m_Locked;
			}
			set
			{
				m_Locked = value;

				if ( m_Locked )
					m_Picker = null;
				
				if( m_Locked )
				{
					Map map = this.Map;
					this.Map = Map.Internal;
					this.Map = map;
				}

				InvalidateProperties();
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public uint KeyValue
		{
			get
			{
				return m_KeyValue;
			}
			set
			{
				m_KeyValue = value;
			}
		}

		public override bool TrapOnOpen
		{
			get
			{
				return !m_TrapOnLockpick;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool TrapOnLockpick
		{
			get
			{
				return m_TrapOnLockpick;
			}
			set
			{
				m_TrapOnLockpick = value;
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 6 ); // version

            writer.Write((Mobile)m_Picker);
			writer.Write( (bool) m_TrapOnLockpick );

			writer.Write( (int) m_RequiredSkill );

			writer.Write( (int) m_MaxLockLevel );

			writer.Write( m_KeyValue );
			writer.Write( (int) m_LockLevel );
			writer.Write( (bool) m_Locked );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
                case 6:
                {
                    m_Picker = reader.ReadMobile(); 
                    
                    goto case 5;
                }
				case 5:
				{
					m_TrapOnLockpick = reader.ReadBool();

					goto case 4;
				}
				case 4:
				{
					m_RequiredSkill = reader.ReadInt();

					goto case 3;
				}
				case 3:
				{
					m_MaxLockLevel = reader.ReadInt();

					goto case 2;
				}
				case 2:
				{
					m_KeyValue = reader.ReadUInt();

					goto case 1;
				}
				case 1:
				{
					m_LockLevel = reader.ReadInt();

					goto case 0;
				}
				case 0:
				{
					if ( version < 3 )
						m_MaxLockLevel = 100;

					if ( version < 4 )
					{
						if ( (m_MaxLockLevel - m_LockLevel) == 40 )
						{
							m_RequiredSkill = m_LockLevel + 6;
							m_LockLevel = m_RequiredSkill - 10;
							m_MaxLockLevel = m_RequiredSkill + 39;
						}
						else
						{
							m_RequiredSkill = m_LockLevel;
						}
					}

					m_Locked = reader.ReadBool();

					break;
				}
			}
			
			if( this.LockLevel < 10 )
				this.LockLevel = 10;
			
			if( this.RequiredSkill < 10 )
				this.RequiredSkill = 10;
		}

		public LockableContainer( int itemID ) : base( itemID )
		{
			m_MaxLockLevel = 100;
		}

		public LockableContainer( Serial serial ) : base( serial )
		{
		}

		public override bool CheckContentDisplay( Mobile from )
		{
			return (!m_Locked ||( from is PlayerMobile && ((PlayerMobile)from).BreakLock )) && base.CheckContentDisplay( from );
		}

		public override bool TryDropItem( Mobile from, Item dropped, bool sendFullMessage )
		{
			if ( from.AccessLevel < AccessLevel.GameMaster && m_Locked )
			{
				from.SendLocalizedMessage( 501747 ); // It appears to be locked.
				return false;
			}

			return base.TryDropItem( from, dropped, sendFullMessage );
		}

		public override bool OnDragDropInto( Mobile from, Item item, Point3D p )
		{
			if ( from.AccessLevel < AccessLevel.GameMaster && m_Locked && !( ( from is PlayerMobile && ((PlayerMobile)from).BreakLock ) ) )
			{
				from.SendLocalizedMessage( 501747 ); // It appears to be locked.
				return false;
			}

			return base.OnDragDropInto( from, item, p );
		}

		public override bool CheckLift( Mobile from, Item item, ref LRReason reject )
		{
			if ( !base.CheckLift( from, item, ref reject ) )
				return false;

			if ( item != this && from.AccessLevel < AccessLevel.GameMaster && m_Locked && !( from is PlayerMobile && ((PlayerMobile)from).BreakLock ) )
				return false;

			return true;
		}

		public override bool CheckItemUse( Mobile from, Item item )
		{
			if ( !base.CheckItemUse( from, item ) )
				return false;

			if ( item != this && from.AccessLevel < AccessLevel.GameMaster && m_Locked && !( from is PlayerMobile && ((PlayerMobile)from).BreakLock) )
			{
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
				return false;
			}

			return true;
		}

		//public override bool DisplaysContent{ get{ return true; }

		public virtual bool CheckLocked( Mobile from )
		{
			bool inaccessible = false;

			if ( m_Locked )
			{
				int number;

				if ( from.AccessLevel >= AccessLevel.GameMaster )
				{
					number = 502502; // That is locked, but you open it with your godly powers.
				}
				else
				{
					number = 501747; // It appears to be locked.
					inaccessible = true;
				}

				from.Send( new MessageLocalized( Serial, ItemID, MessageType.Regular, 0x3B2, 3, number, "", "" ) );
			}

			return inaccessible;
		}

		public override void OnTelekinesis( Mobile from )
		{
			if ( CheckLocked( from ) )
			{
				Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 5022 );
				Effects.PlaySound( Location, Map, 0x1F5 );
				return;
			}

			base.OnTelekinesis( from );
		}
		
		public override void OnSingleClick( Mobile from )
		{
			ObjectPropertyList opl = this.PropertyList;

			if ( opl.Header > 0 )
				from.Send( new MessageLocalized( this.Serial, this.ItemID, MessageType.Label, 0x3B2, 3, opl.Header, this.Name, opl.HeaderArgs ) );
			
			if ( !this.Locked )
				LabelTo( from, "({0} items, {1} stones)", TotalItems, TotalWeight );
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			AddNameProperties( list );
			
			if ( !this.Locked )//CheckContentDisplay( from ) )
				list.Add( 1050044, "{0}\t{1}", TotalItems, TotalWeight );
		}

		public override void OnDoubleClickSecureTrade( Mobile from )
		{
			if ( CheckLocked( from ) )
				return;

			base.OnDoubleClickSecureTrade( from );
		}

		public override void Open( Mobile from )
		{
			if( from.AccessLevel < AccessLevel.GameMaster && this.Locked && from is PlayerMobile && ((PlayerMobile)from).AutoPicking )
			{
				from.PlaySound( 0x241 );
				new LockPickTimer( from, this ).Start();
			}
		
			else
			{
				if ( CheckLocked( from ) )
					return;

				if( from.AccessLevel < AccessLevel.GameMaster )
					from.RevealingAction();
				
				base.Open( from );
			}
		}
		
		public class LockPickTimer : Timer
		{
			private Mobile m_From;
			private ILockpickable m_Item;
		
			public LockPickTimer( Mobile from, ILockpickable item ) : base( TimeSpan.FromSeconds( 3.0 ) )
			{
				m_From = from;
				m_Item = item;
				Priority = TimerPriority.TwoFiftyMS;
				
				if( from.AccessLevel < AccessLevel.GameMaster )
					from.RevealingAction();
			}
			
			protected override void OnTick()
			{
				Item item = (Item)m_Item;
                double LockpickingSkill = m_From.Skills[SkillName.Lockpicking].Value;

                if (m_From is PlayerMobile)
                {
                    PlayerMobile pm = m_From as PlayerMobile;
                    pm.CriminalActivity = true;
                    LockpickingSkill += Utility.Random(pm.Feats.GetFeatLevel(FeatList.Safecracking) * Utility.RandomMinMax(pm.RawInt, pm.RawInt + 100));
                }

				if ( !m_From.InRange( item.GetWorldLocation(), 2 ) )
					return;

				if ( m_Item.LockLevel == 0 || m_Item.LockLevel == -255 )
				{
					// LockLevel of 0 means that the door can't be picklocked
					// LockLevel of -255 means it's magic locked
					item.SendLocalizedMessageTo( m_From, 502073 ); // This lock cannot be picked by normal means
					return;
				}

				if ( LockpickingSkill < m_Item.RequiredSkill )
				{
					/*
					// Do some training to gain skills
					m_From.CheckSkill( SkillName.Lockpicking, 0, m_Item.LockLevel );*/

					// The LockLevel is higher thant the LockPicking of the player
					item.SendLocalizedMessageTo( m_From, 502072 ); // You don't see how that lock can be manipulated.
					return;
				}

				if ( LockpickingSkill >= Utility.RandomMinMax( m_Item.LockLevel, m_Item.MaxLockLevel ) )
				{
					// Success! Pick the lock!
					item.SendLocalizedMessageTo( m_From, 502076 ); // The lock quickly yields to your skill.
					m_From.PlaySound( 0x4A );
					m_Item.LockPick( m_From );
				}
				else
				{
					item.SendLocalizedMessageTo( m_From, 502075 ); // You are unable to pick the lock.
				}
			}
		}

		public override void OnSnoop( Mobile from )
		{
			if ( CheckLocked( from ) )
				return;

			base.OnSnoop( from );
		}

		public virtual void LockPick( Mobile from )
		{
            if (from is PlayerMobile && ((PlayerMobile)from).BreakLock)
                Locked = false;
            else
            {
                Locked = false;
                base.Open(from);
                StartRelockTimer();
            }

			
			Picker = from;

			if ( this.TrapOnLockpick && ExecuteTrap( from ) )
			{
				this.TrapOnLockpick = false;
			}
		}

        private void StartRelockTimer()
        {
            this.relockTimer = new ContainerRelockTimer(this);
            this.relockTimer.Start();
        }

		#region ICraftable Members

		public int OnCraft( int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue )
		{
			if ( from.CheckSkill( SkillName.Tinkering, -5.0, 15.0 ) )
			{
				from.SendLocalizedMessage( 500636 ); // Your tinker skill was sufficient to make the item lockable.

				Key key = new Key( KeyType.Copper, Key.RandomValue() );

				KeyValue = key.KeyValue;
				DropItem( key );

				double tinkering = from.Skills[SkillName.Tinkering].Value;
				int level = (int)(tinkering * 0.8);

				RequiredSkill = level - 4;
				LockLevel = level - 14;
				MaxLockLevel = level + 35;

				if ( LockLevel == 0 )
					LockLevel = -1;
				else if ( LockLevel > 95 )
					LockLevel = 95;

				if ( RequiredSkill > 95 )
					RequiredSkill = 95;

				if ( MaxLockLevel > 95 )
					MaxLockLevel = 95;
			}
			else
			{
				from.SendLocalizedMessage( 500637 ); // Your tinker skill was insufficient to make the item lockable.
			}

			return 1;
		}

		#endregion
	}
}
