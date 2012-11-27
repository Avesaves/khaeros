using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Misc
{
    public class CompassionHeal : BaseCustomSpell
    {
        public override bool CanTargetSelf { get { return true; } }
        public override bool AffectsMobiles { get { return true; } }
        public override bool UsesTarget { get { return true; } }
        public override bool UsesFaith { get { return true; } }
        public override bool UsesFullEffect { get { return !Self; } }
        public override FeatList Feat { get { return FeatList.Compassion; } }
        public override string Name { get { return "Compassion Heal"; } }
        public override int BaseCost { get { return 40; } }
        public override double FullEffect { get { return (Caster.Skills[SkillName.Faith].Base * 0.15); } }
        public override double PartialEffect { get { return (Caster.Skills[SkillName.Faith].Base * 0.10); } }

        public CompassionHeal(Mobile caster, int featLevel)
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

            int injCount = HealthAttachment.GetHA(target).CurrentInjuries.Count;

            for (int i = injCount - 1; i > -1; i--)
            {
                HealthAttachment.GetHA(target).CurrentInjuries[i].Stop();
                HealthAttachment.GetHA(target).CurrentInjuries.RemoveAt(i);
            }

            target.SendMessage("You feel revitalized!");
        }

        public static void Initialize()
        {
            CommandSystem.Register("CompassionHeal", AccessLevel.Player, new CommandEventHandler(CompassionHeal_OnCommand));
        }

        [Usage("CompassionHeal")]
        [Description("Heals the body of all serious injuries.")]
        private static void CompassionHeal_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile != null)
            {
                if (e.Mobile is PlayerMobile && ((PlayerMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Compassion) < 3)
                {
                    e.Mobile.SendMessage("You lack the compassion for this feat.");
                    return;
                }

                SpellInitiator(new CompassionHeal(e.Mobile, GetSpellPower(e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Compassion))));
            }
        }
    }
}