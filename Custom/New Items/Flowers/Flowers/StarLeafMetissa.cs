using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class StarLeafMetissa : Flower
	{
		public override int FlowerID { get { return 6367; } }
		public override int BouquetID { get { return 0x3BBB; } }
	
		[Constructable]
		public StarLeafMetissa() : base( 6367 )
		{
			Name = "Star-Leaf Metissa";
		}

		public StarLeafMetissa( Serial serial ) : base( serial )
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