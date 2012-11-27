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
    public class Informants
    {
        public override string ToString()
		{
			return "...";
        }

        private Mobile m_Informant1;
        private Mobile m_Informant2;
        private Mobile m_Informant3;
        private Mobile m_Informant4;
        private Mobile m_Informant5;

        [CommandProperty( AccessLevel.Player )]
        public Mobile Informant1
        {
            get { return m_Informant1; }
            set { m_Informant1 = value; }
        }

        [CommandProperty( AccessLevel.Player )]
        public Mobile Informant2
        {
            get { return m_Informant2; }
            set { m_Informant2 = value; }
        }

        [CommandProperty( AccessLevel.Player )]
        public Mobile Informant3
        {
            get { return m_Informant3; }
            set { m_Informant3 = value; }
        }

        [CommandProperty( AccessLevel.Player )]
        public Mobile Informant4
        {
            get { return m_Informant4; }
            set { m_Informant4 = value; }
        }

        [CommandProperty( AccessLevel.Player )]
        public Mobile Informant5
        {
            get { return m_Informant5; }
            set { m_Informant5 = value; }
        }

        public Informants()
		{
		}

		public Informants( GenericReader reader )
		{
			int version = reader.ReadInt();

            m_Informant1 = reader.ReadMobile();
            m_Informant2 = reader.ReadMobile();
            m_Informant3 = reader.ReadMobile();
            m_Informant4 = reader.ReadMobile();
            m_Informant5 = reader.ReadMobile();
		}

        public static void Serialize( GenericWriter writer, Informants info )
		{
			writer.Write( (int) 1 ); // version

            writer.Write( (Mobile)info.m_Informant1 );
            writer.Write( (Mobile)info.m_Informant2 );
            writer.Write( (Mobile)info.m_Informant3 );
            writer.Write( (Mobile)info.m_Informant4 );
            writer.Write( (Mobile)info.m_Informant5 );
		}
    }
}
