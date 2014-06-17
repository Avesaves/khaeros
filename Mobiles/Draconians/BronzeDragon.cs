using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Dragon corpse" )]
	public class BronzeDragon : Dragon
	{
		[Constructable]
		public BronzeDragon () 
		{
			Name = "A Dragon";
			Body = 61;
			
			SetHits( 1100, 1300 );

			SetDamage( 42, 47 );
			
			Fame = 60000;
			Karma = -60000;
            PackItem( new RewardToken( 9 ) );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			DragonHead head = new DragonHead();
			head.Hue = 0;
			bpc.DropItem( head );
            bpc.DropItem(new DragonEye()); 
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 1 );
			AddLoot( LootPack.Gems, 10 );
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
