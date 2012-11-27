using System;
using Server;

namespace Server.Items
{
	public class TallAlyrianBedEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new TallAlyrianBedEastDeed(); } }

		[Constructable]
		public TallAlyrianBedEastAddon()
		{
			AddComponent( new AddonComponent( 0x3054 ), 0,  0, 0 );
			AddComponent( new AddonComponent( 0x3053 ), 1,  0, 0 );
			AddComponent( new AddonComponent( 0x3055 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 0x3052 ), 2,  0, 0 );
		}

		public TallAlyrianBedEastAddon( Serial serial ) : base( serial )
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

	public class TallAlyrianBedEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new TallAlyrianBedEastAddon(); } }
		public override int LabelNumber{ get{ return 1072859; } } // tall elven bed (east)

		[Constructable]
		public TallAlyrianBedEastDeed()
		{
		}

		public TallAlyrianBedEastDeed( Serial serial ) : base( serial )
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
