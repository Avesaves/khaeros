using System;
using Server;

namespace Server.Items
{
	public class TallSouthernBedSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed{ get{ return new TallSouthernBedSouthDeed(); } }

		[Constructable]
		public TallSouthernBedSouthAddon()
		{
			AddComponent( new AddonComponent( 0x3058 ),  0,  0, 0 ); // angolo alto sx
			AddComponent( new AddonComponent( 0x3057 ), -1,  1, 0 ); // angolo basso sx
			AddComponent( new AddonComponent( 0x3059 ),  0, -1, 0 ); // angolo alto dx
			AddComponent( new AddonComponent( 0x3056 ),  0,  1, 0 ); // angolo basso dx
		}

		public TallSouthernBedSouthAddon( Serial serial ) : base( serial )
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

	public class TallSouthernBedSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new TallSouthernBedSouthAddon(); } }
		public override int LabelNumber{ get{ return 1072858; } } // tall elven bed (south)

		[Constructable]
		public TallSouthernBedSouthDeed()
		{
		}

		public TallSouthernBedSouthDeed( Serial serial ) : base( serial )
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
