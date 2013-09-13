using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class HaluarocChild : BaseKhaerosMobile, IHaluaroc
	{
		[Constructable]
		public HaluarocChild() : base( Nation.Haluaroc ) 
		{			
			if ( this.Female = Utility.RandomBool() ) 
			{ 
				this.Body = 516; 
				this.Name = RandomName( Nation.Haluaroc, true );
			} 
			
			else 
			{ 
				this.Body = 512; 
				this.Name = BaseKhaerosMobile.RandomName( Nation.Haluaroc, false );
			} 
			
			SetStr( 5 );
			SetDex( 5 );
			SetInt( 5 );
			SetHits( 5 );

			SetDamage( 1, 2 );
			Fame = 10;
			Blessed = true;
			Hue = 0;
		}

		public HaluarocChild(Serial serial) : base(serial)
		{
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
