using System.Collections.Generic;

namespace Server.Government
{
	public class Employee
	{
		private List<KeyValuePair<ResourceType, int>> m_Wage;
		private int m_DaysLeft;
		private Mobile m_Mobile;
		
		public List<KeyValuePair<ResourceType, int>> Wage{ get{ return m_Wage; } set { m_Wage = value; } }
		public Mobile Mobile{ get{ return m_Mobile; } set { m_Mobile = value; } }
		public int DaysLeft{ get{ return m_DaysLeft; } set { m_DaysLeft = value; } }
	}
}