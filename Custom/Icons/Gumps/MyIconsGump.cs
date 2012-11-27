using System;
using Server.Network;
using Server.Engines.XmlSpawner2;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Gumps
{
	public class MyIconsGump : Gump
	{
		public static Dictionary<int, bool> ValidIcons; // for faster validation
		public static int[] ValidIconsArray; // for faster page generation
		private PlayerMobile m_Mobile;
		private int m_PageType;
		private int m_PageNum;
		private int m_FocusedIcon;
		
		public static void Initialize()
		{
			ValidIcons = new Dictionary<int, bool>();
			for ( int i = 6016; i <= 6047; i++ )
				ValidIcons[i] = true;
			for ( int i = 6057; i <= 6185; i++ )
				ValidIcons[i] = true;
			for ( int i = 20992; i <= 21020; i++ )
				ValidIcons[i] = true;
			for ( int i = 20480; i <= 20496; i++ )
				ValidIcons[i] = true;
			for ( int i = 20736; i <= 20745; i++ )
				ValidIcons[i] = true;
			for ( int i = 21280; i <= 21287; i++ )
				ValidIcons[i] = true;
			for ( int i = 21536; i <= 21542; i++ )
				ValidIcons[i] = true;
			for ( int i = 23000; i <= 23015; i++ )
				ValidIcons[i] = true;
			for ( int i = 2240; i <= 2303; i++ )
				ValidIcons[i] = true;
			
			ValidIconsArray = new int[ValidIcons.Count];
			int j=0;
			foreach ( KeyValuePair<int, bool> kvp in ValidIcons )
				ValidIconsArray[j++] = kvp.Key;
		}
		
		public MyIconsGump( PlayerMobile mob ) : this( 1, 0, 0, mob )
		{
		}
		
		public MyIconsGump( int pageType, int pageNum, int focusedIcon, PlayerMobile mob ) : base(0, 0)
		{
			m_Mobile = mob;
			m_PageType = pageType;
			m_PageNum = pageNum;
			m_FocusedIcon = focusedIcon;
			
			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			
			IconAttachment ia = IconAttachment.GetIA( mob );
			
			this.AddPage(0);
			
			if ( pageType == 1 ) // main screen
			{
				this.AddBackground(27, 15, 504, 418, 9200);
				this.AddLabel(251, -1, 247, "My icons");
				
				int xOffset = 69;
				int yOffset = 52;

				for ( int i=pageNum*6; i<pageNum*6+6; i++ )
				{
					if ( ia.Icons[i].SpawnsOnLogin )
					{
						this.AddButton(63+xOffset, 24+yOffset, 5601, 5605, 500+i, GumpButtonType.Reply, 0); // move right
						this.AddButton(24+xOffset, 64+yOffset, 5602, 5606, 600+i, GumpButtonType.Reply, 0); // move down
						this.AddButton(24+xOffset, -15+yOffset, 5600, 5604, 700+i, GumpButtonType.Reply, 0); // move up
						this.AddButton(-16+xOffset, 24+yOffset, 5603, 5607, 800+i, GumpButtonType.Reply, 0); // move left
						
						this.AddButton(68+xOffset, 24+yOffset, 5601, 5605, 1500+i, GumpButtonType.Reply, 0); // move right
						this.AddButton(24+xOffset, 69+yOffset, 5602, 5606, 1600+i, GumpButtonType.Reply, 0); // move down
						this.AddButton(24+xOffset, -20+yOffset, 5600, 5604, 1700+i, GumpButtonType.Reply, 0); // move up
						this.AddButton(-21+xOffset, 24+yOffset, 5603, 5607, 1800+i, GumpButtonType.Reply, 0); // move left
					}
					this.AddBackground( xOffset, yOffset, 64, 64, 9270 );
					this.AddLabel( xOffset + 7, yOffset - 33, 240, ia.Icons[i].Name );
					this.AddImage( xOffset + 10, yOffset + 10, ia.Icons[i].IconID ); // icon
					this.AddLabel( xOffset - 18, yOffset + 80, 0, "Spawn on screen" );
					this.AddButton( xOffset - 32, yOffset + 85, 2362, 2361, 100+i, GumpButtonType.Reply, 0 ); // spawn button
					
					this.AddLabel( xOffset - 18, yOffset + 100, 0, "Remove from screen");
					this.AddButton( xOffset - 32, yOffset + 105, 2362, 2361, 200+i, GumpButtonType.Reply, 0); // remove button
					
					this.AddLabel( xOffset - 18, yOffset + 120, 0, "Modify");
					this.AddButton( xOffset - 32, yOffset + 125, 2362, 2361, 300+i, GumpButtonType.Reply, 0); // modify button
					
					if ( i%3 == 2 )
					{
						xOffset = 69;
						yOffset += 179;
					}
					else
						xOffset += 164;
				}
				if ( pageNum > 0 )
					this.AddButton(345, 411, 2468, 2467, (int)Buttons.PreviousPage, GumpButtonType.Reply, 0);
				if ( pageNum < 4 )
					this.AddButton(434, 410, 2471, 2470, (int)Buttons.NextPage, GumpButtonType.Reply, 0);
			}
			else if ( pageType == 2 ) // icon options screen
			{
				int index = m_PageNum; // acts as icon array index
				
				this.AddBackground(95, 15, 329, 289, 9200);
				this.AddLabel(221, 0, 247, "Icon options");
				this.AddBackground(228, 42, 64, 64, 9270);
				this.AddImage(238, 52, ia.Icons[index].IconID );
				
				this.AddLabel(116, 51, 0, "Change picture");
				this.AddButton(102, 56, 2362, 2361, 400 + index, GumpButtonType.Reply, 0);
				this.AddImage(206, 19, 2445);
				this.AddTextEntry(213, 17, 103, 20, 0, (int)Buttons.NameEntry, ia.Icons[index].Name);
				
				this.AddLabel(110, 117, 0, "Command #1:");
				this.AddLabel(110, 137, 0, "Command #2:");
				this.AddLabel(110, 157, 0, "Command #3:");
				this.AddLabel(110, 177, 0, "Command #4:");
				this.AddLabel(110, 197, 0, "Command #5:");
				this.AddImage(197, 120, 1803);
				this.AddTextEntry(200, 117, 205, 20, 0, (int)Buttons.CmdEntry1, ia.Icons[index].Commands[0]);
				
				this.AddImage(197, 140, 1803);
				this.AddTextEntry(200, 137, 205, 20, 0, (int)Buttons.CmdEntry2, ia.Icons[index].Commands[1]);
				
				this.AddImage(197, 160, 1803);
				this.AddTextEntry(200, 157, 205, 20, 0, (int)Buttons.CmdEntry3, ia.Icons[index].Commands[2]);
				
				this.AddImage(197, 180, 1803);
				this.AddTextEntry(200, 177, 205, 20, 0, (int)Buttons.CmdEntry4, ia.Icons[index].Commands[3]);
				
				this.AddImage(197, 200, 1803);
				this.AddTextEntry(200, 197, 205, 20, 0, (int)Buttons.CmdEntry5, ia.Icons[index].Commands[4]);
				
				this.AddButton(228, 292, 247, 248, 0, GumpButtonType.Reply, 0);
			}
			
			else if ( pageType == 3 ) // change picture screen
			{
				this.AddBackground(95, 15, 329, 289, 9200);
				this.AddLabel(200, -1, 247, "Select new picture");
				int xOffset = 117;
				int yOffset = 36;
				bool atEnd = false;
				for ( int i=pageNum*20; i<pageNum*20+20; i++ )
				{
					if ( i >= ValidIconsArray.Length )
					{
						atEnd = true;
						break;
					}
					
					this.AddButton(xOffset, yOffset, ValidIconsArray[i], ValidIconsArray[i], 2000+ValidIconsArray[i], GumpButtonType.Reply, 0);
					xOffset += 59;
					if ( (i+1)%5 == 0 )
					{
						xOffset = 117;
						yOffset += 70;
					}
					if ( i+1 >= ValidIconsArray.Length )
						atEnd = true;
				}
				
				
				if ( pageNum > 0 )
					this.AddButton(278, 305, 2468, 2467, (int)Buttons.PreviousPage, GumpButtonType.Reply, 0);
				if ( !atEnd )
					this.AddButton(367, 305, 2471, 2470, (int)Buttons.NextPage, GumpButtonType.Reply, 0);
			}
		}

		public enum Buttons
		{
			NextPage = 3,
			PreviousPage,
			NameEntry,
			CmdEntry1,
			CmdEntry2,
			CmdEntry3,
			CmdEntry4,
			CmdEntry5
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			IconAttachment ia = IconAttachment.GetIA( m_Mobile );
			int button = -1;
			int oldPageType = m_PageType;
			int oldPageNum = m_PageNum;
			if ( info.ButtonID == 0 ) // close
			{
				if ( m_PageType == 1 )
					return;
				else if ( m_PageType == 2 ) // was editing options
				{
					m_PageType = 1;
					m_PageNum = (m_PageNum/6);
				}
				else if ( m_PageType == 3 ) // was picking new picture
				{
					m_PageType = 2;
					m_PageNum = m_FocusedIcon;
				}
			}
			
			if ( info.ButtonID >= 2000 ) // clicked on new picture
			{
				button = info.ButtonID - 2000;
				ia.Icons[m_FocusedIcon].IconID = button;
				m_PageType = 2;
				m_PageNum = m_FocusedIcon;
				if ( ia.Icons[m_FocusedIcon].SpawnsOnLogin )
				{
					ia.Icons[m_FocusedIcon].CloseGump( m_Mobile );
					ia.Icons[m_FocusedIcon].SpawnGump( m_Mobile );
				}
			}
			else if ( info.ButtonID >= 1800 ) // move left
			{
				button = info.ButtonID - 1800;
				if ( button >= ia.Icons.Length )
					return;
				ia.Icons[button].Location = new Point2D( ia.Icons[button].Location.X-50, ia.Icons[button].Location.Y );
				ia.Icons[button].CloseGump( m_Mobile );
				ia.Icons[button].SpawnGump( m_Mobile );
			}
			else if ( info.ButtonID >= 1700 ) // move up
			{
				button = info.ButtonID - 1700;
				if ( button >= ia.Icons.Length )
					return;
				ia.Icons[button].Location = new Point2D( ia.Icons[button].Location.X, ia.Icons[button].Location.Y-50 );
				ia.Icons[button].CloseGump( m_Mobile );
				ia.Icons[button].SpawnGump( m_Mobile );
			}
			else if ( info.ButtonID >= 1600 ) // move down
			{
				button = info.ButtonID - 1600;
				if ( button >= ia.Icons.Length )
					return;
				ia.Icons[button].Location = new Point2D( ia.Icons[button].Location.X, ia.Icons[button].Location.Y+50 );
				ia.Icons[button].CloseGump( m_Mobile );
				ia.Icons[button].SpawnGump( m_Mobile );
			}
			else if ( info.ButtonID >= 1500 ) // move right
			{
				button = info.ButtonID - 1500;
				if ( button >= ia.Icons.Length )
					return;
				ia.Icons[button].Location = new Point2D( ia.Icons[button].Location.X+50, ia.Icons[button].Location.Y );
				ia.Icons[button].CloseGump( m_Mobile );
				ia.Icons[button].SpawnGump( m_Mobile );
			}
			else if ( info.ButtonID >= 800 ) // move left
			{
				button = info.ButtonID - 800;
				if ( button >= ia.Icons.Length )
					return;
				ia.Icons[button].Location = new Point2D( ia.Icons[button].Location.X-5, ia.Icons[button].Location.Y );
				ia.Icons[button].CloseGump( m_Mobile );
				ia.Icons[button].SpawnGump( m_Mobile );
			}
			else if ( info.ButtonID >= 700 ) // move up
			{
				button = info.ButtonID - 700;
				if ( button >= ia.Icons.Length )
					return;
				ia.Icons[button].Location = new Point2D( ia.Icons[button].Location.X, ia.Icons[button].Location.Y-5 );
				ia.Icons[button].CloseGump( m_Mobile );
				ia.Icons[button].SpawnGump( m_Mobile );
			}
			else if ( info.ButtonID >= 600 ) // move down
			{
				button = info.ButtonID - 600;
				if ( button >= ia.Icons.Length )
					return;
				ia.Icons[button].Location = new Point2D( ia.Icons[button].Location.X, ia.Icons[button].Location.Y+5 );
				ia.Icons[button].CloseGump( m_Mobile );
				ia.Icons[button].SpawnGump( m_Mobile );
			}
			else if ( info.ButtonID >= 500 ) // move right
			{
				button = info.ButtonID - 500;
				if ( button >= ia.Icons.Length )
					return;
				ia.Icons[button].Location = new Point2D( ia.Icons[button].Location.X+5, ia.Icons[button].Location.Y );
				ia.Icons[button].CloseGump( m_Mobile );
				ia.Icons[button].SpawnGump( m_Mobile );
			}
			else if ( info.ButtonID >= 400 ) // change picture
			{
				button = info.ButtonID - 400;
				if ( button >= ia.Icons.Length )
					return;
				m_FocusedIcon = button;
				m_PageType = 3;
				m_PageNum = 0;
			}
			else if ( info.ButtonID >= 300 ) // modify icon
			{
				button = info.ButtonID - 300;
				if ( button >= ia.Icons.Length )
					return;
				m_PageType = 2;
				m_PageNum = button;
			}
			else if ( info.ButtonID >= 200 ) // remove from screen icon
			{
				button = info.ButtonID - 200;
				if ( button >= ia.Icons.Length )
					return;
				ia.Icons[button].SpawnsOnLogin = false;
				ia.Icons[button].CloseGump( m_Mobile );
			}
			else if ( info.ButtonID >= 100 ) // spawn icon
			{
				button = info.ButtonID - 100;
				if ( button >= ia.Icons.Length )
					return;
				ia.Icons[button].SpawnsOnLogin = true;
				ia.Icons[button].CloseGump( m_Mobile );
				ia.Icons[button].SpawnGump( m_Mobile );
			}
			else if ( info.ButtonID == (int)Buttons.NextPage )
			{
				if ( m_PageType == 1 && m_PageNum < 4 )
					m_PageNum++;
				else if ( m_PageType == 3 && m_PageNum < 9000 ) // overflow?
					m_PageNum++;
			}
			else if ( info.ButtonID == (int)Buttons.PreviousPage )
			{
				if ( m_PageType == 1 && m_PageNum > 0 )
					m_PageNum--;
				else if ( m_PageType == 3 && m_PageNum > 0 )
					m_PageNum--;
			}
			
			if ( oldPageType == 2 ) // came from icon options screen, grab the text entries & save
			{
				for ( int i=0; i<info.TextEntries.Length; i++ )
				{
					if ( info.TextEntries[i] == null )
						continue;
					string value = info.TextEntries[i].Text;
					if ( String.IsNullOrEmpty( value ) )
						value = "";
					else if ( value.IndexOf( '$' ) != -1 )
						continue;
					if ( i == 0 )
						ia.Icons[oldPageNum].Name = value; // name
					else if ( i <= 5 )
					{
						ia.Icons[oldPageNum].Commands[i-1] = value;
					}
				}
			}
			
			m_Mobile.SendGump( new MyIconsGump( m_PageType, m_PageNum, m_FocusedIcon, m_Mobile ) );
		}
	}
}
