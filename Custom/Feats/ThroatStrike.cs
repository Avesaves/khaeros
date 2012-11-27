using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class ThroatStrike : BaseFeat
	{
		public override string Name{ get{ return "Throat Strike"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ThroatStrike; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.MartialOffence }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You know how to deliver a precise blow at your opponent's throat, dealing " +
					"additional damage and preventing speech. [target cannot speak for 10 seconds and loses 1 stam every second.]"; } }
		public override string SecondDescription{ get{ return "[target cannot speak for 20 seconds and loses 1 stam every second.]"; } }
		public override string ThirdDescription{ get{ return "[target cannot speak for 30 seconds and loses 1 stam every second.]"; } }

		public override string FirstCommand{ get{ return ".ThroatStrike"; } }
		public override string SecondCommand{ get{ return ".ThroatStrike"; } }
		public override string ThirdCommand{ get{ return ".ThroatStrike"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ThroatStrike()); }
		
		public ThroatStrike() {}
	}
}
