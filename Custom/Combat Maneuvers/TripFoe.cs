using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
	public class TripFoe : BaseCombatManeuver
	{
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.TripFoe; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			IKhaerosMobile featuser = attacker as IKhaerosMobile;
			
			if ( attacker.Mounted )
			{
				attacker.SendMessage( 60, "You cannot perform this maneuver while mounted." );
				featuser.DisableManeuver();
			}
			else if ( ((IKhaerosMobile)defender).TrippedTimer != null || CombatSystemAttachment.GetCSA( defender ).PerformingSequence )
			{
				attacker.SendMessage( 60, "You cannot trip them right now." );
				featuser.DisableManeuver();
			}
			
			else if( ((BaseWeapon)attacker.Weapon).Skill == SkillName.Polearms )
                if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                    attacker.Emote( "*swings {0} polearm in an attempt to trip {1}*", ((IKhaerosMobile)attacker).GetPossessivePronoun(), defender.Name );

            else
            {
                attacker.SendMessage( 60, "You need to be equipping a polearm in order to perform this attack." );
                featuser.DisableManeuver();
            }
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			Effect( attacker, defender, FeatLevel );
		}
		
		public static void Effect( Mobile attacker, Mobile defender, int featlevel )
		{
			if( defender.Mounted )
			{
				if( attacker != null )
					Misc.Dismount.DismountCheck( attacker, defender, 0, ((BaseWeapon)attacker.Weapon).Skill, featlevel );
				else
					Misc.Dismount.Effect( defender, featlevel );
				
				return;
			}
			else
				AddTripTimer( defender, featlevel );
		}
		
		public static void AddTripTimer( Mobile defender, int featlevel )
		{
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( defender );
			csa.DoTrip( featlevel );
		}
		
		public TripFoe()
		{
		}
		
		public TripFoe( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.TripFoe) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.TripFoe);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize() 
		{
			CommandSystem.Register( "TripFoe", AccessLevel.Player, new CommandEventHandler( TripFoe_OnCommand ) );
		}
		
		[Usage( "TripFoe" )]
        [Description( "Allows the user to attempt to trip his foe." )]
        private static void TripFoe_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.TripFoe) ) )
            {
                if( ((BaseWeapon)m.Weapon).Skill == SkillName.Polearms )
                    m.ChangeManeuver( new TripFoe( m.Feats.GetFeatLevel(FeatList.TripFoe) ), FeatList.TripFoe, "You prepare to trip your foe." );

                else
                    m.SendMessage( 60, "You need to be equipping a polearm in order to perform this attack." );
            }
        }

        public class TripFoeTimer : Timer
        {
            private Mobile m_from;
			public int m_Stage;
			private int m_FeatLevel;
            public bool m_Repeat;
//TimeSpan.FromSeconds(((double)featlevel) * 2.0 - 0.4)
            public TripFoeTimer( Mobile from, int featlevel ) : base( TimeSpan.FromSeconds( 0.4 ), TimeSpan.FromSeconds( 0.4 ) )
            {
				Priority = TimerPriority.TwoFiftyMS;
                m_from = from;
				m_Stage = 0;
				m_FeatLevel = featlevel;
                from.SendMessage( 60, "You have lost your balance." );
				CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( from );
				if ( from.Body.Type == BodyType.Human )
				{
					csa.Animate ( 21, 7, 1, true, false, 0 );
				}
				else if ( from.Body.Type == BodyType.Animal )
				{
					csa.Animate( 8, 4, 1, true, false, 0 );
				}
				else if ( from.Body.Type == BodyType.Sea ) // this really, eh.. can't fall down
				{
				}
				else if (from.Body.Type == BodyType.Monster )
				{
					csa.Animate( 2, 4, 1, true, false, 0 );
				}
				this.Interval = TimeSpan.FromSeconds(((double)m_FeatLevel) * 2.0 - 0.4);
            }

            protected override void OnTick()
            {
            	if( m_from == null )
            		return;

				CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m_from );
				
				if ( m_Stage == 0 )
				{
					CombatSystemAttachment.TripLoopCallback( csa );
					this.Interval = TimeSpan.FromSeconds( 0.4 );
					this.Delay = TimeSpan.FromSeconds(((double)m_FeatLevel) * 2.0 - 0.4);
				}
				else if ( m_Stage == 1 )
				{
					csa.StopAnimating( false );
					if ( m_from.Body.Type == BodyType.Human )
					{
						csa.Animate( 21, 6, 1, false, false, 0 );
					}
					else if ( m_from.Body.Type == BodyType.Animal )
					{
						csa.Animate( 8, 4, 1, false, false, 0 );
					}
					else if ( m_from.Body.Type == BodyType.Sea ) // this really, eh.. can't fall down
					{
					}
					else if ( m_from.Body.Type == BodyType.Monster )
					{
						csa.Animate( 2, 4, 1, false, false, 0 );
					}
					this.Delay = this.Interval = TimeSpan.FromSeconds( 0.4 );
				}
				else if ( m_Stage == 2 )
				{
					CombatSystemAttachment.TripGetUpCallback( csa );
					Stop();
					return;
				}

                if( !m_Repeat )
				    m_Stage++;
            }
        }
	}
}
