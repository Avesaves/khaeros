using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Cleave : BaseFeat
	{
		public override string Name{ get{ return "Cleave"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Cleave; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.BruteStrength }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "When you land a killing blow, you automatically switch to an " +
					"opponent in range (if there is one) and instantly strike them with an unblockable attack, using your " +
					"weapon's best direction modifier to determine damage. [If you kill your target you get a free " +
					"swing at the next target. Damage is 50% of your normal attack. Mobiles on your ally list are not affected.]"; } }
		public override string SecondDescription{ get{ return "[Damage is 75% of your normal attack.]"; } }
		public override string ThirdDescription{ get{ return "[Damage is 100% of your normal attack.]"; } }
	
		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Cleave()); }
		
		public Cleave() {}
	}
}
