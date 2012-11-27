using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBMasterCook : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBMasterCook()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( FoodPlate ), 5, 20, 0x9D7, 0 ) );
				Add( new GenericBuyInfo( typeof( BakersBoard ), 10, 20, 0x14EA, 0 ) );
				Add( new GenericBuyInfo( typeof( CooksCauldron ), 10, 20, 0x9ED, 0 ) );
				Add( new GenericBuyInfo( typeof( FryingPan ), 10, 20, 0x9E2, 0 ) );

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
