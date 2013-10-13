using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class SummonProtector : BaseFeat
	{
		public override string Name{ get{ return "Summon Protector"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.SummonProtector; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.DivineConsecration }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "This power allows you to summon a powerful creature with affinity for your culture, " +
					"who will serve you for some time. [Lasts 5 minutes. Costs 30 mana]"; } }
		public override string SecondDescription{ get{ return "[Lasts 10 minutes. Costs 60 mana]"; } }
		public override string ThirdDescription{ get{ return "[Lasts 15 minutes. Costs 90 mana]"; } }
		
		public override string FirstCommand{ get{ return ".SummonProtector | .SummonProtector 1"; } }
		public override string SecondCommand{ get{ return ".SummonProtector | .SummonProtector 2"; } }
		public override string ThirdCommand{ get{ return ".SummonProtector | .SummonProtector 3"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new SummonProtector()); }
		
		public SummonProtector() {}
	}
}
