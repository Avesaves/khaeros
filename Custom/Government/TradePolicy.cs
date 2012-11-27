using System.Collections.Generic;

namespace Server.Government
{
	public class TradePolicy : Policy
	{
		private KeyValuePair<ResourceType, int> m_OurResource;
		private KeyValuePair<ResourceType, int> m_TheirResource;
		private int m_DoNotGoBelowAmount; // if we would go below this amount, then do not perform trade
		private int m_DoNotWantAboveAmount; // if we would go above this amount, then do not perform trade
		
		public int DoNotGoBelowAmount{ get{ return m_DoNotGoBelowAmount; } set { m_DoNotGoBelowAmount = value; } }
		public int DoNotWantAboveAmount{ get{ return m_DoNotWantAboveAmount; } set { m_DoNotWantAboveAmount = value; } }
		public KeyValuePair<ResourceType, int> OurResource{ get{ return m_OurResource; } set { m_OurResource = value; } }
		public KeyValuePair<ResourceType, int> TheirResource{ get{ return m_TheirResource; } set { m_TheirResource = value; } }
		
		public override void EnactPolicy()
		{
		}
	}
}