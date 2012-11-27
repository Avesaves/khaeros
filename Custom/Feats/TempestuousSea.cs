using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class TempestuousSea : BaseFeat
	{
		public override string Name{ get{ return "Tempestuous Sea"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.TempestuousSea; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Buildup }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Staggering about like a drunken seaman on deck in a storm, " +
					"not your enemies nor yourself can predict your moves. [Random Bonuses Between -15% and +15% to all combat statistics.] " + 
                    "Note: These bonuses change each time you enter the Tempestuous Sea."; } }
        public override string SecondDescription { get { return "[Random Bonuses Between -30% and +30% to all combat statistics.]"; } }
        public override string ThirdDescription { get { return "[Random Bonuses Between -45% and +45% to all combat statistics.]"; } }

		public override string FirstCommand{ get{ return ".TempestuousSea"; } }
		public override string SecondCommand{ get{ return ".TempestuousSea"; } }
		public override string ThirdCommand{ get{ return ".TempestuousSea"; } }

		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new TempestuousSea()); }
		
		public TempestuousSea() {}
	}
}
