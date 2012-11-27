using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class StunningBlow : BaseCombatManeuver
	{
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.StunningBlow; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			IKhaerosMobile featuser = attacker as IKhaerosMobile;
			
			if( featuser.CanUseMartialPower )
	            if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
	                attacker.Emote( "*focuses and attacks {0} with all {1} strength, in an attempt to stun {2}*", defender.Name, ((IKhaerosMobile)attacker).GetPossessivePronoun(), ((IKhaerosMobile)defender).GetReflexivePronoun() );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			Effect( defender, FeatLevel );
		}
		
		public static void Effect( Mobile defender, int featlevel )
		{
			IKhaerosMobile defplayer = defender as IKhaerosMobile;
			
			if( defplayer.StunnedTimer != null )
				defplayer.StunnedTimer.Stop();
			
			defplayer.StunnedTimer = new StunningBlowTimer( defender, featlevel );
			defplayer.StunnedTimer.Start();
		}
		
		public StunningBlow()
		{
		}
		
		public StunningBlow( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.StunningBlow) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.StunningBlow);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "StunningBlow", AccessLevel.Player, new CommandEventHandler( StunningBlow_OnCommand ) );
		}
		
		[Usage( "StunningBlow" )]
        [Description( "Allows the user to attempt a Stunning Blow." )]
        private static void StunningBlow_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.StunningBlow) ) && m.CanUseMartialPower )
                m.ChangeManeuver( new StunningBlow( m.Feats.GetFeatLevel(FeatList.StunningBlow) ), FeatList.StunningBlow, "You prepare a Stunning Blow." );
        }
		
		public class StunningBlowTimer : Timer
        {
            private Mobile m_from;

            public StunningBlowTimer( Mobile from, int featlevel )
            	: base( TimeSpan.FromSeconds( (featlevel * 1.5) ) )
            {
                m_from = from;
                from.SendMessage( 60, "You have been stunned." );
            }

            protected override void OnTick()
            {
            	if( m_from == null )
            		return;
                
                m_from.SendMessage( 60, "You are no longer stunned" );
            	((IKhaerosMobile)m_from).StunnedTimer = null;
            }
        }
	}
}
