using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class FocusedAttack : BaseFeat
	{
		public override string Name{ get{ return "Focused Attack"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.FocusedAttack; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Tactics }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You take your time to aim a blow, sacrificing speed for damage " +
					"and accuracy. [-3% speed +5% Hit Chance +5% dmg]"; } }
		public override string SecondDescription{ get{ return "[-6% speed +10% Hit Chance +10% dmg]"; } }
		public override string ThirdDescription{ get{ return "[-9% speed +15% Hit Chance +15% dmg]"; } }

		public override string FirstCommand{ get{ return ".FocusedAttack"; } }
		public override string SecondCommand{ get{ return ".FocusedAttack"; } }
		public override string ThirdCommand{ get{ return ".FocusedAttack"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new FocusedAttack()); }
		
		public FocusedAttack() {}
	}
}
