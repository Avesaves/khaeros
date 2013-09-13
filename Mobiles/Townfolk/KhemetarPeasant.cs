using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class HaluarocPeasant : BaseKhaerosMobile, IHaluaroc
	{
		[Constructable]
		public HaluarocPeasant() : base( Nation.Haluaroc ) 
		{			
			BaseKhaerosMobile.RandomPoorClothes( this, Nation.Haluaroc );
		}

		public HaluarocPeasant(Serial serial) : base(serial)
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
