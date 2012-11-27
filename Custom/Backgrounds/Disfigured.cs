using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Disfigured : BaseBackground
	{
		public override int Cost{ get{ return -3000; } }
		public override string Name{ get{ return "Disfigured"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Disfigured; } }
		public override string Description{ get{ return "This flaw will change the general description of your " +
					"looks to Disfigured (when writing your description, take this into consideration)."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Disfigured()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return !HasAnotherLookBackground( m, message );
		}
		
		public Disfigured() {}
	}
}
