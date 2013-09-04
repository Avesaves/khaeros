using System;
using Server;

namespace Server.Items
{
	public class SouthernDresserEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new SouthernDresserEastDeed(); } }

		[Constructable]
		public SouthernDresserEastAddon()
		{
			AddComponent( new AddonComponent( 0x30E4 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x30E3 ), 0, -1, 0 );
		}

		public SouthernDresserEastAddon( Serial serial ) : base( serial )
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

	public class SouthernDresserEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new SouthernDresserEastAddon(); } }
		public override int LabelNumber{ get{ return 1073388; } } // elven dresser (east)

		[Constructable]
		public SouthernDresserEastDeed()
		{
		}

		public SouthernDresserEastDeed( Serial serial ) : base( serial )
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
