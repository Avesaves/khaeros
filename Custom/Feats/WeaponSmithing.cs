using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class WeaponSmithing: BaseFeat
    {
        public override string Name{ get{ return "Weapon Smithing"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.WeaponSmithing; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.VampireAbilities }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ /*FeatList.WeaponEnameling,*/ FeatList.Damage, FeatList.Speed, FeatList.HCI, FeatList.DCI }; } }
		
		public override string FirstDescription{ get{ return "This skill allows you to assemble weapons from various pieces as well as greater chanes for making higher quality weapons. [Weapons crafted by you has a small chance for increased quality] "; } }
        public override string SecondDescription { get { return "[Armor crafted by you has a medium chance for increased quality]"; } }
        public override string ThirdDescription { get { return "[Armor crafted by you has a high chance for increased quality]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new WeaponSmithing()); }

        /*public override bool MeetsOurRequirements(PlayerMobile m)
        {
            if (!m.SmithTesting)
                return false;

            return base.MeetsOurRequirements(m);
        }*/
		
 		   public override bool MeetsOurRequirements(PlayerMobile m)
        {
            if (m.AccessLevel == AccessLevel.Player)
                return false;

            return base.MeetsOurRequirements(m);
        }

        public override bool ShouldDisplayTo(PlayerMobile m)
        {
            if (m.AccessLevel == AccessLevel.Player)
                return false;

            return base.ShouldDisplayTo(m);
        } 
		
		public WeaponSmithing() {}
    }
}
