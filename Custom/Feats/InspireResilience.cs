using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class InspireResilience : BaseFeat
	{
		public override string Name{ get{ return "Inspire Resilience"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.InspireResilience; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Faith }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "With this skill, you will be able to inspire your allies to defend themselves " +
					"better and withstand more punishment. [Damage reduction 1 and +5% Defence Chance]"; } }
		public override string SecondDescription{ get{ return "Reduced stamina cost, better bonuses. [Damage reduction 2 and +10% Defence Chance]"; } }
		public override string ThirdDescription{ get{ return "Reduced stamina cost, better bonuses. [Damage reduction 3 and +15% Defence Chance]"; } }

		public override string FirstCommand{ get{ return ".InspireResilience | .CancelCommand"; } }
		public override string SecondCommand{ get{ return ".InspireResilience | .CancelCommand"; } }
		public override string ThirdCommand{ get{ return ".InspireResilience | .CancelCommand"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new InspireResilience()); }
		
		public InspireResilience() {}
	}
}
