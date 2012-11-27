using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.Targeting;
using Server.Network;

namespace Server.Misc
{
    public class HumilityShield : BaseCustomSpell
    {
        public override bool AffectsMobiles { get { return true; } }
        public override bool UsesTarget { get { return true; } }
        public override bool UsesFullEffect { get { return true; } }
        public override FeatList Feat { get { return FeatList.Humility; } }
        public override string Name { get { return "Humility Shield"; } }
        public override int BaseCost { get { return 10; } }
        public override double FullEffect { get { return FeatLevel + ( Caster.Mana / 33 ) ; } }
        public override double PartialEffect { get { return FeatLevel; } }

        public HumilityShield(Mobile caster, int featLevel)
            : base(caster, featLevel)
        {
        }

        public override void Effect()
        {
            if (TargetCanBeAffected && CasterHasEnoughMana)
            {
                if (((IKhaerosMobile)TargetMobile).ShieldingMobile != null || ((IKhaerosMobile)TargetMobile).ShieldedMobile != null)
                {
                    Caster.SendMessage("That target is already involved in another Humility Shield.");
                    return;
                }

                if (((IKhaerosMobile)Caster).ShieldingMobile != null || ((IKhaerosMobile)Caster).ShieldedMobile != null)
                {
                    Caster.SendMessage("You are already involved in another Humility Shield.");
                    return;
                }

                Caster.Mana -= TotalCost;
                FinalEffect(Caster, TargetMobile, (int)FullEffect, (int)PartialEffect);
                Success = true;
            }
        }

        public static void FinalEffect(Mobile caster, Mobile target, int protection, int duration)
        {
            target.PlaySound(0x202);
            ((IKhaerosMobile)target).ShieldingMobile = caster;
            ((IKhaerosMobile)target).ShieldValue = protection;
            ((IKhaerosMobile)caster).ShieldedMobile = target;
            Timer.DelayCall(TimeSpan.FromMinutes(duration), new TimerCallback(((IKhaerosMobile)target).RemoveShieldOfSacrifice));
            Timer.DelayCall(TimeSpan.FromMinutes(duration), new TimerCallback(((IKhaerosMobile)caster).RemoveShieldOfSacrifice));
        }

        public static void Initialize()
        {
            CommandSystem.Register("HumilityShield", AccessLevel.Player, new CommandEventHandler(HumilityShield_OnCommand));
        }

        [Usage("HumilityShield")]
        [Description("Shield another with your own sense of humility.")]
        private static void HumilityShield_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile != null)
            {
                if(e.Mobile is PlayerMobile && ((PlayerMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Humility) < 1)
                {
                    e.Mobile.SendMessage("You lack the humility for this feat.");
                    return;
                }
                SpellInitiator(new HumilityShield(e.Mobile, GetSpellPower(e.ArgString, ((IKhaerosMobile)e.Mobile).Feats.GetFeatLevel(FeatList.Humility))));
            }
        }
    }
}
