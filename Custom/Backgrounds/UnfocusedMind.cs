using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class UnfocusedMind : BaseBackground
	{
		public override int Cost{ get{ return -2000; } }
		public override string Name{ get{ return "Unfocused Mind"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.UnfocusedMind; } }
		public override string Description{ get{ return "This flaw will degrade the rate with which you regenerate Mana."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new UnfocusedMind()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.FocusedMind, message );
		}
		
		public UnfocusedMind() {}
	}
}
