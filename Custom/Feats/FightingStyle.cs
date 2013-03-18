using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class FightingStyle : BaseFeat
	{
		public override string Name{ get{ return "Fighting Style"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.FightingStyle; } }
		public override FeatCost CostLevel{ get{ return FeatCost.Low; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Swordsmanship, FeatList.Macing, FeatList.ExoticWeaponry, 
				FeatList.Fencing, FeatList.Axemanship, FeatList.Polearms }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.WeaponSpecialization }; } }
		
		public override string FirstDescription{ get{ return "When using his chosen combat style, the weapon specialist gains some bonus damage for " +
					"every successful hit and parry, but loses some of it if his attacks are parried or he gets hit. The bonus " +
					"cannot go below 0%, or above 45%. If 20 seconds elapse after the last bonus/penalty, the bonus is removed. [3% damage gain, 3% damage loss]"; } }
		public override string SecondDescription{ get{ return "[6% damage gain, 6% damage loss]"; } }
		public override string ThirdDescription{ get{ return "[9% damage gain, 9% damage loss]"; } }
		
		public override string FirstCommand{ get{ return ".FightingStyle"; } }
		public override string SecondCommand{ get{ return ".FightingStyle"; } }
		public override string ThirdCommand{ get{ return ".FightingStyle"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new FightingStyle()); }
		
		public FightingStyle() {}

        public override void OnLevelLowered( PlayerMobile owner )
        {
            owner.CombatStyles.Axemanship = 0;
            owner.CombatStyles.ExoticWeaponry = 0;
            owner.CombatStyles.Fencing = 0;
            owner.CombatStyles.MaceFighting = 0;
            owner.CombatStyles.Polearms = 0;
            owner.CombatStyles.Swordsmanship = 0;
        }
	}
}
