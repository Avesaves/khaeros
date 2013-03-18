using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class MountedArchery : BaseFeat
	{
		public override string Name{ get{ return "Mounted Archery"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.MountedArchery; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Riding }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You are proficient with shooting a bow from horseback. [50% " +
					"penalty to hit while on horseback]"; } }
		public override string SecondDescription{ get{ return "[25% penalty to hit while on horseback]"; } }
		public override string ThirdDescription{ get{ return "[no penalty to hit while on horseback]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new MountedArchery()); }
		
		public MountedArchery() {}
	}
}
