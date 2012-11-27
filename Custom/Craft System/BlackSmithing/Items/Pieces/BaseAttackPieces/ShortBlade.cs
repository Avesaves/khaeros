using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class ShortBlade : BaseAttackPiece
    {
        [Constructable]
        public ShortBlade()
            : base(2550)
        {
            Weight = 1.0;
            Name = "A Short Blade";
            ResourceAmount = 3;
        }

        public ShortBlade(Serial serial)
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

