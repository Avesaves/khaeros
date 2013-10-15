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
				Add( new GenericBuyInfo( typeof( Arrow ), 3, 20, 0xF3F, 0 ) );
				Add( new GenericBuyInfo( typeof( Bolt ), 3, 20, 0x1BFB, 0 ) );

				Add( new GenericBuyInfo( typeof( Backpack ), 60, 20, 0x9B2, 0 ) );
				Add( new GenericBuyInfo( typeof( Pouch ), 30, 20, 0xE79, 0 ) );
				Add( new GenericBuyInfo( typeof( Bag ), 45, 20, 0xE76, 0 ) );
				
				Add( new GenericBuyInfo( typeof( Candle ), 10, 20, 0xA28, 0 ) );
				Add( new GenericBuyInfo( typeof( Torch ), 15, 20, 0xF6B, 0 ) );
				Add( new GenericBuyInfo( typeof( Lantern ), 20, 20, 0xA25, 0 ) );

				Add( new GenericBuyInfo( typeof( Lockpick ), 30, 20, 0x14FC, 0 ) );

				Add( new GenericBuyInfo( typeof( BreadLoaf ), 7, 10, 0x103B, 0 ) );
				Add( new GenericBuyInfo( typeof( LambLeg ), 20, 20, 0x160A, 0 ) );
				Add( new GenericBuyInfo( typeof( ChickenLeg ), 12, 20, 0x1608, 0 ) );
				Add( new GenericBuyInfo( typeof( CookedBird ), 42, 20, 0x9B7, 0 ) );

				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Ale, 18, 20, 0x99F, 0 ) );
				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Wine, 20, 20, 0x9C7, 0 ) );
				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Liquor, 18, 20, 0x99B, 0 ) );
				Add( new BeverageBuyInfo( typeof( Jug ), BeverageType.Cider, 34, 20, 0x9C8, 0 ) );

				Add( new GenericBuyInfo( typeof( Pear ), 6, 20, 0x994, 0 ) );
				Add( new GenericBuyInfo( typeof( Apple ), 6, 20, 0x9D0, 0 ) );

				Add( new GenericBuyInfo( typeof( RedBook ), 36, 20, 0xFF1, 0 ) );
				Add( new GenericBuyInfo( typeof( BlueBook ), 36, 20, 0xFF2, 0 ) );
				Add( new GenericBuyInfo( typeof( TanBook ), 36, 20, 0xFF0, 0 ) );

				Add( new GenericBuyInfo( typeof( WoodenBox ), 100, 20, 0xE7D, 0 ) );
				Add( new GenericBuyInfo( typeof( Key ), 20, 20, 0x100E, 0 ) );

				Add( new GenericBuyInfo( typeof( Bedroll ), 30, 20, 0xA59, 0 ) );
				Add( new GenericBuyInfo( typeof( Kindling ), 3, 20, 0xDE1, 0 ) );

				Add( new GenericBuyInfo( "1016450", typeof( Chessboard ), 32, 20, 0xFA6, 0 ) );
				Add( new GenericBuyInfo( "1016449", typeof( CheckerBoard ), 32, 20, 0xFA6, 0 ) );
				Add( new GenericBuyInfo( typeof( Backgammon ), 32, 20, 0xE1C, 0 ) );
				
				if ( Core.AOS )
					Add( new GenericBuyInfo( typeof( Engines.Mahjong.MahjongGame ), 40, 20, 0xFAA, 0 ) );
				Add( new GenericBuyInfo( typeof( Dices ), 25, 20, 0xFA7, 0 ) );
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
