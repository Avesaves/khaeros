using System;
using Server.Engines.Alchemy;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class Opium : BaseIngredient
    {
        public override KeyValuePair<CustomEffect, int>[] Effects
        {
            get
            {
                return new KeyValuePair<CustomEffect, int>[] {
			};
            }
        }

        public override int SkillRequired { get { return 500; } }

        [Constructable]
        public Opium(int amount)
            : base(0x103D, amount)
        {
            Name = "opium";
            Hue = 2017;
            Stackable = true;
			Amount = amount;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile && from.Backpack != null && IsChildOf(from.Backpack))
            {
                from.SendMessage("Target a pipe to smoke this in.");
                from.Target = new PickTarget(this);
            }
        }

        [Constructable]
        public Opium()
            : this(1)
        {
        }

        public Opium(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        private class PickTarget : Target
        {
            private Opium m_Opium;
            public PickTarget(Opium item)
                : base(15, false, TargetFlags.None)
            {
                m_Opium = item;
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                if (from is PlayerMobile && from.Backpack != null && m_Opium.IsChildOf(from.Backpack))
                {
                    if (targ is Pipe)
                    {
                        Pipe pipe = targ as Pipe;
                        if (pipe.RootParent == from)
                        {
                            if (pipe.ContentRemaining > 0)
                                from.SendMessage("That pipe still contains some substance. Empty it first.");
                            else
                            {
                                m_Opium.Consume(1);
                                pipe.ContentRemaining = 3;
                                pipe.ContentType = ContentType.Opium;
                                from.SendMessage("You refill your pipe.");
                            }
                        }
                        else
                            from.SendMessage("That needs to be in your pack.");
                    }
                    else
                        from.SendMessage("You must target a smoking pipe.");
                }
            }
        }
    }
}
