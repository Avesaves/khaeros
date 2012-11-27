using System;
using Server.Network;

namespace Server.Items
{
	public class BagOfCornmeal : Item
	{

		[Constructable]
		public BagOfCornmeal() : base( 0x1039 )
		{
			Weight = 5.0;
			Stackable = false;
			Hue = 0x466;
			Name = "Bag of Cornmeal";
		}

		public BagOfCornmeal( Serial serial ) : base( serial )
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
