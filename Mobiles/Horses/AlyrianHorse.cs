using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class AlyrianHorse : Horse
	{
		public override int[] Hues{ get{ return new int[]{2817,2966,2982}; } }
		
		[Constructable]
		public AlyrianHorse() : this( "an Alyrian Horse" )
		{
		}
		
		[Constructable]
		public AlyrianHorse( string name ) : base( name )
		{
			NewBreed = "Alyrian Horse";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new AlyrianHorse() );
		}

		public AlyrianHorse(Serial serial) : base(serial)
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
