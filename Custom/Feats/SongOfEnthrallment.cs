using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class SongOfEnthrallment : BaseFeat
	{
		public override string Name{ get{ return "Song of Enthrallment"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.SongOfEnthrallment; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "With this skill, you will be able to soothe all non-ally creatures around you, " +
					"causing them to abandon combat for the duration of the song or until they are attacked. [Chance is (60 + 30 - creature's " +
					"int score) or 20, whichever is higher]"; } }
		public override string SecondDescription{ get{ return "Reduced stamina cost, better effectiveness. [Chance is (60 + 60 - creature's int " +
					"score) or 20, whichever is higher]"; } }
		public override string ThirdDescription{ get{ return "Minimum stamina cost, maximum effectiveness. [Chance is (60 + 90 - creature's int " +
					"score) or 20, whichever is higher]"; } }

		public override string FirstCommand{ get{ return ".SongOfEnthrallment"; } }
		public override string SecondCommand{ get{ return ".SongOfEnthrallment"; } }
		public override string ThirdCommand{ get{ return ".SongOfEnthrallment"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new SongOfEnthrallment()); }
		
		public SongOfEnthrallment() {}
	}
}
