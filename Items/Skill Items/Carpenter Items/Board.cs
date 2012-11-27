using System;

namespace Server.Items
{
	[FlipableAttribute( 0x1BD7, 0x1BDA )]
    public class Board : Item, ICommodity, IEasyCraft
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format( Amount == 1 ? "{0} board" : "{0} boards", Amount );
			}
		}
		
		public override double DefaultWeight
		{
			get { return 1.0; }
		}

		[Constructable]
		public Board()
			: this( 1 )
		{
		}

		[Constructable]
		public Board( int amount )
			: base( 0x1BD7 )
		{
			Stackable = true;
			Amount = amount;
		}

		public Board( Serial serial )
			: base( serial )
		{
		}

		

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Weight != 1.0 )
				Weight = 1.0;
		}
	}
}
