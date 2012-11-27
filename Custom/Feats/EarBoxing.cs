using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class EarBoxing : BaseFeat
	{
		public override string Name{ get{ return "Ear Boxing"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.EarBoxing; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill allows the player to clap the flat of their palms against an opponent's " +
					"ears, causing sharp pain and temporary deafness. [+2.5% Hit Chance, +3 base damage, target is deafened for 30 " +
					"seconds. Target gets -25% Hit Chance for 3 seconds.]"; } }
		public override string SecondDescription{ get{ return "[+5% Hit Chance, +6 base damage, target is deafened for 60 seconds. Target gets -25% " +
					"Hit Chance for 6 seconds.]"; } }
		public override string ThirdDescription{ get{ return "[+7.5% Hit Chance, +9 base damage, target is deafened for 90 seconds. Target gets -25% " +
					"Hit Chance for 9 seconds.]"; } }

		public override string FirstCommand{ get{ return ".EarBoxing"; } }
		public override string SecondCommand{ get{ return ".EarBoxing"; } }
		public override string ThirdCommand{ get{ return ".EarBoxing"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new EarBoxing()); }
		
		public EarBoxing() {}
	}
}
