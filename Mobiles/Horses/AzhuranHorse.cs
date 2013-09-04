using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class WesternHorse : Horse
	{
		public override int[] Hues{ get{ return new int[]{2932,2827,2818}; } }
		
		[Constructable]
		public WesternHorse() : this( "an Western Horse" )
		{
		}
		
		[Constructable]
		public WesternHorse( string name ) : base( name )
		{
			NewBreed = "Western Horse";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new WesternHorse() );
		}

		public WesternHorse(Serial serial) : base(serial)
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
