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
    public class HumilitySacrifice : BaseCustomSpell
    {
        public override bool AffectsMobiles { get { return true; } }
        public override bool UsesTarget { get { return false; } }
        public override bool UsesFullEffect { get { return true; } }
        public override bool IsHarmful { get { return false; } }
        public override FeatList Feat { get { return FeatList.Humility; } }
        public override string Name { get { return "Humility Sacrifice"; } }
        public override int BaseCost { get { return 1; } }
        public override double FullEffect { get { return 1; } }
        public override double PartialEffect { get { return 0.5; } }
        public override bool CanTargetSelf { get { return false; } }

        public HumilitySacrifice(Mobile caster, int featLevel)
            : base(caster, featLevel)
        {
        }

        public override void Effect()
        {
            if (CasterHasEnoughMana)
            {
                Caster.Mana -= TotalCost;
                FinalEffect(Caster, TargetMobile, TotalEffect);
                Success = true;
            }
        }

        public static void FinalEffect(Mobile caster, Mobile target, int effect)
        {
            IPooledEnumerable eable = caster.Map.GetMobilesInRange(caster.Location, caster.ManaMax / 10);
            foreach (Mobile m in eable)
            {
                if ( ((PlayerMobile)caster).AllyList.Contains( m ) )
                {
                    int heal = m.HitsMax * effect;
                    m.PlaySound(0x1F2);
                    m.FixedEffect(0x376A, 9, 32);
                    m.Hits += heal;
                    m.LocalOverheadMessage(MessageType.Regular, 170, false, "+" + heal);
                }
            }
            eable.Free();

            caster.SendMessage("You have sacrificed yourself!");
            caster.DoSpeech("*collapses with a thud!*", new int[] { -1 }, MessageType.Emote, caster.EmoteHue);
            caster.Kill();
        }

        public static void Initialize()
        {
            CommandSystem.Register("HumilitySacrifice", AccessLevel.Player, new CommandEventHandler(HumilitySacrifice_OnCommand));
        }

        [Usage("HumilitySacrifice")]
        [Description("You may sacrifice yourself for the good of the many.")]
        private static void HumilitySacrifice_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile != null)
            {
                if (e.Mobile is PlayerMobile && ((PlayerMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Humility) < 3)
                {
                    e.Mobile.SendMessage("You lack the humility for this feat.");
                    return;
                }
                SpellInitiator(new HumilitySacrifice(e.Mobile, GetSpellPower(e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Humility))));
            }
        }
    }
}