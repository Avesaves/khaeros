using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class MountedCharge : BaseCombatManeuver
	{
		public override int DamageBonus{ get{ return 3; } }
		public override int AccuracyBonus{ get{ return 5; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.MountedCharge; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			IKhaerosMobile featuser = attacker as IKhaerosMobile;
			
			if( attacker.Frozen || !attacker.Mounted )
			{
            	attacker.SendMessage( 60, "You cannot use this attack while you are frozen or not mounted." );
            	featuser.DisableManeuver();
            }
			
            else if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
	        	attacker.Emote( "*clashes against {0} in a mounted charge attack*", defender.Name );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
		}
		
		public MountedCharge()
		{
		}
		
		public MountedCharge( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.MountedCharge) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.MountedCharge);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize() 
		{
			CommandSystem.Register( "MountedCharge", AccessLevel.Player, new CommandEventHandler( MountedCharge_OnCommand ) );
		}
		
		[Usage( "MountedCharge" )]
        [Description( "Allows the user to attempt a Mounted Charge." )]
        private static void MountedCharge_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.MountedCharge) ) )
                m.ChangeManeuver( new MountedCharge( m.Feats.GetFeatLevel(FeatList.MountedCharge) ), FeatList.MountedCharge, "You prepare a Mounted Charge." );
        }
	}
}
