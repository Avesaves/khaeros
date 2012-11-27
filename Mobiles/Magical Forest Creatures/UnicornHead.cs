using System;

namespace Server.Items
{
	public class UnicornHead : Item
	{
		[Constructable]
		public UnicornHead() : base( 0x3159 )
		{
			Weight = 1.0;
            Name = "unicorn head";
		}

		public UnicornHead( Serial serial ) : base( serial )
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
