using System;

namespace Server.Items
{
	public class PottedCactus : Item
	{
		[Constructable]
		public PottedCactus() : base(0x1E0F)
		{
			Weight = 100;
		}

		public PottedCactus(Serial serial) : base(serial)
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

	public class PottedCactus1 : Item
	{
		[Constructable]
		public PottedCactus1() : base(7695)
		{
			Weight = 10.0;
		}

		public PottedCactus1(Serial serial) : base(serial)
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
	
	public class PottedCactus2 : Item
	{
		[Constructable]
		public PottedCactus2() : base(7696)
		{
			Weight = 10.0;
		}

		public PottedCactus2(Serial serial) : base(serial)
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
	
	public class PottedCactus3 : Item
	{
		[Constructable]
		public PottedCactus3() : base(7697)
		{
			Weight = 10.0;
		}

		public PottedCactus3(Serial serial) : base(serial)
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
	
	public class PottedCactus4 : Item
	{
		[Constructable]
		public PottedCactus4() : base(7698)
		{
			Weight = 10.0;
		}

		public PottedCactus4(Serial serial) : base(serial)
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
	
	public class PottedCactus5 : Item
	{
		[Constructable]
		public PottedCactus5() : base(7699)
		{
			Weight = 10.0;
		}

		public PottedCactus5(Serial serial) : base(serial)
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
	
	public class PottedCactus6 : Item
	{
		[Constructable]
		public PottedCactus6() : base(7700)
		{
			Weight = 10.0;
		}

		public PottedCactus6(Serial serial) : base(serial)
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
