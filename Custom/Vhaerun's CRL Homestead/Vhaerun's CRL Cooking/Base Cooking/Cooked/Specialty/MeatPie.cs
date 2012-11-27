using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
	public class MeatPie : Food
	{
		[Constructable]
		public MeatPie() : base( 0x1041 )
		{
			Stackable = false;
			this.Weight = 1.0;
			this.FillFactor = 5;
			Name = "meat pie";
		}

		public MeatPie( Serial serial ) : base( serial )
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
