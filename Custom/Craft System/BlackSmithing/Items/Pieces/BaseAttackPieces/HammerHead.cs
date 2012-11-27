using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class HammerHead : BaseAttackPiece
    {
        [Constructable]
        public HammerHead()
            : base(3706)
        {
            Weight = 12.0;
            Name = "A Hammer Head";
            ResourceAmount = 6;
        }

        public HammerHead(Serial serial)
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

