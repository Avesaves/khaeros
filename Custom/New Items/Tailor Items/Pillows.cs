using System;

namespace Server.Items
{
	[FlipableAttribute( 5029, 5028 )]
	public class Pillow1 : Item, IDyable
	{
		[Constructable]
		public Pillow1() : base(5029)
		{
			Weight = 0.5;
		}

		public Pillow1(Serial serial) : base(serial)
		{
		}
		
		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
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
	
	public class Pillow2 : Item, IDyable
	{
		[Constructable]
		public Pillow2() : base(5030)
		{
			Weight = 0.5;
		}

		public Pillow2(Serial serial) : base(serial)
		{
		}
		
		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
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
	
	public class Pillow3 : Item, IDyable
	{
		[Constructable]
		public Pillow3() : base(5031)
		{
			Weight = 0.5;
		}

		public Pillow3(Serial serial) : base(serial)
		{
		}
		
		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
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
	
	public class Pillow4 : Item, IDyable
	{
		[Constructable]
		public Pillow4() : base(5032)
		{
			Weight = 0.5;
		}

		public Pillow4(Serial serial) : base(serial)
		{
		}
		
		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
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
	
	[FlipableAttribute( 5034, 5033 )]
	public class Pillow5 : Item, IDyable
	{
		[Constructable]
		public Pillow5() : base(5033)
		{
			Weight = 0.5;
		}

		public Pillow5(Serial serial) : base(serial)
		{
		}
		
		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
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
	
	public class Pillow6 : Item, IDyable
	{
		[Constructable]
		public Pillow6() : base(5035)
		{
			Weight = 0.5;
		}

		public Pillow6(Serial serial) : base(serial)
		{
		}
		
		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
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
	
	public class Pillow7 : Item, IDyable
	{
		[Constructable]
		public Pillow7() : base(5036)
		{
			Weight = 0.5;
		}

		public Pillow7(Serial serial) : base(serial)
		{
		}
		
		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
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
	
	[FlipableAttribute( 5038, 5037 )]
	public class Pillow8 : Item, IDyable
	{
		[Constructable]
		public Pillow8() : base(5037)
		{
			Weight = 0.5;
		}

		public Pillow8(Serial serial) : base(serial)
		{
		}
		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
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
	
	public class Pillow9 : Item, IDyable
	{
		[Constructable]
		public Pillow9() : base(5690)
		{
			Weight = 0.5;
		}

		public Pillow9(Serial serial) : base(serial)
		{
		}

		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
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
	
	public class Pillow10 : Item, IDyable
	{
		[Constructable]
		public Pillow10() : base(5691)
		{
			Weight = 0.5;
		}

		public Pillow10(Serial serial) : base(serial)
		{
		}
		
		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
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
	
	public class Pillow11 : Item, IDyable
	{
		[Constructable]
		public Pillow11() : base(5692)
		{
			Weight = 0.5;
		}

		public Pillow11(Serial serial) : base(serial)
		{
		}
		
		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
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
	
	public class Pillow12 : Item, IDyable
	{
		[Constructable]
		public Pillow12() : base(6421)
		{
			Weight = 0.5;
		}

		public Pillow12(Serial serial) : base(serial)
		{
		}
		
		public virtual bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;
			else if ( RootParent is Mobile && from != RootParent )
				return false;

			Hue = sender.DyedHue;

			return true;
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
