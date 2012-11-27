using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items
	{
	public class Perch : Item, ICarvable
	{
		public void Carve ( Mobile from, Item item)
		{
			base.ScissorHelper(from, new RawFishSteak(), 4 );
			}
			[Constructable]
			public Perch() : this (1)
			{
			}
			[Constructable]
			public Perch( int amount) : base (0x3B06)
			{
				Stackable = true;
				Weight = 1.0;
				Amount = amount;
				Name= "Perch";
			}
			public Perch ( Serial serial) : base (serial)
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