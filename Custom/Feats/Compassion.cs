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
    public class Compassion : BaseFeat
    {
        public override string Name { get { return "Compassion"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.Compassion; } }
        public override FeatCost CostLevel { get { return FeatCost.High; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.Faith }; } }
        public override FeatList[] Allows{ get{ return new FeatList[]{ }; } }

        public override string FirstDescription { get { return "Your incredible compassion for humanity allows you to end the violence and strife that divides us. [All characters and NPCs within a short distance cannot engage in violence while in your vicinity.]"; } }
        public override string SecondDescription { get { return "You are a paragon of compassion, and may channel that wellspring of love to rid your allies of festering diseases. [Target is cured of all diseases and addictions.]"; } }
        public override string ThirdDescription { get { return "Your compassion is almost inhuman, and others now find your universal empathy for others somewhat disturbing, though it allows you rid the body of injuries. [Target is cured of all injuries.]"; } }
        

        public override string FirstCommand { get { return ".compassionsanctuary"; } }
        public override string SecondCommand { get { return ".compassionpurge"; } }
        public override string ThirdCommand { get { return ".compassionheal"; } }

        public override string FullDescription { get { return GetFullDescription(this); } }

        public static void Initialize() { }

        public Compassion() { }

        public override bool MeetsOurRequirements(PlayerMobile m)
        {
            if (m.AccessLevel == AccessLevel.Player)
                return false;

            if (!m.CanBeFaithful)
                return false;

            if(m.Feats.GetFeatLevel(FeatList.Justice) > 0)
                return false;

            if(m.Feats.GetFeatLevel(FeatList.Humility) > 0)
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

            if (m.Feats.GetFeatLevel(FeatList.Humility) > 0)
                return false;

            return base.ShouldDisplayTo(m);
        }
    }
}