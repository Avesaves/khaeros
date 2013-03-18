using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Potter : BaseFeat
	{
		public override string Name{ get{ return "Potter"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Potter; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Craftsmanship }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Sculptor }; } }
		
		public override string FirstDescription{ get{ return "You know how to find clay suitable for pottery."; } }
		public override string SecondDescription{ get{ return "You have learned the art of pottery."; } }
		public override string ThirdDescription{ get{ return "Your skill has reached such levels that you are able to craft unique pottery pieces."; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return ".EngravePottery"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Potter()); }
		
		public Potter() {}
	}
}
