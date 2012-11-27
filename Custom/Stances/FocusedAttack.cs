using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class FocusedAttack : BaseStance
	{
		public override string Name{ get{ return "Focused Attack"; } }
		public override double DamageBonus{ get{ return 0.05; } }
		public override int AccuracyBonus{ get{ return 5; } }
		public override double SpeedBonus{ get{ return -0.03; } }
		public override int DefensiveBonus{ get{ return 0; } }
		public override string TurnedOnEmote{ get{ return "*goes on a more focused fighting stance*"; } }
		public override string TurnedOffEmote{ get{ return "*goes back to a regular fighting stance*"; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Armour{ get{ return true; } }
		
		public FocusedAttack( int featlevel ) : base( featlevel )
		{
		}
		
		public FocusedAttack() : this( 0 )
		{
		}
		
		public override bool CanUseThisStance( Mobile mob )
		{
			if( base.CanUseThisStance( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.FocusedAttack) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.FocusedAttack);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "FocusedAttack", AccessLevel.Player, new CommandEventHandler( FocusedAttack_OnCommand ) );
		}
		
		[Usage( "FocusedAttack" )]
        [Description( "Allows the user to change their combat stance to a more deadly one." )]
        private static void FocusedAttack_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.FocusedAttack) ) )
            	m.ChangeStance( new FocusedAttack( m.Feats.GetFeatLevel(FeatList.FocusedAttack) ) );
        }
	}
}
