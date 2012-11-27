using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class TyreanHorse : Horse
	{
		public override int[] Hues{ get{ return new int[]{2796,2840,2797}; } }
		
		[Constructable]
		public TyreanHorse() : this( "a Tyrean Horse" )
		{
		}
		
		[Constructable]
		public TyreanHorse( string name ) : base( name )
		{
			NewBreed = "Tyrean Horse";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new TyreanHorse() );
		}

		public TyreanHorse(Serial serial) : base(serial)
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
