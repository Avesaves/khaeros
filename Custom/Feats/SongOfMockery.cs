using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class SongOfMockery : BaseFeat
	{
		public override string Name{ get{ return "Song of Mockery"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.SongOfMockery; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "With this skill, you will be able to anger every non-ally creature around you and " +
					"cause them to wish to change their attacking target to you. [Chance is (60 + 30 - creature's int score) or 20, whichever " +
					"is higher]"; } }
		public override string SecondDescription{ get{ return "Reduced stamina cost, better effectiveness. [Chance is (60 + 60 - creature's int " +
					"score) or 20, whichever is higher]"; } }
		public override string ThirdDescription{ get{ return "Minimum stamina cost, maximum effectiveness. [Chance is (60 + 90 - creature's int " +
					"score) or 20, whichever is higher]"; } }

		public override string FirstCommand{ get{ return ".SongOfMockery"; } }
		public override string SecondCommand{ get{ return ".SongOfMockery"; } }
		public override string ThirdCommand{ get{ return ".SongOfMockery"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new SongOfMockery()); }
		
		public SongOfMockery() {}
	}
}
