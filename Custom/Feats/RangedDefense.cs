using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class RangedDefense : BaseFeat
    {
        public override string Name { get { return "Ranged Defense"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.RangedDefense; } }
        public override FeatCost CostLevel { get { return FeatCost.Medium; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.MediumArmour }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "You have mastered the art of using your armour to catch and defend against arrows. [Each piece of medium armour worn, inlcuding shields, increases the chance that a ranged attack's damage will be substantially reduced; at this level, a 3% chance.]"; } }
        public override string SecondDescription { get { return "[Each piece of medium armour worn, including shields, gives you 6% chance of reducing ranged damage.]"; } }
        public override string ThirdDescription { get { return "[Each piece of medium armour worn, including shields, gives you 9% chance of reducing ranged damage.]"; } }

        public override string FirstCommand { get { return "None"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public override string FullDescription { get { return GetFullDescription(this); } }

        public static void Initialize() { WriteWebpage(new RangedDefense()); }

        public RangedDefense() { }
    }
}