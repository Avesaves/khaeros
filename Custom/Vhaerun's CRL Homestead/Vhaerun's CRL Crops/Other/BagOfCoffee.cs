using System;
using Server.Network;

namespace Server.Items
{
	public class BagOfCoffee : Item
	{

		[Constructable]
		public BagOfCoffee() : base( 0x1039 )
		{
			Weight = 5.0;
			Stackable = false;
			Hue = 0x46A;
			Name = "Bag of Coffee";
		}

		public BagOfCoffee( Serial serial ) : base( serial )
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

            if( Amount > 1 )
                Amount = 1;
		}
	}
}
