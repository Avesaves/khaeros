using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class AxeHead : BaseAttackPiece
    {
        [Constructable]
        public AxeHead()
            : base(3907)
        {
            Weight = 10.0;
            Name = "An Axe Head";
            ResourceAmount = 5;
        }

        public AxeHead(Serial serial)
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

