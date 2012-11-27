using System;

namespace Server.Items
{
	public class Tambourine : BaseInstrument
	{
		[Constructable]
		public Tambourine() : base( 0x3BCA, 82, 83 )
		{
			Weight = 1.0;
			Layer = Layer.TwoHanded;
			Name = "Tambourine";
		}

		public Tambourine( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Weight == 2.0 )
				Weight = 1.0;
			
			if( version < 1 )
			{
				ItemID = 0x3BCA;
				Layer = Layer.TwoHanded;
			}
		}
	}
}
