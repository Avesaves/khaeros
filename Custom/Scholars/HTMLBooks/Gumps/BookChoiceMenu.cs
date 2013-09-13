using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using System.Text.RegularExpressions;

namespace Server.Gumps
{
	public class BookChoiceMenu : Gump
	{
		private HTMLBook m_Book;
		private int m_Menu;
		private int m_EncryptionLevel;
		private Language m_Language;
		public BookChoiceMenu( HTMLBook book ) : this( book, 0, 0 )
		{
		}
		
		public BookChoiceMenu( HTMLBook book, int menu ) : this( book, menu, 0, book.Language )
		{
		}
		
		public BookChoiceMenu( HTMLBook book, int menu, int encLevel ) : this( book, menu, encLevel, book.Language )
		{
		}
		
		public BookChoiceMenu( HTMLBook book, int menu, int encLevel, Language language ) : base( 0, 0 )
		{
			m_Language = language;
			m_Menu = menu;
			m_Book = book;
			m_EncryptionLevel = encLevel;
			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
			
			this.AddBackground(67, 55, 270, 234, 5120);
			this.AddLabel(152, 69, 337, "I would like to ...");
			this.AddImage(58, 61, 1231);
			this.AddLabel(144, 109, 910, "Edit content");
			this.AddLabel(144, 139, 910, "Edit styles");
			
			if ( m_Book.Writable )
			{
				this.AddLabel(144, 169, 910, "Seal the book");
				this.AddButton(122, 170, 9702, 9703, (int)Buttons.SealBook, GumpButtonType.Reply, 0);
			}
			else
			{
				this.AddLabel(144, 169, 910, "Unseal the book");
				this.AddButton(122, 170, 9702, 9703, (int)Buttons.UnsealBook, GumpButtonType.Reply, 0);
			}
			this.AddButton(122, 110, 9702, 9703, (int)Buttons.EditContent, GumpButtonType.Reply, 0);
			this.AddButton(122, 140, 9702, 9703, (int)Buttons.EditStyles, GumpButtonType.Reply, 0);
			this.AddButton(170, 274, 243, 242, 0, GumpButtonType.Reply, 0);
			this.AddLabel(144, 199, 910, "Translate");
			this.AddButton(122, 200, 9702, 9703, (int)Buttons.Translate, GumpButtonType.Reply, 0);
			
			if ( m_Book.Cypher.Length == 0 )
			{// TODO Scholar feat encryption
				this.AddLabel(144, 229, 910, "Encrypt content");
				this.AddButton(122, 230, 9702, 9703, (int)Buttons.Encrypt, GumpButtonType.Reply, 0);
			}
			else
			{
				this.AddLabel(144, 229, 910, "Decrypt content");
				this.AddButton(122, 230, 9702, 9703, (int)Buttons.Decrypt, GumpButtonType.Reply, 0);
			}
			
			if ( menu == 1 ) // encrypt dialog is open
			{
				this.AddBackground(328, 61, 146, 195, 2620);
				this.AddLabel(348, 71, 337, "Encryption Level");
				this.AddButton(345, 103, (m_EncryptionLevel == 1 ? 11400 : 11410), (m_EncryptionLevel == 1 ? 11402 : 11412), (int)Buttons.Simple, GumpButtonType.Reply, 0);
				this.AddLabel(366, 101, 56, "Simple");
				this.AddButton(345, 133, (m_EncryptionLevel == 2 ? 11400 : 11410), (m_EncryptionLevel == 2 ? 11402 : 11412), (int)Buttons.Moderate, GumpButtonType.Reply, 0);
				this.AddLabel(366, 131, 153, "Moderate");
				this.AddButton(345, 163, (m_EncryptionLevel == 3 ? 11400 : 11410), (m_EncryptionLevel == 3 ? 11402 : 11412), (int)Buttons.Complex, GumpButtonType.Reply, 0);
				this.AddLabel(366, 161, 139, "Complex");
				this.AddImageTiled(364, 193, 74, 16, 1803);
				this.AddImage(356, 193, 1802);
				this.AddImageTiled(438, 193, 7, 16, 1804);
				this.AddTextEntry(362, 191, 79, 20, 798, (int)Buttons.EncryptionCypher, "");
				int digits = m_EncryptionLevel * 2;
				this.AddLabel(357, 211, 100, "[" + digits + " digits max.]");
				this.AddButton(370, 243, 239, 240, (int)Buttons.DoEncrypt, GumpButtonType.Reply, 0);
			}
			else if ( menu == 2 ) // decrypt dialog is open
			{
				this.AddBackground(328, 61, 146, 135, 2620);
				this.AddLabel(347, 71, 337, "Decrypt Content");
				this.AddImageTiled(364, 130, 74, 16, 1803);
				this.AddImage(356, 130, 1802);
				this.AddImageTiled(438, 130, 7, 16, 1804);
				this.AddTextEntry(362, 128, 79, 20, 798, (int)Buttons.DecryptionCypher, "");
				int digits = m_Book.Cypher.Length;
				this.AddLabel(357, 148, 100, "[" + digits + " digits]");
				this.AddButton(370, 183, 239, 240, (int)Buttons.DoDecrypt, GumpButtonType.Reply, 0);
				this.AddLabel(358, 107, 51, "Enter Cypher");
			}
			else if ( menu == 3 ) // translation menu is open
			{
				this.AddBackground(328, 61, 146, 205, 2620);
				this.AddLabel(364, 71, 337, "Translation");
				this.AddLabel(366, 101, ( m_Language == Language.Southern ? 56 : 95 ), "Southern");
				this.AddButton(370, 253, 239, 240, (int)Buttons.DoTranslate, GumpButtonType.Reply, 0);
				this.AddButton(350, 105, 2362, 2361, (int)Buttons.Southern, GumpButtonType.Reply, 0);
				this.AddLabel(366, 121, ( m_Language == Language.Western ? 56 : 95 ), "Western");
				this.AddButton(350, 125, 2362, 2361, (int)Buttons.Western, GumpButtonType.Reply, 0);
				this.AddLabel(366, 141, ( m_Language == Language.Common ? 56 : 95 ), "Common");
				this.AddButton(350, 145, 2362, 2361, (int)Buttons.Common, GumpButtonType.Reply, 0);
				this.AddLabel(366, 161, ( m_Language == Language.Haluaroc ? 56 : 95 ), "Ancient");
				this.AddButton(350, 165, 2362, 2361, (int)Buttons.Haluaroc, GumpButtonType.Reply, 0);
				this.AddLabel(366, 181, ( m_Language == Language.Northern ? 56 : 95 ), "Northern");
				this.AddButton(350, 185, 2362, 2361, (int)Buttons.Northern, GumpButtonType.Reply, 0);

			}
		}

