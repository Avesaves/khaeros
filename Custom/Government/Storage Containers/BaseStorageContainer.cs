using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Items
{
	public abstract class BaseStorageContainer : LockableContainer
	{
		private Nation m_Nation;
		private string m_OwnersName;
		private Mobile m_Owner;
		private int m_Price = 500;
		private Container m_Treasury;
		private DateTime m_LastRent;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public bool DoClear
		{ 
			get{ return false; }
			set
			{
				if( value == true )
					Clear();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime LastRent{ get{ return m_LastRent; } set{ m_LastRent = value; } }
		
		public void HandleRent()
		{
			if( m_Owner == null || m_Owner.Deleted )
				Clear();
			
			else if( DateTime.Compare(DateTime.Now, (m_LastRent + TimeSpan.FromDays(30))) > 0 )
			{
				if( m_Owner.BankBox == null || !m_Owner.BankBox.ConsumeTotal(typeof(Copper), 500) )
					Clear();
				
				else
					m_LastRent = DateTime.Now;
			}
		}
		
		public void Clear()
		{
			Name = "For Sale: " + m_Price + " copper pieces";
			m_Price = 500;
			m_Owner = null;
			this.KeyValue = 0;
			this.Locked = false;
			
			ArrayList list = new ArrayList();
			
			foreach ( Item item in this.Items )
                list.Add( item );
			
			for( int i = 0; i < list.Count; i++ )
			{
				Item item = list[i] as Item;
				
				try
				{
					item.Delete();
				}
				
				catch( Exception e )
				{
					Console.WriteLine( e.Message );
				}
			}
		}
		
		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			if( this.Owner != null && from != null && this.Owner == from )
				list.Add( new MenuEntry( this, from ) );
		}
		
		private class MenuEntry : ContextMenuEntry
		{
			private BaseStorageContainer m_Item;
			private Mobile m_From;
			
			public MenuEntry( BaseStorageContainer item, Mobile from ) : base( 6118 ) // 3006132  2132
			{
				m_Item = item;
				m_From = from;
			}

			public override void OnClick()
			{
				if( m_From == null || m_Item == null || m_From.Deleted || m_Item.Deleted )
					return;
				
				if( m_Item.Owner != null && m_From != null && m_Item.Owner == m_From )
					m_Item.Clear();
			}
		}
		
		[CommandProperty( AccessLevel.GameMaster )]
        public Nation Nation
        {
            get { return m_Nation; }
            set { m_Nation = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public string OwnersName
        {
            get { return m_OwnersName; }
            set { m_OwnersName = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public Mobile Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public int Price
        {
            get { return m_Price; }
            set 
            {
            	if( m_Owner == null )
            		Name = "For Sale: " + value + " copper pieces";
            	
            	m_Price = value; 
            }
        }
        
        [CommandProperty( AccessLevel.GameMaster )]
        public Container Treasury
        {
            get { return m_Treasury; }
            set { m_Treasury = value; }
        }
		
		public BaseStorageContainer( int itemid, Nation nation ) : base( itemid )
		{
			Weight = 10.0;
			LockLevel = 200;
			MaxLockLevel = 200;
			RequiredSkill = 200;
			Name = "For Sale: " + m_Price + " copper pieces";
			Movable = false;
			Nation = nation;
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			if( from == null || !from.InRange( this.Location, 2 ) || !from.InLOS( this.Location ) || !from.Alive || from.Paralyzed )
			{
				from.SendMessage( "You are too far away." );
				return;
			}
			
			if( m_Owner == null )
			{
				bool haschest = false;
				
				foreach( Item item in from.GetItemsInRange( 10 ) )
				{
					if( item is BaseStorageContainer )
					{
						BaseStorageContainer cont = item as BaseStorageContainer;
						
						if( cont.Owner == from )
							haschest = true;
					}
				}
				
				if( haschest )
				{
					from.SendMessage( "You already have a storage box in this area." );
					return;
				}
				
				Container pack = from.Backpack;
				
				if ( !pack.ConsumeTotal( typeof( Copper ), m_Price ) )
				{
					from.SendMessage( "You need to be carrying " + m_Price + " copper coins in order to purchase this storage box." );
				}

				else
				{
					m_Owner = from;
					m_OwnersName = from.Name;
					
					if( m_OwnersName.EndsWith( "s" ) )
						m_OwnersName = m_OwnersName + "'";
					
					else
						m_OwnersName = m_OwnersName + "'s";
					
					this.Name = "" + m_OwnersName + " Storage Box";
					
					Key key = new Key();
					
					uint newlock = Key.RandomValue();
					this.KeyValue = newlock;
					this.Locked = true;
					key.KeyValue = newlock;
					pack.DropItem( key );
					this.LastRent = DateTime.Now;
					from.SendMessage( "A key to the storage box has been placed in your backpack and " + m_Price + " copper coins have been charged from you." );
					
					Copper copper = new Copper( m_Price );
					
					if( this.Nation != Nation.None && this.AssignTreasury() )
					{
						if( this.Treasury is BaseContainer )
							( (BaseContainer)this.Treasury ).DropAndStack( copper );
					}
				}
			}
			
			else
				base.OnDoubleClick( from );
		}
		
		public bool AssignTreasury()
		{
			foreach( Item item in World.Items.Values )
			{
				if( item is Treasury && (item as Treasury).Nation == this.Nation)
				{
                    this.Treasury = item as Container;
					return true;
				}
			}
			
			return false;
		}

		public BaseStorageContainer( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
			
			writer.Write( (DateTime) m_LastRent );
			writer.Write( (int) m_Nation );
			writer.Write( (string) m_OwnersName );
			writer.Write( (Mobile) m_Owner );
			writer.Write( (int) m_Price );
			writer.Write( (Item) m_Treasury );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version > 0 )
				m_LastRent = reader.ReadDateTime();
			
			else
				m_LastRent = DateTime.Now;
			
			m_Nation = (Nation)reader.ReadInt();
			m_OwnersName = reader.ReadString();
			m_Owner = reader.ReadMobile();
			m_Price = reader.ReadInt();
			m_Treasury = (Container)reader.ReadItem();
		}
	}
	
	[Flipable( 0x9AB, 0xE7C )]
	public class MetalStorageChest : BaseStorageContainer
	{
		[Constructable]
		public MetalStorageChest( Nation nation ) : base( 0x9AB, nation )
		{
		}

		public MetalStorageChest( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	[Flipable( 0xE41, 0xE40 )]
	public class MetalGoldenStorageChest : BaseStorageContainer
	{
		[Constructable]
		public MetalGoldenStorageChest( Nation nation ) : base( 0xE41, nation )
		{
		}

		public MetalGoldenStorageChest( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	[Flipable( 0xE43, 0xE42 )]
	public class WoodenStorageChest : BaseStorageContainer
	{
		[Constructable]
		public WoodenStorageChest( Nation nation ) : base( 0xE43, nation )
		{
		}

		public WoodenStorageChest( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	[Flipable( 0x9A8, 0xE80 )]
	public class MetalStorageBox : BaseStorageContainer
	{
		[Constructable]
		public MetalStorageBox( Nation nation ) : base( 0x9A8, nation )
		{
		}

		public MetalStorageBox( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
	
	[Flipable( 0x9AA, 0xE7D )]
	public class WoodenStorageBox : BaseStorageContainer
	{
		[Constructable]
		public WoodenStorageBox( Nation nation ) : base( 0x9AA, nation )
		{
		}

		public WoodenStorageBox( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
