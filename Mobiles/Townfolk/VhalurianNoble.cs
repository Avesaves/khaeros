using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class NorthernNoble : BaseKhaerosMobile, INorthern
	{
		[Constructable]
		public NorthernNoble() : base( Nation.Northern ) 
		{			
			BaseKhaerosMobile.RandomRichClothes( this, Nation.Northern );
		}

		public NorthernNoble(Serial serial) : base(serial)
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
