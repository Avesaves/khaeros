using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class Pathology : BaseFeat
    {
        public override string Name { get { return "Pathology"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.Pathology; } }
        public override FeatCost CostLevel { get { return FeatCost.Medium; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.Medicine }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "You can diagnose a subject's diseases and ailments."; } }
        public override string SecondDescription { get { return "The effectiveness of disease-healing agents is increased by 10% when worked by you."; } }
        public override string ThirdDescription { get { return "The effectiveness of disease-healing agents is increased by 30% when worked by you."; } }

        public override string FirstCommand { get { return "None"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public override string FullDescription { get { return GetFullDescription(this); } }

        public static void Initialize() { WriteWebpage(new Pathology()); }

        public Pathology() { }
    }
}