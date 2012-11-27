using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Lockpicking : BaseFeat
	{
		public override string Name{ get{ return "Lockpicking"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Lockpicking; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Lockpicking }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Stealing }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Locksmith }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Lockpicking skill, which will " +
					"allow you to attempt to open locked containers. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Lockpicking()); }
		
		public Lockpicking() {}
	}
}
