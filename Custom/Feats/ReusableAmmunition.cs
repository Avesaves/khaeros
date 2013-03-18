using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ReusableAmmunition : BaseFeat
	{
		public override string Name{ get{ return "Reusable Ammunition"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ReusableAmmunition; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Archery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You know how to fire off your arrows/bolts in a way to " +
					"increase their chance of not breaking upon impact. [+20% chance of reclaiming ammunition] <br>"; } }
		public override string SecondDescription{ get{ return "[+40% chance of reclaiming ammunition]"; } }
		public override string ThirdDescription{ get{ return "[+60% chance of reclaiming ammunition]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ReusableAmmunition()); }
		
		public ReusableAmmunition() {}
	}
}
