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
    public class HumilityAwe : BaseCustomSpell
    {
        public override bool AffectsMobiles { get { return true; } }
        public override bool UsesTarget { get { return true; } }
        public override bool UsesFullEffect { get { return true; } }
        public override bool IsHarmful { get { return true; } }
        public override FeatList Feat { get { return FeatList.Humility; } }
        public override string Name { get { return "Humility Awe"; } }
        public override int BaseCost { get { return 20; } }
        public override double FullEffect { get { return Caster.Int / 10; } }
        public override double PartialEffect { get { return Caster.Int / 20; } }
        public override bool CanTargetSelf { get { return false; } }

        public HumilityAwe(Mobile caster, int featLevel)
            : base(caster, featLevel)
        {
        }

        public override void Effect()
        {
            if (TargetCanBeAffected && CasterHasEnoughMana)
            {
                Caster.Mana -= TotalCost;
                FinalEffect(Caster, TargetMobile, TotalEffect, PartialEffect);
                Success = true;
            }
        }

        public static void FinalEffect(Mobile caster, Mobile target, int duration, double protection)
        {
            int[] keywords = new int[] { -1 };

            caster.DoSpeech("*stares at " + target.Name + " with disapproval*", keywords, MessageType.Emote, caster.EmoteHue);

            XmlAwe aweAtt = new XmlAwe((int)protection, duration);
            XmlAttach.AttachTo(target, aweAtt);
            target.SendMessage("You feel so small, so ashamed...");
            target.DoSpeech("*shrinks in awe*", keywords, MessageType.Emote, target.EmoteHue);
        }

        public static void Initialize()
        {
            CommandSystem.Register("HumilityAwe", AccessLevel.Player, new CommandEventHandler(HumilityAwe_OnCommand));
        }

        [Usage("HumilityAwe")]
        [Description("Inspire humility in others through your example.")]
        private static void HumilityAwe_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile != null)
            {
                if (e.Mobile is PlayerMobile && ((PlayerMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Humility) < 2)
                {
                    e.Mobile.SendMessage("You lack the humility for this feat.");
                    return;
                }
                SpellInitiator(new HumilityAwe(e.Mobile, GetSpellPower(e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Humility))));
            }
        }
    }
}