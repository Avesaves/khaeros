using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class AdvancedDying : BaseFeat
	{
		public override string Name{ get{ return "Advanced Dying"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.AdvancedDying; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ FeatList.LeatherDying, FeatList.RacialDyes }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Tailoring }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.LeatherDying, FeatList.RacialDyes }; } }
		
		public override string FirstDescription{ get{ return "You are able to obtain a few hues to dye clothing with. [10 hues]"; } }
		public override string SecondDescription{ get{ return "[20 hues]"; } }
		public override string ThirdDescription{ get{ return "[30 hues]"; } }

		public override string FirstCommand{ get{ return ".AdvancedDying"; } }
		public override string SecondCommand{ get{ return ".AdvancedDying"; } }
		public override string ThirdCommand{ get{ return ".AdvancedDying"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new AdvancedDying()); }
		
		public AdvancedDying() {}
	}
}
