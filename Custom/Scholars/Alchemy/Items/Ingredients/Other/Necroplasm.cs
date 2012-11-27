using System;

namespace Server.Items
{
	public class Necroplasm : Item
	{
		public Necroplasm() : this( 1 )
		{
		}
		
		[Constructable]
		public Necroplasm( int amount ) : base( 0xC7B )
		{
			Weight = 1.0;
            Name = "necroplasm";
            Hue = 2964;
            Stackable = true;
			Amount = amount;
		}

		public Necroplasm( Serial serial ) : base( serial )
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
