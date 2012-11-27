using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class SnowWolf : Wolf, ITundraCreature
	{
		public override int[] Hues{ get{ return new int[]{2984,2984,2984}; } }
		
		[Constructable]
		public SnowWolf() : base()
		{
			NewBreed = "Snow Wolf";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new SnowWolf() );
		}

		public SnowWolf(Serial serial) : base(serial)
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
				Hue = 2984;
		}
	}
}
