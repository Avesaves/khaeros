using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Alchemy : BaseFeat
	{
		public override string Name{ get{ return "Alchemy"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Alchemy; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Alchemy }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.LowerSideEffects, FeatList.OilMaking, FeatList.BlackPowder,
		FeatList.PoisonResistance }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Alchemy skill, which will " +
					"allow you to craft a variety of potions, oils and bombs by using your alchemy kit. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Alchemy()); }
		
		public Alchemy() {}
	}
}
