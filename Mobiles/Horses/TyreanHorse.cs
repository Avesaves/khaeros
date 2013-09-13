using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class SnowHorse : Horse
	{
		public override int[] Hues{ get{ return new int[]{2796,2840,2797}; } }
		
		[Constructable]
		public SnowHorse() : this( "a Snow Horse" )
		{
		}
		
		[Constructable]
		public SnowHorse( string name ) : base( name )
		{
			NewBreed = "Snow Horse";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new SnowHorse() );
		}

		public SnowHorse(Serial serial) : base(serial)
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
