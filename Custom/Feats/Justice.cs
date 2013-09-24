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
    public class Justice : BaseFeat
    {
        public override string Name { get { return "Justice"; } }
        public override FeatList ListName { get { return Mobiles.FeatList.Justice; } }
        public override FeatCost CostLevel { get { return FeatCost.High; } }

        public override SkillName[] AssociatedSkills { get { return new SkillName[] { }; } }
        public override FeatList[] AssociatedFeats { get { return new FeatList[] { }; } }

        public override FeatList[] Requires { get { return new FeatList[] { FeatList.Faith }; } }
        public override FeatList[] Allows { get { return new FeatList[] { }; } }

        public override string FirstDescription { get { return "The universe itself rearranges itself around you, taking an eye for an eye where you cannot. [Whenever you are injured by a player via knockout, your injury will also be inflicted upon the player who killed you.]"; } }
        public override string SecondDescription { get { return "Your incredible sense of right and wrong may root a person to the spot, forcing them to submit to your judgment. [You may temporarily paralyze a target.] "; } }
        public override string ThirdDescription { get { return "You are the embodiment of justice -- for you, no wrong can go unpunished, no matter the sin nor the sinner. [You may create an aura that causes all damage dealt to the target to be dealt, instead, to the attacker.]"; } }

        public override string FirstCommand { get { return "[None]"; } }
        public override string SecondCommand { get { return ".justiceprison"; } }
        public override string ThirdCommand { get { return ".justiceaura"; } }

        public override string FullDescription { get { return GetFullDescription(this); } }

        public static void Initialize() { }

        public Justice() { }

        public override bool MeetsOurRequirements(PlayerMobile m)
        {
            if (m.AccessLevel == AccessLevel.Player)
                return false;

            if (!m.CanBeFaithful)
                return false;

            if (m.Feats.GetFeatLevel(FeatList.Humility) > 0)
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

            if (m.Feats.GetFeatLevel(FeatList.Humility) > 0)
                return false;

            if (m.Feats.GetFeatLevel(FeatList.Compassion) > 0)
                return false;

            return base.ShouldDisplayTo(m);
        }
    }
}