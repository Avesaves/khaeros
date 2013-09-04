using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class SwipingClaws : BaseStance
	{
		public override string Name{ get{ return "Swiping Claws"; } }
		public override bool MartialArtistStance{ get{ return true; } }
		public override string TurnedOnEmote{ get{ return "*moves with athleticism and grace, ready to strike with grasping hands*"; } }
		public override string TurnedOffEmote{ get{ return "*goes back to a regular fighting stance*"; } }

		public override int AccuracyBonus{ get{ return -10; } }
		public override double DamageBonus{ get{ return 0.0; } }
		public override double SpeedBonus{ get{ return 7.50; } }
		public override int DefensiveBonus{ get{ return -5; } }

		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Armour{ get{ return false; } }
		
		public SwipingClaws( int featlevel ) : base( featlevel )
		{
		}
		
		public SwipingClaws() : this( 0 )
		{
		}
		
		public override bool CanUseThisStance( Mobile mob )
		{
			if( base.CanUseThisStance( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.SwipingClaws) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.SwipingClaws);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "SwipingClaws", AccessLevel.Player, new CommandEventHandler( SwipingClaws_OnCommand ) );
		}
		
		[Usage( "SwipingClaws" )]
        [Description( "Allows the user to change their combat stance to Westerns' racial style." )]
        private static void SwipingClaws_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.SwipingClaws) ) )
            	m.ChangeStance( new SwipingClaws( m.Feats.GetFeatLevel(FeatList.SwipingClaws) ) );
        }
	}
}
