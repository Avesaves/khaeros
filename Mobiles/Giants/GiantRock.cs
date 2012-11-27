using System;

namespace Server.Items
{
	public class GiantRock : Item
	{
		[Constructable]
		public GiantRock() : base( 0x1363 )
		{
			Weight = 30.0;
		}

		public GiantRock( Serial serial ) : base( serial )
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
