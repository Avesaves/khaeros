using System;

namespace Server.Items
{
    public class Quest6Item : Item, IGem
	{
		[Constructable]
		public Quest6Item() : base( 0x3155 )
		{
			Weight = 3.0;
            Name = "The heart of a fallen star";
            Hue = 2832;
		}

		public Quest6Item( Serial serial ) : base( serial )
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
