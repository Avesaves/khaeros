using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an Oak Treefellow corpse" )]
	public class OakTreefellow : Treefellow
	{
		[Constructable]
		public OakTreefellow()
		{
			Name = "an Oak Treefellow";
			Hue = 0;

			SetHits( 118, 132 );

			SetDamage( 12, 16 );

			PackItem( new Log( Utility.RandomMinMax( 15, 20 ) ) );
		}

		public OakTreefellow( Serial serial ) : base( serial )
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
