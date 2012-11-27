using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class FocusedShot : BaseStance
	{
		public override string Name{ get{ return "Focused Shot"; } }
		public override double DamageBonus{ get{ return 0.05; } }
		public override int AccuracyBonus{ get{ return 5; } }
		public override double SpeedBonus{ get{ return -0.03; } }
		public override int DefensiveBonus{ get{ return 0; } }
		public override string TurnedOnEmote{ get{ return "*goes on a more focused fighting stance*"; } }
		public override string TurnedOffEmote{ get{ return "*goes back to a regular fighting stance*"; } }
		public override bool Melee{ get{ return false; } }
		public override bool Ranged{ get{ return true; } }
		public override bool Armour{ get{ return true; } }
		
		public FocusedShot( int featlevel ) : base( featlevel )
		{
		}
		
		public FocusedShot() : this( 0 )
		{
		}
		
		public override bool CanUseThisStance( Mobile mob )
		{
			if( base.CanUseThisStance( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.FocusedShot) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.FocusedShot);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "FocusedShot", AccessLevel.Player, new CommandEventHandler( FocusedShot_OnCommand ) );
		}
		
		[Usage( "FocusedShot" )]
        [Description( "Allows the user to change their combat stance to a more deadly one." )]
        private static void FocusedShot_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.FocusedShot) ) )
            	m.ChangeStance( new FocusedShot( m.Feats.GetFeatLevel(FeatList.FocusedShot) ) );
        }
	}
}
