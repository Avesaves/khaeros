using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class CripplingBlow : BaseCombatManeuver
	{
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.CripplingBlow; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
				attacker.Emote( "*strikes {0} in an attempt to cripple {1}*", defender.Name, ((IKhaerosMobile)defender).GetReflexivePronoun() );
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
			
			IKhaerosMobile defplayer = defender as IKhaerosMobile;
			
			if( defplayer.CrippledTimer != null )
				defplayer.CrippledTimer.Stop();
			
			defplayer.CrippledTimer = new CripplingBlowTimer( defender, featlevel );
			defplayer.CrippledTimer.Start();
			if ( defender is PlayerMobile )
				((PlayerMobile)defender).CantRunIconRefresh();
		}
		
		public CripplingBlow()
		{
		}
		
		public CripplingBlow( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.CripplingBlow) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.CripplingBlow);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "CripplingBlow", AccessLevel.Player, new CommandEventHandler( CripplingBlow_OnCommand ) );
		}
		
		[Usage( "CripplingBlow" )]
        [Description( "Allows the user to attempt a Crippling Blow." )]
        private static void CripplingBlow_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.CripplingBlow) ) )
                m.ChangeManeuver( new CripplingBlow( m.Feats.GetFeatLevel(FeatList.CripplingBlow) ), FeatList.CripplingBlow, "You prepare a Crippling Blow." );
        }
		
		public class CripplingBlowTimer : Timer
        {
            private Mobile m_from;

            public CripplingBlowTimer( Mobile from, int featlevel )
            	: base( TimeSpan.FromSeconds( (double)featlevel * 1.5 ) )
            {
                m_from = from;
                from.SendMessage( 60, "You have been crippled." );
            }

            protected override void OnTick()
            {
            	if( m_from == null )
            		return;
                
                m_from.SendMessage( 60, "You are no longer crippled." );
            	((IKhaerosMobile)m_from).CrippledTimer = null;
				if ( m_from is PlayerMobile )
					((PlayerMobile)m_from).CantRunIconRefresh();
            }
        }
	}
}
