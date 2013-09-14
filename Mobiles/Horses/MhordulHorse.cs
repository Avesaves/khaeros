using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class SteppeHorse : Horse
	{
		public override int[] Hues{ get{ return new int[]{2796,2820,2991}; } }
		
		[Constructable]
		public SteppeHorse() : this( "a Steppe Horse" )
		{
		}
		
		[Constructable]
		public SteppeHorse( string name ) : base( name )
		{
			NewBreed = "Steppe Horse";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new SteppeHorse() );
		}

		public SteppeHorse(Serial serial) : base(serial)
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
