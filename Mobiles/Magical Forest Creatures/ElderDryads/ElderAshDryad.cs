using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an Elder Ash Dryad corpse" )]
	public class ElderAshDryad : ElderDryad
	{
		[Constructable]
		public ElderAshDryad()
		{
			Name = "an Elder Ash Dryad";
			PackItem( new AshLog( Utility.RandomMinMax( 10, 15 ) ) );
			Hue = 0x966;
			SetHits( 273, 378 );
			SetDamage( 21, 23 );
		}

		public ElderAshDryad( Serial serial ) : base( serial )
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
