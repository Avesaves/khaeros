using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an Ash Treefellow corpse" )]
	public class AshTreefellow : Treefellow
	{
		[Constructable]
		public AshTreefellow()
		{
			Name = "an Ash Treefellow";
			Hue = 2406;

			SetHits( 178, 192 );

			SetDamage( 18, 22 );

			PackItem( new AshLog( Utility.RandomMinMax( 15, 20 ) ) );
		}

		public AshTreefellow( Serial serial ) : base( serial )
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
