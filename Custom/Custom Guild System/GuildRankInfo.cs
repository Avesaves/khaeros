using System;
using Server;
using Server.Items;

namespace Server.Misc
{
    [PropertyObject]
    public class GuildRankInfo
    {
        public override string ToString()
		{
			return "...";
        }

        private int m_Rank;
		public int Rank{ get{ return m_Rank; } set{ m_Rank = value; } }
		
		private string m_Title;
		public string Title{ get{ return m_Title; } set{ m_Title = value; } }
		
        private string m_Prefix;
        public string Prefix{ get{ return m_Prefix; } set{ m_Prefix = value; } }
        
        private string m_Name;
        public string Name{ get{ return m_Name; } set{ m_Name = value; } }
        
        private int m_Fee;
		public int Fee{ get{ return m_Fee; } set{ m_Fee = value; } }
		
		private int m_Pay;
		public int Pay{ get{ return m_Pay; } set{ m_Pay = value; } }
		
		private bool m_IsOfficer;
		public bool IsOfficer{ get{ return m_IsOfficer; } set{ m_IsOfficer = value; } }

        private bool m_IsMilitary;
        public bool IsMilitary { get { return m_IsMilitary; } set { m_IsMilitary = value; } }

        private bool m_IsEconomic;
        public bool IsEconomic { get { return m_IsEconomic; } set { m_IsEconomic = value; } }

        public GuildRankInfo()
		{
		}

		public GuildRankInfo( GenericReader reader )
		{
			int version = reader.ReadInt();
			
			if( version > 0 )
				m_Name = reader.ReadString();
			
			m_Rank = reader.ReadInt();
			m_Fee = reader.ReadInt();
			m_Pay = reader.ReadInt();
			m_Title = reader.ReadString();
			m_Prefix = reader.ReadString();
			m_IsOfficer = reader.ReadBool();

            if(version > 1)
                m_IsMilitary = reader.ReadBool();

            if(version > 2)
                m_IsEconomic = reader.ReadBool();
		}

        public static void Serialize( GenericWriter writer, GuildRankInfo info )
		{
			writer.Write( (int) 3 ); // version
			writer.Write( (string) info.Name );
			writer.Write( (int) info.Rank );
			writer.Write( (int) info.Fee );
			writer.Write( (int) info.Pay );
			writer.Write( (string) info.Title );
			writer.Write( (string) info.Prefix );
			writer.Write( (bool) info.IsOfficer );
            writer.Write( (bool) info.IsMilitary );
            writer.Write( (bool) info.IsEconomic );
		}
    }
}
