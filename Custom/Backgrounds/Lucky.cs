using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Lucky : BaseBackground
	{
		public override int Cost{ get{ return 10000; } }
		public override string Name{ get{ return "Lucky"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Lucky; } }
		public override string Description{ get{ return "This merit will have a faint positive impact on almost everything combat related."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Lucky()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.Unlucky, message );
		}
		
		public Lucky() {}
	}
}
