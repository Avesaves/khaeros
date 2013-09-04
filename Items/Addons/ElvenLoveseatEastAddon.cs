using System;
using Server;

namespace Server.Items
{
	public class SouthernBenchEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new SouthernBenchEastDeed(); } }

		[Constructable]
		public SouthernBenchEastAddon()
		{
			AddComponent( new AddonComponent( 0x3089 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x3088 ), 1, 0, 0 );
		}

		public SouthernBenchEastAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}

	public class SouthernBenchEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new SouthernBenchEastAddon(); } }
		public override int LabelNumber{ get{ return 1073372; } } // elven loveseat (east)

		[Constructable]
		public SouthernBenchEastDeed()
		{
		}

		public SouthernBenchEastDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
