using System;
using Server.Engines.Alchemy;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items
{
    public class PoppyResin : BaseIngredient
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
        public PoppyResin(int amount)
            : base(0x1F13, amount)
        {
            Name = "poppy resin";
            Hue = 1527;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile && from.Backpack != null && IsChildOf(from.Backpack))
            {
                from.SendMessage("Target an oven or open flame to cook this in.");
                from.Target = new PickTarget(this);
            }
        }

        [Constructable]
        public PoppyResin()
            : this(1)
        {
        }

        public PoppyResin(Serial serial)
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
            private PoppyResin m_Resin;
            public PickTarget(PoppyResin item)
                : base(15, false, TargetFlags.None)
            {
                m_Resin = item;
            }

            protected override void OnTarget(Mobile from, object targ)
            {
                Item target = targ as Item;

                if (from is PlayerMobile && from.Backpack != null && m_Resin.IsChildOf(from.Backpack))
                {
                    if (IsHeatSource(target))
                    {
                        if (from.InRange(target.Location, 1) && from.CanSee(target))
                            {
                            m_Resin.Consume(1);
                            Opium opiumDrug = new Opium(2);
                            from.PlaySound(0x356);
                            from.Backpack.DropItem(opiumDrug);
                            from.SendMessage("You cook the resin until it becomes usable opium.");
                        }
                        else
                        {
                            from.SendMessage("You are too far away from that.");
                        }
                    }
                    else
                    {
                        from.SendMessage("You need a decent flame.");
                    }
                }
                else
                {
                    from.SendMessage("That must be in your backpack for you to use it.");
                }
            }

            public static bool IsHeatSource(object targeted)
            {
                int itemID;

                if (targeted is Item)
                    itemID = ((Item)targeted).ItemID & 0x3FFF;
                else if (targeted is StaticTarget)
                    itemID = ((StaticTarget)targeted).ItemID & 0x3FFF;
                else
                    return false;

                if (itemID >= 0xDE3 && itemID <= 0xDE9)
                    return true; // Campfire
                else if (itemID >= 0x461 && itemID <= 0x48E)
                    return true; // Sandstone oven/fireplace
                else if (itemID >= 0x92B && itemID <= 0x96C)
                    return true; // Stone oven/fireplace
                else if (itemID == 0xFAC)
                    return true; // Firepit
                else if (itemID >= 0x184A && itemID <= 0x184C)
                    return true; // Heating stand (left)
                else if (itemID >= 0x184E && itemID <= 0x1850)
                    return true; // Heating stand (right)
                else if (itemID >= 0x398C && itemID <= 0x399F)
                    return true; // Fire field

                return false;
            }
        }
    }
}
