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
    public class VendorEntry
    {
        public override string ToString()
		{
			return "...";
        }

        private string m_TypeName;
        private int m_Price;
        private bool m_Remove;
        private Type m_Type;
        private string m_ItemName;

        [CommandProperty( AccessLevel.GameMaster )]
        public string TypeName
        {
            get { return m_TypeName; }
            set { m_TypeName = value; }
        }

        [CommandProperty( AccessLevel.GameMaster )]
        public int Price
        {
            get { return m_Price; }
            set { m_Price = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public bool Remove
        {
            get { return m_Remove; }
            set { m_Remove = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public string ItemName
        {
            get { return m_ItemName; }
            set { m_ItemName = value; }
        }

        public Type Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }

        public VendorEntry()
		{
		}
        
        public VendorEntry( string itemName, string typeName, int price, Type type )
		{
        	ItemName = itemName;
        	TypeName = typeName;
        	Price = price;
        	Type = type;
		}

		public VendorEntry( GenericReader reader )
		{
			int version = reader.ReadInt();
            
			m_TypeName = reader.ReadString();
            m_Price = reader.ReadInt();
            m_Remove = reader.ReadBool();
            
            if( version > 0 )
            	m_ItemName = reader.ReadString();
		}

        public static void Serialize( GenericWriter writer, VendorEntry info )
		{
			writer.Write( (int) 1 ); // version

			writer.Write( (string)info.m_TypeName );
            writer.Write( (int)info.m_Price );
            writer.Write( (bool)info.m_Remove );
            writer.Write( (string)info.m_ItemName );
		}
    }
}
