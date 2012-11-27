using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Steel Dragon corpse" )]
	public class SteelDragon : Dragon
	{
		[Constructable]
		public SteelDragon ()
		{
			Name = "a Steel Dragon";
			Hue = 1401;
			
			SetHits( 878, 995 );

			SetDamage( 36, 42 );
			
			Fame = 50000;
			Karma = 50000;
            PackItem( new RewardToken( 4 ) );
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			DragonHead head = new DragonHead();
			head.Hue = 1401;
			bpc.DropItem( head );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 1 );
			AddLoot( LootPack.Gems, 8 );
		}

		public SteelDragon( Serial serial ) : base( serial )
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
