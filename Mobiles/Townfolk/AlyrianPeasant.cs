using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class AlyrianPeasant : BaseKhaerosMobile, IAlyrian
	{
		[Constructable]
		public AlyrianPeasant() : base( Nation.Alyrian ) 
		{			
			BaseKhaerosMobile.RandomPoorClothes( this, Nation.Alyrian );
		}

		public AlyrianPeasant(Serial serial) : base(serial)
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
