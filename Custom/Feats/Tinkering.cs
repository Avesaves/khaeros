using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Tinkering : BaseFeat
	{
		public override string Name{ get{ return "Tinkering"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Tinkering; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Tinkering }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Craftsmanship }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.JewelryCrafting, FeatList.Safecracking }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Tinkering skill, which will " +
					"allow you to craft a variety of tools and utensils. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Tinkering()); }
		
		public Tinkering() {}
	}
}
