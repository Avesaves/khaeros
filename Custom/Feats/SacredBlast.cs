using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class SacredBlast : BaseFeat
	{
		public override string Name{ get{ return "Sacred Blast"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.SacredBlast; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.InflictWounds }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "A more complex offensive spell that does less damage than Inflict Wounds, but " +
					"provides healing for the caster or an adjacent ally. [Deals 10 damage and heals 10 or 5 on self, costs 15 mana]"; } }
		public override string SecondDescription{ get{ return "[Deals 20 damage and heals 20 or 10 on self, costs 30 mana]"; } }
		public override string ThirdDescription{ get{ return "[Deals 30 damage and " +
					"heals 30 or 15 on self, costs 45 mana]"; } }

		public override string FirstCommand{ get{ return ".SacredBlast | .SacredBlast 1"; } }
		public override string SecondCommand{ get{ return ".SacredBlast | .SacredBlast 2"; } }
		public override string ThirdCommand{ get{ return ".SacredBlast | .SacredBlast 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new SacredBlast()); }
		
		public SacredBlast() {}
	}
}
