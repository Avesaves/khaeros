using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class Disable : BaseCombatManeuver
	{
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.Disable; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( ((IKhaerosMobile)attacker).CanUseMartialPower && BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                attacker.Emote( "*reaches out in an attempt to strike {0} in a nerve*", defender.Name );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			DisableCheck( attacker, defender, FeatLevel );
		}
		
		public static void DisableCheck( Mobile attacker, Mobile defender, int featlevel )
		{
			int dischance = (int)(Math.Max( attacker.Skills[SkillName.Tactics].Base - defender.Skills[SkillName.Tactics].Base, 20 * featlevel ));
                        
            if( dischance >= Utility.RandomMinMax( 1, 100 ) )
            {
                int target = Utility.RandomMinMax( 1, 4 );
                Effect( defender, featlevel, target );
            }
		}
		
		public static void Effect( Mobile defender, int featlevel, int target )
		{
			IKhaerosMobile defplayer = defender as IKhaerosMobile;
			
			if( target == 0 )
				target = Utility.RandomMinMax( 1, 4 );
			
			switch( target )
            {
                case 1:
        		{
					if( defplayer.DisabledLegsTimer != null )
						defplayer.DisabledLegsTimer.Stop();
					
        			defplayer.DisabledLegsTimer = new DisableTimer( defender, featlevel, 1, "legs" );
        			defplayer.DisabledLegsTimer.Start();
        			defender.Emote( "*got {0} legs disabled*", ((IKhaerosMobile)defender).GetPossessivePronoun() );
					if ( defender is PlayerMobile )
						((PlayerMobile)defender).CantRunIconRefresh();
        			break;
        		}
                case 2:
        		{
					if( defplayer.DisabledLeftArmTimer != null )
						defplayer.DisabledLeftArmTimer.Stop();
					
        			defplayer.DisabledLeftArmTimer = new DisableTimer( defender, featlevel, 2, "left arm" );
        			defplayer.DisabledLeftArmTimer.Start();
        			defender.Emote( "*got {0} left arm disabled*", ((IKhaerosMobile)defender).GetPossessivePronoun() );
        			break;
        		}
                case 3:
        		{
					if( defplayer.DisabledRightArmTimer != null )
						defplayer.DisabledRightArmTimer.Stop();
					
        			defplayer.DisabledRightArmTimer = new DisableTimer( defender, featlevel, 3, "right arm" );
        			defplayer.DisabledRightArmTimer.Start();
        			defender.Emote( "*got {0} right arm disabled*", ((IKhaerosMobile)defender).GetPossessivePronoun() );
        			break;
        		}
                case 4: goto case 1;
            }
		}
		
		public Disable()
		{
		}
		
		public Disable( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Disable) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Disable);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "Disable", AccessLevel.Player, new CommandEventHandler( Disable_OnCommand ) );
		}
		
		[Usage( "Disable" )]
        [Description( "Allows the user to attempt an attack to Disable your enemy." )]
        private static void Disable_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.Disable) ) && m.CanUseMartialPower )
                m.ChangeManeuver( new Disable( m.Feats.GetFeatLevel(FeatList.Disable) ), FeatList.Disable, "You prepare an attack to Disable your enemy." );
        }
		
		public class DisableTimer : Timer
        {
            private Mobile m_from;
            private string m_limb = "insert limb";
            private int m_target;

            public DisableTimer( Mobile from, int featlevel, int target, string limb )
                : base( TimeSpan.FromSeconds( featlevel * 4 ) )
            {
                m_from = from;
                m_target = target;
                m_limb = limb;
            }

            protected override void OnTick()
            {
            	if( m_from == null )
            		return;

                m_from.Emote( "*{0} {1} {2} no longer disabled*", ((IKhaerosMobile)m_from).GetPossessivePronoun(), m_limb, m_target == 1 ? "are" : "is" );
                
                switch( m_target )
                {
                    case 1:
					{
						((IKhaerosMobile)m_from).DisabledLegsTimer = null; 
						if ( m_from is PlayerMobile )
							((PlayerMobile)m_from).CantRunIconRefresh();
						break;
					}
                    case 2: ((IKhaerosMobile)m_from).DisabledLeftArmTimer = null; break;
                	case 3: ((IKhaerosMobile)m_from).DisabledRightArmTimer = null; break;
                }
             }
        }
	}
}
