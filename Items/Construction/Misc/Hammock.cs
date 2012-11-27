using System;

namespace Server.Items
{
	[Furniture]
	[Flipable(0x11F0, 0x11F1, 0x11F2, 0x11F3)]
	public class Hammock : Item, ICanBeStained
	{
		[Constructable]
		public Hammock() : base(0x11F0)
		{
			Weight = 1.0;
			Name = "Hammock";
		}

		public Hammock(Serial serial) : base(serial)
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
