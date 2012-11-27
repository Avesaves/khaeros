using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an Elder Greenheart Dryad corpse" )]
	public class ElderGreenheartDryad : ElderDryad
	{
		[Constructable]
		public ElderGreenheartDryad()
		{
			Name = "an Elder Greenheart Dryad";
			PackItem( new GreenheartLog( Utility.RandomMinMax( 10, 15 ) ) );
			Hue = 0xBAA;
			SetHits( 293, 398 );
			SetDamage( 23, 25 );
		}

		public ElderGreenheartDryad( Serial serial ) : base( serial )
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
