using Server;
using Server.Network;
using Server.Items;
using System.Text.RegularExpressions;

namespace Server.Gumps
{
	public class EnterCypherGump : Gump
	{
		private HTMLBook m_Book;
		public EnterCypherGump( HTMLBook book ) : base(0, 0)
		{
			m_Book = book;
			
			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			
			this.AddBackground(134, 63, 146, 135, 2620);
			this.AddLabel(153, 73, 337, "Decrypt Content");
			this.AddImageTiled(170, 132, 74, 16, 1803);
			this.AddImage(162, 132, 1802);
			this.AddImageTiled(244, 132, 7, 16, 1804);
			this.AddTextEntry(168, 130, 79, 20, 798, (int)Buttons.Cypher, "");
			int len = m_Book.Cypher.Length;
			this.AddLabel(163, 150, 100, "[" + len + " digits]");
			this.AddButton(176, 185, 239, 240, (int)Buttons.Apply, GumpButtonType.Reply, 0);
			this.AddLabel(164, 109, 51, "Enter Cypher");
		}

		public enum Buttons
		{
			Cypher = 1,
			Apply,
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			switch ( info.ButtonID )
			{
				case (int)Buttons.Apply:
				{
					if ( info.TextEntries[0] != null )
					{
						string cypher = info.TextEntries[0].Text;
						if ( !Regex.IsMatch(cypher, @"^\d+$") )
						{
							from.SendMessage( "Cyphers can only constitute of numbers." );
							from.SendGump( new EnterCypherGump( m_Book ) );
						}
						else if ( cypher.Length < m_Book.Cypher.Length )
						{
							from.SendMessage( "This cypher is too short. Try something longer." );
							from.SendGump( new EnterCypherGump( m_Book ) );
						}
						else if ( cypher.Length > m_Book.Cypher.Length )
						{
							from.SendMessage( "This cypher is too long. Try something shorter." );
							from.SendGump( new EnterCypherGump( m_Book ) );
						}
						else if ( cypher != m_Book.Cypher )
						{
							from.SendMessage( "That's not the correct cypher." );
							from.SendGump( new EnterCypherGump( m_Book ) );
						}
						else
						{
							from.SendMessage( "That cypher is correct. You've managed to decrypt the book's content." );
							if ( HTMLBook.UnderstandsLanguage( m_Book.Language, from ) )
								m_Book.SpawnGump( from );
							else
								from.SendMessage( "It's all jibberish, though, since you don't know the language." );
						}
					}
					break;
				}
			}
		}
	}
}
