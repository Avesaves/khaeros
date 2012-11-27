using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SetOfScales : BaseArmorPiece
    {
        [Constructable]
        public SetOfScales()
            : base(3544)
        {
            Weight = 6.0;
            Name = "A Set of Metal Scales";
            ResourceAmount = 6;
        }

        public SetOfScales(Serial serial)
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

