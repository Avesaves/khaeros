using System;

namespace Server.Items
{
	public class QuaraphonLimb : Item
	{
		[Constructable]
		public QuaraphonLimb() : base( 0x1DB1 )
		{
			Weight = 3.0;
            Name = "quaraphon limb";
            Hue = 2583;
		}

		public QuaraphonLimb( Serial serial ) : base( serial )
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
