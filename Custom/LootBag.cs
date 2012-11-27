//  Lootbag by Xanthos
//	Used with XmlGrab
//
//
using System;
using Server;

namespace Server.Items
{
    public class LootBag : Bag
    {		
		[Constructable]
		public LootBag() : base()
		{
			Weight = 0.0;
			Hue = 1266;
			Name = "a loot bag";
			LootType = LootType.Blessed;
		}

		public LootBag( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
    }
}
