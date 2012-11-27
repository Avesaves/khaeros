using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class SunderWeapon : BaseCombatManeuver
	{        
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 10; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.SunderWeapon; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( defender.Weapon is Fists )
			{
				attacker.SendMessage( 60, "You cannot use that attack against unarmed foes." );
				((IKhaerosMobile)attacker).DisableManeuver();
			}
			
			if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                attacker.Emote( "*firmly clashes {0} weapon against {1}{2} weapon*", attacker.Female == true ? "her" : "his", defender.Name, defender.Name.EndsWith( "s" ) == true ? "'" : "'s" );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			Effect( defender, FeatLevel, ((BaseWeapon)attacker.Weapon).ComputeDamage( attacker, defender ), " by " + attacker.Name );
		}
		
		public static void Effect( Mobile defender, int featlevel, int damage, string source )
		{
			if( source == null )
				source = "";
			
			BaseWeapon sundered = defender.Weapon as BaseWeapon;
			sundered.HitPoints -= (int)( damage * (featlevel * 0.20) );

            if( sundered.HitPoints < 0 )
            {
                sundered.MaxHitPoints += sundered.HitPoints;
                sundered.HitPoints = 0;

                if( sundered.MaxHitPoints < 1 )
                {
                    sundered.Delete();
                    defender.Emote( "*got {0} weapon destroyed{1}*", ((IKhaerosMobile)defender).GetPossessivePronoun(), source );
                }
            }

            defender.Emote( "*got {0} weapon damaged{1}*", ((IKhaerosMobile)defender).GetPossessivePronoun(), source );
		}
		
		public SunderWeapon()
		{
		}
		
		public SunderWeapon( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.SunderWeapon) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.SunderWeapon);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "SunderWeapon", AccessLevel.Player, new CommandEventHandler( SunderWeapon_OnCommand ) );
		}
		
		[Usage( "SunderWeapon" )]
        [Description( "Allows the user to attempt to Sunder their enemy's Weapon." )]
        private static void SunderWeapon_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.SunderWeapon) ) )
                m.ChangeManeuver( new SunderWeapon( m.Feats.GetFeatLevel(FeatList.SunderWeapon) ), FeatList.SunderWeapon, "You prepare an attack to Sunder your enemy's Weapon." );
        }
	}
}
