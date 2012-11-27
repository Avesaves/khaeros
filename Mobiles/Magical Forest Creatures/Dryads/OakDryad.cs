using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an Oak Dryad corpse" )]
	public class OakDryad : Dryad
	{
		[Constructable]
		public OakDryad()
		{
			Name = "an Oak Dryad";
			PackItem( new Log( Utility.RandomMinMax( 5, 10 ) ) );
		}

		public OakDryad( Serial serial ) : base( serial )
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
