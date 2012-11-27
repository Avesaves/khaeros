using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class ArmourSmithing: BaseFeat
    {
        public override string Name{ get{ return "Armour Smithing"; } }
		public override FeatList ListName{ get{ return Mobiles.FeatList.ArmourSmithing; } }
		public override FeatCost CostLevel{ get{ return FeatCost.High; } }
		
		public override SkillName[] AssociatedSkills{ get{ return new SkillName[]{ }; } }
		public override FeatList[] AssociatedFeats{ get{ return new FeatList[]{ }; } }
		
		public override FeatList[] Requires{ get{ return new FeatList[]{ FeatList.Blacksmithing }; } }
		public override FeatList[] Allows{ get{ return new FeatList[]{ FeatList.ArmourEnameling, FeatList.Blunt, FeatList.Slashing, FeatList.Piercing }; } }
		
		public override string FirstDescription{ get{ return "This skill allows you to assemble armor from various pieces as well as greater chanes for making higher quality armor. [Armor crafted by you has a small chance for increased quality] "; } }
        public override string SecondDescription { get { return "[Armor crafted by you has a medium chance for increased quality]"; } }
        public override string ThirdDescription { get { return "[Armor crafted by you has a high chance for increased quality]"; } }

		public override string FirstCommand{ get{ return "None"; } }
		public override string SecondCommand{ get{ return "None"; } }
		public override string ThirdCommand{ get{ return "None"; } }
		
		public override string FullDescription{ get{ return GetFullDescription(this); } }
		
		public static void Initialize(){ WriteWebpage(new ArmourSmithing()); }

        public override bool MeetsOurRequirements(PlayerMobile m)
        {
            if (!m.SmithTesting)
                return false;

            return base.MeetsOurRequirements(m);
        }
		
		public ArmourSmithing() {}
    }
}
