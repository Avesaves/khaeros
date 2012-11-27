using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class WoodStaining : BaseFeat
	{
		public override string Name{ get{ return "Wood Staining"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.WoodStaining; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ FeatList.RacialStaining }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Carpentry, FeatList.Fletching }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.RacialStaining }; } }
		
		public override string FirstDescription{ get{ return "You are able to obtain a few hues to use with wood staining. [8 hues]"; } }
		public override string SecondDescription{ get{ return "[16 hues]"; } }
		public override string ThirdDescription{ get{ return "[24 hues]"; } }

		public override string FirstCommand{ get{ return ".WoodStaining"; } }
		public override string SecondCommand{ get{ return ".WoodStaining"; } }
		public override string ThirdCommand{ get{ return ".WoodStaining"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new WoodStaining()); }
		
		public WoodStaining() {}
	}
}
