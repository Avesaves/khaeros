using Server;
using System;
using Server.Network;
using Server.Items;

namespace Server.Gumps
{
	public class OffsetableBookGump : Gump, IHTMLBookGump
	{
		private OffsetableBook m_Book;
		private int m_Page;

		public OffsetableBookGump( OffsetableBook book ) : this ( book, 0 )
		{
		}

		public OffsetableBookGump( OffsetableBook book, int page ) : base(0, 0)
		{
			m_Book = book;
			m_Page = page;

			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage( 0 );
			
			string text1 = "";

			if ( book.HTMLContent.CachedHTMLContent[page].Lines != null )
				for (int i = 0; i < book.HTMLContent.CachedHTMLContent[page].Lines.Length; i++)
					text1 += book.HTMLContent.CachedHTMLContent[page].Lines[i];
				
			string text2 = "";

			if ( book.HTMLContent.CachedHTMLContent[page+1].Lines != null )
				for (int i = 0; i < book.HTMLContent.CachedHTMLContent[page+1].Lines.Length; i++)
					text2 += book.HTMLContent.CachedHTMLContent[page+1].Lines[i];
				
			(this as IHTMLBookGump).DrawToSurface( this, 59, 104, text1, text2, page );
		}
		
		Rectangle2D IHTMLBookGump.GetDimensions( string page1Text, string page2Text )
		{
			return new Rectangle2D( 0, 0, 646, 362 );
		}
		
		void IHTMLBookGump.DrawToSurface(Gump gump, int x, int y, string page1Text, string page2Text, int page)
		{
			/*gump.AddImage(x, y, m_Book.GumpID);
			
			gump.AddHtml( 29 + x + m_Book.HTMLOffset.X, 23 + y + m_Book.HTMLOffset.Y, 147, 167, page1Text, (bool)false, (bool)false);
			gump.AddHtml( 186 + x + m_Book.HTMLOffset.X, 23 + y + m_Book.HTMLOffset.Y, 147, 167, page2Text, (bool)false, (bool)false);*/
			gump.AddBackground(x-22+22, y+4, 646, 362, 5120);
			gump.AddBackground(x+22, y+13, 606, 343, 9300);
			if ( page + 2 < m_Book.HTMLContent.CachedHTMLContent.Length )
				gump.AddButton(x+576, y+0, 2469, 2471, (int)Buttons.NextPage, GumpButtonType.Reply, 0);
			if ( page - 2 >= 0 )
				gump.AddButton(x+492, y+0, 2466, 2468, (int)Buttons.PreviousPage, GumpButtonType.Reply, 0);
			
			gump.AddHtml( x+31, y+26, 274, 314, page1Text, (bool)false, (bool)false);
			gump.AddHtml( x+343, y+26, 274, 314, page2Text, (bool)false, (bool)false);
			gump.AddImage(x+316, y+341, 10204);
			gump.AddImageTiled(x+316, y+28, 17, 318, 10201);
			gump.AddImage(x+316, y+13, 10202);
			gump.AddHtml( x+31, y+333, 274, 20, "<CENTER><SMALL><B>" + (page+1), (bool)false, (bool)false);
			gump.AddHtml( x+343, y+333, 274, 20, "<CENTER><SMALL><B>" + (page+2), (bool)false, (bool)false);
			
			/*if ( page + 2 < m_Book.HTMLContent.CachedHTMLContent.Length )
				gump.AddButton(294 + x + m_Book.NextPageButtonOffset.X, 4 + y + m_Book.NextPageButtonOffset.Y, 2236, 2236, (int)Buttons.NextPage, GumpButtonType.Reply, 0);

			if ( page - 2 >= 0 )
				gump.AddButton(23 + x + m_Book.PrevPageButtonOffset.X, 5 + y + m_Book.PrevPageButtonOffset.Y, 2235, 2235, (int)Buttons.PreviousPage, GumpButtonType.Reply, 0);
			*/
		}
		
		public enum Buttons
		{
			NextPage = 1,
			PreviousPage = 2,
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( info.ButtonID == 0 ) // close
				return;

			Mobile from = sender.Mobile;

			switch ( info.ButtonID )
			{
				case (int)Buttons.NextPage:
				{
					if ( m_Page + 2 >= m_Book.Pages.Length )
						return;
					from.SendGump( new OffsetableBookGump( m_Book, m_Page + 2 ) );

					break;
				}
			
				case (int)Buttons.PreviousPage:
				{
					if ( m_Page - 2 < 0 )
						return;
					from.SendGump( new OffsetableBookGump( m_Book, m_Page - 2 ) );

					break;
				}
			}
		}
	}
}
