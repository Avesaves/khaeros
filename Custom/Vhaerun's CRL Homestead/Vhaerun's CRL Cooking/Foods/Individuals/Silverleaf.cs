using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
	public class Silverleaf : Food
	{
		[Constructable]
		public Silverleaf() : this( 1 )
		{
		}

		[Constructable]
		public Silverleaf( int amount ) : base( amount, 0x9B6 )  // friedeggs graphic
		{
			this.Name = "Silverleaf meal";
			this.Hue = 96;			// ... open to suggestions. ;)
			this.Weight = 0.5;
			this.FillFactor = 0;		// yes, 0.
		}

		public Silverleaf( Serial serial ) : base( serial )
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
