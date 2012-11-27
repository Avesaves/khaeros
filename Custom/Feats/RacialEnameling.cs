using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class RacialEnameling : BaseFeat
	{
		public override string Name{ get{ return "Racial Enameling"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.RacialEnameling; } }
		public override FeatCost CostLevel{ get{ return FeatCost.None; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.ArmourEnameling }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This skill will give you access to two racial enameling dyes."; } }
		public override string SecondDescription{ get{ return "[This skill will give you access to two additional racial enameling dyes.]"; } }
		public override string ThirdDescription{ get{ return "[This skill will give you access to two additional racial enameling dyes.]"; } }

		public override string FirstCommand{ get{ return ".Enamel"; } }
		public override string SecondCommand{ get{ return ".Enamel"; } }
		public override string ThirdCommand{ get{ return ".Enamel"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new RacialEnameling()); }
		
		public RacialEnameling() {}
	}
}
