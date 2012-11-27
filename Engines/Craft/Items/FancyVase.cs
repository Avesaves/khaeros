using System;

namespace Server.Items
{
    public class FancyVase : Item
    {
        [Constructable]
        public FancyVase()
            : base( 0xB48 )
        {
            Weight = 10;
            Name = "Fancy Vase";
        }

        public FancyVase(Serial serial)
            : base( serial )
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class LargeFancyVase : Item
    {
        [Constructable]
        public LargeFancyVase()
            : base( 0xB47 )
        {
            Weight = 15;
            Name = "Large Fancy Vase";
        }

        public LargeFancyVase(Serial serial)
            : base( serial )
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class TallVase : Item
    {
        [Constructable]
        public TallVase()
            : base( 0x31EB )
        {
            Weight = 15;
            Name = "Tall Vase";
        }

        public TallVase(Serial serial)
            : base( serial )
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class OrnamentedVase : Item
    {
        [Constructable]
        public OrnamentedVase()
            : base( 0x31EC )
        {
            Weight = 15;
            Name = "Ornamented Vase";
        }

        public OrnamentedVase(Serial serial)
            : base( serial )
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }

    public class Basin : Item
    {
        [Constructable]
        public Basin()
            : base( 0xE78 )
        {
            Weight = 15;
            Name = "Basin";
        }

        public Basin(Serial serial)
            : base( serial )
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }
}
