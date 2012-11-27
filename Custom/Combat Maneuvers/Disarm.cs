using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class Disarm : BaseCombatManeuver
	{        
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 10; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.Disarm; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( defender.Weapon is Fists || ((BaseWeapon)defender.Weapon).LootType == LootType.Blessed )
			{
				attacker.SendMessage( 60, "You cannot use that attack against unarmed foes." );
				((IKhaerosMobile)attacker).DisableManeuver();
			}
			
			else if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
                attacker.Emote( "*reaches out, applying an armlock on {0} as {1} tries to disarm {2}*", defender.Name, ((IKhaerosMobile)attacker).GetPersonalPronoun(), ((IKhaerosMobile)defender).GetReflexivePronoun() );
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
			DisarmCheck( attacker, defender, FeatLevel );
		}
		
		public static void DisarmCheck( Mobile attacker, Mobile defender, int featlevel )
		{
			/*int dischance = (int)(Math.Max( attacker.Skills[((BaseWeapon)attacker.Weapon).Skill].Base - defender.Skills[((BaseWeapon)attacker.Weapon).Skill].Base, 10 * featlevel ));
			
			if( dischance >= Utility.RandomMinMax(1, 100) )
				Effect( attacker, defender, (dischance >= Utility.RandomMinMax(1, 100)) );*/
		}
		
		public static void Effect( Mobile attacker, Mobile defender )
		{
			if( attacker == null || defender == null || defender.Weapon is Fists || !((BaseWeapon)defender.Weapon).Movable )
				return;
			
			if( Utility.RandomBool() && defender.Backpack != null )
			{
				defender.Backpack.DropItem( (BaseWeapon)defender.Weapon );
				defender.Emote( "*was disarmed by {0}*", attacker.Name );
			}
			else if ( Utility.RandomBool() && attacker.Backpack != null )
			{
				attacker.Backpack.DropItem( (BaseWeapon)defender.Weapon );
				defender.Emote( "*was disarmed by {0}*", attacker.Name );
			}
			else
			{
				defender.Emote( "*{0} weapon fell to the ground*", ((IKhaerosMobile)defender).GetPossessivePronoun() );
				((BaseWeapon)defender.Weapon).MoveToWorld( defender.Location, defender.Map );
			}
		}
		
		public Disarm()
		{
		}
		
		public Disarm( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Disarm) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.Disarm);
				return true;
			}
			
			return false;
		}
		
		/*public static void Initialize()
		{
			CommandSystem.Register( "Disarm", AccessLevel.Player, new CommandEventHandler( Disarm_OnCommand ) );
		}
		
		[Usage( "Disarm" )]
        [Description( "Allows the user to attempt to Disarm someone." )]
        private static void Disarm_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.Disarm) ) )
                m.ChangeManeuver( new Disarm( m.Feats.GetFeatLevel(FeatList.Disarm) ), FeatList.Disarm, "You prepare an attack to Disarm your enemy." );
        }*/
	}
}
