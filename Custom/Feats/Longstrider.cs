using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class Longstrider : BaseFeat
    {
        public override string Name { get { return "Longstrider"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.Longstrider; } }
        public override FeatCost CostLevel { get { return FeatCost.Low; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.MountedDefence }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "You have mastered the art of the bow upon the quick and graceful Strider. [Ranged Damage + 3 while mounted on a Strider; Ranged Damage - 3 while unmounted]"; } }
        public override string SecondDescription { get { return "[Ranged Damage + 6 while mounted on a Strider; Ranged Damage - 6 while unmounted]"; } }
        public override string ThirdDescription { get { return "[Ranged Damage + 9 while mounted on a Strider; Ranged Damage - 9 while unmounted]"; } }

        public override string FirstCommand { get { return "None"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public override string FullDescription { get { return GetFullDescription(this); } }

        public static void Initialize() { WriteWebpage(new Longstrider()); }

        public override bool MeetsOurRequirements(PlayerMobile m)
        {
            if (m.Feats.GetFeatLevel(FeatList.Ridgeking) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.ScarabWarrior) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.Bearjarl) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.Fanglord) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.Horselord) > 0)
                return false;

            return base.MeetsOurRequirements(m);
        }

        public Longstrider() { }
    }
}