using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Invocation : BaseFeat
	{
		public override string Name{ get{ return "Invocation"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Invocation; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Invocation }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ FeatList.CureFamine }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Faith, FeatList.Magery, FeatList.CureFamine }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Invocation skill, which will " +
					"give you access to more magic powers, lower your enemies' chance of resisting your spells, and also grant you the Cure Famine spell. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return ".CureFamine | .CureFamine 1"; } }
		public override string SecondCommand{ get{ return ".CureFamine | .CureFamine 2"; } }
		public override string ThirdCommand{ get{ return ".CureFamine | .CureFamine 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Invocation()); }
		
		public Invocation() {}
	}
}
