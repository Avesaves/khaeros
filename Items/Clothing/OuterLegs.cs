using System;

namespace Server.Items
{
	public abstract class BaseOuterLegs : BaseClothing
	{
		public BaseOuterLegs( int itemID ) : this( itemID, 0 )
		{
		}

		public BaseOuterLegs( int itemID, int hue ) : base( itemID, Layer.OuterLegs, hue )
		{
		}

		public BaseOuterLegs( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[Flipable( 0x230C, 0x230B )]
	public class FurSarong : BaseOuterLegs
	{
		[Constructable]
		public FurSarong() : this( 0 )
		{
		}

		[Constructable]
		public FurSarong( int hue ) : base( 0x230C, hue )
		{
			Weight = 3.0;
		}

		public FurSarong( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Weight == 4.0 )
				Weight = 3.0;
		}
	}

	[Flipable( 0x1516, 0x1531 )]
	public class Skirt : BaseOuterLegs
	{
		[Constructable]
		public Skirt() : this( 0 )
		{
		}

		[Constructable]
		public Skirt( int hue ) : base( 0x1516, hue )
		{
			Weight = 4.0;
		}

		public Skirt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[Flipable( 0x1537, 0x1538 )]
	public class Kilt : BaseOuterLegs
	{
		[Constructable]
		public Kilt() : this( 0 )
		{
		}

		[Constructable]
		public Kilt( int hue ) : base( 0x1537, hue )
		{
			Weight = 2.0;
		}

		public Kilt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}

	[Flipable( 0x279A, 0x27E5 )]
	public class Hakama : BaseOuterLegs
	{
		[Constructable]
		public Hakama() : this( 0 )
		{
		}

		[Constructable]
		public Hakama( int hue ) : base( 0x279A, hue )
		{
			Weight = 2.0;
		}

