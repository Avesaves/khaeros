using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
    public class JusticeAura : BaseCustomSpell
    {
        public override bool CanTargetSelf { get { return false; } }
        public override bool AffectsMobiles { get { return true; } }
        public override bool IsHarmful { get { return false; } }
        public override bool UsesTarget { get { return true; } }
        public override bool UsesFullEffect { get { return true; } }
        public override FeatList Feat { get { return FeatList.Justice; } }
        public override string Name { get { return "Justice Aura"; } }
        public override int BaseCost { get { return 30; } }
        public override double FullEffect { get { return 3.5; } }

        public JusticeAura(Mobile caster, int featLevel)
            : base(caster, featLevel)
        {
        }

        public override void Effect()
        {
            if (TargetCanBeAffected && CasterHasEnoughMana)
            {
                Caster.Mana -= TotalCost;
                FinalEffect(Caster, TargetMobile, TotalEffect);
                Success = true;
            }
        }

        public static void FinalEffect(Mobile caster, Mobile target, int thorns)
        {
            if (((IKhaerosMobile)target).JusticeAura != null)
                ((IKhaerosMobile)target).JusticeAura.Stop();

            ((IKhaerosMobile)target).JusticeAura = new JusticeAuraTimer(target, thorns);
            ((IKhaerosMobile)target).JusticeAura.Start();
        }

        public static void Initialize()
        {
            CommandSystem.Register("JusticeAura", AccessLevel.Player, new CommandEventHandler(JusticeAura_OnCommand));
        }

        [Usage("JusticeAura")]
        [Description("You temporarily protect your ally from injury while dealing damage to their enemy.")]
        private static void JusticeAura_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile != null)
                SpellInitiator(new JusticeAura(e.Mobile, GetSpellPower(e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Justice))));
        }

        public class JusticeAuraTimer : Timer
        {
            private Mobile m;

            public JusticeAuraTimer(Mobile from, int delay)
                : base(TimeSpan.FromSeconds(delay))
            {
                m = from;
            }

            protected override void OnTick()
            {
                if (m == null || m.Deleted)
                    return;

                ((IKhaerosMobile)m).JusticeAura = null;
                m.SendMessage("Your sense of invulnerability wears away.");
                Stop();
            }
        }
    }
}