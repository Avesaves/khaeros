using System;
using Server.Network;

namespace Server.Items
{
	public class Apple : Food
	{
		[Constructable]
		public Apple() : this( 1 )
		{
		}

		[Constructable]
		public Apple( int amount ) : base( amount, 0x9D0 )
		{
			this.Weight = 1.0;
			this.FillFactor = 2;
		}

		public Apple( Serial serial ) : base( serial )
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
