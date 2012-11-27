using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class ManedWolf : Wolf, IJungleCreature
	{
		public override int[] Hues{ get{ return new int[]{2787,2787,2787}; } }
		
		[Constructable]
		public ManedWolf() : base()
		{
			NewBreed = "Maned Wolf";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new ManedWolf() );
		}

		public ManedWolf(Serial serial) : base(serial)
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
				Hue = 2787;
		}
	}
}
