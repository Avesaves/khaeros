using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class TravelingShot : BaseFeat
	{
		public override string Name{ get{ return "Traveling Shot"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.TravelingShot; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.BowMastery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.QuickTravelingShot }; } }
		
		public override string FirstDescription{ get{ return "You are able to fire off your bow while on the move. [50% speed penalty]"; } }
		public override string SecondDescription{ get{ return "[40% speed penalty]"; } }
		public override string ThirdDescription{ get{ return "[30% speed penalty]"; } }

		public override string FirstCommand{ get{ return ".TravelingShot"; } }
		public override string SecondCommand{ get{ return ".TravelingShot"; } }
		public override string ThirdCommand{ get{ return ".TravelingShot"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new TravelingShot()); }
		
		public TravelingShot() {}
	}
}
