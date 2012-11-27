using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class NonLethalTraps : BaseFeat
	{
		public override string Name{ get{ return "Non-Lethal Traps"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.NonLethalTraps; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.DisarmTraps }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.RopeTrick }; } }
		
		public override string FirstDescription{ get{ return "You have learned the art of dismounting people with bolas and stopping them " +
					"in their tracks with foot traps. [Chance to dismount is based on skill level vs riding skill (defender's riding backgrounds " +
					"apply). Foot traps last 10 minutes and to escape one must make a strength check vs your Arm/Disarm Traps Skill]"; } }
		public override string SecondDescription{ get{ return "[Improved chance to hit with bolas. Foot traps last 20 minutes.]"; } }
		public override string ThirdDescription{ get{ return "[Improved chance to hit with bolas. Foot traps last 30 minutes.]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new NonLethalTraps()); }
		
		public NonLethalTraps() {}
	}
}
