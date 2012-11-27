using System;
using Server;

namespace Server.Items
{
	public class AlyrianBenchEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new AlyrianBenchEastDeed(); } }

		[Constructable]
		public AlyrianBenchEastAddon()
		{
			AddComponent( new AddonComponent( 0x3089 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x3088 ), 1, 0, 0 );
		}

		public AlyrianBenchEastAddon( Serial serial ) : base( serial )
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

	public class AlyrianBenchEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new AlyrianBenchEastAddon(); } }
		public override int LabelNumber{ get{ return 1073372; } } // elven loveseat (east)

		[Constructable]
		public AlyrianBenchEastDeed()
		{
		}

		public AlyrianBenchEastDeed( Serial serial ) : base( serial )
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
