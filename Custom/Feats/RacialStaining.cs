using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class RacialStaining : BaseFeat
	{
		public override string Name{ get{ return "Racial Staining"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.RacialStaining; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.WoodStaining }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you access to two racial wood staining dyes."; } }
		public override string SecondDescription{ get{ return "[This skill will give you access to two additional racial wood staining dyes.]"; } }
		public override string ThirdDescription{ get{ return "[This skill will give you access to two additional racial wood staining dyes.]"; } }

		public override string FirstCommand{ get{ return ".WoodStaining"; } }
		public override string SecondCommand{ get{ return ".WoodStaining"; } }
		public override string ThirdCommand{ get{ return ".WoodStaining"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new RacialStaining()); }
		
		public RacialStaining() {}
	}
}
