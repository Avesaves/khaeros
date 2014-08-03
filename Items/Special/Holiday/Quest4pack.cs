using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x232A, 0x232B )]
	public class Quest4pack : GiftBox
	{
		[Constructable]
		public Quest4pack()
		{
            Name = "A giftbox"; 
			DropItem( new Copper(500,1500) );
            DropItem(new Silver(10, 15));
            DropItem(new Silver(1, 2));
            DropItem(new RewardToken(15)); 

		}

		public Quest4pack( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
