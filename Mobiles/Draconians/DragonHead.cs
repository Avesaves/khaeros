using System;

namespace Server.Items
{
	public class DragonHead : Item
	{
		[Constructable]
		public DragonHead() : base( 0x2234 )
		{
			Weight = 1.0;
            Name = "a dragon head";
		}

		public DragonHead( Serial serial ) : base( serial )
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
