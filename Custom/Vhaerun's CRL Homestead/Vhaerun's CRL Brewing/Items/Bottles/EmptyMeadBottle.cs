
using System;

namespace Server.Items
{
	public class EmptyMeadBottle : Item
	{
		[Constructable]
		public EmptyMeadBottle() : this( 1 )
		{
		}
		
		[Constructable]
        	public EmptyMeadBottle(int amount) : base(0x99B)
		{
			Stackable = true;
			Weight = 1.0;
			Name = "Empty Mead Bottle";
			Amount = amount;
			Layer = Layer.OneHanded;
		}

		public EmptyMeadBottle( Serial serial ) : base( serial )
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
