using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class WesternChild : BaseKhaerosMobile, IWestern
	{
		[Constructable]
		public WesternChild() : base( Nation.Western ) 
		{			
			if ( this.Female = Utility.RandomBool() ) 
			{ 
				this.Body = 516; 
				this.Name = RandomName( Nation.Western, true );
			} 
			
			else 
			{ 
				this.Body = 512; 
				this.Name = BaseKhaerosMobile.RandomName( Nation.Western, false );
			} 
			
			SetStr( 5 );
			SetDex( 5 );
			SetInt( 5 );
			SetHits( 5 );

			SetDamage( 1, 2 );
			Fame = 10;
			Blessed = true;
			Hue = 1142;
		}

		public WesternChild(Serial serial) : base(serial)
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
