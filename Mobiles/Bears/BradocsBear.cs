using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    public class BradocsBear : Bear, IForestCreature
    {
        public override int[] Hues { get { return new int[] { 2110, 2111, 2112 }; } }

        [Constructable]
        public BradocsBear()
            : base()
        {            
            Hue = Utility.RandomMinMax(2110, 2112);
            NewBreed = "Bradoc's Bear";
        }

        public override void PrepareToGiveBirth()
        {
            GiveBirth(new BradocsBear());
        }

        public BradocsBear(Serial serial)
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