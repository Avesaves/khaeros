using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class RopeTrick : BaseFeat
	{
		public override string Name{ get{ return "Rope Trick"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.RopeTrick; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ FeatList.EscortPrisoner }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.NonLethalTraps }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.EscortPrisoner }; } }
		
		public override string FirstDescription{ get{ return "You can use your rope to lasso someone. [3-second delay before the effect. Range 2. " +
					"40% chance to hit. Lasts for 1.5 seconds. Ress-sick players will always be lassoed and will only be able to break " +
					"free when they are no longer ress-sick.]"; } }
		public override string SecondDescription{ get{ return "[2-second delay before the effect. Range 3. 50% chance to hit. Lasts for 3 seconds.]"; } }
		public override string ThirdDescription{ get{ return "[1-second delay before the effect. Range 4. 60% chance to hit. Lasts for 4.5 seconds.]"; } }

		public override string FirstCommand{ get{ return ".RopeTrick"; } }
		public override string SecondCommand{ get{ return ".RopeTrick"; } }
		public override string ThirdCommand{ get{ return ".RopeTrick"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new RopeTrick()); }
		
		public RopeTrick() {}
	}
}
