using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    public class CorvinusBear : Bear, IPlainsCreature
    {
        public override int[] Hues { get { return new int[] { 1897, 1898, 1899 }; } }

        [Constructable]
        public CorvinusBear()
            : base()
        {   
            Hue = Utility.RandomMinMax(1897, 1899);
            NewBreed = "Corvinus Bear";

        }

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new CorvinusBear());
        }

        public CorvinusBear(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}