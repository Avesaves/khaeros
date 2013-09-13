using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class Saluki : Dog, IDesertCreature
	{
		public override int[] Hues{ get{ return new int[]{2720,2601,2986}; } }
		
		[Constructable]
		public Saluki() : base()
		{
			NewBreed = "Saluki";
		}

		public Saluki(Serial serial) : base(serial)
		{
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new Saluki() );
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
