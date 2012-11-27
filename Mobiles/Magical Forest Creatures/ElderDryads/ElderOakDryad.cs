using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an Elder Oak Dryad corpse" )]
	public class ElderOakDryad : ElderDryad
	{
		[Constructable]
		public ElderOakDryad()
		{
			Name = "an Elder Oak Dryad";
			PackItem( new Log( Utility.RandomMinMax( 10, 15 ) ) );
		}

		public ElderOakDryad( Serial serial ) : base( serial )
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
