using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    public class WasteBear : Bear, ITundraCreature
    {
        public override int[] Hues { get { return new int[] { 954, 955, 956 }; } }

        [Constructable]
        public WasteBear()
            : base()
        {
            Hue = Utility.RandomMinMax(954, 956);
            NewBreed = "Waste Bear";

        }

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new WasteBear());
        }

        public WasteBear(Serial serial)
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