using System;

namespace Server.Items
{
    public class Quest5Item : Item, IGem
	{
		[Constructable]
		public Quest5Item() : base( 0x223F )
		{
			Weight = 3.0;
            Name = "monstrous teeth";
            Hue = 2832;
		}

		public Quest5Item( Serial serial ) : base( serial )
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
