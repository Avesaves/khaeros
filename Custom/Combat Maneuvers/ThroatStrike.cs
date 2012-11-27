using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class ThroatStrike : BaseCombatManeuver
	{        
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.ThroatStrike; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( ((IKhaerosMobile)attacker).CanUseMartialPower && BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                attacker.Emote( "*fires a straight jab at {0}, aimed at {1} throat*", defender.Name, ((IKhaerosMobile)defender).GetPossessivePronoun() );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			Effect( defender, FeatLevel );
		}
		
		public static void Effect( Mobile defender, int featlevel )
		{
			IKhaerosMobile defplayer = defender as IKhaerosMobile;
			
			if( defplayer.MutenessTimer != null )
				defplayer.MutenessTimer.Stop();
			
			defplayer.MutenessTimer = new ThroatStrikeTimer( defender, featlevel );
			defplayer.MutenessTimer.Start();
		}
		
		public ThroatStrike()
		{
		}
		
		public ThroatStrike( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.ThroatStrike) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.ThroatStrike);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "ThroatStrike", AccessLevel.Player, new CommandEventHandler( ThroatStrike_OnCommand ) );
		}
		
		[Usage( "ThroatStrike" )]
        [Description( "Allows the user to attempt a Throat Strike." )]
        private static void ThroatStrike_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.ThroatStrike) ) && m.CanUseMartialPower )
                m.ChangeManeuver( new ThroatStrike( m.Feats.GetFeatLevel(FeatList.ThroatStrike) ), FeatList.ThroatStrike, "You prepare a Throat Strike." );
        }
        
        public class ThroatStrikeTimer : Timer
        {
            public Mobile m_from;
            public int m_reps;

            public ThroatStrikeTimer( Mobile from, int seconds )
                : base( TimeSpan.FromSeconds( 0 ), TimeSpan.FromSeconds( 1 ), 10 * seconds )
            {
                m_from = from;
				m_reps = 10 * seconds;
				m_from.SendMessage( 60, "You have been muted." );
            }

            protected override void OnTick()
            {
            	if( m_from == null )
            		return;
            	
                m_reps--;
                m_from.Stam--;
	
                if( m_reps == 0 )
                {
                	m_from.SendMessage( 60, "You are no longer mute." );
                	((IKhaerosMobile)m_from).MutenessTimer = null;
                }
            }
        }
	}
}
