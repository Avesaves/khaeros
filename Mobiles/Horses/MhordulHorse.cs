using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class MhordulHorse : Horse
	{
		public override int[] Hues{ get{ return new int[]{2796,2820,2991}; } }
		
		[Constructable]
		public MhordulHorse() : this( "a Mhordul Horse" )
		{
		}
		
		[Constructable]
		public MhordulHorse( string name ) : base( name )
		{
			NewBreed = "Mhordul Horse";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new MhordulHorse() );
		}

		public MhordulHorse(Serial serial) : base(serial)
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
				this.BodyValue = 204;
				this.ItemID = 0x3EA2;
			}
		}
	}
}
