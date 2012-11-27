using System;
using Server;
using Server.Network;
using Server.Mobiles;
using Server.Misc;

namespace Server.BackgroundInfo
{	
	public class Unlucky : BaseBackground
	{
		public override int Cost{ get{ return -5000; } }
		public override string Name{ get{ return "Unlucky"; } }
		public override BackgroundList ListName{ get{ return BackgroundList.Unlucky; } }
        public override string Description { get { return "This flaw will have a faint negative impact on almost everything combat related."; } }
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Unlucky()); }
		
		public override bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
			return TestBackgroundForPurchase( m, BackgroundList.Lucky, message );
		}
		
		public Unlucky() {}
	}
}
