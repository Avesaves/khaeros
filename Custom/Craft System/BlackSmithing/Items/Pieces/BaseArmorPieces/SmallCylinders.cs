using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SmallCylinders : BaseArmorPiece
    {
        [Constructable]
        public SmallCylinders()
            : base(11020)
        {
            Weight = 3.0;
            Name = "Small Cylinders";
            ResourceAmount = 2;
        }

        public SmallCylinders(Serial serial)
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

