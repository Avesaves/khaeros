using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Huge Dragon corpse" )]
	public class SilverDragon : Dragon, IPeacefulPredator
	{
		[Constructable]
		public SilverDragon ()
		{
			Name = "A Huge Dragon";
			Body = 59;
			
			SetHits( 1500, 1700 );

			SetDamage( 45, 50 );
			
			Fame = 80000;
			Karma = -80000;
            PackItem( new RewardToken( 12 ) );
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

		public SilverDragon( Serial serial ) : base( serial )
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
