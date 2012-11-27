using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class GoodLooking : BaseBackground
	{
		public override int Cost{ get{ return 6000; } }
		public override string Name{ get{ return "Good-Looking"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.GoodLooking; } }
		public override string Description{ get{ return "This merit will change the general description of your " +
					"looks to Good-Looking (when writing your description, take this into consideration)."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new GoodLooking()); }

		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return !HasAnotherLookBackground( m, message );
		}
		
		public GoodLooking() {}
	}
}
