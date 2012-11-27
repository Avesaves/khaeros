using System;

namespace Server.Items
{

	[Furniture]
	public class ElegantLowTable : Item, ICanBeStained
	{
		[Constructable]
		public ElegantLowTable() : base(0x2819)
		{
			Weight = 1.0;
		}

		public ElegantLowTable(Serial serial) : base(serial)
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

	[Furniture]
	public class PlainLowTable : Item, ICanBeStained
	{
		[Constructable]
		public PlainLowTable() : base(0x281A)
		{
			Weight = 1.0;
		}

		public PlainLowTable(Serial serial) : base(serial)
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

	[Furniture]
	[Flipable(0xB90,0xB7D)]
	public class LargeTable : Item, ICanBeStained
	{
		[Constructable]
		public LargeTable() : base(0xB90)
		{
			Weight = 1.0;
		}

		public LargeTable(Serial serial) : base(serial)
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

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}
	
	[Furniture]
	[Flipable(0xB3F,0xB40, 0xB3D, 0xB3E)]
	public class ShopCounter : Item, ICanBeStained
	{
		[Constructable]
		public ShopCounter() : base(0xB40)
		{
			Weight = 1.0;
		}

		public ShopCounter(Serial serial) : base(serial)
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

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}

	[Furniture]
	[Flipable(0xB35,0xB34)]
	public class Nightstand : Item, ICanBeStained
	{
		[Constructable]
		public Nightstand() : base(0xB35)
		{
			Weight = 1.0;
		}

		public Nightstand(Serial serial) : base(serial)
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

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}

	[Furniture]
	[Flipable(0xB8F,0xB7C)]
	public class YewWoodTable : Item, ICanBeStained
	{
		[Constructable]
		public YewWoodTable() : base(0xB8F)
		{
			Weight = 1.0;
		}

		public YewWoodTable(Serial serial) : base(serial)
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

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}
	
	[Furniture]
	[Flipable(0xB77,0xB78, 0xB79, 0xB7A, 0xB7B, 0XB76, 0xB75, 0xB88, 0xB89, 0xB8A, 0xB8B, 0xB8C, 0xB8D, 0xB8E)]
	public class LargeOakTable : Item, ICanBeStained
	{
		[Constructable]
		public LargeOakTable() : base(0xB77)
		{
			Weight = 1.0;
			Movable = true;
		}

		public LargeOakTable(Serial serial) : base(serial)
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

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}
	[Furniture]
	[Flipable(0xB6E,0xB6F, 0xB70, 0xB71, 0xB72, 0XB73, 0xB74, 0xB81, 0xB82, 0xB83, 0xB84, 0xB85, 0xB86, 0xB87)]
	public class LargeFancyTable : Item, ICanBeStained
	{
		[Constructable]
		public LargeFancyTable() : base(0xB6E)
		{
			Weight = 1.0;
			Movable = true;
		}

		public LargeFancyTable(Serial serial) : base(serial)
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

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}
	[Furniture]
	[Flipable(0x11D6,0x11D7, 0x11D8, 0x11D9, 0x11DA, 0X11DB)]
	public class RattanTable : Item, ICanBeStained
	{
		[Constructable]
		public RattanTable() : base(0x11D6)
		{
			Weight = 1.0;
			Movable = true;
		}

		public RattanTable(Serial serial) : base(serial)
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

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}
	[Furniture]
	[Flipable(0x11DC,0x11DD, 0x11DE, 0x11DF, 0x11E0, 0X11E1)]
	public class LogTable : Item, ICanBeStained
	{
		[Constructable]
		public LogTable() : base(0x11DC)
		{
			Weight = 1.0;
			Movable = true;
		}

		public LogTable(Serial serial) : base(serial)
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

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}
	[Furniture]
	[Flipable(0x118B,0x118C, 0x118D, 0x118E, 0x118F, 0X1190, 0x1191, 0x1192)]
	public class ClothTable : Item, ICanBeStained
	{
		[Constructable]
		public ClothTable() : base(0x118B)
		{
			Weight = 1.0;
			Movable = true;
		}

		public ClothTable(Serial serial) : base(serial)
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

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}
	[Furniture]
	[Flipable(0x1910,0x1911, 0x1912, 0x1913, 0x1918, 0X1919, 0x191A, 0x191B, 0x191C, 0x191D, 0x191E, 0x191F)]
	public class BarPart : Item, ICanBeStained
	{
		[Constructable]
		public BarPart() : base(0x1910)
		{
			Weight = 1.0;
			Movable = true;
		}

		public BarPart(Serial serial) : base(serial)
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

			if ( Weight == 4.0 )
				Weight = 1.0;
		}
	}
}
