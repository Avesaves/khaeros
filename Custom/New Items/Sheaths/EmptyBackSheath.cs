using System;
using Server.Mobiles;

namespace Server.Items
{
	public class EmptyBackSheath : Item
	{
		[Constructable]
		public EmptyBackSheath() : base( 0x3B35 )
		{
			Name = "empty back sheath";
			Weight = 1.0;
			Layer = Layer.Invalid;
		}

		public EmptyBackSheath( Serial serial ) : base( serial )
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
