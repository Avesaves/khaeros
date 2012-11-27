using System;
using Server;
using Server.Network;
using Server.Items;
using System.Collections;

namespace Server.Items
{
    public class SGLocationAddon2South : BaseAddon
    {
        [Constructable]
        public SGLocationAddon2South()
        {
            AddComponent(new AddonComponent(2321), 0, 0, 0); 
        }

        public SGLocationAddon2South(Serial serial) : base(serial)
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

    public class SGLocationLantern2South : AddonComponent
    {
        [Constructable]
        public SGLocationLantern2South() : base(2572)
        {
            Name = "A Torch";
            Light = LightType.Circle225;
        }

        public SGLocationLantern2South(Serial serial) : base(serial)
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