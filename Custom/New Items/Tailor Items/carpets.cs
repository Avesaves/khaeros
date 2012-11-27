using System;

namespace Server.Items
{
	[Flipable( 2729, 2730, 2731, 2732, 2733, 2734, 2735, 2736, 2737, 2738 )]
	public class BrownRug : Item, IDyable
	{
		[Constructable]
		public BrownRug() : base(2729)
		{
			Weight = 0.5;
			Name = "Brown Rug";
		}

		public BrownRug(Serial serial) : base(serial)
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
	
	[Flipable( 2739, 2740, 2741, 2742, 2743, 2744, 2745, 2746, 2747, 2748 )]
	public class GreenRug : Item, IDyable
	{
		[Constructable]
		public GreenRug() : base(2739)
		{
			Weight = 0.5;
			Name ="Green Rug";
		}

		public GreenRug(Serial serial) : base(serial)
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
	
	[Flipable( 2758, 2759, 2760, 2761, 2762, 2763, 2764, 2765, 2766, 2767, 2768 )]
	public class RedCarpet : Item, IDyable
	{
		[Constructable]
		public RedCarpet() : base(2759)
		{
			Weight = 0.5;
			Name ="Red Carpet";
		}

		public RedCarpet(Serial serial) : base(serial)
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
	[Flipable( 2769, 2770, 2771, 2772, 2773, 2774, 2775, 2776, 2777 )]
	public class BlueCarpet : Item, IDyable
	{
		[Constructable]
		public BlueCarpet() : base(2769)
		{
			Weight = 0.5;
			Name ="Blue Carpet";
		}

		public BlueCarpet(Serial serial) : base(serial)
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
	[Flipable( 2778, 2779, 2780, 2781, 2782, 2783, 2784, 2785, 2786 )]
	public class GoldCarpet : Item, IDyable
	{
		[Constructable]
		public GoldCarpet() : base(2778)
		{
			Weight = 0.5;
			Name ="Gold Carpet";
		}

		public GoldCarpet(Serial serial) : base(serial)
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
	
	[Flipable( 10698, 10699, 10700, 10701, 10702, 10703, 10704, 10705, 10706, 10707, 10708, 10709, 10710, 10711, 10712, 10713, 10714, 10715  )]
	public class GazaMat : Item, IDyable
	{
		[Constructable]
		public GazaMat() : base(10698)
		{
			Weight = 0.5;
			Name ="Mat";
		}

		public GazaMat(Serial serial) : base(serial)
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
	[Flipable( 16136, 16137, 16138, 16151, 16152, 16145, 16141 )]
	public class SmallCarpet : Item, IDyable
	{
		[Constructable]
		public SmallCarpet() : base(16136)
		{
			Weight = 0.5;
			Name ="Carpet";
		}

		public SmallCarpet(Serial serial) : base(serial)
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