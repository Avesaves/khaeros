using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class MountedMomentum : BaseFeat
	{
		public override string Name{ get{ return "Mounted Momentum"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.MountedMomentum; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.MountedCharge }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "When mounted and charging your opponent at great distances, " +
					"you build up such momentum that if your charge is parried, it will cause the opponent to be thrown to " +
					"the ground for a short amount of time. Opponent remains on the ground for two seconds. Opponent must be " +
					"on foot for this skill to work. [16 tiles of charging required]"; } }
		public override string SecondDescription{ get{ return "[13 tiles of charging required]"; } }
		public override string ThirdDescription{ get{ return "[10 tiles of charging required]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new MountedMomentum()); }
		
		public MountedMomentum() {}
	}
}
