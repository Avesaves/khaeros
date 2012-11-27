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
    public class CombatStyles
    {
        public override string ToString()
		{
			return "...";
        }

        private int m_SearingBreath;
        private int m_SwipingClaws;
        private int m_TempestuousSea;
        private int m_SilentHowl;
        private int m_ThunderingHooves;
        private int m_VenomousWay;
        private int m_Swordsmanship;
        private int m_MaceFighting;
        private int m_Fencing;
        private int m_Polearms;
        private int m_ExoticWeaponry;
        private int m_Axemanship;
        private int m_Throwing;

        [CommandProperty( AccessLevel.GameMaster )]
        public int SearingBreath
        {
            get { return m_SearingBreath; }
            set { m_SearingBreath = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int SwipingClaws
        {
            get { return m_SwipingClaws; }
            set { m_SwipingClaws = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int TempestuousSea
        {
            get { return m_TempestuousSea; }
            set { m_TempestuousSea = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int SilentHowl
        {
            get { return m_SilentHowl; }
            set { m_SilentHowl = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int ThunderingHooves
        {
            get { return m_ThunderingHooves; }
            set { m_ThunderingHooves = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int VenomousWay
        {
            get { return m_VenomousWay; }
            set { m_VenomousWay = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Swordsmanship
        {
            get { return m_Swordsmanship; }
            set { m_Swordsmanship = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int MaceFighting
        {
            get { return m_MaceFighting; }
            set { m_MaceFighting = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Fencing
        {
            get { return m_Fencing; }
            set { m_Fencing = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Polearms
        {
            get { return m_Polearms; }
            set { m_Polearms = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int ExoticWeaponry
        {
            get { return m_ExoticWeaponry; }
            set { m_ExoticWeaponry = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Axemanship
        {
            get { return m_Axemanship; }
            set { m_Axemanship = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Throwing
        {
            get { return m_Throwing; }
            set { m_Throwing = value; }
        }

        public CombatStyles()
		{
		}

		public CombatStyles( GenericReader reader )
		{
			int version = reader.ReadInt();

            m_SearingBreath = reader.ReadInt();
            m_SwipingClaws = reader.ReadInt();
            m_TempestuousSea = reader.ReadInt();
            m_SilentHowl = reader.ReadInt();
            m_ThunderingHooves = reader.ReadInt();
            m_VenomousWay = reader.ReadInt();
            m_Swordsmanship = reader.ReadInt();
            m_MaceFighting = reader.ReadInt();
            m_Fencing = reader.ReadInt();
            m_Polearms = reader.ReadInt();
            m_ExoticWeaponry = reader.ReadInt();
            m_Axemanship = reader.ReadInt();
            m_Throwing = reader.ReadInt();
		}

        public static void Serialize( GenericWriter writer, CombatStyles info )
		{
			writer.Write( (int) 1 ); // version

            writer.Write( (int)info.m_SearingBreath );
            writer.Write( (int)info.m_SwipingClaws );
            writer.Write( (int)info.m_TempestuousSea );
            writer.Write( (int)info.m_SilentHowl );
            writer.Write( (int)info.m_ThunderingHooves );
            writer.Write( (int)info.m_VenomousWay );
            writer.Write( (int)info.m_Swordsmanship );
            writer.Write( (int)info.m_MaceFighting );
            writer.Write( (int)info.m_Fencing );
            writer.Write( (int)info.m_Polearms );
            writer.Write( (int)info.m_ExoticWeaponry );
            writer.Write( (int)info.m_Axemanship );
            writer.Write( (int)info.m_Throwing );
		}
    }
}
