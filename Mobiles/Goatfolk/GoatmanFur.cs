using System;

namespace Server.Items
{
	public class GoatmanFur : Item
	{
		[Constructable]
		public GoatmanFur() : base( 0x11F5 )
		{
			Weight = 1.0;
            Name = "goatman fur";
            Hue = 1631;
            Stackable = true;
		}

		public GoatmanFur( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				Stackable = true;
		}
	}
}