		public enum Buttons
		{
			EditContent = 1,
			EditStyles,
			SealBook,
			UnsealBook,
			Translate,
			Encrypt,
			Decrypt,
			Simple,
			Moderate,
			Complex,
			Southern,
			Western,
			Common,
			Haluaroc,
			Northern,
			DoTranslate,
			DoDecrypt,
			DoEncrypt,
			DecryptionCypher,
			EncryptionCypher
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			
			if ( info.ButtonID == 0 ) // close
				return;
			
			else if ( m_Book.RequiresFormatting )
			{
				m_Book.FixContent();
				m_Book.HTMLContent.UpdateCache();
				m_Book.RequiresFormatting = false;
			}
			
			if ( m_Book.RootParent != from )
			{
				from.SendMessage( "The book must be on your person in order to edit it." );
				return;
			}

			switch ( info.ButtonID )
			{
				case (int)Buttons.Encrypt:
				{
					if ( m_Book.Cypher.Length == 0 )
					{
						if ( m_Book.Writable )
						{
							from.SendMessage( "Only sealed books can be encrypted." );
							from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
						}
						else
						{
							if( ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Cryptography) > 0 )
								from.SendGump( new BookChoiceMenu( m_Book, 1 ) );
							else
							{
								from.SendMessage( "You lack the appropriate feat." );
								from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
							}
						}
					}
					
					break;
				}
				
				case (int)Buttons.Decrypt:
				{
					if ( m_Book.Cypher.Length > 0 )
					{
						from.SendGump( new BookChoiceMenu( m_Book, 2 ) );
					}
					
					break;
				}
				case (int)Buttons.DoTranslate:
				{
					if ( HTMLBook.UnderstandsLanguage( m_Language, from ) && HTMLBook.UnderstandsLanguage( m_Book.Language, from ))
					{
						m_Book.Language = m_Language;
						from.SendMessage( "You've successfully translated the book into the chosen language." );
					}
					else
					{
						from.SendMessage( "You don't know that language!" );
						from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
					}
					break;
				}
				case (int)Buttons.Common:
				{
					if ( HTMLBook.UnderstandsLanguage( Language.Common, from ) && HTMLBook.UnderstandsLanguage( m_Book.Language, from ))
						m_Language = Language.Common;
					else
						from.SendMessage( "You don't know that language!" );
					
					from.SendGump( new BookChoiceMenu( m_Book, m_Menu, m_EncryptionLevel, m_Language ) );
					break;
				}
				
