using System;

namespace Server.Items
{
	public class Lute : BaseInstrument
	{
		[Constructable]
		public Lute() : base( 0x3BC9, 76, 77 )
		{
			Weight = 2.0;
			Layer = Layer.TwoHanded;
			Name = "Lute";
		}

		public Lute( Serial serial ) : base( serial )
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

			if ( Weight == 5.0 )
				Weight = 2.0;
			
			if( version < 1 )
			{
				ItemID = 0x3BC9;
				Layer = Layer.TwoHanded;
			}
		}
	}
}
