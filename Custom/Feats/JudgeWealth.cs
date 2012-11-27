using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class JudgeWealth : BaseFeat
	{
		public override string Name{ get{ return "Judge Wealth"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.JudgeWealth; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Snooping }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you a rough estimate of the amount of coins a player is " +
					"carrying. [Lets you know whether target has more or less than 100 copper]"; } }
		public override string SecondDescription{ get{ return "[This skill level will improve the accuracy of the estimate you get. [Lets you " +
					"know whether target has more or less than 100, 500 or 1000 copper]"; } }
		public override string ThirdDescription{ get{ return "[This skill level will grant you exact knowledge of the kind of riches a player " +
					"is carrying. [Tells you the exact amount]"; } }

		public override string FirstCommand{ get{ return ".JudgeWealth"; } }
		public override string SecondCommand{ get{ return ".JudgeWealth"; } }
		public override string ThirdCommand{ get{ return ".JudgeWealth"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new JudgeWealth()); }
		
		public JudgeWealth() {}
	}
}
