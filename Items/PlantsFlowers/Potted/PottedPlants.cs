using System;

namespace Server.Items
{
	public class PottedPlant : Item
	{
		[Constructable]
		public PottedPlant() : base(0x11CA)
		{
			Weight = 100;
		}

		public PottedPlant(Serial serial) : base(serial)
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

	public class PottedPlant1 : Item
	{
		[Constructable]
		public PottedPlant1() : base(4554)
		{
			Weight = 3.0;
		}

		public PottedPlant1(Serial serial) : base(serial)
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
	
	public class PottedPlant2 : Item
	{
		[Constructable]
		public PottedPlant2() : base(4555)
		{
			Weight = 3.0;
		}

		public PottedPlant2(Serial serial) : base(serial)
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
	
	public class PottedPlant3 : Item
	{
		[Constructable]
		public PottedPlant3() : base(4556)
		{
			Weight = 3.0;
		}

		public PottedPlant3(Serial serial) : base(serial)
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
