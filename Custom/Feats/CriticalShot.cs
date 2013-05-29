using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class CriticalShot : BaseFeat
	{
		public override string Name{ get{ return "Critical Shot"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.CriticalShot; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.BowMastery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You know where to shoot your arrows so that they really " +
					"hurt. [7 bonus damage]"; } }
		public override string SecondDescription{ get{ return "[14 bonus damage]"; } }
		public override string ThirdDescription{ get{ return "[21 bonus damage]"; } }

		public override string FirstCommand{ get{ return ".CriticalShot"; } }
		public override string SecondCommand{ get{ return ".CriticalShot"; } }
		public override string ThirdCommand{ get{ return ".CriticalShot"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new CriticalShot()); }
		
		public CriticalShot() {}
	}
}
