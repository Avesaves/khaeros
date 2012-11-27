using System;

namespace Server.Items
{
	public class Towel : Item, IDyable
	{
		[Constructable]
		public Towel() : base(6420)
		{
			Weight = 0.5;
		}

		public Towel(Serial serial) : base(serial)
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

	[FlipableAttribute( 2668, 2669 )]
	public class Blanket : Item, IDyable
	{
		[Constructable]
		public Blanket() : base(2668)
		{
			Weight = 0.5;
		}

		public Blanket(Serial serial) : base(serial)
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

	[FlipableAttribute( 2706, 2707 )]
	public class Sheets : Item, IDyable
	{
		[Constructable]
		public Sheets() : base(2706)
		{
			Weight = 0.5;
		}

		public Sheets(Serial serial) : base(serial)
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