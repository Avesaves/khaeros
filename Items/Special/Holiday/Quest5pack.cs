using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x232A, 0x232B )]
	public class Quest5pack : GiftBox
	{
		[Constructable]
		public Quest5pack()
		{
            Name = "A giftbox"; 
			DropItem( new Copper(100,1500) );
            DropItem(new Silver(10, 15));
            DropItem(new Silver(1, 6));
            DropItem(new RewardToken(25)); 

		}

		public Quest5pack( Serial serial ) : base( serial )
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
