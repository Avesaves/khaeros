using System;
using Server;

namespace Server.Items
{
	public class Marble : Item
	{
		public override double DefaultWeight
		{
			get { return 10.0; }
		}

		[Constructable]
		public Marble() : this( 1 )
		{
		}

		[Constructable]
		public Marble( int amount ) : base( 0x1779 )
		{
			Stackable = true;
			Amount = amount;
			Name = "Marble";
			Hue = 2796;
		}

		public Marble( Serial serial ) : base( serial )
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
