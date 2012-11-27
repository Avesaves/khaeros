using System;
using Server;

namespace Server.Items
{
	public class FancyAlyrianTableSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new FancyAlyrianTableSouthDeed(); } }

		[Constructable]
		public FancyAlyrianTableSouthAddon()
		{
			AddComponent( new AddonComponent( 0x3095 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 0x3096 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x3097 ), 0, -1, 0 );
		}

		public FancyAlyrianTableSouthAddon( Serial serial ) : base( serial )
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

	public class FancyAlyrianTableSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new FancyAlyrianTableSouthAddon(); } }
		public override int LabelNumber{ get{ return 1073385; } } // hardwood table (south)

		[Constructable]
		public FancyAlyrianTableSouthDeed()
		{
		}

		public FancyAlyrianTableSouthDeed( Serial serial ) : base( serial )
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
