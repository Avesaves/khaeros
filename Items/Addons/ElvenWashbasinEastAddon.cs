using System;
using Server;

namespace Server.Items
{
	public class AlyrianWashBasinEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new AlyrianWashBasinEastDeed(); } }

		[Constructable]
		public AlyrianWashBasinEastAddon()
		{
			AddComponent( new AddonComponent( 0x30DF ), 0, -1, 0 );
			AddComponent( new AddonComponent( 0x30E0 ), 0, 0, 0 );
		}

		public AlyrianWashBasinEastAddon( Serial serial ) : base( serial )
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

	public class AlyrianWashBasinEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new AlyrianWashBasinEastAddon(); } }
		public override int LabelNumber{ get{ return 1073387; } } // elven wash basin (east)

		[Constructable]
		public AlyrianWashBasinEastDeed()
		{
		}

		public AlyrianWashBasinEastDeed( Serial serial ) : base( serial )
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
