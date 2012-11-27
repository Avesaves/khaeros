using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class SavageStrike : BaseCombatManeuver
	{        
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 10; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.SavageStrike; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                attacker.Emote( "*roars as {0} unleashes a savage strike on {1}*", ((IKhaerosMobile)attacker).GetPersonalPronoun(), defender.Name );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
		}
		
		public SavageStrike()
		{
		}
		
		public SavageStrike( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.SavageStrike) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.SavageStrike);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "SavageStrike", AccessLevel.Player, new CommandEventHandler( SavageStrike_OnCommand ) );
		}
		
		[Usage( "SavageStrike" )]
        [Description( "Allows the user to attempt a Savage Strike." )]
        private static void SavageStrike_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.SavageStrike) ) )
                m.ChangeManeuver( new SavageStrike( m.Feats.GetFeatLevel(FeatList.SavageStrike) ), FeatList.SavageStrike, "You prepare a Savage Strike." );
        }
	}
}