				case (int)Buttons.Northern:
				{
					if ( HTMLBook.UnderstandsLanguage( Language.Northern, from ) && HTMLBook.UnderstandsLanguage( m_Book.Language, from ))
						m_Language = Language.Northern;
					else
						from.SendMessage( "You don't know that language!" );
					
					from.SendGump( new BookChoiceMenu( m_Book, m_Menu, m_EncryptionLevel, m_Language ) );
					break;
				}
				
			
				
				case (int)Buttons.Haluaroc:
				{
					if ( HTMLBook.UnderstandsLanguage( Language.Haluaroc, from ) && HTMLBook.UnderstandsLanguage( m_Book.Language, from ))
						m_Language = Language.Haluaroc;
					else
						from.SendMessage( "You don't know that language!" );
					
					from.SendGump( new BookChoiceMenu( m_Book, m_Menu, m_EncryptionLevel, m_Language ) );
					break;
				}
				

				
				case (int)Buttons.Southern:
				{
					if ( HTMLBook.UnderstandsLanguage( Language.Southern, from ) && HTMLBook.UnderstandsLanguage( m_Book.Language, from ))
						m_Language = Language.Southern;
					else
						from.SendMessage( "You don't know that language!" );
					
					from.SendGump( new BookChoiceMenu( m_Book, m_Menu, m_EncryptionLevel, m_Language ) );
					break;
				}
				
				case (int)Buttons.Western:
				{
					if ( HTMLBook.UnderstandsLanguage( Language.Western, from ) && HTMLBook.UnderstandsLanguage( m_Book.Language, from ))
						m_Language = Language.Western;
					else
						from.SendMessage( "You don't know that language!" );
					
					from.SendGump( new BookChoiceMenu( m_Book, m_Menu, m_EncryptionLevel, m_Language ) );
					break;
				}
				
				case (int)Buttons.Translate:
				{
					if ( m_Book.Cypher.Length > 0 ) // can't translate while encrypted
					{
						from.SendGump( new BookChoiceMenu( m_Book, 0 ) );
						from.SendMessage( "You cannot translate a book that is encrypted -- decrypt it first." );
					}
					else if ( !HTMLBook.UnderstandsLanguage( m_Book.Language, from ) )
					{
						from.SendMessage( "You don't know the language the book's written in!" );
						from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
					}
					else
						from.SendGump( new BookChoiceMenu( m_Book, 3 ) );
					
					break;
				}
				
				case (int)Buttons.DoDecrypt:
				{
					if ( m_Menu == 2 && info.TextEntries[0] != null && m_Book.Cypher.Length > 0 )
					{
						string cypher = info.TextEntries[0].Text;
						if ( !Regex.IsMatch(cypher, @"^\d+$") )
						{
							from.SendMessage( "Cyphers can only constitute of numbers." );
							from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
						}
						else if ( cypher.Length < m_Book.Cypher.Length )
						{
							from.SendMessage( "This cypher is too short. Try something longer." );
							from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
						}
						else if ( cypher.Length > m_Book.Cypher.Length )
						{
							from.SendMessage( "This cypher is too long. Try something shorter." );
							from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
						}
						else if ( cypher != m_Book.Cypher )
						{
							from.SendMessage( "That's not the correct cypher." );
							from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
						}
						else
						{
							from.SendMessage( "That cypher is correct. You've managed to decrypt the book's content." );
							m_Book.Cypher = "";
							from.SendGump( new BookChoiceMenu( m_Book, 0 ) );
						}
					}
					
					break;
				}
				case (int)Buttons.Simple: // simple encryption
				{
					if ( m_Menu == 1 )
					{
						if( ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Cryptography) > 0 )
							from.SendGump( new BookChoiceMenu( m_Book, m_Menu, 1 ) );
						
						else
							from.SendMessage( "You lack the appropriate feat" );
					}
					break;
				}
				
				case (int)Buttons.Moderate: // moderate encryption
				{
					if ( m_Menu == 1 )
					{
						if( ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Cryptography) > 1 )
							from.SendGump( new BookChoiceMenu( m_Book, m_Menu, 2 ) );
						
						else
							from.SendMessage( "You lack the appropriate feat" );
					}
					break;
				}
				
