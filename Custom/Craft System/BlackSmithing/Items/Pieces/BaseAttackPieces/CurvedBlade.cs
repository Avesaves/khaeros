using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class CurvedBlade : BaseAttackPiece
    {
        [Constructable]
        public CurvedBlade()
            : base(9568)
        {
            Weight = 6.0;
            Name = "A Curved Blade";
            ResourceAmount = 6;
        }

        public CurvedBlade(Serial serial)
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

