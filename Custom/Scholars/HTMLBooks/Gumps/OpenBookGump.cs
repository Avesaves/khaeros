using Server;
using System;
using Server.Network;
using Server.Items;

namespace Server.Gumps
{
	public class OpenBookGump : Gump
	{
		private HTMLBook m_Book;
		public OpenBookGump( HTMLBook book ) : base( 0, 0 )
		{
			m_Book = book;
			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(173, 98, 115, 58, 9350);
			this.AddButton(184, 109, 2224, 2224, (int)Buttons.View, GumpButtonType.Reply, 0);
			this.AddLabel(207, 106, 44, "View book");
			this.AddButton(184, 128, 2224, 2224, (int)Buttons.Edit, GumpButtonType.Reply, 0);
			this.AddLabel(207, 125, 44, "Edit book");
		}
		
		public enum Buttons
		{
			View = 1,
			Edit
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			switch ( info.ButtonID )
			{
				case (int)Buttons.View:
				{
					if ( m_Book.Cypher.Length > 0 )
					{
						from.CloseGump( typeof( EnterCypherGump ) );
						from.SendGump( new EnterCypherGump( m_Book ) );
						from.SendMessage( "This book is encrypted. In order to read it, you need to input the correct cypher." );
					}
					else if ( HTMLBook.UnderstandsLanguage( m_Book.Language, from ) )
					{
						m_Book.SpawnGump( from );
					}
					else
						from.SendMessage( "It's all gibberish, since you don't know the language." );
					break;
				}
				
				case (int)Buttons.Edit:
				{
					if ( m_Book.RootParent == from )
					{
						from.CloseGump( typeof( BookChoiceMenu ) );
						from.SendGump( new BookChoiceMenu( m_Book ) );
					}
					else
						from.SendMessage( "The book must be on your person in order to edit it." );
					break;
				}
			}
		}
	}
}
