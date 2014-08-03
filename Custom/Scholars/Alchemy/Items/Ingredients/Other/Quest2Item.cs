using System;

namespace Server.Items
{
    public class Quest2Item : Item, IGem
	{
		[Constructable]
		public Quest2Item() : base( 0xF0A )
		{
			Weight = 3.0;
            Name = "strange plant serum";
            Hue = 2832;
		}

		public Quest2Item( Serial serial ) : base( serial )
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
