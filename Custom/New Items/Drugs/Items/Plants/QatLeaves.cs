using System;
using Server.Engines.Alchemy;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class QatLeaves : BaseIngredient
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
        public QatLeaves(int amount)
            : base(0x1B1F, amount)
        {
            Name = "qat leaves";
            Hue = 1445;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile && from.Backpack != null && IsChildOf(from.Backpack))
            {
                from.SendMessage("Target a mortar to grind this up in.");
                from.Target = new PickTarget(this);
            }
        }

        [Constructable]
        public QatLeaves()
            : this(1)
        {
        }

        public QatLeaves(Serial serial)
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
            private QatLeaves m_QatLeaf;
            public PickTarget(QatLeaves item)
                : base(15, false, TargetFlags.None)
            {
                m_QatLeaf = item;
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                if (from is PlayerMobile && from.Backpack != null && m_QatLeaf.IsChildOf(from.Backpack))
                {
                    if (targ is AlchemyTool)
                    {
                        Item mp = targ as Item;
                        if (mp.IsChildOf(from.Backpack))
                        {
                            m_QatLeaf.Consume(1);
                            Qat qatDrug = new Qat();
                            from.Backpack.DropItem(qatDrug);
                            from.PlaySound(0x242);
                            from.SendMessage("You grind the leaves into chew.");
                        }
                        else
                            from.SendMessage("That needs to be in your pack.");
                    }
                    else
                        from.SendMessage("You must target a mortar to grind that up!");
                }
            }
        }
    }
}