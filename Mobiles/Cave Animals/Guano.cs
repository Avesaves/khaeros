using System;

namespace Server.Items
{
	public class Guano : Item
	{
		[Constructable]
		public Guano() : base( 0xF3B )
		{
			Name = "guano";
			Hue = 1873;
			Weight = 1.0;
			Stackable = true;
		}

		public Guano( Serial serial ) : base( serial )
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
