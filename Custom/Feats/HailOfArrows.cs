using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class HailOfArrows : BaseFeat
	{
		public override string Name{ get{ return "Hail of Arrows"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.HailOfArrows; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.BowMastery }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You are able to knock a multitude of arrows on your bow and " +
					"shoot them at the battlefield. [You will shoot one additional arrow that will hit a random " +
					"target near your primary one and deal half damage. Allies are not affected]"; } }
		public override string SecondDescription{ get{ return "[two additional arrows]"; } }
		public override string ThirdDescription{ get{ return "[three additional arrows]"; } }
		
		public override string FirstCommand{ get{ return ".HailOfArrows"; } }
		public override string SecondCommand{ get{ return ".HailOfArrows"; } }
		public override string ThirdCommand{ get{ return ".HailOfArrows"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new HailOfArrows()); }
		
		public HailOfArrows() {}
	}
}
