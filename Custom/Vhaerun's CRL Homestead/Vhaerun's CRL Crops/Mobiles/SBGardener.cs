using System; 
using System.Collections; 
using Server.Items; 
using Server.Items.Crops;

namespace Server.Mobiles 
{ 
	public class SBGardener : SBInfo 
	{ 
		private ArrayList m_BuyInfo = new InternalBuyInfo(); 
		private IShopSellInfo m_SellInfo = new InternalSellInfo(); 

		public SBGardener() 
		{ 
		} 

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override ArrayList BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : ArrayList 
		{ 
			public InternalBuyInfo() 
			{ 
				Add( new GenericBuyInfo( "1060834", typeof( Engines.Plants.PlantBowl ), 2, 20, 0x15FD, 0 ) );
				Add( new GenericBuyInfo( "Cotton Seed", typeof( CottonSeed ), 250, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Flax Seed", typeof( FlaxSeed ), 250, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Wheat Seed", typeof( WheatSeed ), 150, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Planting Corn", typeof( CornSeed ), 150, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Carrot Seed", typeof( CarrotSeed ), 50, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Onion Seed", typeof( OnionSeed ), 50, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Garlic Seed", typeof( GarlicSeed ), 50, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Lettuce Seed", typeof( LettuceSeed ), 50, 20, 0xF27, 0x5E2 ) );
				Add( new GenericBuyInfo( "Cabbage Seed", typeof( CabbageSeed ), 50, 20, 0xF27, 0x5E2 ) );
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
