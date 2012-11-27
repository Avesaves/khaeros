using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a Greenheart Dryad corpse" )]
	public class GreenheartDryad : Dryad
	{
		[Constructable]
		public GreenheartDryad()
		{
			Name = "a Greenheart Dryad";
			PackItem( new GreenheartLog( Utility.RandomMinMax( 5, 10 ) ) );
			Hue = 0xBAA;
			SetHits( 193, 298 );
			SetDamage( 19, 23 );
		}

		public GreenheartDryad( Serial serial ) : base( serial )
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
