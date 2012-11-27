using System;
using Server;
using Server.Network;
using Server.Items;
using System.Collections;

namespace Server.Items
{
    public class SGLocationAddon3South : BaseAddon
    {
        [Constructable]
        public SGLocationAddon3South ()
        {
            AddComponent(new AddonComponent(2321), 0, 0, 0); 
        }

        public SGLocationAddon3South(Serial serial) : base(serial)
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

    public class SGLocationLantern3South : AddonComponent
    {
        [Constructable]
        public SGLocationLantern3South() : base(2572)
        {
            Name = "A Torch";
            Light = LightType.Circle225;
        }

        public SGLocationLantern3South(Serial serial) : base(serial)
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

    public class SGBlockA3Step : AddonComponent
    {
        [Constructable]
        public SGBlockA3Step() : base(1873)
        {
            
        }

        public SGBlockA3Step(Serial serial) : base(serial)
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

    public class SGBlockA3Step2 : AddonComponent
    {
        [Constructable]
        public SGBlockA3Step2() : base(1875)
        {
            
        }

        public SGBlockA3Step2(Serial serial) : base(serial)
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

    public class SGBlockA3Corner1 : AddonComponent
    {
        [Constructable]
        public SGBlockA3Corner1()
            : base(1877)
        {
            
        }

        public SGBlockA3Corner1(Serial serial)
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

    public class SGBlockA3Corner2 : AddonComponent
    {
        [Constructable]
        public SGBlockA3Corner2() : base(117)
        {
            
        }

        public SGBlockA3Corner2(Serial serial) : base(serial)
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

    public class SGBlockA3Corner3 : AddonComponent
    {
        [Constructable]
        public SGBlockA3Corner3() : base(116)
        {
            
        }

        public SGBlockA3Corner3(Serial serial) : base(serial)
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

    public class SGBlockA3Top : AddonComponent
    {
        [Constructable]
        public SGBlockA3Top() : base(8783)
        {
            
        }

        public SGBlockA3Top(Serial serial) : base(serial)
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