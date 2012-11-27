using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class PorkHock : Item
	{

		[Constructable]
		public PorkHock() : this( 1 )
		{
		}

		[Constructable]
		public PorkHock( int amount ) : base( 0x9D3 )
		{
			this.Stackable = true;
			this.Weight = 2.0;
			this.Amount = amount;
			this.Name = "Pork Hock";
			this.Hue = 0x457;
		}

		

		public PorkHock( Serial serial ) : base( serial )
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
