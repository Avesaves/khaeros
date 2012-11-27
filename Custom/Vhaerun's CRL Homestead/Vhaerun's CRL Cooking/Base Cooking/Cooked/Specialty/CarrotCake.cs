using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
	public class CarrotCake : Food
	{
		[Constructable]
		public CarrotCake() : base( 0x9E9 )
		{
			Name = "a carrot cake";
			Stackable = false;
			this.Weight = 1.0;
			this.FillFactor = 15;
			HitsBonus = 1;
			Hue = 248;
		}

		public CarrotCake( Serial serial ) : base( serial )
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
