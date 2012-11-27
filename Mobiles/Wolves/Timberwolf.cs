using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class Timberwolf : Wolf, ICaveCreature
	{
		public override int[] Hues{ get{ return new int[]{2986,2986,2986}; } }
		
		[Constructable]
		public Timberwolf() : base()
		{
			NewBreed = "Timberwolf";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new Timberwolf() );
		}

		public Timberwolf(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 2);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			
			if( version < 2 )
			{
				this.Name = "a Timberwolf";
				this.ChangeBreed = "Timberwolf Wolf";
			}
			
			if( version < 1 )
				Hue = 2986;
		}
	}
}
