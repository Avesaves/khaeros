using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class GemHarvesting : BaseFeat
	{
		public override string Name{ get{ return "Gem Harvesting"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.GemHarvesting; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.AdvancedMining }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You are able to find common gems while gathering resources. [Miners can harvest " +
					"citrines, tourmalines and amethysts. Lumberjacks can harvest amber]"; } }
		public override string SecondDescription{ get{ return "[Miners can harvest rubies, emeralds and sapphires]"; } }
		public override string ThirdDescription{ get{ return "[Miners can harvest star sapphires, cinnabar and diamonds]"; } }

		public override string FirstCommand{ get{ return ".gemharvesting"; } }
		public override string SecondCommand{ get{ return ".gemharvesting"; } }
		public override string ThirdCommand{ get{ return ".gemharvesting"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new GemHarvesting()); }
		
		public GemHarvesting() {}
	}
}
