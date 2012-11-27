using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class FarShot : BaseFeat
	{
		public override string Name{ get{ return "Far Shot"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.FarShot; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.BowMastery, FeatList.CrossbowMastery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "Feeling your bow and the surroundings you are able to shoot " +
					"your arrows further and further. [+2 range]"; } }
		public override string SecondDescription{ get{ return "[+4 range]"; } }
		public override string ThirdDescription{ get{ return "[+6 range]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new FarShot()); }
		
		public FarShot() {}
	}
}
