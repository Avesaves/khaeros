using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	public class WesternCrafter : BaseKhaerosMobile, IWestern
	{
		[Constructable]
		public WesternCrafter() : base( Nation.Western ) 
		{			
			BaseKhaerosMobile.RandomCrafterClothes( this, Nation.Western );
		}

		public WesternCrafter(Serial serial) : base(serial)
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
