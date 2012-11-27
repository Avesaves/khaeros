using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class KhemetarHorse : Horse
	{
		public override int[] Hues{ get{ return new int[]{2811,2706,2932}; } }
		
		[Constructable]
		public KhemetarHorse() : this( "a Khemetar Horse" )
		{
		}
		
		[Constructable]
		public KhemetarHorse( string name ) : base( name )
		{
			NewBreed = "Khemetar Horse";
		}
		
		public override void PrepareToGiveBirth()
		{
			GiveBirth( new KhemetarHorse() );
		}

		public KhemetarHorse(Serial serial) : base(serial)
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
