using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class AzhuranHorse : Horse
	{
		public override int[] Hues{ get{ return new int[]{2932,2827,2818}; } }
		
		[Constructable]
		public AzhuranHorse() : this( "an Azhuran Horse" )
		{
		}
		
		[Constructable]
		public AzhuranHorse( string name ) : base( name )
		{
			NewBreed = "Azhuran Horse";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new AzhuranHorse() );
		}

		public AzhuranHorse(Serial serial) : base(serial)
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
