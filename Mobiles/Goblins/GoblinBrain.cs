using System;

namespace Server.Items
{
	public class GoblinBrain : Item
	{
		[Constructable]
		public GoblinBrain() : base( 0x1CEE )
		{
			Weight = 1.0;
            Name = "goblin brain";
            Hue = 2986;
		}

		public GoblinBrain( Serial serial ) : base( serial )
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
