using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class CriticalShot : BaseCombatManeuver
	{        
		public override int DamageBonus{ get{ return 7; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return false; } }
		public override bool Ranged{ get{ return true; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.CriticalShot; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                attacker.Emote( "*aims for a vital spot and shoots*" );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
		}
		
		public CriticalShot()
		{
		}
		
		public CriticalShot( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.CriticalShot) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.CriticalShot);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "CriticalShot", AccessLevel.Player, new CommandEventHandler( CriticalShot_OnCommand ) );
		}
		
		[Usage( "CriticalShot" )]
        [Description( "Allows the user to attempt a Critical Shot." )]
        private static void CriticalShot_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.CriticalShot) ) )
                m.ChangeManeuver( new CriticalShot( m.Feats.GetFeatLevel(FeatList.CriticalShot) ), FeatList.CriticalShot, "You prepare a Critical Shot." );
        }
	}
}
