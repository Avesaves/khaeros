using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class EasternCalla : Flower
	{
		public override int FlowerID { get { return 6373; } }
		public override int BouquetID { get { return 0x3BBC; } }
	
		[Constructable]
		public EasternCalla() : base( 6373 )
		{
			Name = "Eastern Calla";
		}

		public EasternCalla( Serial serial ) : base( serial )
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