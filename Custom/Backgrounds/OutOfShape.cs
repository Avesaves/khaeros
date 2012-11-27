using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class OutOfShape : BaseBackground
	{
		public override int Cost{ get{ return -2000; } }
		public override string Name{ get{ return "Out of Shape"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.OutOfShape; } }
		public override string Description{ get{ return "This flaw will degrade the rate with which you regenerate Stamina."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new OutOfShape()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.Resilient, message );
		}
		
		public OutOfShape() {}
	}
}
