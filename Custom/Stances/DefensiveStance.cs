using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class DefensiveStance : BaseStance
	{
		public override string Name{ get{ return "Defensive Stance"; } }
		public override double DamageBonus{ get{ return -0.015; } }
		public override int AccuracyBonus{ get{ return 0; } }
		public override double SpeedBonus{ get{ return -0.015; } }
		public override int DefensiveBonus{ get{ return 10; } }
		public override string TurnedOnEmote{ get{ return "*goes on a more defensive fighting stance*"; } }
		public override string TurnedOffEmote{ get{ return "*goes back to a regular fighting stance*"; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Armour{ get{ return true; } }
		
		public DefensiveStance( int featlevel ) : base( featlevel )
		{
		}
		
		public DefensiveStance() : this( 0 )
		{
		}
		
		public override bool CanUseThisStance( Mobile mob )
		{
			if( base.CanUseThisStance( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.DefensiveStance) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.DefensiveStance);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "DefensiveStance", AccessLevel.Player, new CommandEventHandler( DefensiveStance_OnCommand ) );
		}
		
		[Usage( "DefensiveStance" )]
        [Description( "Allows the user to change their combat stance to a more defensive one." )]
        private static void DefensiveStance_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.DefensiveStance) ) )
            	m.ChangeStance( new DefensiveStance( m.Feats.GetFeatLevel(FeatList.DefensiveStance) ) );
        }
	}
}
