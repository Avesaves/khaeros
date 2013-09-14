using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class PolearmsMastery : BaseFeat
	{
		public override string Name{ get{ return "Polearms Mastery"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.PolearmsMastery; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Polearms }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Unhorse }; } }
		
		public override string FirstDescription{ get{ return "Having specialized in polearms, you are now able to prepare " +
					"them faster than normal. Reduces the speed penalty for polearms. [Reduces penalty from 50% to 40%]"; } }
		public override string SecondDescription{ get{ return "[Reduces penalty from 40% to 30%]"; } }
		public override string ThirdDescription{ get{ return "[Reduces penalty from 30% to 20%]"; } }
		
		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new PolearmsMastery()); }
		
		public PolearmsMastery() {}
	}
}
