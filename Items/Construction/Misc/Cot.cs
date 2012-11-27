using System;

namespace Server.Items
{
	[Furniture]
	[Flipable(0x11FD, 0x11FE, 0x11FF, 0x1200 )]
	public class Cot : Item, ICanBeStained
	{
		[Constructable]
		public Cot() : base(0x11FD)
		{
			Weight = 1.0;
			Name = "Cot";
		}

		public Cot(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if ( Weight == 10.0 )
				Weight = 1.0;
		}
	}
}
