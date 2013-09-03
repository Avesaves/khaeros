using Server;
using Server.Items;

namespace Khaeros.Scripts.Khaeros.Custom.Vhaerun_s_CRL_Homestead.Vhaerun_s_CRL_Cooking.Base_Cooking
{
    public abstract class ItemWithMultipleUses : Item, IHasQuantity
    {
        private int m_Quantity;

        public ItemWithMultipleUses(int id) :base(id)
        {
            m_Quantity = 6;
        }

        public ItemWithMultipleUses(Serial serial) : base(serial)
        {}

        [CommandProperty(AccessLevel.GameMaster)]
        public int Quantity
        {
            get { return m_Quantity; }
            set { CalculateQuantity(value); }
        }

        void CalculateQuantity(int value)
        {
            if (value < 0)
                value = 0;
            else if (value > 6)
                value = 6;

            m_Quantity = value;

            InvalidateProperties();

            if (m_Quantity == 0)
                Delete();
            else if (m_Quantity < 6 && (ItemID == 0x1039 || ItemID == 0x1045))
                ++ItemID;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060584, m_Quantity.ToString()); // uses remaining: ~1_val~
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version

            writer.Write((int)m_Quantity);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        m_Quantity = reader.ReadInt();
                        break;
                    }
                case 0:
                    {
                        m_Quantity = 6;
                        break;
                    }
            }
        }
    }
}
