using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
	public class SecondSpecialization : BaseFeat
	{
		public override string Name{ get{ return "Second Specialization"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.SecondSpecialization; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.WeaponSpecialization }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }
		
		public override string FirstDescription{ get{ return "You are able to focus on a second weapon. [+7.5% Damage and Speed with " +
					"second chosen weapon, -7.5% with all others]"; } }
		public override string SecondDescription{ get{ return "[+15% Damage and Speed with second chosen weapon, -15% with all others]"; } }
		public override string ThirdDescription{ get{ return "[+22.5% Damage and Speed with second chosen weapon, -22.5% with all others]"; } }

		public override string FirstCommand{ get{ return ".WeaponSpecialization"; } }
		public override string SecondCommand{ get{ return ".WeaponSpecialization"; } }
		public override string ThirdCommand{ get{ return ".WeaponSpecialization"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new SecondSpecialization()); }
		
		public SecondSpecialization() {}

        public override void OnLevelLowered( PlayerMobile owner )
        {
            owner.SecondSpecialization = null;
        }
	}
}
