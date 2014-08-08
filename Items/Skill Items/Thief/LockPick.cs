using System;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public interface ILockpickable : IPoint2D
	{
		int LockLevel{ get; set; }
		bool Locked{ get; set; }
		Mobile Picker{ get; set; }
		int MaxLockLevel{ get; set; }
		int RequiredSkill{ get; set; }

		void LockPick( Mobile from );
	}

	[FlipableAttribute( 0x14fc, 0x14fb )]
	public class Lockpick : Item
	{
		[Constructable]
		public Lockpick() : this( 1 )
		{
		}

		[Constructable]
		public Lockpick( int amount ) : base( 0x14FC )
		{
			Stackable = true;
			Amount = amount;
		}

		public Lockpick( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( version == 0 && Weight == 0.1 )
				Weight = -1;
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendLocalizedMessage( 502068 ); // What do you want to pick?
			from.Target = new InternalTarget( this );
		}

		private class InternalTarget : Target
		{
			private Lockpick m_Item;

			public InternalTarget( Lockpick item ) : base( 1, false, TargetFlags.None )
			{
				m_Item = item;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( m_Item.Deleted )
					return;

				if ( targeted is ILockpickable )
				{
                    ((PlayerMobile)from).CriminalActivity = true; // This is a criminal thing to do, picking a lock!

					Item item = (Item)targeted;
					from.Direction = from.GetDirectionTo( item );
					
					if( item is BaseDoor && from is PlayerMobile && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Locksmith) < 1 )
					{
						from.SendMessage( "You need the first level of the Locksmith feat in order to attempt that." );
						return;
					}

					if ( ((ILockpickable)targeted).Locked )
					{
						if( item is BaseDoor && !((BaseDoor)item).AllowPicking( from ) )
							return;
						
						from.PlaySound( 0x241 );

						new InternalTimer( from, (ILockpickable)targeted, m_Item ).Start();
					}
					else
					{
						// The door is not locked
						from.SendLocalizedMessage( 502069 ); // This does not appear to be locked
					}
				}
				else
				{
					from.SendLocalizedMessage( 501666 ); // You can't unlock that!
				}
			}

			private class InternalTimer : Timer
			{
				private Mobile m_From;
				private ILockpickable m_Item;
				private Lockpick m_Lockpick;
			
				public InternalTimer( Mobile from, ILockpickable item, Lockpick lockpick ) : base( TimeSpan.FromSeconds( 30.0 - (from.RawInt / 10) ) )
				{
					m_From = from;
					m_Item = item;
					m_Lockpick = lockpick;
					Priority = TimerPriority.TwoFiftyMS;
					
					if( from.AccessLevel < AccessLevel.GameMaster )
						from.RevealingAction();
				}

				protected void BrokeLockPickTest()
				{
					// When failed, a 25% chance to break the lockpick
					if ( Utility.Random( 4 ) == 0 )
					{
						Item item = (Item)m_Item;

						// You broke the lockpick.
						item.SendLocalizedMessageTo( m_From, 502074 );

						m_From.PlaySound( 0x3A4 );
						m_Lockpick.Consume();
					}
				}
				
				protected override void OnTick()
				{
					Item item = (Item)m_Item;


                    double LockpickingSkill = m_From.Skills[SkillName.Lockpicking].Value;
                   if (m_Item is tChest)
                        LockpickingSkill += m_From.Skills[SkillName.Tinkering].Value;

                    if (m_From is PlayerMobile)
                    {
                        PlayerMobile pm = m_From as PlayerMobile;
                        pm.CriminalActivity = true;
                        if (m_Item is tChest)
                            LockpickingSkill += Utility.Random(pm.Feats.GetFeatLevel(FeatList.Counterfeiting) * Utility.Random(100 + Utility.Random(pm.RawInt)));
                        else
                            LockpickingSkill += Utility.Random(pm.Feats.GetFeatLevel(FeatList.Safecracking) * Utility.Random(100 + Utility.Random(pm.RawInt)));
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

                    if ( LockpickingSkill >= Utility.RandomMinMax(m_Item.LockLevel, m_Item.MaxLockLevel))
					{
						// Success! Pick the lock!
						item.SendLocalizedMessageTo( m_From, 502076 ); // The lock quickly yields to your skill.
						m_From.PlaySound( 0x4A );
						m_Item.LockPick( m_From );
					}
					else
					{
						if( m_From is PlayerMobile && ((PlayerMobile)m_From).Feats.GetFeatLevel(FeatList.Locksmith) < 1 )
						{
							// The player failed to pick the lock
							BrokeLockPickTest();
						}
						
						item.SendLocalizedMessageTo( m_From, 502075 ); // You are unable to pick the lock.
					}
				}
			}
		}
	}
}
