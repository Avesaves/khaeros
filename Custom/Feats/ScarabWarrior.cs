using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class ScarabWarrior : BaseFeat
    {
        public override string Name { get { return "Scarab Warrior"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.ScarabWarrior; } }
        public override FeatCost CostLevel { get { return FeatCost.Low; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.MountedDefence }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "You have learned to shield yourself with the desert beetle's hardened wings. [DCI + 5% while mounted on a Beetle; DCI - 5% while unmounted]"; } }
        public override string SecondDescription { get { return "[DCI + 10% while mounted on a Beetle; DCI - 10% while unmounted]"; } }
        public override string ThirdDescription { get { return "[DCI + 15% while mounted on a Beetle; DCI - 15% while unmounted]"; } }

        public override string FirstCommand { get { return "None"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public override string FullDescription { get { return GetFullDescription(this); } }

        public static void Initialize() { WriteWebpage(new ScarabWarrior()); }

        public override bool MeetsOurRequirements(PlayerMobile m)
        {
            if (m.Feats.GetFeatLevel(FeatList.Ridgeking) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.Longstrider) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.Bearjarl) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.Fanglord) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.Horselord) > 0)
                return false;

            return base.MeetsOurRequirements(m);
        }

        public ScarabWarrior() { }
    }
}