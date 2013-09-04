using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class BloodHound : Dog, IPlainsCreature
	{
		public override int[] Hues{ get{ return new int[]{2822,2816,2821}; } }
		
		[Constructable]
		public BloodHound() : base()
		{
			NewBreed = "bloodhound";
		}

		public BloodHound(Serial serial) : base(serial)
		{
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new BloodHound() );
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
