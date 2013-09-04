using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class VenomousWay : BaseStance
	{
		public override string Name{ get{ return "Venomous Way"; } }
		public override bool MartialArtistStance{ get{ return true; } }
		public override string TurnedOnEmote{ get{ return "*goes slack and sways, arms weaving rhythmically*"; } }
		public override string TurnedOffEmote{ get{ return "*goes back to a regular fighting stance*"; } }

		public override int AccuracyBonus{ get{ return 15; } }
		public override double DamageBonus{ get{ return 0.00; } }
        public override double SpeedBonus { get { return 0; } }
		public override int DefensiveBonus{ get{ return -15; } }

		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Armour{ get{ return false; } }

		
		public VenomousWay( int featlevel ) : base( featlevel )
		{
		}
		
		public VenomousWay() : this( 0 )
		{
		}
		
		public override bool CanUseThisStance( Mobile mob )
		{
			if( base.CanUseThisStance( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.VenomousWay) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.VenomousWay);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "VenomousWay", AccessLevel.Player, new CommandEventHandler( VenomousWay_OnCommand ) );
		}
		
		[Usage( "VenomousWay" )]
        [Description( "Allows the user to change their combat stance to Northerns' racial style." )]
        private static void VenomousWay_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.VenomousWay) ) )
            	m.ChangeStance( new VenomousWay( m.Feats.GetFeatLevel(FeatList.VenomousWay) ) );
        }
	}
}
