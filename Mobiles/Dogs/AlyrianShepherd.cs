using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class AlyrianShepherd : Dog, IForestCreature
	{
		public override int[] Hues{ get{ return new int[]{2818,2932,2814}; } }
		
		[Constructable]
		public AlyrianShepherd() : base()
		{
			NewBreed = "Alyrian Shepherd";
		}

		public AlyrianShepherd(Serial serial) : base(serial)
		{
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new AlyrianShepherd() );
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
