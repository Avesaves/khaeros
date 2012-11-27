using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
	{
	public class Tuna : Item, ICarvable
	{
		public void Carve ( Mobile from, Item item)
		{
			base.ScissorHelper(from, new RawFishSteak(), 4 );
		}
			[Constructable]
			public Tuna() : this (1)
			{
			}
			[Constructable]
			public Tuna ( int amount) : base (0x09CE)
			{
				Stackable = true;
				Weight = 1.0;
				Amount = amount;
				Name= "Tuna";
			}
			public Tuna ( Serial serial) : base (serial)
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