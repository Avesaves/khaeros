using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Attractive : BaseBackground
	{
		public override int Cost{ get{ return 3000; } }
		public override string Name{ get{ return "Attractive"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Attractive; } }
		public override string Description{ get{ return "This merit will change the general description of your " +
					"looks to Attractive (when writing your description, take this into consideration)."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Attractive()); }

		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return !HasAnotherLookBackground( m, message );
		}
		
		public Attractive() {}
	}
}
