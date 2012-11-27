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
    public class Friendship
    {
        public override string ToString()
		{
			return "...";
        }
        
        private int m_Brigand;
        private int m_Minotaur;
        private int m_Beastman;
        private int m_Goblin;
        private int m_Ogre;
        private int m_Undead;
        private int m_YuanTi;
        private int m_Giant;
        private int m_Draconic;
        private int m_Canine;
        private int m_Serpent;
        private int m_Rodent;
        private int m_Goatman;
        private int m_Troll;
        private int m_Troglin;
        private int m_Spider;
        private int m_Drider;
        private int m_Formian;
        private int m_Kobold;
        private int m_Beholder;
        private int m_Feline;
        private int m_Bear;
        private int m_Abyssal;
        private int m_Elemental;
        private int m_Reptile;
        private int m_GiantBug;
        private int m_Celestial;
        private int m_Brotherhood;
        private int m_Society;
        private int m_Insularii;

        [CommandProperty( AccessLevel.GameMaster )]
        public int Brigand
        {
            get { return m_Brigand; }
            set { m_Brigand = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Minotaur
        {
            get { return m_Minotaur; }
            set { m_Minotaur = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Beastman
        {
            get { return m_Beastman; }
            set { m_Beastman = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Goblin
        {
            get { return m_Goblin; }
            set { m_Goblin = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Ogre
        {
            get { return m_Ogre; }
            set { m_Ogre = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Undead
        {
            get { return m_Undead; }
            set { m_Undead = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int YuanTi
        {
            get { return m_YuanTi; }
            set { m_YuanTi = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Giant
        {
            get { return m_Giant; }
            set { m_Giant = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Draconic
        {
            get { return m_Draconic; }
            set { m_Draconic = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Canine
        {
            get { return m_Canine; }
            set { m_Canine = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Serpent
        {
            get { return m_Serpent; }
            set { m_Serpent = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Rodent
        {
            get { return m_Rodent; }
            set { m_Rodent = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Goatman
        {
            get { return m_Goatman; }
            set { m_Goatman = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Troll
        {
            get { return m_Troll; }
            set { m_Troll = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Troglin
        {
            get { return m_Troglin; }
            set { m_Troglin = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Spider
        {
            get { return m_Spider; }
            set { m_Spider = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Drider
        {
            get { return m_Drider; }
            set { m_Drider = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Formian
        {
            get { return m_Formian; }
            set { m_Formian = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Kobold
        {
            get { return m_Kobold; }
            set { m_Kobold = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Beholder
        {
            get { return m_Beholder; }
            set { m_Beholder = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Feline
        {
            get { return m_Feline; }
            set { m_Feline = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Bear
        {
            get { return m_Bear; }
            set { m_Bear = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Abyssal
        {
            get { return m_Abyssal; }
            set { m_Abyssal = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Elemental
        {
            get { return m_Elemental; }
            set { m_Elemental = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Reptile
        {
            get { return m_Reptile; }
            set { m_Reptile = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int GiantBug
        {
            get { return m_GiantBug; }
            set { m_GiantBug = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Celestial
        {
            get { return m_Celestial; }
            set { m_Celestial = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Brotherhood
        {
            get { return m_Brotherhood; }
            set { m_Brotherhood = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Society
        {
            get { return m_Society; }
            set { m_Society = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Insularii
        {
            get { return m_Insularii; }
            set { m_Insularii = value; }
        }

        public Friendship()
		{
		}

		public Friendship( GenericReader reader )
		{
			int version = reader.ReadInt();

            m_Brigand = reader.ReadInt();
            m_Minotaur = reader.ReadInt();
            m_Beastman = reader.ReadInt();
            m_Goblin = reader.ReadInt();
            m_Ogre = reader.ReadInt();
            m_Undead = reader.ReadInt();
            
            if( version > 1 )
           	{
	            m_YuanTi = reader.ReadInt();
		        m_Giant = reader.ReadInt();
		        m_Draconic = reader.ReadInt();
		        m_Canine = reader.ReadInt();
		        m_Serpent = reader.ReadInt();
		        m_Rodent = reader.ReadInt();
		        m_Goatman = reader.ReadInt();
		        m_Troll = reader.ReadInt();
		        m_Troglin = reader.ReadInt();
		        m_Spider = reader.ReadInt();
		        m_Drider = reader.ReadInt();
		        m_Formian = reader.ReadInt();
		        m_Kobold = reader.ReadInt();
		        m_Beholder = reader.ReadInt();
		        m_Feline = reader.ReadInt();
		        m_Bear = reader.ReadInt();
		        m_Abyssal = reader.ReadInt();
		        m_Elemental = reader.ReadInt();
		        m_Reptile = reader.ReadInt();
		        m_GiantBug = reader.ReadInt();
            }
            
            if( version > 2 )
            	m_Celestial = reader.ReadInt();
            
            if( version > 3 )
            {
            	m_Brotherhood = reader.ReadInt();
            	m_Society = reader.ReadInt();
            	m_Insularii = reader.ReadInt();
            }
		}

        public static void Serialize( GenericWriter writer, Friendship info )
		{
			writer.Write( (int) 4 ); // version

            writer.Write( (int)info.m_Brigand );
            writer.Write( (int)info.m_Minotaur );
            writer.Write( (int)info.m_Beastman );
            writer.Write( (int)info.m_Goblin );
            writer.Write( (int)info.m_Ogre );
            writer.Write( (int)info.m_Undead );
            writer.Write( (int)info.m_YuanTi );
	        writer.Write( (int)info.m_Giant );
	        writer.Write( (int)info.m_Draconic );
	        writer.Write( (int)info.m_Canine );
	        writer.Write( (int)info.m_Serpent );
	        writer.Write( (int)info.m_Rodent );
	        writer.Write( (int)info.m_Goatman );
	        writer.Write( (int)info.m_Troll );
	        writer.Write( (int)info.m_Troglin );
	        writer.Write( (int)info.m_Spider );
	        writer.Write( (int)info.m_Drider );
	        writer.Write( (int)info.m_Formian );
	        writer.Write( (int)info.m_Kobold );
	        writer.Write( (int)info.m_Beholder );
	        writer.Write( (int)info.m_Feline );
	        writer.Write( (int)info.m_Bear );
	        writer.Write( (int)info.m_Abyssal );
	        writer.Write( (int)info.m_Elemental );
	        writer.Write( (int)info.m_Reptile );
	        writer.Write( (int)info.m_GiantBug );
	        writer.Write( (int)info.m_Celestial );
	        writer.Write( (int)info.m_Brotherhood );
	        writer.Write( (int)info.m_Society );
	        writer.Write( (int)info.m_Insularii );
		}
    }
}
