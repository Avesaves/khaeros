using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class MountedCharge : BaseFeat
	{
		public override string Name{ get{ return "Mounted Charge"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.MountedCharge; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.MountedEndurance }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.MountedMomentum, FeatList.MountedCombat }; } }
		
		public override string FirstDescription{ get{ return "Your superior training in mounted combat allows you to charge for longer distances " +
					"and deliver powerful blows to your adversaries. [Increases the duration of charge by 0.5 seconds when used while mounted]"; } }
		public override string SecondDescription{ get{ return "[Increases the duration of charge by 1 second when used while mounted]"; } }
		public override string ThirdDescription{ get{ return "[Increases the duration of charge by 1.5 seconds when used while mounted]"; } }

		public override string FirstCommand{ get{ return ".Charge"; } }
		public override string SecondCommand{ get{ return ".Charge"; } }
		public override string ThirdCommand{ get{ return ".Charge"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new MountedCharge()); }
		
		public MountedCharge() {}
	}
}
