using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class LongBlade : BaseAttackPiece
    {
        [Constructable]
        public LongBlade()
            : base(9597)
        {
            Weight = 8.0;
            Name = "A Long Blade";
            ResourceAmount = 8;
        }

        public LongBlade(Serial serial)
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

