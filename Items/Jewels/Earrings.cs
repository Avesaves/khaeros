using System;

namespace Server.Items
{
	public abstract class BaseEarrings : BaseJewel
	{
		public override int BaseGemTypeNumber{ get{ return 1044203; } } // star sapphire earrings

		public BaseEarrings( int itemID ) : base( itemID, Layer.Earrings )
		{
		}

		public BaseEarrings( Serial serial ) : base( serial )
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

	public class GoldEarrings : BaseEarrings
	{
		[Constructable]
		public GoldEarrings() : base( 0x1087 )
		{
			Weight = 0.1;
			Name = "Gold Earrings";
		}

		public GoldEarrings( Serial serial ) : base( serial )
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

	public class SilverEarrings : BaseEarrings
	{
		[Constructable]
		public SilverEarrings() : base( 0x1F07 )
		{
			Weight = 0.1;
			Name = "Silver Earrings";
		}

		public SilverEarrings( Serial serial ) : base( serial )
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
	
	public class LargeGoldEarrings : BaseEarrings
	{
		[Constructable]
		public LargeGoldEarrings() : base( 0x3B91 )
		{
			Weight = 0.1;
			Name = "Large Gold Earrings";
		}

		public LargeGoldEarrings( Serial serial ) : base( serial )
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

	public class LargeSilverEarrings : BaseEarrings
	{
		[Constructable]
		public LargeSilverEarrings() : base( 0x3B90 )
		{
			Weight = 0.1;
			Name = "Large Silver Earrings";
		}

		public LargeSilverEarrings( Serial serial ) : base( serial )
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
