using System;

namespace Server.Items
{
    public class Quest3Item : Item, IGem
	{
		[Constructable]
		public Quest3Item() : base( 0xF8E )
		{
			Weight = 3.0;
            Name = "A piece of a blue elemental";
            Hue = 2832;
		}

		public Quest3Item( Serial serial ) : base( serial )
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
