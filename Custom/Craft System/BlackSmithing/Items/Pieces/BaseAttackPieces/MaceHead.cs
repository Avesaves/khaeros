using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MaceHead : BaseAttackPiece
    {
        [Constructable]
        public MaceHead()
            : base(3699)
        {
            Weight = 12.0;
            Name = "A Mace Head";
            ResourceAmount = 4;
        }

        public MaceHead(Serial serial)
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

