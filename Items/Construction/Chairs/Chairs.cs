using System;

namespace Server.Items
{
	[Furniture]
	[Flipable( 0xB4F, 0xB4E, 0xB50, 0xB51 )]
	public class FancyWoodenChairCushion : Item, ICanBeStained
	{
		[Constructable]
		public FancyWoodenChairCushion() : base(0xB4F)
		{
			Weight = 20.0;
		}

		public FancyWoodenChairCushion(Serial serial) : base(serial)
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

			if ( Weight == 6.0 )
				Weight = 20.0;
		}
	}

	[Furniture]
	[Flipable( 0xB53, 0xB52, 0xB54, 0xB55 )]
	public class WoodenChairCushion : Item, ICanBeStained
	{
		[Constructable]
		public WoodenChairCushion() : base(0xB53)
		{
			Weight = 20.0;
		}

		public WoodenChairCushion(Serial serial) : base(serial)
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

			if ( Weight == 6.0 )
				Weight = 20.0;
		}
	}

	[Furniture]
	[Flipable( 0xB57, 0xB56, 0xB59, 0xB58 )]
	public class WoodenChair : Item, ICanBeStained
	{
		[Constructable]
		public WoodenChair() : base(0xB57)
		{
			Weight = 20.0;
		}

		public WoodenChair(Serial serial) : base(serial)
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

			if ( Weight == 6.0 )
				Weight = 20.0;
		}
	}

	[Furniture]
	[Flipable( 0xB5B, 0xB5A, 0xB5C, 0xB5D )]
	public class BambooChair : Item, ICanBeStained
	{
		[Constructable]
		public BambooChair() : base(0xB5B)
		{
			Weight = 20.0;
		}

		public BambooChair(Serial serial) : base(serial)
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

			if ( Weight == 6.0 )
				Weight = 20.0;
		}
	}

	[DynamicFliping]
	[Flipable(0x1218, 0x1219, 0x121A, 0x121B)]
	public class StoneChair : Item
	{
		[Constructable]
		public StoneChair() : base(0x1218)
		{
			Weight = 20;
		}

		public StoneChair(Serial serial) : base(serial)
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

	[DynamicFliping]
	[Flipable( 0x2DE3, 0x2DE4, 0x2DE5, 0x2DE6 )]
	public class OrnateSouthernChair : Item, ICanBeStained
	{
		[Constructable]
		public OrnateSouthernChair() : base( 0x2DE3 )
		{
			Weight = 1.0;
		}

		public OrnateSouthernChair( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	[DynamicFliping]
	[Flipable( 0x2DEB, 0x2DEC, 0x2DED, 0x2DEE )]
	public class CozySouthernChair : Item, ICanBeStained
	{
		[Constructable]
		public CozySouthernChair() : base( 0x2DEB )
		{
		}

		public CozySouthernChair( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	[DynamicFliping]
	[Flipable( 0x2DF5, 0x2DF6 )]
	public class SouthernLowChair : Item, ICanBeStained
	{
		[Constructable]
		public SouthernLowChair() : base( 0x2DF5 )
		{
		}

		public SouthernLowChair( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
