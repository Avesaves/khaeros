using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class VhalurianBloodhound : Dog, IPlainsCreature
	{
		public override int[] Hues{ get{ return new int[]{2822,2816,2821}; } }
		
		[Constructable]
		public VhalurianBloodhound() : base()
		{
			NewBreed = "Vhalurian Bloodhound";
		}

		public VhalurianBloodhound(Serial serial) : base(serial)
		{
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new VhalurianBloodhound() );
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
