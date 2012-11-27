using System;

namespace Server.Items
{
	public interface ICanBeStained
	{
	}
	
	[Furniture]
	public class WoodenBear : Item, ICanBeStained
	{
		[Constructable]
		public WoodenBear() : base( 0x2118 )
		{
			Weight = 3;
			Name = "Wooden Bear";
		}

		public WoodenBear(Serial serial) : base(serial)
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
	public class WoodenRat : Item, ICanBeStained
	{
		[Constructable]
		public WoodenRat() : base( 0x2123 )
		{
			Weight = 3;
			Name = "Wooden Rat";
		}

		public WoodenRat(Serial serial) : base(serial)
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
	public class WoodenChicken : Item, ICanBeStained
	{
		[Constructable]
		public WoodenChicken() : base( 0x20D1 )
		{
			Weight = 3;
			Name = "Wooden Chicken";
		}

		public WoodenChicken(Serial serial) : base(serial)
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
	public class WoodenDragon : Item, ICanBeStained
	{
		[Constructable]
		public WoodenDragon() : base( 0x20D6 )
		{
			Weight = 3;
			Name = "Wooden Dragon";
		}

		public WoodenDragon(Serial serial) : base(serial)
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
	public class WoodenCrocodile : Item, ICanBeStained
	{
		[Constructable]
		public WoodenCrocodile() : base( 0x2131 )
		{
			Weight = 3;
			Name = "Wooden Crocodile";
		}

		public WoodenCrocodile(Serial serial) : base(serial)
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
	public class WoodenOgre : Item, ICanBeStained
	{
		[Constructable]
		public WoodenOgre() : base( 0x20E9 )
		{
			Weight = 3;
			Name = "Wooden Ogre";
		}

		public WoodenOgre(Serial serial) : base(serial)
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
	public class WoodenPanther : Item, ICanBeStained
	{
		[Constructable]
		public WoodenPanther() : base( 0x2119 )
		{
			Weight = 3;
			Name = "Wooden Panther";
		}

		public WoodenPanther(Serial serial) : base(serial)
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
	public class WoodenCat : Item, ICanBeStained
	{
		[Constructable]
		public WoodenCat() : base( 0x211B )
		{
			Weight = 3;
			Name = "Wooden Cat";
		}

		public WoodenCat(Serial serial) : base(serial)
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
	public class WoodenBird : Item, ICanBeStained
	{
		[Constructable]
		public WoodenBird() : base( 0x211A )
		{
			Weight = 3;
			Name = "Wooden Bird";
		}

		public WoodenBird(Serial serial) : base(serial)
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
	public class WoodenDog : Item, ICanBeStained
	{
		[Constructable]
		public WoodenDog() : base( 0x211C )
		{
			Weight = 3;
			Name = "Wooden Dog";
		}

		public WoodenDog(Serial serial) : base(serial)
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
	public class WoodenHorse : Item, ICanBeStained
	{
		[Constructable]
		public WoodenHorse() : base( 0x211F )
		{
			Weight = 3;
			Name = "Wooden Horse";
		}

		public WoodenHorse(Serial serial) : base(serial)
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
	public class WoodenWolf : Item, ICanBeStained
	{
		[Constructable]
		public WoodenWolf() : base( 0x2122 )
		{
			Weight = 3;
			Name = "Wooden Wolf";
		}

		public WoodenWolf(Serial serial) : base(serial)
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
}
