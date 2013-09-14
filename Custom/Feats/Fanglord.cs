using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class Skirmisher : BaseFeat
    {
        public override string Name { get { return "Skirmisher"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.Skirmisher; } }
        public override FeatCost CostLevel { get { return FeatCost.Low; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.MountedDefence }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "You are quite skilled in attacking while on the back of the speedy Galloway Horse. [HCI + 5% while mounted on a Galloway Horse]"; } }
        public override string SecondDescription { get { return "[HCI + 10% while mounted on a Galloway Horse]"; } }
        public override string ThirdDescription { get { return "[HCI + 15% while mounted on a Galloway Horse]"; } }

        public override string FirstCommand { get { return "None"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public override string FullDescription { get { return GetFullDescription(this); } }

        public static void Initialize() { WriteWebpage(new Skirmisher()); }

        public override bool MeetsOurRequirements(PlayerMobile m)
        {
            if (m.Feats.GetFeatLevel(FeatList.KudaRider) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.Clibanarii) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.SteppeRaider) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.HorseArcher) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.HeavyCavalry) > 0)
                return false;

            return base.MeetsOurRequirements(m);
        }

        public Skirmisher() { }
    }
}