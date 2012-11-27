using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Redwood Treefellow corpse" )]
	public class RedwoodTreefellow : Treefellow
	{
		[Constructable]
		public RedwoodTreefellow()
		{
			Name = "a Redwood Treefellow";
			Hue = 1194;

			SetHits( 158, 172 );

			SetDamage( 16, 20 );

			PackItem( new RedwoodLog( Utility.RandomMinMax( 15, 20 ) ) );
		}

		public RedwoodTreefellow( Serial serial ) : base( serial )
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
