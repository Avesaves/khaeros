using System;

namespace Server.Items
{
	[Flipable( 5535, 5534 )]
	public class SmallCurtain : Item, IDyable
	{
		[Constructable]
		public SmallCurtain() : base(5534)
		{
			Weight = 0.5;
		}

		public SmallCurtain(Serial serial) : base(serial)
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
	
	[Flipable( 4826, 4827, 4828, 4829, 4830, 4831, 4832, 4833, 4837, 4838, 4839, 4840, 4841, 4842, 4843, 4844, 5463, 5646 )]
	public class Curtain : Item, IDyable
	{
		[Constructable]
		public Curtain() : base(4826)
		{
			Weight = 0.5;
		}

		public Curtain(Serial serial) : base(serial)
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
	
	[Flipable( 4834, 4835, 4836, 4845 )]
	public class CurtainSash : Item, IDyable
	{
		[Constructable]
		public CurtainSash() : base(4834)
		{
			Weight = 0.5;
		}

		public CurtainSash(Serial serial) : base(serial)
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
