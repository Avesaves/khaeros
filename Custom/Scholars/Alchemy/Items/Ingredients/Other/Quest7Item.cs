using System;

namespace Server.Items
{
    public class Quest7Item : Item, IGem
	{
		[Constructable]
		public Quest7Item() : base( 0x3155 )
		{
			Weight = 3.0;
            Name = "The heart of a fallen star";
            Hue = 2832;
		}

		public Quest7Item( Serial serial ) : base( serial )
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
