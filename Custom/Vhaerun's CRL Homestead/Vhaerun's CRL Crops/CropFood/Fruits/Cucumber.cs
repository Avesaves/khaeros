using System;
using Server.Network;

namespace Server.Items
{
        [FlipableAttribute( 0xc80, 0xc81 )]
	public class Cucumber : Food
	{
		[Constructable]
		public Cucumber() : this( 1 )
		{
		}

		[Constructable]
		public Cucumber( int amount ) : base( amount, 0xc81 )
		{
                        Name = "Cucumber";
                        Hue = 0x2FF;
			this.Weight = 1.0;
			this.FillFactor = 1;
		}

		public Cucumber( Serial serial ) : base( serial )
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
