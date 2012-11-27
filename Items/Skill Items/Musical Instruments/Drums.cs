using System;

namespace Server.Items
{
	public class Drums : BaseInstrument
	{
		[Constructable]
		public Drums() : base( 0x3BC7, 748, 749 )
		{
			Weight = 3.0;
			Layer = Layer.TwoHanded;
			Name = "Drums";
		}

		public Drums( Serial serial ) : base( serial )
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

			if ( Weight == 4.0 )
				Weight = 3.0;
			
			if( version < 1 )
			{
				ItemID = 0x3BC7;
				Layer = Layer.TwoHanded;
			}
		}
	}
}
