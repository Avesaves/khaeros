using System;

namespace Server.Items
{
	[FlipableAttribute( 0x1BD7, 0x1BDA )]
	public class BoatPart : Item
	{

		public override double DefaultWeight
		{
			get { return 0.1; }
		}

		[Constructable]
		public BoatPart()
			: this( 1 )
		{
		}

		[Constructable]
		public BoatPart( int amount )
			: base( 0x1BD7 )
		{
			Stackable = true;
			Amount = amount;
			Name = "Boat Part";
			Hue = 2830;
			Weight = 0.1;
		}

		public BoatPart( Serial serial )
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

			if ( Weight != 0.1 )
				Weight = 0.1;
		}
	}
}
