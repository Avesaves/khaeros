using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Dismount : BaseFeat
	{
		public override string Name{ get{ return "Dismount"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Dismount; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.MartialOffence }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You know how to grab your opponent and dismount them from their steed. [Chance " +
					"to hit is the difference between your weapon skill and the target's riding skill (apt and inept rider add +/-10). " +
					"Attacker has a 20% bonus. Minimum 10% hit chance + bonus/penalty from apt/inept rider]"; } }
		public override string SecondDescription{ get{ return "[20% bonus]"; } }
		public override string ThirdDescription{ get{ return "[30% bonus]"; } }

		public override string FirstCommand{ get{ return ".Dismount"; } }
		public override string SecondCommand{ get{ return ".Dismount"; } }
		public override string ThirdCommand{ get{ return ".Dismount"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Dismount()); }
		
		public Dismount() {}
	}
}
