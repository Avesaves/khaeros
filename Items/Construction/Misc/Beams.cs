using System;

namespace Server.Items
{

[Furniture]
	[Flipable(0x39F, 0x39C, 0x39D, 0x39E)]
	public class Beam : Item, ICanBeStained
	{
		[Constructable]
		public Beam() : base(0x39C)
		{
			Weight = 5.0;
			Movable = true;
		}

		public Beam(Serial serial) : base(serial)
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
				Weight = 5.0;
		}
	}
}