		public Hakama( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class WaistSash : BaseOuterLegs
	{
		[Constructable]
		public WaistSash() : this( 0 )
		{
		}

		[Constructable]
		public WaistSash( int hue ) : base( 0x3BC3, hue )
		{
			Weight = 2.0;
			Name = "Waist Sash";
			Layer = Layer.Waist;
		}

		public WaistSash( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				Layer = Layer.Waist;
		}
	}
	
	public class FemaleLoinCloth : BaseOuterLegs
	{
		[Constructable]
		public FemaleLoinCloth() : this( 0 )
		{
		}

		[Constructable]
		public FemaleLoinCloth( int hue ) : base( 0x3C59, hue )
		{
			Weight = 1.0;
			Name = "Female Loin Cloth";
		}

		public FemaleLoinCloth( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class MaleLoinCloth : BaseOuterLegs
	{
		[Constructable]
		public MaleLoinCloth() : this( 0 )
		{
		}

		[Constructable]
		public MaleLoinCloth( int hue ) : base( 0x3C58, hue )
		{
			Weight = 1.0;
			Name = "Male Loin Cloth";
		}

		public MaleLoinCloth( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class LoinCloth : BaseOuterLegs
	{
		[Constructable]
		public LoinCloth() : this( 0 )
		{
		}

		[Constructable]
		public LoinCloth( int hue ) : base( 0x3C57, hue )
		{
			Weight = 1.0;
			Name = "Loin Cloth";
		}

		public LoinCloth( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class RaggedSkirt : BaseOuterLegs
	{
		[Constructable]
		public RaggedSkirt() : this( 0 )
		{
		}

		[Constructable]
		public RaggedSkirt( int hue ) : base( 0x3CA2, hue )
		{
			Weight = 2.0;
			Name = "Ragged Skirt";
		}

		public RaggedSkirt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class SmallRaggedSkirt : BaseOuterLegs
	{
		[Constructable]
		public SmallRaggedSkirt() : this( 0 )
		{
		}

		[Constructable]
		public SmallRaggedSkirt( int hue ) : base( 0x3CA1, hue )
		{
			Weight = 1.0;
			Name = "Small Ragged Skirt";
		}

		public SmallRaggedSkirt( Serial serial ) : base( serial )
		{
		}
		
		public override bool AllowMaleWearer{ get{ return false; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class WaistCloth : BaseOuterLegs
	{
		[Constructable]
		public WaistCloth() : this( 0 )
		{
		}

		[Constructable]
		public WaistCloth( int hue ) : base( 0x3C5F, hue )
		{
			Weight = 1.0;
			Name = "Waist Cloth";
		}

		public WaistCloth( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class OrnateWaistCloth : BaseOuterLegs
	{
		[Constructable]
		public OrnateWaistCloth() : this( 0 )
		{
		}

		[Constructable]
		public OrnateWaistCloth( int hue ) : base( 0x3C66, hue )
		{
			Weight = 1.0;
			Name = "Ornate Waist Cloth";
		}

		public OrnateWaistCloth( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class ElegantWaistCloth : BaseOuterLegs
	{
		[Constructable]
		public ElegantWaistCloth() : this( 0 )
		{
		}

		[Constructable]
		public ElegantWaistCloth( int hue ) : base( 0x3C72, hue )
		{
			Weight = 1.0;
			Name = "Elegant Waist Cloth";
		}

		public ElegantWaistCloth( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class OrnateKilt : BaseOuterLegs
	{
		[Constructable]
		public OrnateKilt() : this( 0 )
		{
		}

		[Constructable]
		public OrnateKilt( int hue ) : base( 0x3C88, hue )
		{
			Weight = 1.0;
			Name = "Ornate Kilt";
		}

		public OrnateKilt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class PlainKilt : BaseOuterLegs
	{
		[Constructable]
		public PlainKilt() : this( 0 )
		{
		}

		[Constructable]
		public PlainKilt( int hue ) : base( 0x3CAD, hue )
		{
			Weight = 1.0;
			Name = "Plain Kilt";
		}

		public PlainKilt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class FemaleKilt : BaseOuterLegs
	{
		[Constructable]
		public FemaleKilt() : this( 0 )
		{
		}

		[Constructable]
		public FemaleKilt( int hue ) : base( 0x3CAE, hue )
		{
			Weight = 1.0;
			Name = "Female Kilt";
		}

		public FemaleKilt( Serial serial ) : base( serial )
		{
		}
		
		public override bool AllowMaleWearer{ get{ return false; } }

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class ElegantFemaleKilt : BaseOuterLegs
	{
		[Constructable]
		public ElegantFemaleKilt() : this( 0 )
		{
		}

		[Constructable]
		public ElegantFemaleKilt( int hue ) : base( 0x3C56, hue )
		{
			Weight = 1.0;
			Name = "Elegant Female Kilt";
		}
		
		public override bool AllowMaleWearer{ get{ return false; } }

		public ElegantFemaleKilt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class ElegantKilt : BaseOuterLegs
	{
		[Constructable]
		public ElegantKilt() : this( 0 )
		{
		}

		[Constructable]
		public ElegantKilt( int hue ) : base( 0x3C55, hue )
		{
			Weight = 1.0;
			Name = "Elegant Kilt";
		}

		public ElegantKilt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class ElegantSkirt : BaseOuterLegs
	{
		[Constructable]
		public ElegantSkirt() : this( 0 )
		{
		}

		[Constructable]
		public ElegantSkirt( int hue ) : base( 0x3C4D, hue )
		{
			Weight = 2.0;
			Name = "Elegant Skirt";
		}
		
		public override bool AllowMaleWearer{ get{ return false; } }

		public ElegantSkirt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	public class LongSkirt : BaseOuterLegs
	{
		[Constructable]
		public LongSkirt() : this( 0 )
		{
		}

		[Constructable]
		public LongSkirt( int hue ) : base( 0x3C62, hue )
		{
			Weight = 2.0;
			Name = "Long Skirt";
		}

		public LongSkirt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
