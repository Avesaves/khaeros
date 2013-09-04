using System;
using Server;

namespace Server.Items
{
	public class SouthernBedEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new SouthernBedEastDeed(); } }

		[Constructable]
		public SouthernBedEastAddon()
		{
			AddComponent( new AddonComponent( 0x304D ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x304C ), 1, 0, 0 );
		}

		public SouthernBedEastAddon( Serial serial ) : base( serial )
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

	public class SouthernBedEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new SouthernBedEastAddon(); } }
		public override int LabelNumber{ get{ return 1072861; } } // elven bed (east)

		[Constructable]
		public SouthernBedEastDeed()
		{
		}

		public SouthernBedEastDeed( Serial serial ) : base( serial )
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
