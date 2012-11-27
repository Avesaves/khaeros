using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Greenheart Treefellow corpse" )]
	public class GreenheartTreefellow : Treefellow
	{
		[Constructable]
		public GreenheartTreefellow()
		{
			Name = "a Greenheart Treefellow";
			Hue = 2986;

			SetHits( 198, 212 );

			SetDamage( 20, 24 );

			PackItem( new GreenheartLog( Utility.RandomMinMax( 15, 20 ) ) );
		}

		public GreenheartTreefellow( Serial serial ) : base( serial )
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
