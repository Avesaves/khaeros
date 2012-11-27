using System;
using Server;

namespace Server.Items
{
	public class AlyrianBedEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new AlyrianBedEastDeed(); } }

		[Constructable]
		public AlyrianBedEastAddon()
		{
			AddComponent( new AddonComponent( 0x304D ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x304C ), 1, 0, 0 );
		}

		public AlyrianBedEastAddon( Serial serial ) : base( serial )
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

	public class AlyrianBedEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new AlyrianBedEastAddon(); } }
		public override int LabelNumber{ get{ return 1072861; } } // elven bed (east)

		[Constructable]
		public AlyrianBedEastDeed()
		{
		}

		public AlyrianBedEastDeed( Serial serial ) : base( serial )
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
