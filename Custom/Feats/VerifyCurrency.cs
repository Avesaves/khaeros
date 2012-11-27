using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class VerifyCurrency : BaseFeat
	{
		public override string Name{ get{ return "Verify Currency"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.VerifyCurrency; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Appraisal }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Counterfeiting }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Having spent some time in the trade business you have the chance to discern " +
					"fake coins from real ones. [5% chance]"; } }
		public override string SecondDescription{ get{ return "[10% chance]"; } }
		public override string ThirdDescription{ get{ return "[100% chance]"; } }

		public override string FirstCommand{ get{ return ".BiteCoin"; } }
		public override string SecondCommand{ get{ return ".BiteCoin"; } }
		public override string ThirdCommand{ get{ return ".BiteCoin"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new VerifyCurrency()); }
		
		public VerifyCurrency() {}
	}
}
