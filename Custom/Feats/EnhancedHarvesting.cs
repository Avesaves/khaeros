using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class EnhancedHarvesting : BaseFeat
	{
		public override string Name{ get{ return "Enhanced Harvesting"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.EnhancedHarvesting; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Farming }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Linen }; } }
		
		public override string FirstDescription{ get{ return "This skill allows the player to harvest material with great efficiency. " +
					"[You harvest 1 unit more with each attempt]"; } }
		public override string SecondDescription{ get{ return "[You harvest 2 units more with each attempt]"; } }
		public override string ThirdDescription{ get{ return "[You harvest 3 units more with each attempt]"; } }
	
		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new EnhancedHarvesting()); }
		
		public EnhancedHarvesting() {}
	}
}
