using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class QuickTravelingShot : BaseFeat
	{
		public override string Name{ get{ return "Quick Traveling Shot"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.QuickTravelingShot; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.TravelingShot }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You are able to fire off arrows faster while moving. Only works with bows. " +
					"[20% speed penalty]"; } }
		public override string SecondDescription{ get{ return "[10% speed penalty]"; } }
		public override string ThirdDescription{ get{ return "[no speed penalty]"; } }

		public override string FirstCommand{ get{ return ".TravelingShot"; } }
		public override string SecondCommand{ get{ return ".TravelingShot"; } }
		public override string ThirdCommand{ get{ return ".TravelingShot"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new QuickTravelingShot()); }
		
		public QuickTravelingShot() {}
	}
}
