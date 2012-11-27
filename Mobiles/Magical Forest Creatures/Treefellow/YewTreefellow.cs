using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Yew Treefellow corpse" )]
	public class YewTreefellow : Treefellow
	{
		[Constructable]
		public YewTreefellow()
		{
			Name = "a Yew Treefellow";
			Hue = 2413;

			SetHits( 138, 152 );

			SetDamage( 14, 18 );

			PackItem( new YewLog( Utility.RandomMinMax( 15, 20 ) ) );
		}

		public YewTreefellow( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 442 )
				BaseSoundID = -1;
		}
	}
}
