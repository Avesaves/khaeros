using System;
using Server;
using Server.Engines.Alchemy;

namespace Server.Items
{
	public class OrchidPlant : BaseFlowerPlant
	{
		public override Type Ingredient { get { return typeof( Orchid ); } }
		
		[Constructable]
		public OrchidPlant() : base( 9036 )
		{
			Hue = Utility.RandomList( 0, 2652, 2678, 2675, 2966, 2971, 2659, 2602, 2661, 2618, 2632 );
			Name = "Orchid";
		}

		public OrchidPlant( Serial serial ) : base( serial )
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