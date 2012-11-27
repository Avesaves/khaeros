using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Resilient : BaseBackground
	{
		public override int Cost{ get{ return 6000; } }
		public override string Name{ get{ return "Resilient"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Resilient; } }
		public override string Description{ get{ return "This merit will improve the rate with which you regenerate Stamina."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Resilient()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.OutOfShape, message );
		}
		
		public Resilient() {}
	}
}
