using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x11EA, 0x11EB )]
	public class Sand : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} sand" : "{0} sand", Amount );
			}
		}

		public override int LabelNumber{ get{ return 1044626; } } // sand

		[Constructable]
		public Sand() : this( 1 )
		{
		}

		[Constructable]
		public Sand( int amount ) : base( 0xEED )
		{
			Stackable = true;
			Hue = 2776;
			Weight = 1.0;
			Name = "sand";		
		}

		public Sand( Serial serial ) : base( serial )
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

			if ( version == 0 && this.Name == "sand" )
				this.Name = null;
			
			if( version < 2 )
			{
				Hue = 2776;
				ItemID = 0xEED;
			}
		}
	}
}