				case (int)Buttons.Complex: // complex encryption
				{
					if ( m_Menu == 1 )
					{
						if( ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Cryptography) > 2 )
							from.SendGump( new BookChoiceMenu( m_Book, m_Menu, 3 ) );
						
						else
							from.SendMessage( "You lack the appropriate feat" );
					}
					break;
				}
				
				case (int)Buttons.DoEncrypt:
				{
					if ( m_Menu == 1 && info.TextEntries[0] != null && m_Book.Cypher.Length == 0 )
					{
						string cypher = info.TextEntries[0].Text;
						if ( m_EncryptionLevel == 0 )
						{
							from.SendMessage( "Select an encryption level first." );
							from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
						}
						else if ( !Regex.IsMatch(cypher, @"^\d+$") )
						{
							from.SendMessage( "Cyphers can only constitute of numbers." );
							from.SendGump( new BookChoiceMenu( m_Book, m_Menu, m_EncryptionLevel ) );
						}
						else if ( cypher.Length > m_EncryptionLevel * 2 )
						{
							from.SendMessage( "That cypher is too long." );
							from.SendGump( new BookChoiceMenu( m_Book, m_Menu, m_EncryptionLevel ) );
						}
						else
						{
							from.SendMessage( "You encrypt the book's content with the cypher." );
							m_Book.Cypher = cypher;
							from.SendGump( new BookChoiceMenu( m_Book, 0 ) );
						}
					}
					
					break;
				}
				
				case (int)Buttons.SealBook:
				{
					if ( m_Book.Writable )
					{
						m_Book.Writable = false;
						from.SendMessage( "You seal the book." );
						m_Book.SealedBy = from;
						from.SendGump( new BookChoiceMenu( m_Book ) );
					}
					break;
				}
				
				case (int)Buttons.UnsealBook:
				{
					if ( m_Book.Writable )
					{
						from.SendMessage( "This book is not sealed." );
						from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
					}
					else if ( m_Book.Cypher.Length != 0 )
					{
						from.SendMessage( "This book is encrypted. Remove its encryption first." );
						from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
					}
					else if ( m_Menu != -1 ) // used as confirmation
					{
						from.SendMessage( 40, "WARNING: Unsealing this book will wipe all of its HTML styles. Click the button again if you wish to do so anyway." );
						from.SendGump( new BookChoiceMenu( m_Book, -1 ) );
					}
					else
					{
						from.SendMessage( "You have unsealed the book, and in doing thus, removed all of its HTML styles." );
						m_Book.Writable = true;
						m_Book.SealedBy = null;
						m_Book.HTMLContent = new HTMLContent( m_Book.Pages.Length, m_Book.MaxLines, m_Book );
						m_Book.FixStyling();
						m_Book.HTMLContent.UpdateCache();
						from.SendGump( new BookChoiceMenu( m_Book, 0 ) );
					}
					break;
				}
				
				case (int)Buttons.EditStyles:
				{
					if ( m_Book.Cypher.Length != 0 )
					{
						from.SendMessage( "This book is encrypted. Remove its encryption first." );
						from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
					}
					else if ( !HTMLBook.UnderstandsLanguage( m_Book.Language, from ) )
					{
						from.SendMessage( "You don't know the language the book's written in!" );
						from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
					}
					else if ( m_Book.Writable )
					{
						from.SendMessage( "This book is not sealed. In order to edit the styles, seal the book's content first." );
						from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
					}
					else
					{
						from.CloseGump( typeof( BookEditingGump ), -1 );
						from.SendGump( new BookEditingGump( m_Book ) );
					}

					break;
				}
			
				case (int)Buttons.EditContent:
				{
					if ( !m_Book.Writable )
					{
						from.SendMessage( "This book is sealed. Unseal the book if you want to edit its content." );
						from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
					}
					else if ( !HTMLBook.UnderstandsLanguage( m_Book.Language, from ) )
					{
						from.SendMessage( "You don't know the language the book's written in!" );
						from.SendGump( new BookChoiceMenu( m_Book, m_Menu ) );
					}
					else
					{
						if ( m_Book.Title == null && m_Book.Author == null )
						{
							m_Book.Title = "a book";
							m_Book.Author = from.Name;
						}
						from.Send( new BookHeader( from, m_Book ) );
						from.Send( new BookPageDetails( m_Book ) );
					}

					break;
				}
			}
		}
	}
}
