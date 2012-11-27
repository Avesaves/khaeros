using System;
using Server;

namespace Server.Items
{
    public class VisualTarget : Item
    {
        [Constructable]
        public VisualTarget() : this( Map.Felucca, Point3D.Zero ) { }

        [Constructable]
        public VisualTarget( Map map, Point3D location ) : base( 6126 )
        {
            Weight = 0.0;
            Movable = false;

            if( location != Point3D.Zero )
                MoveToWorld( location, map );
        }

        public VisualTarget( Serial serial )
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
            Delete();
        }
    }
}
