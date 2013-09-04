using System;
using Server;

namespace Server.Items
{
	public class SouthernDresserSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new SouthernDresserSouthDeed(); } }

		[Constructable]
		public SouthernDresserSouthAddon()
		{
			AddComponent( new AddonComponent( 0x30E5 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x30E6 ), 1, 0, 0 );
		}

		public SouthernDresserSouthAddon( Serial serial ) : base( serial )
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

	public class SouthernDresserSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new SouthernDresserSouthAddon(); } }
		public override int LabelNumber{ get{ return 1072864; } } // elven dresser (south)

		[Constructable]
		public SouthernDresserSouthDeed()
		{
		}

		public SouthernDresserSouthDeed( Serial serial ) : base( serial )
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
