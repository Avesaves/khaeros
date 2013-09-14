using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class RuganHorse : Horse, IWarHorse
	{
		public override int[] Hues{ get{ return new int[]{2796,2840,2797}; } }
		
		[Constructable]
		public RuganHorse() : this( "a Rugan Horse" )
		{
		}
		
		[Constructable]
		public RuganHorse( string name ) : base( name )
		{
			NewBreed = "Rugan Horse";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new RuganHorse() );
		}

		public RuganHorse(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 1);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			
			if( version < 1 )
			{
				this.BodyValue = 226;
				this.ItemID = 0x3EA0;
			}
		}
	}
}
