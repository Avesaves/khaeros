using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class SearingBreath : BaseStance
	{
		public override string Name{ get{ return "Searing Breath"; } }
		public override bool MartialArtistStance{ get{ return true; } }

        public override double DamageBonus { get { return 7.50; } }
		public override int AccuracyBonus{ get{ return 0; } }
        public override double SpeedBonus{ get{ return -5.0; } }
        public override int DefensiveBonus { get { return -5; } }

		public override string TurnedOnEmote{ get{ return "*extends arms, hands searching for a foe to grasp in a brutal embrace*"; } }
		public override string TurnedOffEmote{ get{ return "*goes back to a regular fighting stance*"; } }

		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Armour{ get{ return false; } }

		
		public SearingBreath( int featlevel ) : base( featlevel )
		{
		}
		
		public SearingBreath() : this( 0 )
		{
		}
		
		public override bool CanUseThisStance( Mobile mob )
		{
			if( base.CanUseThisStance( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.SearingBreath) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.SearingBreath);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "SearingBreath", AccessLevel.Player, new CommandEventHandler( SearingBreath_OnCommand ) );
		}
		
		[Usage( "SearingBreath" )]
        [Description( "Allows the user to change their combat stance to Mhorduls' racial style." )]
        private static void SearingBreath_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.SearingBreath) ) )
            	m.ChangeStance( new SearingBreath( m.Feats.GetFeatLevel(FeatList.SearingBreath) ) );
        }
	}
}
