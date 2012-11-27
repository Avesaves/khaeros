using System;

namespace Server.Items
{
	public class GiantsSkull : Item
	{
		[Constructable]
		public GiantsSkull() : base( 0x2203 )
		{
			Weight = 3.0;
            Name = "giant's skull";
		}

		public GiantsSkull( Serial serial ) : base( serial )
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
