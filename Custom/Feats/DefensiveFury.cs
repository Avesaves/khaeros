using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class DefensiveFury : BaseFeat
	{
		public override string Name{ get{ return "Defensive Fury"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.DefensiveFury; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.Rage, FeatList.TirelessRage }; } }
		
		public override string FirstDescription{ get{ return "When you take enough beating you become enraged, launching " +
					"yourself in to a battle frenzy. [+2 damage ignore, +10 bonus hit points, +5% defence chance, -5% hit chance. " +
					"When you finish raging you will get a penalty of 1/6 of your stamina and lose the hp bonus]"; } }
		public override string SecondDescription{ get{ return "[+4 damage ignore, +20 bonus hit points, +10% defence chance, " +
					"-10% hit chance. When you finish raging you will get a penalty of 2/6 of your stamina and lose the hp bonus]"; } }
		public override string ThirdDescription{ get{ return "[+6 damage ignore, +30 bonus hit points, +15% defence chance, -15% " +
					"hit chance. When you finish raging you will get a penalty of 3/6 of your stamina and lose the hp bonus]"; } }

		public override string FirstCommand{ get{ return ".DefensiveFury"; } }
		public override string SecondCommand{ get{ return ".DefensiveFury"; } }
		public override string ThirdCommand{ get{ return ".DefensiveFury"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new DefensiveFury()); }
		
		public DefensiveFury() {}
	}
}
