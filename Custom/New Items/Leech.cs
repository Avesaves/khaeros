using System;
using Server;
using Server.Targets;
using Server.Targeting;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    public class Leech : Item
    {
        [Constructable]
        public Leech()
            : base(0x0979)
        {
            Hue = 1896;
            Name = "a leech";
        }

        public Leech(Serial serial)
            : base(serial)
        {

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (this.RootParentEntity == from && from.HasFreeHand())
            {
                from.PlaySound(0x384);
                from.Target = new LeechTarget(this);
                from.Emote("*readies a leech in " + (from.Female ? "her" : "his") + " hand*");
                from.SendMessage("Choose a target whose blood you wish to leech.");
            }
            else if (this.RootParentEntity != from)
                from.SendMessage("That must be in your backpack to use it.");
            else if (!from.HasFreeHand())
                from.SendMessage("You must have a free hand to do that.");

            base.OnDoubleClick(from);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }

        private class LeechTarget : Target
        {
            private Leech m_Leech;

            public LeechTarget(Leech l)
                : base(1, true, TargetFlags.Beneficial)
            {
                m_Leech = l;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Leech == null || m_Leech.Deleted)
                    return;

                if (targeted is Mobile && from.InRange((targeted as Mobile).Location, 1) && m_Leech.RootParentEntity == from)
                {
                    Mobile m = targeted as Mobile;

                    if (m.Warmode)
                    {
                        from.SendMessage("You cannot leech that person while they are busy.");
                        return;
                    }
                    if (m.Combatant != null)
                    {
                        m.Combatant = null;
                    }
                    if (from.Warmode)
                    {
                        from.SendMessage("You cannot leech that person while you are busy.");
                        return;
                    }
                    if (from.Combatant != null)
                    {
                        from.Combatant = null;
                    }

                    from.SendMessage("You attach the leech to " + m.Name + ".");
                    m.SendMessage(from.Name + " has placed a leech onto your flesh.");
                    LeechTimer newTimer = new LeechTimer(m);
                    newTimer.Start();
                    m_Leech.Consume(1);
                }
                else if (!(targeted is Mobile))
                {
                    from.SendMessage("You cannot leech that.");
                    return;
                }
                else if (!from.InRange((targeted as Mobile).Location, 1))
                {
                    from.SendMessage("You are too far away to do that.");
                    return;
                }
                else if (m_Leech.RootParentEntity != from)
                {
                    from.SendMessage("The leech must be in your possession for you to use it.");
                    return;
                }

                base.OnTarget(from, targeted);
            }

            private class LeechTimer : Timer
            {
                private Mobile m_Target;
                private int m_CountDown;

                public LeechTimer(Mobile leeched)
                    : base(TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20)), TimeSpan.FromSeconds(Utility.RandomMinMax(15, 45)))
                {
                    m_Target = leeched;
                    m_CountDown = Utility.RandomMinMax(1, 5);
                }

                protected override void OnTick()
                {
                    if (m_Target == null || m_Target.Deleted || !m_Target.Alive || (m_Target is PlayerMobile && (m_Target as PlayerMobile).RessSick))
                        return;

                    m_Target.SendMessage("The leech clings to your flesh, sucking at your blood.");
                    m_Target.PlaySound(0x386);
                    m_Target.Damage(Utility.RandomMinMax(1, 5));
                    if (m_Target is PlayerMobile && (m_Target as PlayerMobile).IsVampire)
                        (m_Target as PlayerMobile).BPs--;

                    if (Utility.RandomMinMax(1, 100) < m_Target.Stam)
                    {
                        XmlAttachment poison = XmlAttach.FindAttachment(m_Target, typeof(PoisonAttachment));
                        if (poison != null &&  ( Utility.RandomMinMax(1,100) > (poison as PoisonAttachment).PoisonStrength ) )
                        {
                            ((PoisonAttachment)poison).WearOff();
                        }
                    }

                    m_CountDown--;

                    if (m_CountDown <= 0)
                    {
                        m_Target.SendMessage("The leech falls from your flesh, sated.");
                        Stop();
                        return;
                    }

                    base.OnTick();
                }
            }
        }
    }

    public class LargeLeech : BaseCreature
    {
        public override int Meat { get { return 1; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }
        public override bool ParryDisabled
        {
            get
            {
                return true;
            }
        }

        [Constructable]
        public LargeLeech() :
            base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a large leech";
            Body = 52;
            Hue = 1896;
            BaseSoundID = 0x384;

            SetDamageType(ResistanceType.Piercing, 50);

            SetStr(5, 10);
            SetDex(1, 10);
            SetHits(10, 25);

            DamageMin = 1;
            DamageMax = 2;

            Fame = 100;
            Karma = 0;

            AddToBackpack(new Leech());
        }

        public LargeLeech(Serial serial)
            : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}