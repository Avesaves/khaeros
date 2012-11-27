using System;
using Server;

namespace Server.Items
{
	public class AlyrianDresserSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new AlyrianDresserSouthDeed(); } }

		[Constructable]
		public AlyrianDresserSouthAddon()
		{
			AddComponent( new AddonComponent( 0x30E5 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x30E6 ), 1, 0, 0 );
		}

		public AlyrianDresserSouthAddon( Serial serial ) : base( serial )
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

	public class AlyrianDresserSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new AlyrianDresserSouthAddon(); } }
		public override int LabelNumber{ get{ return 1072864; } } // elven dresser (south)

		[Constructable]
		public AlyrianDresserSouthDeed()
		{
		}

		public AlyrianDresserSouthDeed( Serial serial ) : base( serial )
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
