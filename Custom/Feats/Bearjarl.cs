using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;

namespace Server.FeatInfo
{
    public class SteppeRaider : BaseFeat
    {
        public override string Name { get { return "Steppe Raider"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.SteppeRaider; } }
        public override FeatCost CostLevel { get { return FeatCost.Low; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.MountedDefence }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "You have mastered the art of using the ferocity of the fearless Steppe Horse to your advantage. [Melee Damage + 3 while mounted on a Steppe Horse]"; } }
        public override string SecondDescription { get { return "[Melee Damage + 6 while mounted on a Steppe Horse]"; } }
        public override string ThirdDescription { get { return "[Melee Damage + 9 while mounted on a Steppe Horse]"; } }

        public override string FirstCommand { get { return "None"; } }
        public override string SecondCommand { get { return "None"; } }
        public override string ThirdCommand { get { return "None"; } }

        public override string FullDescription { get { return GetFullDescription(this); } }

        public static void Initialize() { WriteWebpage(new SteppeRaider()); }

        public override bool MeetsOurRequirements(PlayerMobile m)
        {
            if (m.Feats.GetFeatLevel(FeatList.KudaRider) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.Clibanarii) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.HorseArcher) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.Skirmisher) > 0)
                return false;
            if (m.Feats.GetFeatLevel(FeatList.HeavyCavalry) > 0)
                return false;

            return base.MeetsOurRequirements(m);
        }

        public SteppeRaider() { }
    }
}