using System;
using Server;
using Server.Network;
using Server.Items;
using System.Collections;

namespace Server.Items
{
    public class SGLocationAddon2East : BaseAddon
    {
        [Constructable]
        public SGLocationAddon2East ()
        {
            AddComponent(new AddonComponent(2321), 0, 0, 0); 
        }

        public SGLocationAddon2East(Serial serial) : base(serial)
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

    public class SGLocationLantern2East : AddonComponent
    {
        [Constructable]
        public SGLocationLantern2East() : base(2567)
        {
            Name = "A Torch";
            Light = LightType.Circle225;
        }

        public SGLocationLantern2East(Serial serial) : base(serial)
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

    public class SGFloorTile2 : AddonComponent
    {
        [Constructable]
        public SGFloorTile2() : base(1316)
        {
           
        }

        public SGFloorTile2(Serial serial) : base(serial)
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

    public class SGBlock2 : AddonComponent
    {
        [Constructable]
        public SGBlock2() : base(1822)
        {
           
        }

        public SGBlock2(Serial serial) : base(serial)
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

    public class SGBlock3 : AddonComponent
    {
        [Constructable]
        public SGBlock3() : base(1179)
        {
           
        }

        public SGBlock3(Serial serial) : base(serial)
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

    public class SGBlock4 : AddonComponent
    {
        [Constructable]
        public SGBlock4() : base(1846)
        {
           
        }

        public SGBlock4(Serial serial) : base(serial)
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

    public class SGBlock5 : AddonComponent
    {
        [Constructable]
        public SGBlock5() : base(1865)
        {
           
        }

        public SGBlock5(Serial serial) : base(serial)
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

    public class SGBlock6 : AddonComponent
    {
        [Constructable]
        public SGBlock6() : base(1866)
        {
           
        }

        public SGBlock6(Serial serial) : base(serial)
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

    public class SGBlock7 : AddonComponent
    {
        [Constructable]
        public SGBlock7() : base(1869)
        {
           
        }

        public SGBlock7(Serial serial) : base(serial)
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

    public class SGBlock8 : AddonComponent
    {
        [Constructable]
        public SGBlock8() : base(1868)
        {
           
        }

        public SGBlock8(Serial serial) : base(serial)
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

    public class SGBlock9 : AddonComponent
    {
        [Constructable]
        public SGBlock9() : base(1867)
        {
           
        }

        public SGBlock9(Serial serial) : base(serial)
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

    public class SGBlock10 : AddonComponent
    {
        [Constructable]
        public SGBlock10() : base(1847)
        {
           
        }

        public SGBlock10(Serial serial) : base(serial)
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

    public class SGBlock11 : AddonComponent
    {
        [Constructable]
        public SGBlock11() : base(1823)
        {
           
        }

        public SGBlock11(Serial serial) : base(serial)
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