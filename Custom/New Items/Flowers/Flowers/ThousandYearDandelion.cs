using System;
using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Items
{
	public class ThousandYearDandelion : Flower
	{
		public override int FlowerID { get { return 3205; } }
		public override int BouquetID { get { return 0x3BBB; } }
		
		[Constructable]
		public ThousandYearDandelion() : base( 3205 )
		{
			Name = "Thousand Year Dandelion";
		}

		public ThousandYearDandelion( Serial serial ) : base( serial )
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