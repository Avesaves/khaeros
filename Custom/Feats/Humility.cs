using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;
using Server.Engines.XmlSpawner2;
using Server.Misc;
using Server.Regions;

namespace Server.FeatInfo
{
    public class Humility: BaseFeat
    {
        public override string Name { get { return "Humility"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.Humility; } }
        public override FeatCost CostLevel { get { return FeatCost.High; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.Faith }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "Your sense of sacrifice and lack of self-importance allow you to shield others from harm. [You create a shield for an ally which transmits damage dealt to them, to you instead.]"; } }
        public override string SecondDescription { get { return "Your incredible sense of humility inspires others, awing them into similar behavior. [You may temporarily awe someone, suspending their ability to participate in combat.]"; } }
        public override string ThirdDescription { get { return "You are a font of humility, devoid of ego or pride -- so much so, that you are willing to sacrifice yourself for the good of others. [You may sacrifice yourself to heal all nearby allies' hit points completely.]"; } }

        public override string FirstCommand { get { return ".humilityshield"; } }
        public override string SecondCommand { get { return ".humilityawe"; } }
        public override string ThirdCommand { get { return ".humilitysacrifice"; } }

        public override string FullDescription { get { return GetFullDescription(this); } }

        public static void Initialize() { }

        public Humility() { }

        public override bool MeetsOurRequirements(PlayerMobile m)
        {
            if (m.AccessLevel == AccessLevel.Player)
                return false;

            if (!m.CanBeFaithful)
                return false;

            if (m.Feats.GetFeatLevel(FeatList.Justice) > 0)
                return false;

            if (m.Feats.GetFeatLevel(FeatList.Compassion) > 0)
                return false;

            return base.MeetsOurRequirements(m);
        }

        public override bool ShouldDisplayTo(PlayerMobile m)
        {
            if (m.AccessLevel == AccessLevel.Player)
                return false;

            if (!m.CanBeFaithful)
                return false;

            if (m.Feats.GetFeatLevel(FeatList.Justice) > 0)
                return false;

            if (m.Feats.GetFeatLevel(FeatList.Compassion) > 0)
                return false;

            return base.ShouldDisplayTo(m);
        }
    }
}