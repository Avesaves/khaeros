using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Yew Dryad corpse" )]
	public class YewDryad : Dryad
	{
		[Constructable]
		public YewDryad()
		{
			Name = "a Yew Dryad";
			PackItem( new YewLog( Utility.RandomMinMax( 5, 10 ) ) );
			Hue = 0x96D;
			SetHits( 133, 238 );
			SetDamage( 13, 17 );
		}

		public YewDryad( Serial serial ) : base( serial )
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
