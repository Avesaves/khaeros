using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Misc;
using Server.ContextMenus;
using Server.Network;
using Server.Items;

namespace Server.Misc
{
    [PropertyObject]
    public class Masterwork
    {
        public override string ToString()
		{
			return "...";
        }

        private int m_WeaponPointsLeft;
        private int m_ArmourPointsLeft;
        private int m_BluntResist;
        private int m_SlashingResist;
        private int m_PiercingResist;
        private int m_WeaponDamage;
        private int m_WeaponSpeed;
        private int m_WeaponAccuracy;
        
        private CraftResource m_WeaponResource;
        private bool m_MasterworkWeapon;
        private bool m_HasWeaponPieces;
        
        private CraftResource m_ArmourResource;
        private bool m_MasterworkArmour;
        private bool m_HasArmourPieces;
        
        private CraftResource m_ClothingResource;
        private bool m_MasterworkClothing;
        private bool m_HasClothingPieces;

        [CommandProperty( AccessLevel.GameMaster )]
        public int WeaponPointsLeft
        {
            get { return m_WeaponPointsLeft; }
            set { m_WeaponPointsLeft = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int ArmourPointsLeft
        {
            get { return m_ArmourPointsLeft; }
            set { m_ArmourPointsLeft = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int BluntResist
        {
            get { return m_BluntResist; }
            set { m_BluntResist = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int SlashingResist
        {
            get { return m_SlashingResist; }
            set { m_SlashingResist = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int PiercingResist
        {
            get { return m_PiercingResist; }
            set { m_PiercingResist = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int WeaponDamage
        {
            get { return m_WeaponDamage; }
            set { m_WeaponDamage = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int WeaponSpeed
        {
            get { return m_WeaponSpeed; }
            set { m_WeaponSpeed = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int WeaponAccuracy
        {
            get { return m_WeaponAccuracy; }
            set { m_WeaponAccuracy = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public CraftResource WeaponResource{ get { return m_WeaponResource; } set { m_WeaponResource = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public CraftResource ArmourResource{ get { return m_ArmourResource; } set { m_ArmourResource = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public CraftResource ClothingResource{ get { return m_ClothingResource; } set { m_ClothingResource = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool MasterworkWeapon{ get { return m_MasterworkWeapon; } set { m_MasterworkWeapon = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool MasterworkArmour{ get { return m_MasterworkArmour; } set { m_MasterworkArmour = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool MasterworkClothing{ get { return m_MasterworkClothing; } set { m_MasterworkClothing = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool HasWeaponPieces{ get { return m_HasWeaponPieces; } set { m_HasWeaponPieces = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool HasArmourPieces{ get { return m_HasArmourPieces; } set { m_HasArmourPieces = value; } }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool HasClothingPieces{ get { return m_HasClothingPieces; } set { m_HasClothingPieces = value; } }
        
        public Masterwork()
		{
		}

		public Masterwork( GenericReader reader )
		{
			int version = reader.ReadInt();

            m_WeaponPointsLeft = reader.ReadInt();
            m_ArmourPointsLeft = reader.ReadInt();
            m_BluntResist = reader.ReadInt();
            m_SlashingResist = reader.ReadInt();
            m_PiercingResist = reader.ReadInt();
            m_WeaponDamage = reader.ReadInt();
            m_WeaponSpeed = reader.ReadInt();
            m_WeaponAccuracy = reader.ReadInt();
            
            if( version > 1 )
            {
           		m_WeaponResource = (CraftResource)reader.ReadInt();
           		m_MasterworkWeapon = reader.ReadBool();
           		m_HasWeaponPieces = reader.ReadBool();
           		m_ArmourResource = (CraftResource)reader.ReadInt();
           		m_MasterworkArmour = reader.ReadBool();
           		m_HasArmourPieces = reader.ReadBool();
           		m_ClothingResource = (CraftResource)reader.ReadInt();
           		m_MasterworkClothing = reader.ReadBool();
           		m_HasClothingPieces = reader.ReadBool();
            }
		}

        public static void Serialize( GenericWriter writer, Masterwork info )
		{
			writer.Write( (int) 2 ); // version

            writer.Write( (int)info.m_WeaponPointsLeft );
            writer.Write( (int)info.m_ArmourPointsLeft );
            writer.Write( (int)info.m_BluntResist );
            writer.Write( (int)info.m_SlashingResist );
            writer.Write( (int)info.m_PiercingResist );
            writer.Write( (int)info.m_WeaponDamage );
            writer.Write( (int)info.m_WeaponSpeed );
            writer.Write( (int)info.m_WeaponAccuracy );
            writer.Write( (int)info.m_WeaponResource );
            writer.Write( (bool)info.m_MasterworkWeapon );
            writer.Write( (bool)info.m_HasWeaponPieces );
            writer.Write( (int)info.m_ArmourResource );
            writer.Write( (bool)info.m_MasterworkArmour );
            writer.Write( (bool)info.m_HasArmourPieces );
            writer.Write( (int)info.m_ClothingResource );
            writer.Write( (bool)info.m_MasterworkClothing );
            writer.Write( (bool)info.m_HasClothingPieces );
		}
    }
}
