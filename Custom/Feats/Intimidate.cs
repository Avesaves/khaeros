using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Intimidate : BaseFeat
	{
		public override string Name{ get{ return "Intimidate"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Intimidate; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Tactics }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You yell and grimmace at your opponent, scaring them and causing " +
					"them to fumble their current attack or defense, while still being delayed as if they had completed it, thus " +
					"giving you ample time to act. This cannot stop defensive formation or charge. [25% chance of success]"; } }
		public override string SecondDescription{ get{ return "[50% chance of success]"; } }
		public override string ThirdDescription{ get{ return "[75% chance of success]"; } }

		public override string FirstCommand{ get{ return ".Intimidate"; } }
		public override string SecondCommand{ get{ return ".Intimidate"; } }
		public override string ThirdCommand{ get{ return ".Intimidate"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Intimidate()); }
		
		public Intimidate() {}
	}
}
