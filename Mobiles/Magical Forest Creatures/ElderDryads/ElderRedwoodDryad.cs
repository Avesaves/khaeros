using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an Elder Redwood Dryad corpse" )]
	public class ElderRedwoodDryad : ElderDryad
	{
		[Constructable]
		public ElderRedwoodDryad()
		{
			Name = "an Elder Redwood Dryad";
			PackItem( new RedwoodLog( Utility.RandomMinMax( 10, 15 ) ) );
			Hue = 0x4AA;
			SetHits( 253, 258 );
			SetDamage( 19, 21 );
		}

		public ElderRedwoodDryad( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
