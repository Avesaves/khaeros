using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Mending : BaseFeat
	{
		public override string Name{ get{ return "Mending"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Mending; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.ConsecrateItem }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "With this spell, you will be able to repair items once a day (every 8 hours in " +
					"real life). [Repairs 10 durability, costs 5 mana]"; } }
		public override string SecondDescription{ get{ return "[Repairs 20 durability, costs 10 mana]"; } }
		public override string ThirdDescription{ get{ return "[Repairs 30 durability, costs 15 mana]"; } }

		public override string FirstCommand{ get{ return ".Mending | .Mending 1"; } }
		public override string SecondCommand{ get{ return ".Mending | .Mending 2"; } }
		public override string ThirdCommand{ get{ return ".Mending | .Mending 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Mending()); }
		
		public Mending() {}
	}
}
