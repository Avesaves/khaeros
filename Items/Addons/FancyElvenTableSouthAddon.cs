using System;
using Server;

namespace Server.Items
{
	public class FancySouthernTableSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new FancySouthernTableSouthDeed(); } }

		[Constructable]
		public FancySouthernTableSouthAddon()
		{
			AddComponent( new AddonComponent( 0x3095 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0x3096 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x3097 ), 0, -1, 0 );
		}

		public FancySouthernTableSouthAddon( Serial serial ) : base( serial )
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

	public class FancySouthernTableSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new FancySouthernTableSouthAddon(); } }
		public override int LabelNumber{ get{ return 1073385; } } // hardwood table (south)

		[Constructable]
		public FancySouthernTableSouthDeed()
		{
		}

		public FancySouthernTableSouthDeed( Serial serial ) : base( serial )
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
