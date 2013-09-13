using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class DesertHorse : Horse
	{
		public override int[] Hues{ get{ return new int[]{2811,2706,2932}; } }
		
		[Constructable]
		public DesertHorse() : this( "a Desert Horse" )
		{
		}
		
		[Constructable]
		public DesertHorse( string name ) : base( name )
		{
			NewBreed = "Desert Horse";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new DesertHorse() );
		}

		public DesertHorse(Serial serial) : base(serial)
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
			{
				this.BodyValue = 228;
				this.ItemID = 0x3EA1;
			}
		}
	}
}
