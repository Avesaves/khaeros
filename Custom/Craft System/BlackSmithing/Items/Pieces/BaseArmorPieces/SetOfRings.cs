using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class SetOfRings : BaseArmorPiece
    {
        [Constructable]
        public SetOfRings()
            : base(4179)
        {
            Weight = 6.0;
            Name = "A Set of Rings";
            ResourceAmount = 6;
        }

        public SetOfRings(Serial serial)
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

