using System;

namespace Server.Items
{
	[Flipable(0x1F77, 0x1F78, 0x1F79, 0x1F7A)]
	public class HalfFur : Item
	{
		[Constructable]	
		public HalfFur() : this( 1 )
		{
		}
		
		[Constructable]
		public HalfFur(int amount) : base( 0x1F77 )
		{
			Weight = 1.0;
            Name = "decorative fur";
			Movable = true;
			Stackable = false;
			Amount = amount;
		}

		public HalfFur( Serial serial ) : base( serial )
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
