using System;

namespace Server.Items
{
    public class Quest4Item : Item, IGem
	{
		[Constructable]
		public Quest4Item() : base( 0x1CDD )
		{
			Weight = 3.0;
            Name = "the arm of a monster";
            Hue = 2832;
		}

		public Quest4Item( Serial serial ) : base( serial )
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
