using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class TirebladdChild : BaseKhaerosMobile, ITirebladd
	{
		[Constructable]
		public TirebladdChild() : base( Nation.Tirebladd ) 
		{			
			if ( this.Female = Utility.RandomBool() ) 
			{ 
				this.Body = 920; 
				this.Name = RandomName( Nation.Tirebladd, true );
			} 
			
			else 
			{ 
				this.Body = 763; 
				this.Name = BaseKhaerosMobile.RandomName( Nation.Tirebladd, false );
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

		public TirebladdChild(Serial serial) : base(serial)
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
