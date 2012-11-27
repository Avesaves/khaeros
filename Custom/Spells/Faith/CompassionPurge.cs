using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
    public class CompassionPurge : BaseCustomSpell
    {
        public override bool CanTargetSelf { get { return true; } }
        public override bool AffectsMobiles { get { return true; } }
        public override bool UsesTarget { get { return true; } }
        public override bool UsesFaith { get { return true; } }
        public override bool UsesFullEffect { get { return !Self; } }
        public override FeatList Feat { get { return FeatList.Compassion; } }
        public override string Name { get { return "Compassion Purge"; } }
        public override int BaseCost { get { return 20; } }
        public override double FullEffect { get { return (Caster.Skills[SkillName.Faith].Base * 0.15); } }
        public override double PartialEffect { get { return (Caster.Skills[SkillName.Faith].Base * 0.10); } }

        public CompassionPurge(Mobile caster, int featLevel)
            : base(caster, featLevel)
        {
        }

        public override void Effect()
        {
            if (TargetCanBeAffected && CasterHasEnoughMana)
            {
                Caster.Mana -= TotalCost;
                FinalEffect(TargetMobile);
                Success = true;
            }
        }

        public static void FinalEffect(Mobile target)
        {
            target.PlaySound(0x1F2);

            int disCount = HealthAttachment.GetHA(target).CurrentDiseases.Count;
            for (int i = disCount - 1; i > -1; i--)
            {
                HealthAttachment.GetHA(target).CurrentDiseases[i].Stop();
                HealthAttachment.GetHA(target).CurrentDiseases.RemoveAt(i);
            }

            XmlAddiction addAtt = XmlAttach.FindAttachment(target, typeof(XmlAddiction)) as XmlAddiction;
            if (addAtt != null && !addAtt.Deleted)
            {
                int addCount = addAtt.Addictions.Count;
                for (int i = addCount - 1; i > -1; i--)
                {
                    addAtt.Addictions[i].Stop();
                    addAtt.Addictions.RemoveAt(i);
                }
            }

            target.SendMessage("You feel rejuvenated!");
        }

        public static void Initialize()
        {
            CommandSystem.Register("CompassionPurge", AccessLevel.Player, new CommandEventHandler(CompassionPurge_OnCommand));
        }

        [Usage("CompassionPurge")]
        [Description("Purges the body through compassion.")]
        private static void CompassionPurge_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile != null)
            {
                if (e.Mobile is PlayerMobile && ((PlayerMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Compassion) < 2)
                {
                    e.Mobile.SendMessage("You lack the compassion for this feat.");
                    return;
                }
                SpellInitiator(new CompassionPurge(e.Mobile, GetSpellPower(e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Compassion))));
            }
        }
    }
}