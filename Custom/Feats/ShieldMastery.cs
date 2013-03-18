using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ShieldMastery : BaseFeat
	{
		public override string Name{ get{ return "Shield Mastery"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ShieldMastery; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.ShieldBash }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.DeflectProjectiles }; } }
		
		public override string FirstDescription{ get{ return "Having adopted a fighting style that includes shields you are better able to " +
					"defend yourself. Increases attack cooldown for the opponent when you parry their attack with a shield. [0.125s cooldown " +
					"increase]"; } }
		public override string SecondDescription{ get{ return "[0.25s cooldown increase]"; } }
		public override string ThirdDescription{ get{ return "[0.5s cooldown increase]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ShieldMastery()); }
		
		public ShieldMastery() {}
	}
}
