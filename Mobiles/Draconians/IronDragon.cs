using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "an Iron Dragon corpse" )]
	public class IronDragon : Dragon
	{
		[Constructable]
		public IronDragon ()
		{
			Name = "an Iron Dragon";
			Hue = 2104;
			
			SetHits( 900, 1100 );

			SetDamage( 40, 45 );
			
			Fame = 50000;
			Karma = -50000;
            PackItem( new RewardToken( 3 ) );
		}

		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			
			DragonHead head = new DragonHead();
			head.Hue = 2104;
			
			bpc.DropItem( head );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich, 1 );
			AddLoot( LootPack.Gems, 8 );
		}

		public IronDragon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
