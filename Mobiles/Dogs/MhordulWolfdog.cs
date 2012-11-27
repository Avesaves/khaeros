using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class MhordulWolfdog : Dog, ICaveCreature
	{
		public override int[] Hues{ get{ return new int[]{2991,2797,2796}; } }
		
		[Constructable]
		public MhordulWolfdog() : base()
		{
			NewBreed = "Mhordul Wolfdog";
		}

		public MhordulWolfdog(Serial serial) : base(serial)
		{
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new MhordulWolfdog() );
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
