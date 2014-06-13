using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class Silphium : Flower
	{
		public override int FlowerID { get { return 3976; } }
		public override int BouquetID { get { return 3976; } }
	
		[Constructable]
		public Silphium() : base( 3976 )
		{
			Name = "Silphium";
		}

		public Silphium( Serial serial ) : base( serial )
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