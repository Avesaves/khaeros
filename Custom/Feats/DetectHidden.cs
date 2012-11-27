using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class DetectHidden : BaseFeat
	{
		public override string Name{ get{ return "Detect Hidden"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.DetectHidden; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.DetectHidden }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Hiding, FeatList.Tracking }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Detect Hidden skill, which will " +
					"allow you to attempt to reveal hidden players at a location. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new DetectHidden()); }
		
		public DetectHidden() {}
	}
}
