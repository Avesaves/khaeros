using System;
using Server;

namespace Server.Items
{
	public class Coal : Item
	{
		public override double DefaultWeight
		{
			get { return 1.0; }
		}

		[Constructable]
		public Coal() : this( 1 )
		{
		}

		[Constructable]
		public Coal( int amount ) : base( 0x19B9 )
		{
			Stackable = true;
			Amount = amount;
			Name = "Coal";
			Hue = 1899;
			Weight = 1.0;
		}

		public Coal( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
				Weight = 1.0;
			
			if( version < 2 )
				ItemID = 0x19B9;
		}
	}
}
