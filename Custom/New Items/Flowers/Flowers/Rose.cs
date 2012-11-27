using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class Rose : Flower
	{
		public override int FlowerID { get { return 9035; } }
		public override int BouquetID { get { return 0x3BBC; } }
	
		[Constructable]
		public Rose() : base( 9035 )
		{
			Name = "Rose";
		}

		public Rose( Serial serial ) : base( serial )
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