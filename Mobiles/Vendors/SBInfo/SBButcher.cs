using System; 
using System.Collections; 
using Server.Items; 

namespace Server.Mobiles 
{ 
	public class SBButcher : SBInfo 
	{ 
		private ArrayList m_BuyInfo = new InternalBuyInfo(); 
		private IShopSellInfo m_SellInfo = new InternalSellInfo(); 

		public SBButcher() 
		{ 
		} 

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } } 
		public override ArrayList BuyInfo { get { return m_BuyInfo; } } 

		public class InternalBuyInfo : ArrayList 
		{ 
			public InternalBuyInfo() 
			{                 
				Add( new GenericBuyInfo( typeof( Bacon ), 7, 20, 0x979, 0 ) );
				Add( new GenericBuyInfo( typeof( Ham ), 26, 20, 0x9C9, 0 ) ); 
				Add( new GenericBuyInfo( typeof( Sausage ), 18, 20, 0x9C0, 0 ) );
				Add( new GenericBuyInfo( typeof( RawChickenLeg ), 6, 20, 0x1607, 0 ) );
				Add( new GenericBuyInfo( typeof( RawBird ), 9, 20, 0x9B9, 0 ) ); 
				Add( new GenericBuyInfo( typeof( RawLambLeg ), 9, 20, 0x1609, 0 ) );
				Add( new GenericBuyInfo( typeof( RawRibs ), 16, 20, 0x9F1, 0 ) );
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
