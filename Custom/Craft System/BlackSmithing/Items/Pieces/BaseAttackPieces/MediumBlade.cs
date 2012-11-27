using System;
using Server;
using Server.Items;

namespace Server.Items
{
    public class MediumBlade : BaseAttackPiece
    {
        [Constructable]
        public MediumBlade()
            : base(9587)
        {
            Weight = 8.0;
            Name = "A Medium Blade";
            ResourceAmount = 5;
        }

        public MediumBlade(Serial serial)
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

