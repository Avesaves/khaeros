using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an Ash Dryad corpse" )]
	public class AshDryad : Dryad
	{
		[Constructable]
		public AshDryad()
		{
			Name = "an Ash Dryad";
			PackItem( new AshLog( Utility.RandomMinMax( 5, 10 ) ) );
			Hue = 0x966;
			SetHits( 173, 278 );
			SetDamage( 17, 21 );
		}

		public AshDryad( Serial serial ) : base( serial )
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
