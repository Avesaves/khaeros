using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Backstab : BaseFeat
	{
		public override string Name{ get{ return "Backstab"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Backstab; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.EnhancedStealth }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "A vicious attack from the back flank or back of the victim, dealing large amounts " +
					"of damage. [This attack replaces your weapon's raw damage by 15 for one attack. You must be hidden and behind the victim's " +
					"back, or at the back flank to succeed. Weapon damage bonuses still apply. This attack cannot be used with polearms, " +
					"blunt or ranged weapons.]"; } }
		public override string SecondDescription{ get{ return "[Increases damage to 20]"; } }
		public override string ThirdDescription{ get{ return "[Increases damage to 25]"; } }

		public override string FirstCommand{ get{ return ".Backstab"; } }
		public override string SecondCommand{ get{ return ".Backstab"; } }
		public override string ThirdCommand{ get{ return ".Backstab"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Backstab()); }
		
		public Backstab() {}
	}
}
