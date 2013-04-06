using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Silver Dragon corpse" )]
	public class SilverDragon : Dragon, IPeacefulPredator
	{
		[Constructable]
		public SilverDragon ()
		{
			Name = "a Silver Dragon";
			Hue = 2985;
			
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
			head.Hue = 2985;
			bpc.DropItem( head );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 1 );
			AddLoot( LootPack.Gems, 8 );
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
