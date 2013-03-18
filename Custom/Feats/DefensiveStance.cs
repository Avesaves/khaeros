using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class DefensiveStance : BaseFeat
	{
		public override string Name{ get{ return "Defensive Stance"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.DefensiveStance; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Medium; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Tactics }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You excel at defensive combat. [-1.5% Speed, -1.5% Damage, +10% Defence Chance]"; } }
		public override string SecondDescription{ get{ return "[-3% Speed, -3% Damage, +20% Defence Chance]"; } }
		public override string ThirdDescription{ get{ return "[-4.5% Speed, -4.5% Damage, +30% Defence Chance]"; } }

		public override string FirstCommand{ get{ return ".DefensiveStance"; } }
		public override string SecondCommand{ get{ return ".DefensiveStance"; } }
		public override string ThirdCommand{ get{ return ".DefensiveStance"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new DefensiveStance()); }
		
		public DefensiveStance() {}
	}
}
