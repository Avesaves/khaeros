using System;
using Server;

namespace Server.Items
{
	public class AlyrianWashBasinSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new AlyrianWashBasinSouthDeed(); } }

		[Constructable]
		public AlyrianWashBasinSouthAddon()
		{
			AddComponent( new AddonComponent( 0x30E1 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x30E2 ), 1, 0, 0 );
		}

		public AlyrianWashBasinSouthAddon( Serial serial ) : base( serial )
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

	public class AlyrianWashBasinSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new AlyrianWashBasinSouthAddon(); } }
		public override int LabelNumber{ get{ return 1072865; } } // elven wash basin (south)

		[Constructable]
		public AlyrianWashBasinSouthDeed()
		{
		}

		public AlyrianWashBasinSouthDeed( Serial serial ) : base( serial )
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
