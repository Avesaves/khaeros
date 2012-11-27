using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Misc;
using Server.Network;
using Server.Gumps;
using Server.Engines.XmlSpawner2;

namespace Server.Gumps.MyIcons
{
	public sealed class Initializer
	{
		public static void Initialize()
		{
			EventSink.Login += new LoginEventHandler( IconAttachment.OnLogin );
			MyIconsGump.Initialize();
		}
	}
}

namespace Server.Engines.XmlSpawner2 
{
	public class IconEntry
	{
		private string m_Name;
		private int m_IconID;
		private string[] m_Commands;
		private Point2D m_Location;
		private bool m_SpawnsOnLogin;
		private int m_Index;
		
		public int IconID{ get{ return m_IconID; } set{ m_IconID = value; } }
		public string[] Commands{ get{ return m_Commands; } set{ m_Commands = value; } }
		public Point2D Location{ get{ return m_Location; } set{ m_Location = value; } }
		public bool SpawnsOnLogin{ get{ return m_SpawnsOnLogin; } set{ m_SpawnsOnLogin = value; } }
		public int Index{ get{ return m_Index; } set{ m_Index = value; } }
		public string Name{ get{ return m_Name;} set{ m_Name = value; } }
		
		public IconEntry( int index )
		{
			m_Index = index;
			Name = "unnamed";
			m_Location = new Point2D( 0, 0 );
			m_IconID = 2297; // energy vortex icon
			m_SpawnsOnLogin = false;
			m_Commands = new string[5];
			for (int i=0; i<m_Commands.Length; i++)
				m_Commands[i] = "";
		}
		
		public void SpawnGump( PlayerMobile pm )
		{
			// create a gump that is of type "IconGump" + Index
			Type type = Type.GetType( "Server.Gumps.FloatingIconGump" + Index );
			
			if ( type != null )
				pm.SendGump( (Gump)Activator.CreateInstance( type, new object[] { this } ) );
		}
		
		public void CloseGump( PlayerMobile pm )
		{
			// close a gump that is of type "IconGump" + Index
			Type type = Type.GetType( "Server.Gumps.FloatingIconGump" + Index );
			if ( type != null )
				pm.CloseGump( type );
			else
				pm.Say("pop");
		}
	}
	public class IconAttachment : XmlAttachment 
	{
		private IconEntry[] m_Icons = new IconEntry[30];
		
		public IconAttachment( ASerial serial ) : base( serial ) 
		{
		}
		
		public IconAttachment()
		{
			for ( int i=0; i<m_Icons.Length; i++ )
				m_Icons[i] = new IconEntry( i );
		}
		
		public IconEntry[] Icons{ get{ return m_Icons; } set { m_Icons = value; } }
		
		public static void OnLogin( LoginEventArgs e )
		{
			PlayerMobile from = e.Mobile as PlayerMobile;
			GetIA( from ).ResendIcons();
		}
		
		public override void OnAttach() 
		{
			if ( !(AttachedTo is PlayerMobile) )
				this.Delete();
			
			base.OnAttach();
		}
		
		public void ResendIcons()
		{
			PlayerMobile pm = AttachedTo as PlayerMobile;
			foreach( IconEntry icon in m_Icons )
			{
				if ( icon != null && icon.SpawnsOnLogin )
					icon.SpawnGump( pm );
			}
		}
		
		public static IconAttachment GetIA( PlayerMobile mob )
		{
			if ( mob == null )
				return null;
			IconAttachment ia = XmlAttach.FindAttachment( mob, typeof( IconAttachment ) ) as IconAttachment;
			if ( ia == null )
			{
				ia = new IconAttachment();
				XmlAttach.AttachTo( mob, ia );
			}
			
			return ia;
		}

		public override void Serialize ( GenericWriter writer ) 
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version
			writer.Write( (int) m_Icons.Length );
			foreach ( IconEntry icon in m_Icons )
			{
				if ( icon == null )
				{
					writer.Write( (int) -1 );
					continue;
				}
				writer.Write( (int) icon.Index );
				writer.Write( (string) icon.Name );
				writer.Write( (int) icon.IconID );
				writer.Write( (Point2D) icon.Location );
				writer.Write( (bool) icon.SpawnsOnLogin );
				writer.Write( (int) icon.Commands.Length );
				foreach ( string cmd in icon.Commands )
				{
					if ( String.IsNullOrEmpty( cmd ) )
						writer.Write( (string) "$" );
					else
						writer.Write( (string) cmd );
				}
			}
		}

		public override void Deserialize ( GenericReader reader ) 
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			
			switch ( version )
			{
				case 1: goto case 0;
				case 0:
				{
					int count = reader.ReadInt();
					for ( int i = 0; i<count; i++ )
					{
						int temp = reader.ReadInt();
						if ( temp == -1 )
						{
							m_Icons[i] = null;
							continue;
						}
						
						IconEntry icon = new IconEntry( temp );
						if ( version > 0 )
							icon.Name = reader.ReadString();
						icon.IconID = reader.ReadInt();
						icon.Location = reader.ReadPoint2D();
						icon.SpawnsOnLogin = reader.ReadBool();
						icon.Commands = new string[reader.ReadInt()];
						for ( int j = 0; j<icon.Commands.Length; j++ )
						{
							icon.Commands[j] = reader.ReadString();
							if ( icon.Commands[j] == "$" )
								icon.Commands[j] = "";
						}
						m_Icons[i] = icon;
					}
					break;
				}
			}
		}
	}
}
