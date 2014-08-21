using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x232A, 0x232B )]
	public class Quest7pack : GiftBox
	{
		[Constructable]
		public Quest7pack()
		{
            Name = "A giftbox"; 
			DropItem( new Copper(1000,5000) );
            DropItem(new Silver(15, 35));
            DropItem(new Gold(1, 25));
            DropItem(new RewardToken(40));
            int rand = Utility.Random(10);
            if (rand > 8)
                DropItem(new ManaBurnScroll());

		}

		public Quest7pack( Serial serial ) : base( serial )
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
