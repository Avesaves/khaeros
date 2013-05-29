using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class BowMastery : BaseFeat
	{
		public override string Name{ get{ return "Bow Mastery"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.BowMastery; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Archery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.FarShot, FeatList.SwiftShot, FeatList.HailOfArrows, 
				FeatList.TravelingShot, FeatList.CripplingShot, FeatList.CriticalShot }; } }
		
		public override string FirstDescription{ get{ return 	"Dedicating yourself to archery you have now become a " +
					"master with the bow. [+5% increased hit chance with bows]"; } }
		public override string SecondDescription{ get{ return "[+10% increased hit chance with bows]"; } }
		public override string ThirdDescription{ get{ return "[+15% increased hit chance with bows]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new BowMastery()); }
		
		public BowMastery() {}
	}
}
