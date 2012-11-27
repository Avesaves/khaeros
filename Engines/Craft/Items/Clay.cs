using System;

namespace Server.Items
{
	public class Clay : Item
	{
		[Constructable]
		public Clay() : this( 1 )
		{
		}

		[Constructable]
		public Clay( int amount ) : base( 0xF81 )
		{
			Weight = 1.0;
			Stackable = true;
			Amount = amount;
            Name = "Clay";
		}

		public Clay( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
