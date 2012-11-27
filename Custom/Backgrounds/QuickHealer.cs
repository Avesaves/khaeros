using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class QuickHealer : BaseBackground
	{
		public override int Cost{ get{ return 6000; } }
		public override string Name{ get{ return "Quick Healer"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.QuickHealer; } }
		public override string Description{ get{ return "This merit will improve the rate with which you regenerate Hit Points."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new QuickHealer()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.SlowHealer, message );
		}
		
		public QuickHealer() {}
	}
}
