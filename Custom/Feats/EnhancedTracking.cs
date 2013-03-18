using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class EnhancedTracking : BaseFeat
	{
		public override string Name{ get{ return "Enhanced Tracking"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.EnhancedTracking; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Tracking }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You are an expert tracker and able to see subtle evidence that would " +
					"be unnoticeable to others. [+25 tiles to tracking range]"; } }
		public override string SecondDescription{ get{ return "[+50 tiles to tracking range]"; } }
		public override string ThirdDescription{ get{ return "[+75 tiles to tracking range]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new EnhancedTracking()); }
		
		public EnhancedTracking() {}
	}
}
