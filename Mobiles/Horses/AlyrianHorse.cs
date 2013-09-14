using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class GallowayHorse : Horse
	{
		public override int[] Hues{ get{ return new int[]{2817,2966,2982}; } }
		
		[Constructable]
		public GallowayHorse() : this( "a Galloway Horse" )
		{
		}
		
		[Constructable]
		public GallowayHorse( string name ) : base( name )
		{
			NewBreed = "Galloway Horse";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new GallowayHorse() );
		}

		public GallowayHorse(Serial serial) : base(serial)
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
				this.BodyValue = 228;
				this.ItemID = 0x3EA1;
			}
		}
	}
}
