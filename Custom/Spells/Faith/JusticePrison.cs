using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
    public class JusticePrison : BaseCustomSpell
    {
        public override bool CanTargetSelf { get { return true; } }
        public override bool AffectsMobiles { get { return true; } }
        public override bool IsHarmful { get { return true; } }
        public override bool UsesTarget { get { return true; } }
        public override bool UsesFullEffect { get { return true; } }
        public override FeatList Feat { get { return FeatList.Justice; } }
        public override string Name { get { return "Justice Prison"; } }
        public override int BaseCost { get { return 20; } }
        public override double FullEffect { get { return 2.0; } }

        public JusticePrison(Mobile caster, int featLevel)
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

        public static void FinalEffect(Mobile caster, Mobile target, int hold)
        {
            target.PlaySound(0x204);
            target.FixedParticles(0x37C4, 1, 8, 9916, 39, 3, EffectLayer.Head);
            target.FixedParticles(0x37C4, 1, 8, 9502, 39, 4, EffectLayer.Head);

            target.Emote("*is rooted to the ground, unmoving*");
            target.SendMessage("You are overwhelmed by guilt over your sins.");

            if (((IKhaerosMobile)target).StunnedTimer != null)
                ((IKhaerosMobile)target).StunnedTimer.Stop();

            ((IKhaerosMobile)target).StunnedTimer = new JusticePrisonTimer(target, hold);
            ((IKhaerosMobile)target).StunnedTimer.Start();
        }

        public static void Initialize()
        {
            CommandSystem.Register("JusticePrison", AccessLevel.Player, new CommandEventHandler(JusticePrison_OnCommand));
        }

        [Usage("JusticePrison")]
        [Description("Imprisons a person in their own sense of guilt.")]
        private static void JusticePrison_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile != null)
                SpellInitiator(new JusticePrison(e.Mobile, GetSpellPower(e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Justice))));
        }

        public class JusticePrisonTimer : Timer
        {
            private Mobile m;

            public JusticePrisonTimer(Mobile from, int delay)
                : base(TimeSpan.FromSeconds(delay))
            {
                m = from;
            }

            protected override void OnTick()
            {
                if (m == null || m.Deleted)
                    return;

                ((IKhaerosMobile)m).StunnedTimer = null;
                m.SendMessage("You are unburdened of your guilt.");
            }
        }
    }
}