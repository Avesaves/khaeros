using System; 
using System.Collections; 
using Server.Items; 

namespace Server.Mobiles 
{ 
	public class SBFarmer : SBInfo 
	{ 
		private ArrayList m_BuyInfo = new InternalBuyInfo(); 
		private IShopSellInfo m_SellInfo = new InternalSellInfo(); 

		public SBFarmer() 
		{ 
		} 

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override ArrayList BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : ArrayList 
		{ 
			public InternalBuyInfo() 
			{ 
				Add( new GenericBuyInfo( typeof( Cabbage ), 10, 20, 0xC7B, 0 ) );
				Add( new GenericBuyInfo( typeof( Cantaloupe ), 12, 20, 0xC79, 0 ) );
				Add( new GenericBuyInfo( typeof( Carrot ), 6, 20, 0xC78, 0 ) );
				Add( new GenericBuyInfo( typeof( HoneydewMelon ), 14, 20, 0xC74, 0 ) );
				Add( new GenericBuyInfo( typeof( Squash ), 6, 20, 0xC72, 0 ) );
				Add( new GenericBuyInfo( typeof( Lettuce ), 10, 20, 0xC70, 0 ) );
				Add( new GenericBuyInfo( typeof( Onion ), 6, 20, 0xC6D, 0 ) );
				Add( new GenericBuyInfo( typeof( Pumpkin ), 22, 20, 0xC6A, 0 ) );
				Add( new GenericBuyInfo( typeof( GreenGourd ), 6, 20, 0xC66, 0 ) );
				Add( new GenericBuyInfo( typeof( YellowGourd ), 6, 20, 0xC64, 0 ) );
				Add( new GenericBuyInfo( typeof( Turnip ), 12, 20, 0xD3A, 0 ) );
				Add( new GenericBuyInfo( typeof( Watermelon ), 14, 20, 0xC5C, 0 ) );
				//Add( new GenericBuyInfo( typeof( EarOfCorn ), 3, 20, XXXXXX, 0 ) );
				Add( new GenericBuyInfo( typeof( Eggs ), 6, 20, 0x9B5, 0 ) );
				Add( new BeverageBuyInfo( typeof( Pitcher ), BeverageType.Milk, 14, 20, 0x9AD, 0 ) );
				Add( new GenericBuyInfo( typeof( Peach ), 6, 20, 0x9D2, 0 ) );
				Add( new GenericBuyInfo( typeof( Pear ), 6, 20, 0x994, 0 ) );
				Add( new GenericBuyInfo( typeof( Grapes ), 6, 20, 0x9D1, 0 ) );
				Add( new GenericBuyInfo( typeof( Apple ), 6, 20, 0x9D0, 0 ) );
				Add( new GenericBuyInfo( typeof( Hay ), 4, 20, 0xF36, 0 ) );
				Add( new GenericBuyInfo( typeof( Garlic ), 6, 20, 0xF84, 0 ) );

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
