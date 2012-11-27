using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class CripplingShot : BaseCombatManeuver
	{
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return false; } }
		public override bool Ranged{ get{ return true; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.CripplingShot; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                attacker.Emote( "*shoots {0} in an attempt to cripple {1}", defender.Name, ((IKhaerosMobile)defender).GetReflexivePronoun() );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			CripplingBlow.Effect( attacker, defender, FeatLevel );
		}
		
		public CripplingShot()
		{
		}
		
		public CripplingShot( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.CripplingShot) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.CripplingShot);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "CripplingShot", AccessLevel.Player, new CommandEventHandler( CripplingShot_OnCommand ) );
		}
		
		[Usage( "CripplingShot" )]
        [Description( "Allows the user to attempt a Crippling Shot." )]
        private static void CripplingShot_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.CripplingShot) ) )
                m.ChangeManeuver( new CripplingShot( m.Feats.GetFeatLevel(FeatList.CripplingShot) ), FeatList.CripplingShot, "You prepare a Crippling Shot." );
        }
	}
}
