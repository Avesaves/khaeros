using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
	
{
	
	public class Halibut : Item, ICarvable
	{
		public void Carve ( Mobile from, Item item)
		{
			base.ScissorHelper(from, new RawFishSteak(), 4 );
			}
			[Constructable]
			public Halibut() : this (1)
			{
			}
			[Constructable]
			public Halibut( int amount) : base (0x09CC)
			{
				Stackable = true;
				Weight = 1.0;
				Amount = amount;
				Name= "Halibut";
			}
			public Halibut ( Serial serial) : base (serial)
			{
			}
			
			public override void Serialize (GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write ( (int) 0); //version
			}

			
			public override void Deserialize (GenericReader reader)
			{ 
				base.Deserialize( reader );
				int version = reader.ReadInt();
			}
		}
}
	