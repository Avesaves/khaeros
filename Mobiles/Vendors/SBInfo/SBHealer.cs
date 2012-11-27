using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBHealer : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBHealer()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add( new GenericBuyInfo( typeof( Bandage ), 1, 20, 0xE21, 0 ) );
				Add( new GenericBuyInfo( typeof( LesserHealPotion ), 12, 20, 0xF0C, 0 ) );
				Add( new GenericBuyInfo( typeof( RefreshPotion ), 12, 20, 0xF0B, 0 ) );
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
