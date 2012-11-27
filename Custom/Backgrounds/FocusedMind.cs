using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class FocusedMind : BaseBackground
	{
		public override int Cost{ get{ return 6000; } }
		public override string Name{ get{ return "Focused Mind"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.FocusedMind; } }
		public override string Description{ get{ return "This merit will improve the rate with which you regenerate Mana."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new FocusedMind()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.UnfocusedMind, message );
		}
		
		public FocusedMind() {}
	}
}
