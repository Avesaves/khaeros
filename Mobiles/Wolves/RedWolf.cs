using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class RedWolf : Wolf, IForestCreature
	{
		public override int[] Hues{ get{ return new int[]{2682,2682,2682}; } }
		
		[Constructable]
		public RedWolf() : base()
		{
			NewBreed = "Red Wolf";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new RedWolf() );
		}

		public RedWolf(Serial serial) : base(serial)
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
				Hue = 2682;
		}
	}
}
