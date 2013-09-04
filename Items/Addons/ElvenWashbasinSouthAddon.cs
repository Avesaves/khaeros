using System;
using Server;

namespace Server.Items
{
	public class SouthernWashBasinSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new SouthernWashBasinSouthDeed(); } }

		[Constructable]
		public SouthernWashBasinSouthAddon()
		{
			AddComponent( new AddonComponent( 0x30E1 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x30E2 ), 1, 0, 0 );
		}

		public SouthernWashBasinSouthAddon( Serial serial ) : base( serial )
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

	public class SouthernWashBasinSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new SouthernWashBasinSouthAddon(); } }
		public override int LabelNumber{ get{ return 1072865; } } // elven wash basin (south)

		[Constructable]
		public SouthernWashBasinSouthDeed()
		{
		}

		public SouthernWashBasinSouthDeed( Serial serial ) : base( serial )
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
