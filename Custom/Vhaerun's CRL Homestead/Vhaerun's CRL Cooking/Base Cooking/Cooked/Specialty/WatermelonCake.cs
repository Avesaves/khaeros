using System;
using System.Collections;
using Server.Network;

namespace Server.Items
{
	public class WatermelonCake : Food
	{
		[Constructable]
		public WatermelonCake() : base( 0x9E9 )
		{
			Name = "a watermelon cake";
			Stackable = false;
			this.Weight = 1.0;
			this.FillFactor = 15;
			HitsBonus = 1;
			Hue = 34;
		}

		public WatermelonCake( Serial serial ) : base( serial )
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
