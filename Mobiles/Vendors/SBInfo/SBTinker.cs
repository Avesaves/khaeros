using System; 
using System.Collections; 
using Server.Items; 

namespace Server.Mobiles 
{ 
	public class SBTinker: SBInfo 
	{ 
		private ArrayList m_BuyInfo = new InternalBuyInfo(); 
		private IShopSellInfo m_SellInfo = new InternalSellInfo(); 

		public SBTinker() 
		{ 
		} 

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override ArrayList BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : ArrayList 
		{ 
			public InternalBuyInfo() 
			{ 
				Add( new GenericBuyInfo( typeof( ScribesPen ), 12, 20, 0xFBF, 0 ) );
				Add( new GenericBuyInfo( typeof( AlchemyTool ), 12, 20, 0xE9B, 0 ) );
				Add( new GenericBuyInfo( typeof( MixingSet ), 12, 20, 6237, 0 ) );
				Add( new GenericBuyInfo( typeof( TinkerTools ), 12, 20, 0x1EB8, 0 ) );
				Add( new GenericBuyInfo( typeof( Tongs ), 12, 20, 0xFBB, 0 ) );
				Add( new GenericBuyInfo( typeof( SmithHammer ), 12, 20, 0x13E3, 0 ) );
/* 				Add( new GenericBuyInfo( typeof( BlackSmithingHammer ), 10, 20, 0x13E3, 0 ) ); */
				Add( new GenericBuyInfo( typeof( SewingKit ), 12, 20, 0xF9D, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Scissors ), 5, 20, 0xF9F, 0 ) );
				Add( new GenericBuyInfo( typeof( DyeTub ), 30, 20, 0xFAB, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Dyes ), 12, 20, 0xFA9, 0 ) ); 
				Add( new GenericBuyInfo( typeof( ButcherKnife ), 12, 20, 0x13F6, 0 ) );
 				Add( new GenericBuyInfo( typeof( Cleaver ), 18, 20, 0xEC3, 0 ) );
				Add( new GenericBuyInfo( typeof( SkinningKnife ), 12, 20, 0xEC4, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Saw ), 24, 20, 0x1034, 0 ) );
				Add( new GenericBuyInfo( typeof( DrawKnife ), 12, 20, 0x10E4, 0 ) );
				Add( new GenericBuyInfo( typeof( Hammer ), 12, 20, 0x102A, 0 ) );
				Add( new GenericBuyInfo( typeof( Froe ), 12, 20, 0x10E5, 0 ) );
				Add( new GenericBuyInfo( typeof( DovetailSaw ), 24, 20, 0x1028, 0 ) );
				Add( new GenericBuyInfo( typeof( Scorp ), 12, 20, 0x10E7, 0 ) );
				Add( new GenericBuyInfo( typeof( Inshave ), 12, 20, 0x10E6, 0 ) );
				Add( new GenericBuyInfo( typeof( RollingPin ), 12, 20, 0x1043, 0 ) );
				Add( new GenericBuyInfo( typeof( FlourSifter ), 18, 20, 0x103E, 0 ) );
				Add( new GenericBuyInfo( "1044567", typeof( Skillet ), 24, 20, 0x97F, 0 ) );
				
				Add( new GenericBuyInfo( "Frying Pan", typeof( FryingPan ), 24, 20, 0x97F, 0 ) );
				Add( new GenericBuyInfo( "Cook's Cauldron", typeof( CooksCauldron ), 24, 20, 0x9ED, 0 ) );
				Add( new GenericBuyInfo( "Baker's Board", typeof( BakersBoard ), 24, 20, 0x14EA, 0 ) );

                Add( new GenericBuyInfo( typeof( WinecraftersTools ), 20, 20, 0xF00, 0x530 ) );
                Add( new GenericBuyInfo( typeof( VinyardLabelMaker ), 20, 20, 0xFC0, 0x218 ) );
                Add( new GenericBuyInfo( typeof( VinyardLabelMaker ), 20, 20, 0xFC0, 0x218 ) );

                Add( new GenericBuyInfo( typeof( BrewersTools ), 20, 20, 0x1EBC, 0 ) );
                Add( new GenericBuyInfo( typeof( BreweryLabelMaker ), 20, 20, 0xFBF, 0 ) );
				
				Add( new GenericBuyInfo( typeof( FishingPole ), 60, 20, 0xDC0, 0 ) );
				Add( new GenericBuyInfo( typeof( Candle ), 12, 20, 0xA28, 0 ) );
				Add( new GenericBuyInfo( typeof( Torch ), 12, 20, 0xF6B, 0 ) );
				Add( new GenericBuyInfo( typeof( Lantern ), 12, 20, 0xA25, 0 ) );
				Add( new GenericBuyInfo( typeof( Pickaxe ), 24, 20, 0xE86, 0 ) );
				Add( new GenericBuyInfo( typeof( Hatchet ), 24, 20, 0xF44, 0 ) );
				Add( new GenericBuyInfo( typeof( Shovel ), 24, 20, 0xF39, 0 ) );
				Add( new GenericBuyInfo( typeof( FletcherTools ), 12, 20, 0x1022, 0 ) );
				Add( new GenericBuyInfo( typeof( Lockpick ), 12, 20, 0x14FC, 0 ) );
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
