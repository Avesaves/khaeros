using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class InflictWounds : BaseFeat
	{
		public override string Name{ get{ return "Inflict Wounds"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.InflictWounds; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Faith }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.SacredBlast }; } }
		
		public override string FirstDescription{ get{ return "A basic offensive spell that will allow you to harm any creature. [Deals 15 damage, " +
					"costs 10 mana]"; } }
		public override string SecondDescription{ get{ return "[Deals 30 damage, costs 20 mana]"; } }
		public override string ThirdDescription{ get{ return "[Deals 45 damage, costs 30 mana]"; } }

		public override string FirstCommand{ get{ return ".InflictWounds | .InflictWounds 1"; } }
		public override string SecondCommand{ get{ return ".InflictWounds | .InflictWounds 2"; } }
		public override string ThirdCommand{ get{ return ".InflictWounds | .InflictWounds 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new InflictWounds()); }
		
		public InflictWounds() {}
	}
}
