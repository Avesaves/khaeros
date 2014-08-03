using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x232A, 0x232B )]
	public class Quest3pack : GiftBox
	{
		[Constructable]
		public Quest3pack()
		{
            Name = "A giftbox"; 
			DropItem( new Copper(300,900) );
            DropItem(new Silver(10, 15));
            DropItem(new RewardToken(10)); 

		}

		public Quest3pack( Serial serial ) : base( serial )
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
