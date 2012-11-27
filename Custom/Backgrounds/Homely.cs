using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Homely : BaseBackground
	{
		public override int Cost{ get{ return -1000; } }
		public override string Name{ get{ return "Homely"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Homely; } }
		public override string Description{ get{ return "This flaw will change the general description of your " +
					"looks to Homely (when writing your description, take this into consideration)."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Homely()); }

		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return !HasAnotherLookBackground( m, message );
		}
		
		public Homely() {}
	}
}
