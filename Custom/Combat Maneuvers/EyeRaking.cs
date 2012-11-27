using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class EyeRaking : BaseCombatManeuver
	{        
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.EyeRaking; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( ((IKhaerosMobile)attacker).CanUseMartialPower && BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                attacker.Emote( "*thrusts {0} hands out and slashes {1} across the face and eyes*", ((IKhaerosMobile)attacker).GetPossessivePronoun(), defender.Name );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			Effect( defender, FeatLevel );
		}
		
		public static void Effect( Mobile defender, int featlevel )
		{
			IKhaerosMobile defplayer = defender as IKhaerosMobile;
			
			if( defplayer.BlindnessTimer != null )
				defplayer.BlindnessTimer.Stop();
			
			defplayer.BlindnessTimer = new EyeRakingTimer( defender, featlevel );
			defplayer.BlindnessTimer.Start();
		}
		
		public EyeRaking()
		{
		}
		
		public EyeRaking( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.EyeRaking) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.EyeRaking);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "EyeRaking", AccessLevel.Player, new CommandEventHandler( EyeRaking_OnCommand ) );
		}
		
		[Usage( "EyeRaking" )]
        [Description( "Allows the user to attempt a Eye Rake." )]
        private static void EyeRaking_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.EyeRaking) ) && m.CanUseMartialPower )
                m.ChangeManeuver( new EyeRaking( m.Feats.GetFeatLevel(FeatList.EyeRaking) ), FeatList.EyeRaking, "You prepare an Eye Rake." );
        }
        
        public class EyeRakingTimer : Timer
        {
            public Mobile m_from;

            public EyeRakingTimer( Mobile from, int featlevel )
                : base( TimeSpan.FromSeconds( featlevel * 2 ) )
            {
                m_from = from;
                from.SendGump( new Gumps.BlindnessGump() );
                from.SendMessage( 60, "You have been blinded. You will be able to see again shortly. Use the arrow keys on your keyboard if you wish to move around." );
            }

            protected override void OnTick()
            {
            	if( m_from == null )
            		return;
            	
                m_from.CloseGump( typeof( Gumps.BlindnessGump ) );
                m_from.SendMessage( 60, "You are no longer blind." );
                ((IKhaerosMobile)m_from).BlindnessTimer = null;
            }
        }
	}
}
