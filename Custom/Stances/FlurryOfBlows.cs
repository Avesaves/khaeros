using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
	public class FlurryOfBlows : BaseStance
	{
		public override string Name{ get{ return "Flurry Of Blows"; } }
		public override double DamageBonus{ get{ return -0.03; } }
		public override int AccuracyBonus{ get{ return 5; } }
		public override double SpeedBonus{ get{ return 0.05; } }
		public override int DefensiveBonus{ get{ return 0; } }
		public override string TurnedOnEmote{ get{ return "*goes on a more rapid fighting stance*"; } }
		public override string TurnedOffEmote{ get{ return "*goes back to a regular fighting stance*"; } }
		public override bool Melee{ get{ return true; } }
		public override bool Ranged{ get{ return false; } }
		public override bool Armour{ get{ return true; } }
		
		public FlurryOfBlows( int featlevel ) : base( featlevel )
		{
		}
		
		public FlurryOfBlows() : this( 0 )
		{
		}
		
		public override bool CanUseThisStance( Mobile mob )
		{
			if( base.CanUseThisStance( mob ) && ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.FlurryOfBlows) > 0 )
			{
				this.FeatLevel = ((IKhaerosMobile)mob).Feats.GetFeatLevel(FeatList.FlurryOfBlows);
				return true;
			}
			
			return false;
		}
	
		public static void Initialize()
		{
			CommandSystem.Register( "FlurryOfBlows", AccessLevel.Player, new CommandEventHandler( FlurryOfBlows_OnCommand ) );
		}
		
		[Usage( "FlurryOfBlows" )]
		[Description( "Allows the user to change their combat stance to a faster one." )]
		private static void FlurryOfBlows_OnCommand( CommandEventArgs e )
		{
		    PlayerMobile m = e.Mobile as PlayerMobile;
		
		    if( m.CanPerformAttack( m.Feats.GetFeatLevel(FeatList.FlurryOfBlows) ) )
		    	m.ChangeStance( new FlurryOfBlows( m.Feats.GetFeatLevel(FeatList.FlurryOfBlows) ) );
		}
	}
}
