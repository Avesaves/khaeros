using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class AlyrianNoble : BaseKhaerosMobile, IAlyrian
	{
		[Constructable]
		public AlyrianNoble() : base( Nation.Alyrian ) 
		{			
			BaseKhaerosMobile.RandomRichClothes( this, Nation.Alyrian );
		}

		public AlyrianNoble(Serial serial) : base(serial)
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
