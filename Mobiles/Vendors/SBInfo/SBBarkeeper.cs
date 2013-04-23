using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBBarkeeper : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBBarkeeper()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{

				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Ale, 9, 20, 0x99F, 0 ) );
				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Wine, 10, 20, 0x9C7, 0 ) );
				Add( new BeverageBuyInfo( typeof( BeverageBottle ), BeverageType.Liquor, 9, 20, 0x99B, 0 ) );
				Add( new BeverageBuyInfo( typeof( Jug ), BeverageType.Cider, 16, 20, 0x9C8, 0 ) );
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Milk, 8, 20, 0x9F0, 0 ) );
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Ale, 13, 20, 0x1F95, 0 ) );
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Cider, 13, 20, 0x1F97, 0 ) );
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Liquor, 13, 20, 0x1F99, 0 ) );
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Wine, 15, 20, 0x1F9B, 0 ) );
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Water, 10, 20, 0x1F9D, 0 ) );

				Add( new GenericBuyInfo( typeof( BreadLoaf ), 7, 10, 0x103B, 0 ) );
				Add( new GenericBuyInfo( typeof( CheeseWheel ), 26, 10, 0x97E, 0 ) );
				Add( new GenericBuyInfo( typeof( CookedBird ), 21, 20, 0x9B7, 0 ) );
				Add( new GenericBuyInfo( typeof( LambLeg ), 10, 20, 0x160A, 0 ) );

				Add( new GenericBuyInfo( typeof( WoodenBowlOfCarrots ), 4, 20, 0x15F9, 0 ) );
				Add( new GenericBuyInfo( typeof( WoodenBowlOfCorn ), 4, 20, 0x15FA, 0 ) );
				Add( new GenericBuyInfo( typeof( WoodenBowlOfLettuce ), 4, 20, 0x15FB, 0 ) );
				Add( new GenericBuyInfo( typeof( WoodenBowlOfPeas ), 4, 20, 0x15FC, 0 ) );
				Add( new GenericBuyInfo( typeof( EmptyPewterBowl ), 2, 20, 0x15FD, 0 ) );
				Add( new GenericBuyInfo( typeof( PewterBowlOfCorn ), 4, 20, 0x15FE, 0 ) );
				Add( new GenericBuyInfo( typeof( PewterBowlOfLettuce ), 4, 20, 0x15FF, 0 ) );
				Add( new GenericBuyInfo( typeof( PewterBowlOfPeas ), 4, 20, 0x1600, 0 ) );
				Add( new GenericBuyInfo( typeof( PewterBowlOfPotatos ), 4, 20, 0x1601, 0 ) );
				Add( new GenericBuyInfo( typeof( WoodenBowlOfStew ), 4, 20, 0x1604, 0 ) );
				Add( new GenericBuyInfo( typeof( WoodenBowlOfTomatoSoup ), 4, 20, 0x1606, 0 ) );

				Add( new GenericBuyInfo( typeof( ApplePie ), 9, 20, 0x1041, 0 ) ); //OSI just has Pie, not Apple/Fruit/Meat

				Add( new GenericBuyInfo( "1016450", typeof( Chessboard ), 2, 20, 0xFA6, 0 ) );
				Add( new GenericBuyInfo( "1016449", typeof( CheckerBoard ), 2, 20, 0xFA6, 0 ) );
				Add( new GenericBuyInfo( typeof( Backgammon ), 2, 20, 0xE1C, 0 ) );
				Add( new GenericBuyInfo( typeof( Dices ), 2, 20, 0xFA7, 0 ) );
				Add( new GenericBuyInfo( "1041243", typeof( ContractOfEmployment ), 1252, 20, 0x14F0, 0 ) );
				Add( new GenericBuyInfo( "a barkeep contract", typeof( BarkeepContract ), 1252, 20, 0x14F0, 0 ) );
				//Add( new GenericBuyInfo( "a mercenary contract", typeof( MercenaryContract ), 1252, 20, 0x14F0, 0 ) );
				//if ( Multis.BaseHouse.NewVendorSystem )
					//Add( new GenericBuyInfo( "1062332", typeof( VendorRentalContract ), 1252, 20, 0x14F0, 0x672 ) );

				/*if ( Map == Tokuno )
					{
						Add( new GenericBuyInfo( typeof( Wasabi ), 2, 20, 0x24E8, 0 ) );
						Add( new GenericBuyInfo( typeof( Wasabi ), 2, 20, 0x24E9, 0 ) );
						Add( new GenericBuyInfo( typeof( BentoBox ), 6, 20, 0x2836, 0 ) );
						Add( new GenericBuyInfo( typeof( BentoBox ), 6, 20, 0x2837, 0 ) );
						Add( new GenericBuyInfo( typeof( GreenTeaBasket ), 2, 20, 0x284B, 0 ) );
					}*/
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
