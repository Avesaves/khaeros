using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class NorthernPeasant : BaseKhaerosMobile, INorthern
	{
		[Constructable]
		public NorthernPeasant() : base( Nation.Northern ) 
		{			
			BaseKhaerosMobile.RandomPoorClothes( this, Nation.Northern );
		}

		public NorthernPeasant(Serial serial) : base(serial)
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
