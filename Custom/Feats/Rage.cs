using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class Rage : BaseFeat
	{
		public override string Name{ get{ return "Rage"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.Rage; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.DefensiveFury }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.DamageIgnore }; } }
		
		public override string FirstDescription{ get{ return "Firing up your blood you rage in to combat, attacking the " +
					"enemies without any thought of injury. [+7.5% damage and speed, +10 hp bonus, -5% Defence Chance, +5% " +
					"Attack Chance. When you finish raging you will get a penalty of 1/6 of your stamina and lose the hp bonus]"; } }
		public override string SecondDescription{ get{ return "[+15% damage and speed, +20 hp bonus, -10% Defence Chance, +10% " +
					"Attack Chance. When you finish raging you will get a penalty of 2/6 of your stamina and lose the hp bonus]"; } }
		public override string ThirdDescription{ get{ return "[+22.5% damage and speed, +30 hp bonus, -15% Defence Chance, +15% " +
					"Attack Chance. When you finish raging you will get a penalty of 3/6 of your stamina and lose the hp bonus]"; } }

		public override string FirstCommand{ get{ return ".Rage"; } }
		public override string SecondCommand{ get{ return ".Rage"; } }
		public override string ThirdCommand{ get{ return ".Rage"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new Rage()); }
		
		public Rage() {}
	}
}
