using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
	public class FishSteak : Food
	{
		[Constructable]
		public FishSteak() : this( 1 )
		{
		}

		[Constructable]
		public FishSteak( int amount ) : base( amount, 0x97B )
		{
			this.Weight = 0.1;
			this.FillFactor = 3;
		}

		public FishSteak( Serial serial ) : base( serial )
		{
		}


		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
