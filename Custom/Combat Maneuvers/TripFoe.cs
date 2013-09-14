using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
	public class Unhorse : BaseCombatManeuver
	{
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.Unhorse; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			IKhaerosMobile featuser = attacker as IKhaerosMobile;
			
			if ( attacker.Mounted || !defender.Mounted)
			{
				attacker.SendMessage( 60, "You can only perform this maneuver while on foot and against a mounted opponent." );
				featuser.DisableManeuver();
			}
			else if ( ((IKhaerosMobile)defender).TrippedTimer != null || CombatSystemAttachment.GetCSA( defender ).PerformingSequence )
			{
				attacker.SendMessage( 60, "You cannot unhorse them right now." );
				featuser.DisableManeuver();
			}
			
			else if( ((BaseWeapon)attacker.Weapon).Skill == SkillName.Polearms )
                if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                    attacker.Emote( "*swings {0} polearm in an attempt to unhorse {1}*", ((IKhaerosMobile)attacker).GetPossessivePronoun(), defender.Name );

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
				Misc.Dismount.Effect( defender, featlevel );
				AddTripTimer( defender, featlevel );
				
				return;
			}
			else
				attacker.SendMessage( 60, "Your opponent must be mounted!" );
		}
		
		public static void AddTripTimer( Mobile defender, int featlevel )
		{
			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( defender );
			csa.DoTrip( featlevel );
		}
		
		public Unhorse()
		{
		}
		
		public Unhorse( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Unhorse) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Unhorse);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize() 
		{
			CommandSystem.Register( "Unhorse", AccessLevel.Player, new CommandEventHandler( Unhorse_OnCommand ) );
		}
		
		[Usage( "Unhorse" )]
        [Description( "Allows the user to attempt to unhorse his foe." )]
        private static void Unhorse_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.Unhorse) ) )
            {
                if( ((BaseWeapon)m.Weapon).Skill == SkillName.Polearms )
                    m.ChangeManeuver( new Unhorse( m.Feats.GetFeatLevel(FeatList.Unhorse) ), FeatList.Unhorse, "You prepare to unhorse your foe." );

                else
                    m.SendMessage( 60, "You need to be equipping a polearm in order to perform this attack." );
            }
        }

        public class UnhorseTimer : Timer
        {
            private Mobile m_from;
			public int m_Stage;
			private int m_FeatLevel;
            public bool m_Repeat;
//TimeSpan.FromSeconds(((double)featlevel) * 2.0 - 0.4)
            public UnhorseTimer( Mobile from, int featlevel ) : base( TimeSpan.FromSeconds( 0.4 ), TimeSpan.FromSeconds( 0.4 ) )
            {
				Priority = TimerPriority.TwoFiftyMS;
                m_from = from;
				m_Stage = 0;
				m_FeatLevel = featlevel;
                from.SendMessage( 60, "You have been knocked to the ground!" );
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
