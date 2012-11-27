using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Redwood Dryad corpse" )]
	public class RedwoodDryad : Dryad
	{
		[Constructable]
		public RedwoodDryad()
		{
			Name = "a Redwood Dryad";
			PackItem( new RedwoodLog( Utility.RandomMinMax( 5, 10 ) ) );
			Hue = 0x4AA;
			SetHits( 153, 258 );
			SetDamage( 15, 19 );
		}

		public RedwoodDryad( Serial serial ) : base( serial )
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
