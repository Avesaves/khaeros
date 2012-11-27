using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class HealWounds : BaseFeat
	{
		public override string Name{ get{ return "Heal Wounds"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.HealWounds; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Faith }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.HolyStrike }; } }
		
		public override string FirstDescription{ get{ return "A basic healing power that will allow you to close the wounds of any living " +
					"creature. [Heals 15 or 10 on self, costs 10 mana]"; } }
		public override string SecondDescription{ get{ return "[Heals 30 or 20 on self, costs 20 mana]"; } }
		public override string ThirdDescription{ get{ return "[Heals 45 or 30 on self, costs 30 mana]"; } }
	
		public override string FirstCommand{ get{ return ".HealWounds | .HealWounds 1"; } }
		public override string SecondCommand{ get{ return ".HealWounds | .HealWounds 2"; } }
		public override string ThirdCommand{ get{ return ".HealWounds | .HealWounds 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new HealWounds()); }
		
		public HealWounds() {}
	}
}
