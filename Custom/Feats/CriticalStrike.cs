using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class CriticalStrike : BaseFeat
	{
		public override string Name{ get{ return "Critical Strike"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.CriticalStrike; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Axemanship }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You know where to hit with your weapon to inflict massive " +
					"damage. [7 damage bonus]"; } }
		public override string SecondDescription{ get{ return "[14 damage bonus]"; } }
		public override string ThirdDescription{ get{ return "[21 damage bonus]"; } }

		public override string FirstCommand{ get{ return ".CriticalStrike"; } }
		public override string SecondCommand{ get{ return ".CriticalStrike"; } }
		public override string ThirdCommand{ get{ return ".CriticalStrike"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new CriticalStrike()); }
		
		public CriticalStrike() {}
	}
}
