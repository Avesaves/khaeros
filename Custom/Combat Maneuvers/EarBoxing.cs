using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class EarBoxing : BaseCombatManeuver
	{        
		public override int DamageBonus{ get{ return 3; } }
		public override int AccuracyBonus{ get{ return 5; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.EarBoxing; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( ((IKhaerosMobile)attacker).CanUseMartialPower && BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                attacker.Emote( "*balls {0} fists up and slams them simultaneously at {1}, aiming for {2} ears*", ((IKhaerosMobile)attacker).GetPossessivePronoun(), defender.Name, ((IKhaerosMobile)defender).GetPossessivePronoun() );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			Effect( defender, FeatLevel );
		}
		
		public static void Effect( Mobile defender, int featlevel )
		{
			IKhaerosMobile defplayer = defender as IKhaerosMobile;
			
			if( defplayer.DeafnessTimer != null )
				defplayer.DeafnessTimer.Stop();
			
			defplayer.DeafnessTimer = new EarBoxingTimer( defender, featlevel );
			defplayer.DeafnessTimer.Start();
			
			Unhorse.AddTripTimer( defender, featlevel );
		}
		
		public EarBoxing()
		{
		}
		
		public EarBoxing( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.EarBoxing) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.EarBoxing);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "EarBoxing", AccessLevel.Player, new CommandEventHandler( EarBoxing_OnCommand ) );
		}
		
		[Usage( "EarBoxing" )]
        [Description( "Allows the user to attempt a Ear Boxe." )]
        private static void EarBoxing_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.EarBoxing) ) && m.CanUseMartialPower )
                m.ChangeManeuver( new EarBoxing( m.Feats.GetFeatLevel(FeatList.EarBoxing) ), FeatList.EarBoxing, "You prepare an Ear Boxe." );
        }
        
        public class EarBoxingTimer : Timer
        {
            private Mobile m_from;

            public EarBoxingTimer( Mobile from, int featlevel )
            	: base( TimeSpan.FromSeconds( featlevel * 30 ) )
            {
                m_from = from;
                from.SendMessage( 60, "You have been deafened." );
            }

            protected override void OnTick()
            {
            	if( m_from == null )
            		return;
                
                m_from.SendMessage( 60, "You are no longer deaf." );
            	((IKhaerosMobile)m_from).DeafnessTimer = null;
            }
        }
	}
}
