using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Tactics : BaseFeat
	{
		public override string Name{ get{ return "Tactics"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Tactics; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ SkillName.Tactics }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.DefensiveStance, FeatList.FlurryOfBlows, FeatList.Feint, 
					FeatList.BackToBack, FeatList.FocusedAttack, FeatList.Intimidate }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you some knowledge in the Tactics skill, thus improving " +
					"both your melee and ranged damage. [20% skill]"; } }
		public override string SecondDescription{ get{ return "[50% skill]"; } }
		public override string ThirdDescription{ get{ return "[100% skill]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Tactics()); }
		
		public Tactics() {}
	}
}
