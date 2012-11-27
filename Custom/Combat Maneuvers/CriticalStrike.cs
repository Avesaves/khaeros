using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class CriticalStrike : BaseCombatManeuver
	{        
		public override int DamageBonus{ get{ return 7; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.CriticalStrike; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                attacker.Emote( "*steps into {0} and hits {1} in a vital point*", defender.Name, ((IKhaerosMobile)defender).GetReflexivePronoun() );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
		}
		
		public CriticalStrike()
		{
		}
		
		public CriticalStrike( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.CriticalStrike) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.CriticalStrike);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "CriticalStrike", AccessLevel.Player, new CommandEventHandler( CriticalStrike_OnCommand ) );
		}
		
		[Usage( "CriticalStrike" )]
        [Description( "Allows the user to attempt a Critical Strike." )]
        private static void CriticalStrike_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.CriticalStrike) ) )
                m.ChangeManeuver( new CriticalStrike( m.Feats.GetFeatLevel(FeatList.CriticalStrike) ), FeatList.CriticalStrike, "You prepare a Critical Strike." );
        }
	}
}
