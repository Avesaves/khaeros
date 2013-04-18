using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Locksmith : BaseFeat
	{
		public override string Name{ get{ return "Locksmith"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Locksmith; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Lockpicking }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Safecracking }; } }
		
		public override string FirstDescription{ get{ return "This skill will improve your knowledge of lockpicking to such an extent that " +
					"you will never break a lockpick again. Additionally, this skill will allow you to attempt to open house doors."; } }
		public override string SecondDescription{ get{ return "This skill level will allow you to say that you have two points in Locksmithing."; } }
		public override string ThirdDescription{ get{ return "This skill level will allow you to break open doors and containers, leaving them unlocked."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return ".AutoPicking"; } }
		public override string ThirdCommand{ get{ return ".BreakLock"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Locksmith()); }
		
		public Locksmith() {}
	}
}
