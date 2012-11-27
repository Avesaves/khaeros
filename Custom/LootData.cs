// LootData.cs
// Author: Oak (ssalter)
// Version: 1.0
// Requirements: Runuo 2.0, XmlSpawner2
// Server Tested with: 2.0 build 64
// Revision Date: 7/1/2006
// Purpose: Player can type 'grab options' to get a gump and select what types of items they want to transfer
// to their lootbag when using the 'claim' command. Uses XMLAttachment for loot options

using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class LootData : XmlAttachment
    {
		//private bool m_getall;
		private bool m_getweapons;
		private bool m_getarmor;
		private bool m_getjewelry;
		private bool m_getpotions;
		private bool m_getregs;
		private bool m_getclothes;
		private bool m_getgems;
		private bool m_getartifacts;
		private bool m_gethides;
		private bool m_getfood;
		private bool m_getgrounditems;
		private bool m_getresources;

        //[CommandProperty( AccessLevel.GameMaster )]
       // public bool GetAll { get { return m_getall; } set { m_getall  = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
        public bool GetWeapons { get { return m_getweapons; } set { m_getweapons  = value; } }
        
		[CommandProperty( AccessLevel.GameMaster )]
        public bool GetArmor { get { return m_getarmor; } set { m_getarmor  = value; } }
        
		[CommandProperty( AccessLevel.GameMaster )]
        public bool GetJewelry { get { return m_getjewelry; } set { m_getjewelry  = value; } }
        
		[CommandProperty( AccessLevel.GameMaster )]
        public bool GetPotions { get { return m_getpotions; } set { m_getpotions  = value; } }
        
		[CommandProperty( AccessLevel.GameMaster )]
        public bool GetRegs { get { return m_getregs; } set { m_getregs  = value; } }
        
		[CommandProperty( AccessLevel.GameMaster )]
        public bool GetClothes { get { return m_getclothes; } set { m_getclothes  = value; } }
        
		[CommandProperty( AccessLevel.GameMaster )]
        public bool GetGems { get { return m_getgems; } set { m_getgems  = value; } }
        
		[CommandProperty( AccessLevel.GameMaster )]
        public bool GetArtifacts { get { return m_getartifacts; } set { m_getartifacts  = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
        public bool GetHides { get { return m_gethides; } set { m_gethides  = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
        public bool GetFood { get { return m_getfood; } set { m_getfood  = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
        public bool GetGroundItems { get { return m_getgrounditems; } set { m_getgrounditems  = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
        public bool GetResources { get { return m_getresources; } set { m_getresources  = value; } }

        public LootData(ASerial serial) : base(serial)
        {
        }

        [Attachable]
        public LootData()
        {
        }

        [Attachable]
        public LootData(bool allflag, bool weaponsflag, bool armorflag, bool jewelryflag, bool potionsflag, bool regsflag, bool clothesflag, bool gemsflag, bool artifactsflag, bool hidesflag, bool foodflag, bool grounditemsflag, bool resourcesflag)
        {
			//m_getall=allflag;
			m_getweapons=weaponsflag;
			m_getarmor=armorflag;
			m_getjewelry=jewelryflag;
			m_getpotions=potionsflag;
			m_getregs=regsflag;
			m_getclothes=clothesflag;
			m_getgems=gemsflag;
			m_getartifacts=artifactsflag;
			m_gethides=hidesflag;
			m_getfood=foodflag;
			m_getgrounditems=grounditemsflag;
			m_getresources=resourcesflag;
        }
        
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize(writer);

			writer.Write( (int) 0 );
			// version 0
			//writer.Write(m_getall);
			writer.Write(m_getweapons);
			writer.Write(m_getarmor);
			writer.Write(m_getjewelry);
			writer.Write(m_getpotions);
			writer.Write(m_getregs);
			writer.Write(m_getclothes);
			writer.Write(m_getgems);
			writer.Write(m_getartifacts);
			writer.Write(m_gethides);
			writer.Write(m_getfood);
			writer.Write(m_getgrounditems);
			writer.Write(m_getresources);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			// version 0
			int version = reader.ReadInt();
			//m_getall = reader.ReadBool();
			m_getweapons = reader.ReadBool();
			m_getarmor = reader.ReadBool();
			m_getjewelry = reader.ReadBool();
			m_getpotions = reader.ReadBool();
			m_getregs = reader.ReadBool();
			m_getclothes = reader.ReadBool();
			m_getgems = reader.ReadBool();
			m_getartifacts = reader.ReadBool();
			m_gethides = reader.ReadBool();
			m_getfood = reader.ReadBool();
			m_getgrounditems = reader.ReadBool();
			m_getresources = reader.ReadBool();
		}
    }
}
