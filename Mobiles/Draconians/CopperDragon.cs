using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Copper Dragon corpse" )]
	public class CopperDragon : Dragon
	{
		[Constructable]
		public CopperDragon ()
		{
			Name = "A Dragon";
			Body = 60;
			
			SetHits( 900, 1100 );

			SetDamage( 40, 45 );
			
			Fame = 50000;
			Karma = -50000;
            PackItem( new RewardToken( 8 ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 1 );
			AddLoot( LootPack.Gems, 8 );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			DragonHead head = new DragonHead();
			head.Hue = 0;
			bpc.DropItem( head );
            bpc.DropItem(new DragonEye()); 
		}

		public CopperDragon( Serial serial ) : base( serial )
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
