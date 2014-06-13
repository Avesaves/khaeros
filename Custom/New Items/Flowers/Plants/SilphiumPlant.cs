using System;
using Server;
using Server.Engines.Alchemy;

namespace Server.Items
{
	public class SilphiumPlant : BaseFlowerPlant
	{
		public override Type Ingredient { get { return typeof( Silphium ); } }
		
		[Constructable]
		public SilphiumPlant() : base( 9035 )
		{
			Hue = 2935;
			Name = "Silphium";
		}

		public SilphiumPlant( Serial serial ) : base( serial )
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