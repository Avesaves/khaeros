using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class DrumsOfWar : BaseFeat
	{
		public override string Name{ get{ return "Drums of War"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.DrumsOfWar; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "With this skill, you will be able to play your drums so loud that everyone in " +
					"the region you are in will know where it is coming from. [50 tiles radius]"; } }
		public override string SecondDescription{ get{ return "[100 tiles radius]"; } }
		public override string ThirdDescription{ get{ return "[150 tiles radius]"; } }
	
		public override string FirstCommand{ get{ return ".DrumsOfWar"; } }
		public override string SecondCommand{ get{ return ".DrumsOfWar"; } }
		public override string ThirdCommand{ get{ return ".DrumsOfWar"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new DrumsOfWar()); }
		
		public DrumsOfWar() {}
	}
}
