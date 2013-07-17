using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ThrowingMastery : BaseFeat
	{
		public override string Name{ get{ return "Throwing Mastery"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ThrowingMastery; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Throwing }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Finesse }; } }
		
		public override string FirstDescription{ get{ return "This skill allows the player to throw a variety of weapons " +
					"over a longer distance. [You can throw any weapon and gain +1 range]"; } }
		public override string SecondDescription{ get{ return "[+2 range]"; } }
		public override string ThirdDescription{ get{ return "[You can throw any weapon and gain +3 range. Additionally, you can " +
					"auto equip new daggers or throwing axes when you throw one of those. Use " +
					".CraftContainer to set up a bag as a craft container in your backpack, then " +
					"place your daggers or throwing axes inside for the auto-equip skillure.]"; } }

		public override string FirstCommand{ get{ return ".Throw"; } }
		public override string SecondCommand{ get{ return ".Throw"; } }
		public override string ThirdCommand{ get{ return ".Throw | .CraftContainer"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ThrowingMastery()); }
		
		public ThrowingMastery() {}
	}
}
