using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x232A, 0x232B )]
	public class Quest6pack : GiftBox
	{
		[Constructable]
		public Quest6pack()
		{
            Name = "A giftbox"; 
			DropItem( new Copper(1000,2500) );
            DropItem(new Silver(15, 35));
            DropItem(new Gold(1, 10));
            DropItem(new RewardToken(25)); 

		}

		public Quest6pack( Serial serial ) : base( serial )
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
