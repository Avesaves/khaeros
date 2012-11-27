using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SmallPlate : BaseArmorPiece
    {
        [Constructable]
        public SmallPlate()
            : base(15534)
        {
            Weight = 5.0;
            Name = "A Small Metal Plate";
            ResourceAmount = 4;
        }

        public SmallPlate(Serial serial)
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

