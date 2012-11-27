using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
	public class Treasury : LockableContainer
	{
		public static List<Treasury> Treasuries = new List<Treasury>();

        private CustomGuildStone m_ControllingGuild;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public CustomGuildStone ControllingGuild
		{
            get { return m_ControllingGuild; }
            set { m_ControllingGuild = value; }
		}

        private Nation m_Nation;

        [CommandProperty(AccessLevel.GameMaster)]
        public Nation Nation
        {
            get { return m_Nation; }
            set { m_Nation = value; }
        }

        public override int MaxWeight
        {
            get
            {
                return 1000000;
            }
        }
		
        [Constructable]
		public Treasury(Nation n) : base( 0x9A8 )
        {
            Name = "a treasury";
            Nation = n;

            LockLevel = 350;
            MaxLockLevel = 400;

            Treasuries.Add( this );
		}

		public Treasury( Serial serial ) : base( serial )
		{
		}
		
		public override bool CheckHold( Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight )
		{
			return true;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 ); // version
			
            writer.Write( (Item) m_ControllingGuild );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

            if( version > 1 )
                m_ControllingGuild = (CustomGuildStone)reader.ReadItem();
			
			Treasuries.Add( this );
		}
		
		public override void OnDelete()
		{
			if( Treasuries.Contains(this) )
				Treasuries.Remove( this );
		}
	}
}
