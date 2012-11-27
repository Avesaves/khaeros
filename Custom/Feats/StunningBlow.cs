using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class StunningBlow : BaseFeat
	{
		public override string Name{ get{ return "Stunning Blow"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.StunningBlow; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.MartialOffence }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Having extensively learned about weak points, you know where to hit in order " +
					"to temporarily paralyze your opponent. [On hit, target is paralyzed for 1.5 seconds]"; } }
		public override string SecondDescription{ get{ return "[On hit, target is paralyzed for 3 seconds]"; } }
		public override string ThirdDescription{ get{ return "[On hit, target is paralyzed for 4.5 seconds]"; } }

		public override string FirstCommand{ get{ return ".StunningBlow"; } }
		public override string SecondCommand{ get{ return ".StunningBlow"; } }
		public override string ThirdCommand{ get{ return ".StunningBlow"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new StunningBlow()); }
		
		public StunningBlow() {}
	}
}
