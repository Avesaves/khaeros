using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class Feint : BaseCombatManeuver
	{
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.Feint; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false );
			//	attacker.Emote( "*feigns some strikes at {0} to confuse {1} and lower {2} guard*", defender.Name, ((IKhaerosMobile)defender).GetReflexivePronoun(), ((IKhaerosMobile)defender).GetPossessivePronoun() );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			//Effect( defender, FeatLevel );
		}
		
		/*public static void Effect( Mobile defender, int featlevel )
		{
			IKhaerosMobile defplayer = defender as IKhaerosMobile;
			
			if( defplayer.FeintTimer != null )
				defplayer.FeintTimer.Stop();
			
			defplayer.FeintTimer = new FeintTimer( defender, featlevel );
			defplayer.FeintTimer.Start();
		}*/
		
		public Feint()
		{
		}
		
		public Feint( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Feint) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Feint);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "Feint", AccessLevel.Player, new CommandEventHandler( Feint_OnCommand ) );
		}
		
		[Usage( "Feint" )]
        [Description( "Allows the user to attempt a Feint." )]
        private static void Feint_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.Feint) ) )
                m.ChangeManeuver( new Feint( m.Feats.GetFeatLevel(FeatList.Feint) ), FeatList.Feint, "You prepare a Feint." );
        }
		
		/*public class FeintTimer : Timer
        {
            private Mobile m_from;

            public FeintTimer( Mobile from, int featlevel )
            	: base( TimeSpan.FromSeconds( featlevel * 3 ) )
            {
                m_from = from;
                from.SendMessage( 60, "You have lost your concentration on the battle." );
            }

            protected override void OnTick()
            {
            	if( m_from == null )
            		return;
                
                m_from.SendMessage( 60, "You have regained your concentration on the battle." );
            	((IKhaerosMobile)m_from).FeintTimer = null;
            }
        }*/
	}
}
