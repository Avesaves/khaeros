using System; 
using System.Collections; 
using Server.Items; 

namespace Server.Mobiles 
{ 
	public class SBEmpty: SBInfo 
	{ 
		private ArrayList m_BuyInfo = new InternalBuyInfo(); 
		private IShopSellInfo m_SellInfo = new InternalSellInfo(); 

		public SBEmpty() 
		{ 
		} 

		public override IShopSellInfo SellInfo { get{ return m_SellInfo; } } 
		public override ArrayList BuyInfo { get{ return m_BuyInfo; } }
		
		public IShopSellInfo CustomSellInfo{ get{ return m_SellInfo; } set{ m_SellInfo = value; } }
		public ArrayList CustomBuyInfo{ get{ return m_BuyInfo; } set{ m_BuyInfo = value; } }

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
			} 
		} 
	} 
}
