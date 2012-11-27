using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Accounting;
using Server.Targeting;
using Server.Targets;

namespace Server.Items
{
    public class ResourceBit : Item
    {
        private ResourceType m_Resource;
        private int m_Value;

        [CommandProperty(AccessLevel.GameMaster)]
        public ResourceType Resource { get { return m_Resource; } set { m_Resource = value; if (m_Value != null && m_Resource != null) Name = m_Value + " " + m_Resource.ToString(); SetResourceBitAppearance(); } }
        [CommandProperty(AccessLevel.GameMaster)]
        public int Value { get { return m_Value; } set { m_Value = value; if (m_Value != null && m_Resource != null) Name = m_Value + " " + m_Resource.ToString(); } }

        [Constructable]
        public ResourceBit(ResourceType resource, int amount) : base(0x0EE7)
        {
            Resource = resource;
            Value = amount;
            SetResourceBitAppearance();

            Weight = Value / (Utility.RandomMinMax(2, 10));

            Stackable = true;
        }

        public override bool StackWith(Mobile from, Item dropped)
        {
            if (dropped is ResourceBit)
            {
                ResourceBit newBit = dropped as ResourceBit;
                if (newBit.Resource == Resource)
                {
                    this.Amount += newBit.Amount;
                    newBit.Delete();

                    if (from != null)
                    {
                        from.SendSound(0x42, GetWorldLocation());
                    }
                }
                else
                    return false;
            }

            return false;
        }

        public ResourceBit(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);

            writer.Write((int)m_Resource);
            writer.Write((int)m_Value);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Resource = (ResourceType)reader.ReadInt();
            m_Value = (int)reader.ReadInt();
        }

        public void SetResourceBitAppearance()
        {
            switch (Resource)
            {
                case ResourceType.Metals:
                    if (Utility.RandomBool())
                        ItemID = 0x1BF1;
                    else
                        ItemID = 0x1BF4;
                    break;
                case ResourceType.Wood:
                    if (Utility.RandomBool())
                        ItemID = 0x1BDE;
                    else
                        ItemID = 0x1BE1;
                    break;
                case ResourceType.Cloth:
                    ItemID = Utility.RandomMinMax(3989, 3996);
                    break;
                case ResourceType.Food:
                    int randomGraphic = Utility.Random(1, 12);
                    switch (randomGraphic)
                    {
                        case 1: ItemID = 0x08AA; break;
                        case 2: ItemID = 0x08AC; break;
                        case 3: ItemID = 0x0481; break;
                        case 4: ItemID = 0x08FE; break;
                        case 5: ItemID = 0x0908; break;
                        case 6: ItemID = 0x090A; break;
                        case 7: ItemID = 0x090C; break;
                        case 8: ItemID = 0x12B4; break;
                        case 9: ItemID = 0x1399; break;
                        case 10: ItemID = 0x149B; break;
                        case 11: if (Utility.RandomBool()) ItemID = 0x1E88; else ItemID = 0x1E89; break;
                        case 12: if (Utility.RandomBool()) ItemID = 0x1E90; else ItemID = 0x1E91; break;
                    }
                    break;
                case ResourceType.Water:
                    if (m_Value != null)
                    {
                        if (m_Value < 50)
                            ItemID = 0x0FFA;
                        else
                            ItemID = 0x154D;
                    }
                    else
                        ItemID = 0x0E7B;
                    break;
                case ResourceType.Influence:
                    ItemID = 0x0E73;
                    break;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile && this.IsChildOf(from.Backpack))
                from.Target = new ResourceBitTarget(this);

            base.OnDoubleClick(from);
        }

        public void Give(Mobile from, GovernmentEntity g)
        {
            if (m_Value < 0 || m_Resource == null)
                Delete();
            else
            {
                if (!g.Resources.ContainsKey(m_Resource))
                {
                    from.SendMessage(g.Name.ToString() + " is unable to receive that resource.");
                    return;
                }

                from.SendMessage("You give " + m_Value.ToString() + " " + m_Resource.ToString() + " to " + g.Name.ToString() + ".");
            }

            g.Resources[m_Resource] += m_Value;

            switch (m_Resource)
            {
                case ResourceType.Metals:
                    from.PlaySound(0x02A);
                    break;
                case ResourceType.Wood:        
                    from.PlaySound(0x23D);
                    break;
                case ResourceType.Cloth: 
                    from.PlaySound(0x3E3);
                    break;
                case ResourceType.Food:          
                    from.PlaySound(0x247);
                    break;
                case ResourceType.Water:
                    from.PlaySound(0x026);
                    break;
                case ResourceType.Influence:
                    from.PlaySound(0x5B4);
                    break;
            }

            this.Delete();
        }
    }

    public class ResourceBitTarget : Target
    {
        private ResourceBit m_ResourceBit;

        public ResourceBitTarget(ResourceBit bit) : base(2,true,TargetFlags.None)
        {
            m_ResourceBit = bit;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is GovernmentEntity)
                m_ResourceBit.Give(from, targeted as GovernmentEntity);
            else
                from.SendMessage("You must target a government's official stone in order to give it these resources.");

            base.OnTarget(from, targeted);
        }
    }
}