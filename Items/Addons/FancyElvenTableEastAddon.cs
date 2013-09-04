using System;
using Server;

namespace Server.Items
{
	public class FancySouthernTableEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new FancySouthernTableEastDeed(); } }

		[Constructable]
		public FancySouthernTableEastAddon()
		{
			AddComponent( new AddonComponent( 0x3094 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 0x3093 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x3092 ), 1, 0, 0 );
		}

		public FancySouthernTableEastAddon( Serial serial ) : base( serial )
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

	public class FancySouthernTableEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new FancySouthernTableEastAddon(); } }
		public override int LabelNumber{ get{ return 1073386; } } // hardwood table (east)

		[Constructable]
		public FancySouthernTableEastDeed()
		{
		}

		public FancySouthernTableEastDeed( Serial serial ) : base( serial )
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
