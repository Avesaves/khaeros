using System;

namespace Server.Items
{
	public abstract class BaseShirt : BaseClothing
	{
		public BaseShirt( int itemID ) : this( itemID, 0 )
		{
		}

		public BaseShirt( int itemID, int hue ) : base( itemID, Layer.Shirt, hue )
		{
		}

		public BaseShirt( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1efd, 0x1efe )]
	public class FancyShirt : BaseShirt
	{
		[Constructable]
		public FancyShirt() : this( 0 )
		{
		}

		[Constructable]
		public FancyShirt( int hue ) : base( 0x1EFD, hue )
		{
			Weight = 2.0;
		}

		public FancyShirt( Serial serial ) : base( serial )
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

	[FlipableAttribute( 0x1517, 0x1518 )]
	public class Shirt : BaseShirt
	{
		[Constructable]
		public Shirt() : this( 0 )
		{
		}

		[Constructable]
		public Shirt( int hue ) : base( 0x1517, hue )
		{
			Weight = 1.0;
		}

		public Shirt( Serial serial ) : base( serial )
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

			if ( Weight == 2.0 )
				Weight = 1.0;
		}
	}

	public class ElegantShirt : BaseShirt
	{
		[Constructable]
		public ElegantShirt() : this( 0 )
		{
		}

		[Constructable]
		public ElegantShirt( int hue ) : base( 0x3176, hue )
		{
			Weight = 2.0;
			Name = "Elegant Shirt";
            Layer = Layer.InnerTorso;
		}

		public ElegantShirt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();

            if( version < 2 )
                Layer = Layer.InnerTorso;
		}
	}
	
	public class MetallicBra : BaseShirt
	{
		[Constructable]
		public MetallicBra() : this( 0 )
		{
		}
		
		[Constructable]
		public MetallicBra( int hue ) : base( 0x3C7D, hue )
		{
			Weight = 1.0;
			Name = "Metallic Bra";
		}

		public MetallicBra( Serial serial ) : base( serial )
		{
		}
		
		public override bool AllowMaleWearer{ get{ return false; } }
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 2 );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			
			if( version < 2 )
				this.Resource = CraftResource.Cotton;
		}
	}
	
	public class FormalShirt : BaseShirt
	{
		[Constructable]
		public FormalShirt() : this( 0 )
		{
		}

		[Constructable]
		public FormalShirt( int hue ) : base( 0x3C4B, hue )
		{
			Weight = 1.0;
			Name = "Formal Shirt";
            Layer = Layer.InnerTorso;
		}

		public FormalShirt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if( version < 2 )
                Layer = Layer.InnerTorso;
		}
	}
	
	public class ExtravagantShirt : BaseShirt
	{
		[Constructable]
		public ExtravagantShirt() : this( 0 )
		{
		}

		[Constructable]
		public ExtravagantShirt( int hue ) : base( 0x3B24, hue )
		{
			Weight = 1.0;
			Name = "Extravagant Shirt";
		}

		public ExtravagantShirt( Serial serial ) : base( serial )
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
	
	public class ExpensiveShirt : BaseShirt
	{
		[Constructable]
		public ExpensiveShirt() : this( 0 )
		{
		}

		[Constructable]
		public ExpensiveShirt( int hue ) : base( 0x3C4C, hue )
		{
			Weight = 1.0;
			Name = "Expensive Shirt";
            Layer = Layer.InnerTorso;
		}

		public ExpensiveShirt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if( version < 2 )
                Layer = Layer.InnerTorso;
		}
	}
	
	public class OrnateShirt : BaseShirt
	{
		[Constructable]
		public OrnateShirt() : this( 0 )
		{
		}

		[Constructable]
		public OrnateShirt( int hue ) : base( 0x3C93, hue )
		{
			Weight = 1.0;
			Name = "Ornate Shirt";
			Hue = 0;
            Layer = Layer.InnerTorso;
		}

		public OrnateShirt( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if( version < 2 )
                Layer = Layer.InnerTorso;
		}
	}
	
	public class LongRaggedBra : BaseShirt
	{
		[Constructable]
		public LongRaggedBra() : this( 0 )
		{
		}
		
		[Constructable]
		public LongRaggedBra( int hue ) : base( 0x3CA4, hue )
		{
			Weight = 1.0;
			Name = "Long Ragged Bra";
		}

		public LongRaggedBra( Serial serial ) : base( serial )
		{
		}
		
		public override bool AllowMaleWearer{ get{ return false; } }
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	
	public class RaggedBra : BaseShirt
	{
		[Constructable]
		public RaggedBra() : this( 0 )
		{
		}
		
		[Constructable]
		public RaggedBra( int hue ) : base( 0x3CA3, hue )
		{
			Weight = 1.0;
			Name = "Ragged Bra";
		}

		public RaggedBra( Serial serial ) : base( serial )
		{
		}
		
		public override bool AllowMaleWearer{ get{ return false; } }
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
	
	public class FancyBra : BaseShirt
	{
		[Constructable]
		public FancyBra() : this( 0 )
		{
		}
		
		[Constructable]
		public FancyBra( int hue ) : base( 0x3C4F, hue )
		{
			Weight = 1.0;
			Name = "Fancy Bra";
		}

		public FancyBra( Serial serial ) : base( serial )
		{
		}
		
		public override bool AllowMaleWearer{ get{ return false; } }
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
