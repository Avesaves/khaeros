using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class SlowHealer : BaseBackground
	{
		public override int Cost{ get{ return -2000; } }
		public override string Name{ get{ return "Slow Healer"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.SlowHealer; } }
		public override string Description{ get{ return "This flaw will degrade the rate with which you regenerate Hit Points."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new SlowHealer()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.QuickHealer, message );
		}
		
		public SlowHealer() {}
	}
}
