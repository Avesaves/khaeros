using System;
using Server;
using Server.Engines.Alchemy;

namespace Server.Items
{
	public class SapphicRosePlant : BaseFlowerPlant
	{
		public override Type Ingredient { get { return typeof( SapphicRose ); } }
		
		[Constructable]
		public SapphicRosePlant() : base( 9035 )
		{
			Hue = 2637;
			Name = "Sapphic Rose";
		}

		public SapphicRosePlant( Serial serial ) : base( serial )
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