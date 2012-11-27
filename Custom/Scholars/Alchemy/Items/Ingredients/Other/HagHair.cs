using System;

namespace Server.Items
{
	public class HagHair : Item
	{
		[Constructable]
		public HagHair() : base( 0x3BCB )
		{
			Weight = 3.0;
            Name = "hag hair";
            Hue = 2984;
		}

		public HagHair( Serial serial ) : base( serial )
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
