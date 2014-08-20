using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a Wyrm corpse" )]
	public class Quest7 : Dragon
	{
		[Constructable]
		public Quest7 ()
		{
			Name = "A Star Wyrm";
			Body = 59;
            Hue = 2832; 
			SetHits( 2000, 2500 );

			SetDamage( 45, 50 );
			
			Fame = 80000;
			Karma = -80000;
            PackItem( new RewardToken( 11 ) );
            PackItem(new Quest7Item());
            int rand = Utility.Random(40);
            if (rand > 39)
                PackItem(new StarmetalOre(2)); 
		}
		
		public override void AddBodyParts( BodyPartsContainer bpc, Corpse corpse )
		{
			base.AddBodyParts( bpc, corpse );
			DragonHead head = new DragonHead();
			head.Hue = 2832;
            head.Name = "A Wyrm's head"; 
			bpc.DropItem( head );
            bpc.DropItem(new DragonEye()); 
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.SuperBoss, 2 );
			AddLoot( LootPack.Gems, 15 );
		}

		public Quest7( Serial serial ) : base( serial )
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
