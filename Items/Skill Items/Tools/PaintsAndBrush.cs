using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{

    public class PaintsAndBrush : BaseTool
    {
        public override CraftSystem CraftSystem { get { return DefPainting.CraftSystem; } }

        [Constructable]
        public PaintsAndBrush() : base( 0xFC1 )
        {
            Weight = 1.0;
        }

        [Constructable]
		public PaintsAndBrush( int uses ) : base( uses, 0xFC1 )
		{
			Weight = 1.0;
		}

        public PaintsAndBrush( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int) 0 );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }
}
