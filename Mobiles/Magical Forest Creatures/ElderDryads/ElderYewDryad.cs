using System;
using Server;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "an Elder Yew Dryad corpse" )]
	public class ElderYewDryad : ElderDryad
	{
		[Constructable]
		public ElderYewDryad()
		{
			Name = "an Elder Yew Dryad";
			PackItem( new YewLog( Utility.RandomMinMax( 10, 15 ) ) );
			Hue = 0x96D;
			SetHits( 233, 338 );
			SetDamage( 17, 19 );
		}

		public ElderYewDryad( Serial serial ) : base( serial )
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
