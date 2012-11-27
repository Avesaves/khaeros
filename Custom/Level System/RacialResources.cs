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
    public class RacialResources
    {
        public override string ToString()
		{
			return "...";
        }
        
        private int m_Alyrian;
        private int m_Azhuran;
        private int m_Khemetar;
        private int m_Mhordul;
        private int m_Tyrean;
        private int m_Vhalurian;

        [CommandProperty( AccessLevel.GameMaster )]
        public int Alyrian
        {
            get { return m_Alyrian; }
            set { m_Alyrian = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Azhuran
        {
            get { return m_Azhuran; }
            set { m_Azhuran = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Khemetar
        {
            get { return m_Khemetar; }
            set { m_Khemetar = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Mhordul
        {
            get { return m_Mhordul; }
            set { m_Mhordul = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Tyrean
        {
            get { return m_Tyrean; }
            set { m_Tyrean = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Vhalurian
        {
            get { return m_Vhalurian; }
            set { m_Vhalurian = value; }
        }

        public RacialResources()
		{
		}

		public RacialResources( GenericReader reader )
		{
			int version = reader.ReadInt();

            m_Alyrian = reader.ReadInt();
            m_Azhuran = reader.ReadInt();
            m_Khemetar = reader.ReadInt();
            m_Mhordul = reader.ReadInt();
            m_Tyrean = reader.ReadInt();
            m_Vhalurian = reader.ReadInt();
		}

        public static void Serialize( GenericWriter writer, RacialResources info )
		{
			writer.Write( (int) 1 ); // version

            writer.Write( (int)info.m_Alyrian );
            writer.Write( (int)info.m_Azhuran );
            writer.Write( (int)info.m_Khemetar );
            writer.Write( (int)info.m_Mhordul );
            writer.Write( (int)info.m_Tyrean );
            writer.Write( (int)info.m_Vhalurian );
		}
    }
}
