using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class GrayWolf : Wolf, IPlainsCreature
	{
		public override int[] Hues{ get{ return new int[]{2617,2617,2617}; } }
		
		[Constructable]
		public GrayWolf() : base()
		{
			NewBreed = "Gray Wolf";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new GrayWolf() );
		}

		public GrayWolf(Serial serial) : base(serial)
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
				Hue = 2617;
		}
	}
}
