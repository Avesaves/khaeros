using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class ThunderingHooves : BaseStance
	{
		public override string Name{ get{ return "Thundering Hooves"; } }
		public override bool MartialArtistStance{ get{ return true; } }
		public override string TurnedOnEmote{ get{ return "*holds their arms and fists like a shield, crouching*"; } }
		public override string TurnedOffEmote{ get{ return "*goes back to a regular fighting stance*"; } }

        public override int AccuracyBonus { get { return -5; } }
        public override double DamageBonus { get { return -5.00; } }
        public override double SpeedBonus { get { return 0; } }
        public override int DefensiveBonus { get { return 15; } }

		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Armour{ get{ return false; } }
		
		public ThunderingHooves( int featlevel ) : base( featlevel )
		{
		}
		
		public ThunderingHooves() : this( 0 )
		{
		}
		
		public override bool CanUseThisStance( Mobile mob )
		{
			if( base.CanUseThisStance( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.ThunderingHooves) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.ThunderingHooves);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "ThunderingHooves", AccessLevel.Player, new CommandEventHandler( ThunderingHooves_OnCommand ) );
		}
		
		[Usage( "ThunderingHooves" )]
        [Description( "Allows the user to change their combat stance to Northerns' racial style." )]
        private static void ThunderingHooves_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.ThunderingHooves) ) )
            	m.ChangeStance( new ThunderingHooves( m.Feats.GetFeatLevel(FeatList.ThunderingHooves) ) );
        }
	}
}
