using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class PlateMastery : BaseFeat
    {
        public override string Name { get { return "Plate Mastery"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.PlateMastery; } }
        public override FeatCost CostLevel { get { return FeatCost.Medium; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.HeavyArmour }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription{ get{ return "You are more proficient in soaking damage with your plate armour. [Each piece of heavy armour worn, including shields, gives you 0.75 Damage Ignore]"; } }
        public override string SecondDescription { get { return "[Each piece of heavy armour worn, including shields, gives you 1.0 Damage Ignore]"; } }
        public override string ThirdDescription { get { return "[Each piece of heavy armour worn, including shields, gives you 1.25 Damage Ignore]"; } }

        public override string FirstCommand { get { return "None"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public override string FullDescription { get { return GetFullDescription( this ); } }

        public static void Initialize() { WriteWebpage( new PlateMastery() ); }

        public PlateMastery() { }
    }
}
