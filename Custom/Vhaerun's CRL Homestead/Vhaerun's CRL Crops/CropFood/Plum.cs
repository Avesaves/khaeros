using System;
using Server.Network;

namespace Server.Items
{
	public class Plum : Food
	{
		[Constructable]
		public Plum() : this( 1 )
		{
		}

		[Constructable]
		public Plum( int amount ) : base( amount, 0x9D2 )
		{
			this.Weight = 1.0;
			this.FillFactor = 2;
			this.Hue = 0x264;
			this.Name = "Plum";
		}

		public Plum( Serial serial ) : base( serial )
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
