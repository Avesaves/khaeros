using System;

namespace Server.Items
{
    public class Quest1Item : Item, IGem
	{
		[Constructable]
		public Quest1Item() : base( 0xF8C )
		{
			Weight = 3.0;
            Name = "powder from a strange goblin";
            Hue = 2832;
		}

		public Quest1Item( Serial serial ) : base( serial )
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
