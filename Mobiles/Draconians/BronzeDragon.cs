using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Bronze Dragon corpse" )]
	public class BronzeDragon : Dragon
	{
		[Constructable]
		public BronzeDragon () 
		{
			Name = "a Bronze Dragon";
			Hue = 2418;
			
			SetHits( 578, 695 );

			SetDamage( 28, 34 );
			
			Fame = 30000;
			Karma = -30000;
            PackItem( new RewardToken( 2 ) );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			DragonHead head = new DragonHead();
			head.Hue = 2418;
			bpc.DropItem( head );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 1 );
			AddLoot( LootPack.Gems, 8 );
		}

		public BronzeDragon( Serial serial ) : base( serial )
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
