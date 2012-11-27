using System;
using Server;

namespace Server.Items
{
	public class AlyrianBedSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new AlyrianBedSouthDeed(); } }

		[Constructable]
		public AlyrianBedSouthAddon()
		{
			AddComponent( new AddonComponent( 0x3050 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 0x3051 ), 0, -1, 0 );
		}

		public AlyrianBedSouthAddon( Serial serial ) : base( serial )
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

	public class AlyrianBedSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new AlyrianBedSouthAddon(); } }
		public override int LabelNumber{ get{ return 1072860; } } // elven bed (south)

		[Constructable]
		public AlyrianBedSouthDeed()
		{
		}

		public AlyrianBedSouthDeed( Serial serial ) : base( serial )
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
