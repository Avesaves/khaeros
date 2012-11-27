using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class HoldPerson : BaseFeat
	{
		public override string Name{ get{ return "Hold Person"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.HoldPerson; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Curse }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This spell allows you to paralyze a creature for a few seconds. [Paralyzes the " +
					"target for 2 seconds, costs 20 mana]"; } }
		public override string SecondDescription{ get{ return "[Paralyzes the target for 4 seconds, costs 40 mana]"; } }
		public override string ThirdDescription{ get{ return "[Paralyzes the target for 6 seconds, costs 60 mana]"; } }
		
		public override string FirstCommand{ get{ return ".HoldPerson | .HoldPerson 1"; } }
		public override string SecondCommand{ get{ return ".HoldPerson | .HoldPerson 2"; } }
		public override string ThirdCommand{ get{ return ".HoldPerson | .HoldPerson 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new HoldPerson()); }
		
		public HoldPerson() {}
	}
}
