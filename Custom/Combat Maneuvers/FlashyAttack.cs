using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
	public class FlashyAttack : BaseCombatManeuver
	{        
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.FlashyAttack; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave ) // this is never called.
		{
			/*if( BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
			{
                attacker.Emote( GetRandomEmote( attacker, defender ) );
			}*/
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
		}
		
		public static string GetRandomEmote( Mobile attacker, Mobile defender ) // this is still called by AttackTimer
        {
            //int number = Utility.Dice( 1, 6, 0 );
            string emote = "";
           /* string defrefpron = ((IKhaerosMobile)defender).GetReflexivePronoun();
            string defpospron = ((IKhaerosMobile)defender).GetPossessivePronoun();
            string atkpospron = ((IKhaerosMobile)attacker).GetPossessivePronoun();
            string defpos = ((IKhaerosMobile)defender).GetPossessive();*/

			emote = "*immediately follows up with another attack*";
            /*switch( number ) // this is no longer a 'trick' attack, but a follow-up attack, so we'd need new emotes if we want more variation
            {
                case 1: emote = "*sidesteps an attack from " + defender.Name + " and strikes " + defrefpron + " from the right flank*"; break;
                case 2: emote = "*feigns a circular slash, but instead thrusts at " + defender.Name + defpos + " chest*"; break;
                case 3: emote = "*swings " + atkpospron + " weapon in front of " + defender.Name + ", confusing " + defrefpron + "*"; break;
                case 4: emote = "*parries an attack from " + defender.Name + " and uses the momentum of the blow to hit " + defrefpron + "*"; break;
                case 5: emote = "*uses " + atkpospron + " off-hand to open " + defender.Name + defpos + " guard and strike " + defrefpron + "*"; break;
                case 6: emote = "*throws a pebble at " + defender.Name + " to distract " + defrefpron + " and strike*"; break;
            }*/

            return emote;
        }
		
		public FlashyAttack()
		{
		}
		
		public FlashyAttack( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.FlashyAttack) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.FlashyAttack);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "FlashyAttack", AccessLevel.Player, new CommandEventHandler( FlashyAttack_OnCommand ) );
		}
		
		[Usage( "FlashyAttack" )]
        [Description( "Allows the user to attempt a Flashy Attack." )]
        private static void FlashyAttack_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.FlashyAttack) ) )
                m.ChangeManeuver( new FlashyAttack( m.Feats.GetFeatLevel(FeatList.FlashyAttack) ), FeatList.FlashyAttack, "You prepare a Flashy Attack." );
        }
	}
}
