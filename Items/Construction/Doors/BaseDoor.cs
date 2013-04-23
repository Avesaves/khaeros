using System;
using System.Collections;
using System.Collections.Generic;
using Server.Commands;
using Server.Network;
using Server.Targeting;
using Server.Mobiles;
using Server.ContextMenus;
using Server.Multis;

namespace Server.Items
{
	public abstract class BaseDoor : Item, ILockable, ITelekinesisable, ILockpickable
	{
		private bool m_Open, m_Locked;
		private int m_OpenedID, m_OpenedSound;
		private int m_ClosedID, m_ClosedSound;
		private Point3D m_Offset;
		private BaseDoor m_Link;
		private uint m_KeyValue;
		
		private int m_LockLevel, m_MaxLockLevel, m_RequiredSkill;
		private Mobile m_Picker;
		private bool m_TrapOnLockpick;
		
		private TrapType m_TrapType;
		private int m_TrapPower;
		private int m_TrapLevel;
		private bool m_ClosesAutomatically = true;
        private bool m_RelockOnClose = false;

		[CommandProperty( AccessLevel.GameMaster )]
		public TrapType TrapType
		{
			get
			{
				return m_TrapType;
			}
			set
			{
				m_TrapType = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int TrapPower
		{
			get
			{
				return m_TrapPower;
			}
			set
			{
				m_TrapPower = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int TrapLevel
		{
			get
			{
				return m_TrapLevel;
			}
			set
			{
				m_TrapLevel = value;
			}
		}

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
		
		public virtual bool TrapOnOpen
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
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool ClosesAutomatically{ get{ return m_ClosesAutomatically;	} set{ m_ClosesAutomatically = value; } }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool RelockOnClose
        {
            get { return m_RelockOnClose; }
            set { m_RelockOnClose = value; }
        }

		private Timer m_Timer;

		private static Point3D[] m_Offsets = new Point3D[]
			{
				new Point3D(-1, 1, 0 ),
				new Point3D( 1, 1, 0 ),
				new Point3D(-1, 0, 0 ),
				new Point3D( 1,-1, 0 ),
				new Point3D( 1, 1, 0 ),
				new Point3D( 1,-1, 0 ),
				new Point3D( 0, 0, 0 ),
				new Point3D( 0,-1, 0 ),

				new Point3D( 0, 0, 0 ),
				new Point3D( 0, 0, 0 ),
				new Point3D( 0, 0, 0 ),
				new Point3D( 0, 0, 0 )
			};

		// Called by RunUO
		public static void Initialize()
		{
			EventSink.OpenDoorMacroUsed += new OpenDoorMacroEventHandler( EventSink_OpenDoorMacroUsed );

			CommandSystem.Register( "Link", AccessLevel.GameMaster, new CommandEventHandler( Link_OnCommand ) );
			CommandSystem.Register( "ChainLink", AccessLevel.GameMaster, new CommandEventHandler( ChainLink_OnCommand ) );
		}
		
		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			list.Add( new MenuEntry( this, from ) );
		}
		
		private class MenuEntry : ContextMenuEntry
		{
			private Item m_Item;
			private Mobile m_From;
			
			public MenuEntry( Item item, Mobile from ) : base( 6132 ) // 3006132  2132
			{
				m_Item = item;
				m_From = from;
			}

			public override void OnClick()
			{
				if( m_From == null || m_Item == null || m_From.Deleted || m_Item.Deleted )
					return;
				
				if( m_From.Alive && m_From.CanSee( m_Item ) && m_From.InLOS( m_Item ) && m_From.InRange( m_Item, 2 ) )
					m_Item.PublicOverheadMessage( MessageType.Regular, 0x3B2, false, "*knock knock*" );
				
				else
					m_From.SendMessage( "That is out of range." );
			}
		}

		[Usage( "Link" )]
		[Description( "Links two targeted doors together." )]
		private static void Link_OnCommand( CommandEventArgs e )
		{
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( Link_OnFirstTarget ) );
			e.Mobile.SendMessage( "Target the first door to link." );
		}

		private static void Link_OnFirstTarget( Mobile from, object targeted )
		{
			BaseDoor door = targeted as BaseDoor;

			if ( door == null )
			{
				from.BeginTarget( -1, false, TargetFlags.None, new TargetCallback( Link_OnFirstTarget ) );
				from.SendMessage( "That is not a door. Try again." );
			}
			else
			{
				from.BeginTarget( -1, false, TargetFlags.None, new TargetStateCallback( Link_OnSecondTarget ), door );
				from.SendMessage( "Target the second door to link." );
			}
		}

		private static void Link_OnSecondTarget( Mobile from, object targeted, object state )
		{
			BaseDoor first = (BaseDoor)state;
			BaseDoor second = targeted as BaseDoor;

			if ( second == null )
			{
				from.BeginTarget( -1, false, TargetFlags.None, new TargetStateCallback( Link_OnSecondTarget ), first );
				from.SendMessage( "That is not a door. Try again." );
			}
			else
			{
				first.Link = second;
				second.Link = first;
				from.SendMessage( "The doors have been linked." );
			}
		}

		[Usage( "ChainLink" )]
		[Description( "Chain-links two or more targeted doors together." )]
		private static void ChainLink_OnCommand( CommandEventArgs e )
		{
			e.Mobile.BeginTarget( -1, false, TargetFlags.None, new TargetStateCallback( ChainLink_OnTarget ), new ArrayList() );
			e.Mobile.SendMessage( "Target the first of a sequence of doors to link." );
		}

		private static void ChainLink_OnTarget( Mobile from, object targeted, object state )
		{
			BaseDoor door = targeted as BaseDoor;

			if ( door == null )
			{
				from.BeginTarget( -1, false, TargetFlags.None, new TargetStateCallback( ChainLink_OnTarget ), state );
				from.SendMessage( "That is not a door. Try again." );
			}
			else
			{
				ArrayList list = (ArrayList)state;

				if ( list.Count > 0 && list[0] == door )
				{
					if ( list.Count >= 2 )
					{
						for ( int i = 0; i < list.Count; ++i )
							((BaseDoor)list[i]).Link = ((BaseDoor)list[(i + 1) % list.Count]);

						from.SendMessage( "The chain of doors have been linked." );
					}
					else
					{
						from.BeginTarget( -1, false, TargetFlags.None, new TargetStateCallback( ChainLink_OnTarget ), state );
						from.SendMessage( "You have not yet targeted two unique doors. Target the second door to link." );
					}
				}
				else if ( list.Contains( door ) )
				{
					from.BeginTarget( -1, false, TargetFlags.None, new TargetStateCallback( ChainLink_OnTarget ), state );
					from.SendMessage( "You have already targeted that door. Target another door, or retarget the first door to complete the chain." );
				}
				else
				{
					list.Add( door );

					from.BeginTarget( -1, false, TargetFlags.None, new TargetStateCallback( ChainLink_OnTarget ), state );

					if ( list.Count == 1 )
						from.SendMessage( "Target the second door to link." );
					else
						from.SendMessage( "Target another door to link. To complete the chain, retarget the first door." );
				}
			}
		}

		private static void EventSink_OpenDoorMacroUsed( OpenDoorMacroEventArgs args )
		{
			Mobile m = args.Mobile;

			if ( m.Map != null )
			{
				int x = m.X, y = m.Y;

				switch ( m.Direction & Direction.Mask )
				{
					case Direction.North:      --y; break;
					case Direction.Right: ++x; --y; break;
					case Direction.East:  ++x;      break;
					case Direction.Down:  ++x; ++y; break;
					case Direction.South:      ++y; break;
					case Direction.Left:  --x; ++y; break;
					case Direction.West:  --x;      break;
					case Direction.Up:    --x; --y; break;
				}

				Sector sector = m.Map.GetSector( x, y );

				foreach ( Item item in sector.Items )
				{
					if ( item.Location.X == x && item.Location.Y == y && (item.Z + item.ItemData.Height) > m.Z && (m.Z + 16) > item.Z && item is BaseDoor && m.CanSee( item ) && m.InLOS( item ) )
					{
						if ( m.CheckAlive() )
						{
							m.SendLocalizedMessage( 500024 ); // Opening door...
							item.OnDoubleClick( m );
						}

						break;
					}
				}
			}
		}

		public static Point3D GetOffset( DoorFacing facing )
		{
			return m_Offsets[(int)facing];
		}

		private class InternalTimer : Timer
		{
			private BaseDoor m_Door;

			public InternalTimer( BaseDoor door ) : base( TimeSpan.FromSeconds( 20.0 ), TimeSpan.FromSeconds( 10.0 ) )
			{
				Priority = TimerPriority.OneSecond;
				m_Door = door;
			}

			protected override void OnTick()
			{
				if ( m_Door.Open && m_Door.IsFreeToClose() )
					m_Door.Open = false;
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

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Open
		{
			get
			{
				return m_Open;
			}
			set
			{
				if ( m_Open != value )
				{
					m_Open = value;

					ItemID = m_Open ? m_OpenedID : m_ClosedID;

					if ( m_Open )
						Location = new Point3D( X + m_Offset.X, Y + m_Offset.Y, Z + m_Offset.Z );
					else
						Location = new Point3D( X - m_Offset.X, Y - m_Offset.Y, Z - m_Offset.Z );

					Effects.PlaySound( this, Map, m_Open ? m_OpenedSound : m_ClosedSound );
					
					if ( m_Open && m_ClosesAutomatically )
						m_Timer.Start();
					else if( m_ClosesAutomatically )
						m_Timer.Stop();
				}
			}
		}

		public bool CanClose()
		{
			if ( !m_Open )
				return true;

			Map map = Map;

			if ( map == null )
				return false;

			Point3D p = new Point3D( X - m_Offset.X, Y - m_Offset.Y, Z - m_Offset.Z );

			return CheckFit( map, p, 16 );
		}

		private bool CheckFit( Map map, Point3D p, int height )
		{
			if ( map == Map.Internal )
				return false;

			int x = p.X;
			int y = p.Y;
			int z = p.Z;

			Sector sector = map.GetSector( x, y );
			List<Item> items  = sector.Items;
			List<Mobile> mobs = sector.Mobiles;

			for ( int i = 0; i < items.Count; ++i )
			{
				Item item = items[i];

				if ( item.ItemID < 0x4000 && item.AtWorldPoint( x, y ) && !(item is BaseDoor) )
				{
					ItemData id = item.ItemData;
					bool surface = id.Surface;
					bool impassable = id.Impassable;

					if ( (surface || impassable) && (item.Z + id.CalcHeight) > z && (z + height) > item.Z )
						return false;
				}
			}

			for ( int i = 0; i < mobs.Count; ++i )
			{
				Mobile m = mobs[i];

				if ( m.Location.X == x && m.Location.Y == y )
				{
					if ( m.Hidden && m.AccessLevel > AccessLevel.Player )
						continue;

					if ( !m.Alive )
						continue;

					if ( (m.Z + 16) > z && (z + height) > m.Z )
						return false;
				}
			}

			return true;
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int OpenedID
		{
			get
			{
				return m_OpenedID;
			}
			set
			{
				m_OpenedID = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int ClosedID
		{
			get
			{
				return m_ClosedID;
			}
			set
			{
				m_ClosedID = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int OpenedSound
		{
			get
			{
				return m_OpenedSound;
			}
			set
			{
				m_OpenedSound = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int ClosedSound
		{
			get
			{
				return m_ClosedSound;
			}
			set
			{
				m_ClosedSound = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Point3D Offset
		{
			get
			{
				return m_Offset;
			}
			set
			{
				m_Offset = value;
			}
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public BaseDoor Link
		{
			get
			{
				if ( m_Link != null && m_Link.Deleted )
					m_Link = null;

				return m_Link;
			}
			set
			{
				m_Link = value;
			}
		}

		public virtual bool UseChainedFunctionality{ get{ return false; } }

		public ArrayList GetChain()
		{
			ArrayList list = new ArrayList();
			BaseDoor c = this;

			do
			{
				list.Add( c );
				c = c.Link;
			} while ( c != null && !list.Contains( c ) );

			return list;
		}

		public bool IsFreeToClose()
		{
			if ( !UseChainedFunctionality )
				return CanClose();

			ArrayList list = GetChain();

			bool freeToClose = true;

			for ( int i = 0; freeToClose && i < list.Count; ++i )
				freeToClose = ((BaseDoor)list[i]).CanClose();

			return freeToClose;
		}

		public void OnTelekinesis( Mobile from )
		{
			Effects.SendLocationParticles( EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x376A, 9, 32, 5022 );
			Effects.PlaySound( Location, Map, 0x1F5 );

			if ( !this.TrapOnOpen || !ExecuteTrap( from ) )
				Use( from );
		}
		
		public void ChangeLock( uint newlock )
		{
			if( this.m_KeyValue == newlock )
				return;
			
			this.m_KeyValue = newlock;
			
			if( this.Link != null )
			{
				BaseDoor link = this.Link;
				
				if( link != null )
				{
					link.ChangeLock( newlock );
				}
			}
		}

		public virtual bool IsInside( Mobile from )
		{
			return false;
		}

		public virtual bool UseLocks()
		{
			return true;
		}

		public virtual void Use( Mobile from )
		{
			if( from.AccessLevel < AccessLevel.GameMaster )
				from.RevealingAction();
			
			if ( m_Locked && !m_Open && UseLocks() )
			{
				if ( from.AccessLevel >= AccessLevel.GameMaster )
				{
					from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 502502 ); // That is locked, but you open it with your godly powers.
					//from.Send( new MessageLocalized( Serial, ItemID, MessageType.Regular, 0x3B2, 3, 502502, "", "" ) ); // That is locked, but you open it with your godly powers.
				}
				else if ( Key.ContainsKey( from.Backpack, this.KeyValue ) && this.KeyValue != 0 )
				{
					from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 501282 ); // You quickly unlock, open, and relock the door
				}
				else if ( IsInside( from ) )
				{
					from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 501280 ); // That is locked, but is usable from the inside.
				}
				else
				{
					if( from is PlayerMobile && ((PlayerMobile)from).AutoPicking && this.AllowPicking( from ) )
					{
                                        						from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 502503 ); // That is locked.

					return;
					}
					else if ( Hue == 0x44E && Map == Map.Malas ) // doom door into healer room in doom
						this.SendLocalizedMessageTo( from, 1060014 ); // Only the dead may pass.
					else
						from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 502503 ); // That is locked.

					return;
				}
			}

			if ( m_Open && !IsFreeToClose() )
				return;

			if ( m_Open )
				OnClosed( from );
			else
				OnOpened( from );

			if ( UseChainedFunctionality )
			{
				bool open = !m_Open;

				ArrayList list = GetChain();

				for ( int i = 0; i < list.Count; ++i )
					((BaseDoor)list[i]).Open = open;
			}
			else
			{
				Open = !m_Open;

				BaseDoor link = this.Link;

				if ( m_Open && link != null && !link.Open )
					link.Open = true;
			}
		}

		public virtual void OnOpened( Mobile from )
		{
			if ( this.TrapOnOpen )
				ExecuteTrap( from );
		}

		public virtual void OnClosed( Mobile from )
		{
			if( from.AccessLevel < AccessLevel.GameMaster )
				from.RevealingAction();
            if (RelockOnClose)
                Timer.DelayCall(TimeSpan.FromMinutes(3), RelockDoor);
		}

        public virtual void RelockDoor()
        {            
            Locked = true;
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( from.AccessLevel == AccessLevel.Player && (/*!from.InLOS( this ) || */!from.InRange( GetWorldLocation(), 2 )) )
				from.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1019045 ); // I can't reach that.
			else
				Use( from );
		}
		
		public virtual bool ExecuteTrap( Mobile from )
		{
			if ( m_TrapType != TrapType.None )
			{
				Point3D loc = this.GetWorldLocation();
				Map facet = this.Map;

				if ( from.AccessLevel >= AccessLevel.GameMaster )
				{
					from.SendMessage( "That is trapped, but you open it with your godly powers." );
					return false;
				}

				switch ( m_TrapType )
				{
					case TrapType.ExplosionTrap:
					{
						from.SendMessage( "You set off a trap!" ); // You set off a trap!

						if ( from.InRange( loc, 3 ) )
						{
							int damage;

							if ( m_TrapLevel > 0 )
								damage = Utility.RandomMinMax( 10, 30 ) * m_TrapLevel;
							else
								damage = m_TrapPower;

							AOS.Damage( from, damage, 0, 100, 0, 0, 0, 0, 0, 0 );

							// Your skin blisters from the heat!
							from.LocalOverheadMessage( Network.MessageType.Regular, 0x2A, 503000 );
						}

						Effects.SendLocationEffect( loc, facet, 0x36BD, 15, 10 );
						Effects.PlaySound( loc, facet, 0x307 );

						break;
					}
					case TrapType.MagicTrap:
					{
						if ( from.InRange( loc, 1 ) )
							from.Damage( m_TrapPower );
							//AOS.Damage( from, m_TrapPower, 0, 100, 0, 0, 0 );

						Effects.PlaySound( loc, Map, 0x307 );

						Effects.SendLocationEffect( new Point3D( loc.X - 1, loc.Y, loc.Z ), Map, 0x36BD, 15 );
						Effects.SendLocationEffect( new Point3D( loc.X + 1, loc.Y, loc.Z ), Map, 0x36BD, 15 );

						Effects.SendLocationEffect( new Point3D( loc.X, loc.Y - 1, loc.Z ), Map, 0x36BD, 15 );
						Effects.SendLocationEffect( new Point3D( loc.X, loc.Y + 1, loc.Z ), Map, 0x36BD, 15 );

						Effects.SendLocationEffect( new Point3D( loc.X + 1, loc.Y + 1, loc.Z + 11 ), Map, 0x36BD, 15 );

						break;
					}
					case TrapType.DartTrap:
					{
						from.SendMessage( "You set off a trap!" ); // You set off a trap!

						if ( from.InRange( loc, 3 ) )
						{
							int damage;

							if ( m_TrapLevel > 0 )
								damage = Utility.RandomMinMax( 5, 15 ) * m_TrapLevel;
							else
								damage = m_TrapPower;

							AOS.Damage( from, damage, 100, 0, 0, 0, 0, 0, 0, 0 );

							// A dart imbeds itself in your flesh!
							from.LocalOverheadMessage( Network.MessageType.Regular, 0x62, 502998 );
						}

						Effects.PlaySound( loc, facet, 0x223 );

						break;
					}
					case TrapType.PoisonTrap:
					{
						from.SendMessage( "You set off a trap!" ); // You set off a trap!

						if ( from.InRange( loc, 3 ) )
						{
							Poison poison;

							if ( m_TrapLevel > 0 )
							{
								poison = Poison.GetPoison( Math.Max( 0, Math.Min( 4, m_TrapLevel - 1 ) ) );
							}
							else
							{
								AOS.Damage( from, m_TrapPower, 0, 0, 0, 100, 0, 0, 0, 0 );
								poison = Poison.Greater;
							}

							from.ApplyPoison( from, poison );

							// You are enveloped in a noxious green cloud!
							from.LocalOverheadMessage( Network.MessageType.Regular, 0x44, 503004 );
						}

						Effects.SendLocationEffect( loc, facet, 0x113A, 10, 20 );
						Effects.PlaySound( loc, facet, 0x231 );

						break;
					}
				}

				m_TrapType = TrapType.None;
				m_TrapPower = 0;
				m_TrapLevel = 0;
				return true;
			}

			return false;
		}
		
		public bool AllowPicking( Mobile from )
		{
			BaseHouse house = BaseHouse.FindHouseAt( this );
			
			if( house == null )
				house = BaseHouse.FindHouseAt( new Point3D( this.X, this.Y + 1, this.Z ), this.Map, 1 );
			
			if( house == null )
				house = BaseHouse.FindHouseAt( new Point3D( this.X + 1, this.Y, this.Z ), this.Map, 1 );
			
			if( house != null && house.Owner != null && !house.Owner.Deleted && house.Owner is PlayerMobile )
			{
				PlayerMobile owner = house.Owner as PlayerMobile;
				
				if( house.Region.Contains( owner.LogoutLocation ) && DateTime.Compare( (owner.LogoutTime+TimeSpan.FromHours(8)), DateTime.Now ) > 0 )
				{
					from.SendMessage( "You realize there is someone at home and decide it is best to leave the lock alone." );
					return false;	
				}
			}
			
			return true;
		}
		
		public virtual void LockPick( Mobile from )
		{
			if( from is PlayerMobile && ((PlayerMobile)from).BreakLock )
				Locked = false;                
			else
                this.Open = true;
			
			Picker = from;
            if (Picker is PlayerMobile)
                (Picker as PlayerMobile).CriminalActivity = true;

			if ( this.TrapOnLockpick && ExecuteTrap( from ) )
			{
				this.TrapOnLockpick = false;
			}
		}

		public BaseDoor( int closedID, int openedID, int openedSound, int closedSound, Point3D offset ) : base( closedID )
		{
			m_OpenedID = openedID;
			m_ClosedID = closedID;
			m_OpenedSound = openedSound;
			m_ClosedSound = closedSound;
			m_Offset = offset;

			m_Timer = new InternalTimer( this );

			Movable = false;
			m_MaxLockLevel = 100;
			this.LockLevel = 100;
			this.RequiredSkill = 100;
		}

		public BaseDoor( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
			
			writer.Write( (int) 9 ); // version

            writer.Write((bool)m_RelockOnClose);

            writer.Write((Mobile)m_Picker);
			writer.Write( (bool) m_ClosesAutomatically );
			writer.Write( (int) m_TrapLevel );
			writer.Write( (int) m_TrapPower );
			writer.Write( (int) m_TrapType );
			writer.Write( (bool) m_TrapOnLockpick );
			writer.Write( (int) m_RequiredSkill );
			writer.Write( (int) m_MaxLockLevel );
			writer.Write( (int) m_LockLevel );

			writer.Write( m_KeyValue );

			writer.Write( m_Open );
			writer.Write( m_Locked );
			writer.Write( m_OpenedID );
			writer.Write( m_ClosedID );
			writer.Write( m_OpenedSound );
			writer.Write( m_ClosedSound );
			writer.Write( m_Offset );
			writer.Write( m_Link );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			version = reader.ReadInt();

            if (version > 8)
                m_RelockOnClose = reader.ReadBool();

            if (version > 7)
                m_Picker = reader.ReadMobile();

			if( version > 6 )
				m_ClosesAutomatically = reader.ReadBool();
			
			m_TrapLevel = reader.ReadInt();
			m_TrapPower = reader.ReadInt();
			m_TrapType = (TrapType)reader.ReadInt();
			m_TrapOnLockpick = reader.ReadBool();
			m_RequiredSkill = reader.ReadInt();
			m_MaxLockLevel = reader.ReadInt();
			m_LockLevel = reader.ReadInt();

			m_KeyValue = reader.ReadUInt();
			
			m_Open = reader.ReadBool();
			m_Locked = reader.ReadBool();
			m_OpenedID = reader.ReadInt();
			m_ClosedID = reader.ReadInt();
			m_OpenedSound = reader.ReadInt();
			m_ClosedSound = reader.ReadInt();
			m_Offset = reader.ReadPoint3D();
			m_Link = reader.ReadItem() as BaseDoor;

			m_Timer = new InternalTimer( this );

			if ( m_Open && m_ClosesAutomatically )
				m_Timer.Start();
			
			if( this.LockLevel < 100 )
				this.LockLevel = 100;
			
			if( this.RequiredSkill < 100 )
				this.RequiredSkill = 100;
		}
	}
    
}
