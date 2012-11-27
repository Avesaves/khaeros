using System;

namespace Server.Items
{
	public class PottedTree : Item
	{
		[Constructable]
		public PottedTree() : base(0x11C8)
		{
			Weight = 100;
		}

		public PottedTree(Serial serial) : base(serial)
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
		}
	}

	public class PottedTree1 : Item
	{
		[Constructable]
		public PottedTree1() : base(4552)
		{
			Weight = 20.0;
		}

		public PottedTree1(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
	
	public class PottedTree2 : Item
	{
		[Constructable]
		public PottedTree2() : base(4553)
		{
			Weight = 20.0;
		}

		public PottedTree2(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
