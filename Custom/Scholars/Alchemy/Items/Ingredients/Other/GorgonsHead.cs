using System;

namespace Server.Items
{
	public class GorgonsHead : Item
	{
		[Constructable]
		public GorgonsHead() : base( 0x1CE9 )
		{
			Weight = 3.0;
            Name = "gorgon's head";
            Hue = 1322;
		}

		public GorgonsHead( Serial serial ) : base( serial )
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
