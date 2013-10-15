using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class ArtificerSupplies : BaseTool
	{
/* 		public override int LabelNumber{ get{ return 1044567; } } // skillet */

		public override CraftSystem CraftSystem{ get{ return DefBuilding.CraftSystem; } }

		[Constructable]
		public ArtificerSupplies() : base( 0x1eba )
		{
            Name = "Artificer Supplies";
            Weight = 1.0;
		}

		[Constructable]
		public ArtificerSupplies( int uses ) : base( uses, 0x1eba )
		{
            Name = "Artificer Supplies";
            Weight = 1.0;
		}

		public ArtificerSupplies( Serial serial ) : base( serial )
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
