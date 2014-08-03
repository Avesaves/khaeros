using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[Flipable( 0x232A, 0x232B )]
	public class Quest2pack : GiftBox
	{
		[Constructable]
		public Quest2pack()
		{
            Name = "A giftbox"; 
			DropItem( new Copper(100,200) );
            DropItem(new Silver(5, 10));
            DropItem(new RewardToken(5)); 

		}

		public Quest2pack( Serial serial ) : base( serial )
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
