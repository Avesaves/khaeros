using System;

namespace Server.Items
{
	public class Fur : Item
	{
		[Constructable]
		public Fur() : this( 1 )
		{
		}
		
		[Constructable]
		public Fur( int amount ) : base( 0x11F5 )
		{
			Weight = 1.0;
            Name = "fur";
            Stackable = true;
            Amount = amount;
		}

		public Fur( Serial serial ) : base( serial )
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
