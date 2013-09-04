using System;
using Server;

namespace Server.Items
{
	public class OrnateSouthernTableEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new OrnateSouthernTableEastDeed(); } }

		[Constructable]
		public OrnateSouthernTableEastAddon()
		{
			AddComponent( new AddonComponent( 0x308E ), -1, 0, 0 );
			AddComponent( new AddonComponent( 0x308D ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x308C ), 1, 0, 0 );
		}

		public OrnateSouthernTableEastAddon( Serial serial ) : base( serial )
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

	public class OrnateSouthernTableEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new OrnateSouthernTableEastAddon(); } }
		public override int LabelNumber{ get{ return 1073384; } } // ornate table (east)

		[Constructable]
		public OrnateSouthernTableEastDeed()
		{
		}

		public OrnateSouthernTableEastDeed( Serial serial ) : base( serial )
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
