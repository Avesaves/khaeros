using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class RoseanHorse : Horse, IWarHorse
	{
		public override int[] Hues{ get{ return new int[]{2803,2725,2830}; } }
		
		[Constructable]
		public RoseanHorse() : this( "a Rosean Horse" )
		{
		}
		
		[Constructable]
		public RoseanHorse( string name ) : base( name )
		{
			NewBreed = "Rosean Horse";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new RoseanHorse() );
		}

		public RoseanHorse(Serial serial) : base(serial)
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
				this.BodyValue = 226;
				this.ItemID = 0x3EA0;
			}
		}
	}
}
