using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class SilentHowl : BaseStance
	{
		public override string Name{ get{ return "Silent Howl"; } }
		public override bool MartialArtistStance{ get{ return true; } }
		public override string TurnedOnEmote{ get{ return "*moves with balance and precision, ready to strike or defend*"; } }
		public override string TurnedOffEmote{ get{ return "*goes back to a regular fighting stance*"; } }

        public override int AccuracyBonus{ get{ return 3; } }
		public override double DamageBonus{ get{ return 1.5; } }
		public override double SpeedBonus{ get{ return 1.5; } }
		public override int DefensiveBonus{ get{ return 3; } }

		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Armour{ get{ return false; } }
		
		public SilentHowl( int featlevel ) : base( featlevel )
		{
		}
		
		public SilentHowl() : this( 0 )
		{
		}
		
		public override bool CanUseThisStance( Mobile mob )
		{
			if( base.CanUseThisStance( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.SilentHowl) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.SilentHowl);
				return true;
			}
			
			return false;
		}
		
		public static void Initialize()
		{
			CommandSystem.Register( "SilentHowl", AccessLevel.Player, new CommandEventHandler( SilentHowl_OnCommand ) );
		}
		
		[Usage( "SilentHowl" )]
        [Description( "Allows the user to change their combat stance to Southerns' racial style." )]
        private static void SilentHowl_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.SilentHowl) ) )
            	m.ChangeStance( new SilentHowl( m.Feats.GetFeatLevel(FeatList.SilentHowl) ) );
        }
	}
}
