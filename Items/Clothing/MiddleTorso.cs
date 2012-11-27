using System;

namespace Server.Items
{
	public abstract class BaseMiddleTorso : BaseClothing
	{
		public BaseMiddleTorso( int itemID ) : this( itemID, 0 )
		{
		}

		public BaseMiddleTorso( int itemID, int hue ) : base( itemID, Layer.MiddleTorso, hue )
		{
		}

		public BaseMiddleTorso( Serial serial ) : base( serial )
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

	[Flipable( 0x1541, 0x1542 )]
	public class BodySash : BaseMiddleTorso
	{
		[Constructable]
		public BodySash() : this( 0 )
		{
		}

		[Constructable]
		public BodySash( int hue ) : base( 0x1541, hue )
		{
			Weight = 1.0;
		}

		public BodySash( Serial serial ) : base( serial )
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

	[Flipable( 0x153d, 0x153e )]
	public class FullApron : BaseMiddleTorso
	{
		[Constructable]
		public FullApron() : this( 0 )
		{
		}

		[Constructable]
		public FullApron( int hue ) : base( 0x153d, hue )
		{
			Weight = 4.0;
		}

		public FullApron( Serial serial ) : base( serial )
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
	
	[Flipable( 0x153d, 0x153e )]
	public class HairStylingApron : BaseMiddleTorso
	{
		[Constructable]
		public HairStylingApron() : this( 0 )
		{
		}

		[Constructable]
		public HairStylingApron( int hue ) : base( 0x153d, hue )
		{
			Weight = 4.0;
			Name = "Hair Styling Apron";
			Hue = 2984;
		}

		public HairStylingApron( Serial serial ) : base( serial )
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

	[Flipable( 0x1f7b, 0x1f7c )]
	public class Doublet : BaseMiddleTorso
	{
		[Constructable]
		public Doublet() : this( 0 )
		{
		}

		[Constructable]
		public Doublet( int hue ) : base( 0x1F7B, hue )
		{
			Weight = 2.0;
		}

		public Doublet( Serial serial ) : base( serial )
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

	[Flipable( 0x1ffd, 0x1ffe )]
	public class Surcoat : BaseMiddleTorso
	{
		[Constructable]
		public Surcoat() : this( 0 )
		{
		}

		[Constructable]
		public Surcoat( int hue ) : base( 0x1FFD, hue )
		{
			Weight = 6.0;
		}

		public Surcoat( Serial serial ) : base( serial )
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

			if ( Weight == 3.0 )
				Weight = 6.0;
		}
	}

	[Flipable( 0x1fa1, 0x1fa2 )]
	public class Tunic : BaseMiddleTorso
	{
		[Constructable]
		public Tunic() : this( 0 )
		{
		}

		[Constructable]
		public Tunic( int hue ) : base( 0x1FA1, hue )
		{
			Weight = 5.0;
		}

		public Tunic( Serial serial ) : base( serial )
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

	[Flipable( 0x1f9f, 0x1fa0 )]
	public class JesterSuit : BaseMiddleTorso
	{
		[Constructable]
		public JesterSuit() : this( 0 )
		{
		}

		[Constructable]
		public JesterSuit( int hue ) : base( 0x1F9F, hue )
		{
			Weight = 4.0;
		}

		public JesterSuit( Serial serial ) : base( serial )
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

	[Flipable( 0x27A1, 0x27EC )]
	public class JinBaori : BaseMiddleTorso
	{
		[Constructable]
		public JinBaori() : this( 0 )
		{
		}

		[Constructable]
		public JinBaori( int hue ) : base( 0x27A1, hue )
		{
			Weight = 3.0;
		}

		public JinBaori( Serial serial ) : base( serial )
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
	
	public class ElegantTunic : BaseMiddleTorso
	{
		[Constructable]
		public ElegantTunic() : this( 0 )
		{
		}

		[Constructable]
		public ElegantTunic( int hue ) : base( 0x3C64, hue )
		{
			Weight = 3.0;
			Name = "Elegant Tunic";
            Layer = Layer.OuterTorso;
		}

		public ElegantTunic( Serial serial ) : base( serial )
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
                Layer = Layer.OuterTorso;
		}
	}
	
	public class ElegantSurcoat : BaseMiddleTorso
	{
		[Constructable]
		public ElegantSurcoat() : this( 0 )
		{
		}

		[Constructable]
		public ElegantSurcoat( int hue ) : base( 0x3C63, hue )
		{
			Weight = 3.0;
			Name = "Elegant Surcoat";
		}

		public ElegantSurcoat( Serial serial ) : base( serial )
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
	
	public class PaddedVest : BaseMiddleTorso
	{
		[Constructable]
		public PaddedVest() : this( 0 )
		{
		}

		[Constructable]
		public PaddedVest( int hue ) : base( 0x3C5D, hue )
		{
			Weight = 3.0;
			Name = "Padded Vest";
		}

		public PaddedVest( Serial serial ) : base( serial )
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
	
	public class FancyDoublet : BaseMiddleTorso
	{
		[Constructable]
		public FancyDoublet() : this( 0 )
		{
		}

		[Constructable]
		public FancyDoublet( int hue ) : base( 0x3C49, hue )
		{
			Weight = 2.0;
			Name = "Fancy Doublet";
		}

		public FancyDoublet( Serial serial ) : base( serial )
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
	
	public class ElegantVest : BaseMiddleTorso
	{
		[Constructable]
		public ElegantVest() : this( 0 )
		{
		}

		[Constructable]
		public ElegantVest( int hue ) : base( 0x3C28, hue )
		{
			Weight = 2.0;
			Name = "Elegant Vest";
		}

		public ElegantVest( Serial serial ) : base( serial )
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
	
	public class ElegantDoublet : BaseMiddleTorso
	{
		[Constructable]
		public ElegantDoublet() : this( 0 )
		{
		}

		[Constructable]
		public ElegantDoublet( int hue ) : base( 0x3C5D, hue )
		{
			Weight = 2.0;
			Name = "Elegant Doublet";
			Hue = 2796;
		}

		public ElegantDoublet( Serial serial ) : base( serial )
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
	
	public class FancySurcoat : BaseMiddleTorso
	{
		[Constructable]
		public FancySurcoat() : this( 0 )
		{
		}

		[Constructable]
		public FancySurcoat( int hue ) : base( 0x3C8F, hue )
		{
			Weight = 3.0;
			Name = "Fancy Surcoat";
		}

		public FancySurcoat( Serial serial ) : base( serial )
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
	
	public class Quiver : BaseMiddleTorso
	{
        public override int BaseBluntResistance
        {
            get
            {
                return 0;
            }
        }
        public override int BaseSlashingResistance
        {
            get
            {
                return 0;
            }
        }
        public override int BasePiercingResistance
        {
            get
            {
                return 0;
            }
        }

		[Constructable]
		public Quiver() : this( 0 )
		{
		}

		[Constructable]
		public Quiver( int hue ) : base( 0x3B33, hue )
		{
			Weight = 3.0;
			Name = "Quiver";
            Layer = Layer.Talisman;
		}

		public Quiver( Serial serial ) : base( serial )
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
                Layer = Layer.Unused_xF;
		}
	}
}
