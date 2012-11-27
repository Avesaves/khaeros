using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class LargeCylinders : BaseArmorPiece
    {
        [Constructable]
        public LargeCylinders()
            : base(10194)
        {
            Weight = 6.0;
            Name = "Large Cylinders";
            ResourceAmount = 8;
        }

        public LargeCylinders(Serial serial)
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
    }
}

