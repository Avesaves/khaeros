using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using System.Collections;

namespace Server.Misc
{
	public class HailOfArrows : BaseCombatManeuver
	{
		public override int DamageBonus{ get{ return 0; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override bool Melee{ get{ return false; } }
		public override bool Ranged{ get{ return true; } }
		public override bool Throwing{ get{ return false; } }
		public override FeatList ListedName{ get{ return FeatList.HailOfArrows; } }
		
		public override void OnSwing( Mobile attacker, Mobile defender, bool Cleave )
		{
			if( !Cleave && BaseWeapon.CheckStam( attacker, FeatLevel, Cleave, false ) )
			{
                attacker.Emote( "*unleashes a hail of arrows upon {0} enemies*", ((IKhaerosMobile)attacker).GetPossessivePronoun() );
                Effect( attacker, defender, FeatLevel );
			}
		}
		
		public override void OnHit( Mobile attacker, Mobile defender )
		{
		}
		
		public static void Effect( Mobile attacker, Mobile defender, int featlevel )
		{
			IKhaerosMobile featuser = attacker as IKhaerosMobile;
			ArrayList list = new ArrayList();
			
            foreach( Mobile m in defender.GetMobilesInRange( featlevel + 2 ) )
            {
            	if( m == null || m.Deleted || m.Map != attacker.Map || !m.Alive || !attacker.CanSee( m ) || !attacker.CanBeHarmful( m ) || featuser.IsAllyOf( m ) )
                	continue;

            	if( !attacker.InRange( m, ((BaseWeapon)attacker.Weapon).MaxRange ) )
                	continue;

                if( m != attacker && Spells.SpellHelper.ValidIndirectTarget( attacker, m ) )
                {
                    if( attacker.InLOS( m ) )
                    	list.Add( m );
                }
            }
                
            for( int i = 0; i < Math.Min( list.Count, featlevel + 1 ); ++i )
            {
                int random = Utility.Random( list.Count );
                Mobile m = (Mobile)list[random];

                featuser.CleaveAttack = true;
                ((BaseWeapon)attacker.Weapon).OnSwing( attacker, m, 0.5, true );
                
            }
		}
		
		public HailOfArrows()
		{
		}
		
		public HailOfArrows( int featlevel ) : base( featlevel )
		{
		}
		
		public override bool CanUseThisManeuver( Mobile mob )
		{
			if( base.CanUseThisManeuver( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.HailOfArrows) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.HailOfArrows);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "HailOfArrows", AccessLevel.Player, new CommandEventHandler( HailOfArrows_OnCommand ) );
		}
		
		[Usage( "HailOfArrows" )]
        [Description( "Allows the user to attempt a Hail of Arrows." )]
        private static void HailOfArrows_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.HailOfArrows) ) )
                m.ChangeManeuver( new HailOfArrows( m.Feats.GetFeatLevel(FeatList.HailOfArrows) ), FeatList.HailOfArrows, "You prepare a Hail of Arrows." );
        }
	}
}
