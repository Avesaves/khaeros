using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Misc;
using Server.ContextMenus;
using Server.Network;

namespace Server.Misc
{
    [PropertyObject]
    public class KnownLanguages
    {
        public override string ToString()
		{
			return "...";
        }

        private int m_Common;
        private int m_Southern;
        private int m_Western;
        private int m_Haluaroc;
        private int m_Mhordul;
        private int m_Tirebladd;
        private int m_Northern;

        [CommandProperty( AccessLevel.GameMaster )]
        public int Common
        {
            get { return m_Common; }
            set { m_Common = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Southern
        {
            get { return m_Southern; }
            set { m_Southern = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Western
        {
            get { return m_Western; }
            set { m_Western = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Haluaroc
        {
            get { return m_Haluaroc; }
            set { m_Haluaroc = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Mhordul
        {
            get { return m_Mhordul; }
            set { m_Mhordul = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Tirebladd
        {
            get { return m_Tirebladd; }
            set { m_Tirebladd = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Northern
        {
            get { return m_Northern; }
            set { m_Northern = value; }
        }

        public KnownLanguages()
		{
		}

		public KnownLanguages( GenericReader reader )
		{
			int version = reader.ReadInt();

            m_Common = reader.ReadInt();
            m_Southern = reader.ReadInt();
            m_Western = reader.ReadInt();
            m_Haluaroc = reader.ReadInt();
            m_Mhordul = reader.ReadInt();
            m_Tirebladd = reader.ReadInt();
            m_Northern = reader.ReadInt();
		}

        public static void Serialize( GenericWriter writer, KnownLanguages info )
		{
			writer.Write( (int) 1 ); // version

            writer.Write( (int)info.m_Common );
            writer.Write( (int)info.m_Southern );
            writer.Write( (int)info.m_Western );
            writer.Write( (int)info.m_Haluaroc );
            writer.Write( (int)info.m_Mhordul );
            writer.Write( (int)info.m_Tirebladd );
            writer.Write( (int)info.m_Northern );
		}
    }
}
