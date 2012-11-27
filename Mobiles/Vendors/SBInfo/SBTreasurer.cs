using System; 
using System.Collections; 
using Server.Items; 

namespace Server.Mobiles 
{ 
	public class SBTreasurer: SBInfo 
	{ 
		private ArrayList m_BuyInfo = new InternalBuyInfo(); 
		private IShopSellInfo m_SellInfo = new InternalSellInfo(); 

		public SBTreasurer() 
		{ 
		} 

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override ArrayList BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : ArrayList 
		{ 
			public InternalBuyInfo() 
			{ 
			} 
		} 

		public class InternalSellInfo : GenericSellInfo 
		{ 
			public InternalSellInfo() 
			{ 
				Add( typeof( TinIngot ), 1 ); 
				Add( typeof( CopperIngot ), 2 ); 
				Add( typeof( BronzeIngot ), 3 ); 
				Add( typeof( IronIngot ), 5 ); 
				Add( typeof( ObsidianIngot ), 10 ); 
				Add( typeof( SteelIngot ), 15 ); 
				Add( typeof( SilverIngot ), 20 ); 
				Add( typeof( GoldIngot ), 200 ); 
				
				Add( typeof( Log ), 2 ); 
				Add( typeof( YewLog ), 4 ); 
				Add( typeof( RedwoodLog ), 6 ); 
				Add( typeof( AshLog ), 10 ); 
				Add( typeof( GreenheartLog ), 20 ); 
				
				Add( typeof( Leather ), 2 ); 
				Add( typeof( ThickLeather ), 4 ); 
				Add( typeof( BeastLeather ), 6 ); 
				Add( typeof( ScaledLeather ), 10 ); 
				
				Add( typeof( Cloth ), 1 ); 
				Add( typeof( Linen ), 2 ); 
			} 
		} 
	} 
}
