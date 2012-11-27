using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class AzhuranRetriever : Dog, IJungleCreature
	{
		public override int[] Hues{ get{ return new int[]{2785,2787,2789}; } }
		
		[Constructable]
		public AzhuranRetriever() : base()
		{
			NewBreed = "Azhuran Retriever";
		}

		public AzhuranRetriever(Serial serial) : base(serial)
		{
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new AzhuranRetriever() );
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
