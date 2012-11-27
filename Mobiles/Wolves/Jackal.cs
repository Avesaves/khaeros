using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class Jackal : Wolf, IDesertCreature
	{
		public override int[] Hues{ get{ return new int[]{2811,2811,2811}; } }
		
		[Constructable]
		public Jackal() : base()
		{
			NewBreed = "Jackal";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new Jackal() );
		}

		public Jackal(Serial serial) : base(serial)
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
				Hue = 2811;
		}
	}
}
