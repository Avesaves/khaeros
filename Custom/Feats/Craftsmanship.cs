using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Craftsmanship : BaseFeat
	{
		public override string Name{ get{ return "Craftsmanship"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Craftsmanship; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Craftsmanship }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.DurableCrafts, FeatList.Painter, FeatList.Blacksmithing, 
				FeatList.Potter, FeatList.GlassBlower, FeatList.Tinkering, FeatList.Fletching, FeatList.Tailoring, FeatList.Inscription,
				FeatList.Carpentry }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Craftsmanship skill, which will " +
					"improve your success/exceptional chance when crafting. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Craftsmanship()); }
		
		public Craftsmanship() {}
	}
}
