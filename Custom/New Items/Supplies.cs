using System;

namespace Server.Items
{
    public class Supplies : Item
    {
        [Constructable]
        public Supplies()
            : base( 1109 )
        {
            Weight = 75.0;
            Name = "Supplies";
            Stackable = false;
        }

        public Supplies( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }
}
