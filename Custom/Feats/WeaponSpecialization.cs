using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class WeaponSpecialization : BaseFeat
	{
		public override string Name{ get{ return "Weapon Specialization"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.WeaponSpecialization; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.FightingStyle }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.WeaponParrying, FeatList.SecondSpecialization, FeatList.CircularAttack }; } }
		
		public override string FirstDescription{ get{ return "You focus exclusively in one weapon, becoming deadlier and deadlier " +
					"with it. [+7.5% Damage and Speed with chosen weapon, -7.5% with all others]"; } }
		public override string SecondDescription{ get{ return "[+15% Damage and Speed with chosen weapon, -15% with all others]"; } }
		public override string ThirdDescription{ get{ return "[+22.5% Damage and Speed with chosen weapon, -22.5% with all others]"; } }

		public override string FirstCommand{ get{ return ".WeaponSpecialization"; } }
		public override string SecondCommand{ get{ return ".WeaponSpecialization"; } }
		public override string ThirdCommand{ get{ return ".WeaponSpecialization"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new WeaponSpecialization()); }
		
		public WeaponSpecialization() {}

        public override void OnLevelLowered( PlayerMobile owner )
        {
            owner.WeaponSpecialization = null;
        }
	}
}
