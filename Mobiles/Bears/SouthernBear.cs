using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    public class SouthernBear : Bear, ICaveCreature
    {
        public override int[] Hues { get { return new int[] { 1047, 1048, 1049 }; } }

        [Constructable]
        public SouthernBear()
            : base()
        {
            Hue = Utility.RandomMinMax(1047, 1049);
            NewBreed = "Southern Bear";
        }

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new SouthernBear());
        }

        public SouthernBear(Serial serial)
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