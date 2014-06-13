using System;
using Server;
using Server.Engines.Alchemy;

namespace Server.Items
{
	public class BegoniaPlant : BaseFlowerPlant
	{
		public override Type Ingredient { get { return typeof( Begonia ); } }
		
		[Constructable]
		public BegoniaPlant() : base( 3205 )
		{
			Hue = Utility.RandomList( 2985 );
			Name = "Wooly-Stalked Begonia";
		}

		public BegoniaPlant( Serial serial ) : base( serial )
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