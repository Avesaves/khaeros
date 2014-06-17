using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Huge Dragon corpse" )]
	public class GoldDragon : Dragon, IPeacefulPredator
	{
		[Constructable]
		public GoldDragon ()
		{
			Name = "A Huge Dragon";
			Body = 59;
			
			SetHits( 1500, 1700 );

			SetDamage( 45, 50 );
			
			Fame = 80000;
			Karma = -80000;
            PackItem( new RewardToken( 13 ) );
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
			AddLoot( LootPack.SuperBoss, 2 );
			AddLoot( LootPack.Gems, 15 );
		}

		public GoldDragon( Serial serial ) : base( serial )
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
