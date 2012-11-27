using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class Cod : Item, ICarvable
	{
		public void Carve ( Mobile from, Item item)
		{
			base.ScissorHelper(from, new RawFishSteak(), 4 );
			}
			[Constructable]
			public Cod() : this (1)
			{
			}
			[Constructable]
			public Cod( int amount) : base (0x3b0A)
			{
				Stackable = true;
				Weight = 1.0;
				Amount = amount;
				Name= "Cod";
			}
			public Cod ( Serial serial) : base (serial)
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