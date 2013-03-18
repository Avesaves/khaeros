using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Archery : BaseFeat
	{
		public override string Name{ get{ return "Archery"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Archery; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Archery }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.ReusableAmmunition, FeatList.CrossbowMastery, FeatList.BowMastery }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Archery skill, which will " +
					"enhance the speed of your attack animation when fighting with ranged weapons. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Archery()); }
		
		public Archery() {}
	}
}
