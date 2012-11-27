using System;

namespace Server.Items
{
	public class PlanarEssence : Item
	{
		[Constructable]
		public PlanarEssence() : base( 0x3735 )
		{
			Weight = 1.0;
            Name = "planar essence";
            Hue = 2968;
		}

		public PlanarEssence( Serial serial ) : base( serial )
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
