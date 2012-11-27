using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class LargePlate : BaseArmorPiece
    {
        [Constructable]
        public LargePlate()
            : base(15969)
        {
            Weight = 10.0;
            Name = "A Large Metal Plate";
            ResourceAmount = 10;
        }

        public LargePlate(Serial serial)
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

