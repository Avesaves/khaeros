using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Huge Dragon corpse" )]
	public class IronDragon : Dragon
	{
		[Constructable]
		public IronDragon ()
		{
			Name = "A Huge Dragon";
			Body = 12;
			
			SetHits( 1300, 1500 );

			SetDamage( 44, 49 );
			
			Fame = 70000;
			Karma = -70000;
            PackItem( new RewardToken( 10 ) );
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
			AddLoot( LootPack.Gems, 12 );
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
