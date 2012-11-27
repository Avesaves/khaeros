using System;
using System.Collections;
using Server.Items;
using Server.Guilds;

namespace Server.Mobiles
{
	public class SBProvisioner : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBProvisioner()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( Arrow ), 2, 20, 0xF3F, 0 ) );
				Add( new GenericBuyInfo( typeof( Bolt ), 2, 20, 0x1BFB, 0 ) );

				Add( new GenericBuyInfo( typeof( Backpack ), 48, 20, 0x9B2, 0 ) );
				Add( new GenericBuyInfo( typeof( Pouch ), 24, 20, 0xE79, 0 ) );
				Add( new GenericBuyInfo( typeof( Bag ), 36, 20, 0xE76, 0 ) );
				
				Add( new GenericBuyInfo( typeof( Candle ), 12, 20, 0xA28, 0 ) );
				Add( new GenericBuyInfo( typeof( Torch ), 12, 20, 0xF6B, 0 ) );
				Add( new GenericBuyInfo( typeof( Lantern ), 12, 20, 0xA25, 0 ) );

				Add( new GenericBuyInfo( typeof( Lockpick ), 12, 20, 0x14FC, 0 ) );

				Add( new GenericBuyInfo( typeof( BreadLoaf ), 6, 10, 0x103B, 0 ) );
				Add( new GenericBuyInfo( typeof( LambLeg ), 8, 20, 0x160A, 0 ) );
				Add( new GenericBuyInfo( typeof( ChickenLeg ), 5, 20, 0x1608, 0 ) );
				Add( new GenericBuyInfo( typeof( CookedBird ), 17, 20, 0x9B7, 0 ) );

				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Ale, 7, 20, 0x99F, 0 ) );
				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Wine, 7, 20, 0x9C7, 0 ) );
				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Liquor, 7, 20, 0x99B, 0 ) );
				Add( new BeverageBuyInfo( typeof( Jug ), BeverageType.Cider, 13, 20, 0x9C8, 0 ) );

				Add( new GenericBuyInfo( typeof( Pear ), 3, 20, 0x994, 0 ) );
				Add( new GenericBuyInfo( typeof( Apple ), 3, 20, 0x9D0, 0 ) );

				Add( new GenericBuyInfo( typeof( RedBook ), 15, 20, 0xFF1, 0 ) );
				Add( new GenericBuyInfo( typeof( BlueBook ), 15, 20, 0xFF2, 0 ) );
				Add( new GenericBuyInfo( typeof( TanBook ), 15, 20, 0xFF0, 0 ) );

				Add( new GenericBuyInfo( typeof( WoodenBox ), 60, 20, 0xE7D, 0 ) );
				Add( new GenericBuyInfo( typeof( Key ), 12, 20, 0x100E, 0 ) );

				Add( new GenericBuyInfo( typeof( Bedroll ), 12, 20, 0xA59, 0 ) );
				Add( new GenericBuyInfo( typeof( Kindling ), 2, 20, 0xDE1, 0 ) );

				Add( new GenericBuyInfo( "1016450", typeof( Chessboard ), 12, 20, 0xFA6, 0 ) );
				Add( new GenericBuyInfo( "1016449", typeof( CheckerBoard ), 12, 20, 0xFA6, 0 ) );
				Add( new GenericBuyInfo( typeof( Backgammon ), 12, 20, 0xE1C, 0 ) );
				
				if ( Core.AOS )
					Add( new GenericBuyInfo( typeof( Engines.Mahjong.MahjongGame ), 16, 20, 0xFAA, 0 ) );
				Add( new GenericBuyInfo( typeof( Dices ), 12, 20, 0xFA7, 0 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
			}
		}
	}
}
