using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Farming : BaseFeat
	{
		public override string Name{ get{ return "Farming"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Farming; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Peacemaking }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.EnhancedHarvesting }; } }
		
		public override string FirstDescription{ get{ return "This skill allows the player to harvest simple crops."; } }
		public override string SecondDescription{ get{ return "This skill allows the player to harvest less simple crops."; } }
		public override string ThirdDescription{ get{ return "This skill allows the player to harvest all crops."; } }
	
		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Farming()); }
		
		public Farming() {}
	}
}
