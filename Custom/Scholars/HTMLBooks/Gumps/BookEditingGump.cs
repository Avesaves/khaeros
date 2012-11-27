using Server;
using System;
using Server.Network;
using Server.Items;
using System.Collections.Generic;
using System.Text;

namespace Server.Gumps
{
	public class BookEditingGump : Gump
	{
		private HTMLBook m_Book;
		private int m_Page;
		private int m_Level;
		private int m_FocusedPage; // used in Page, Line, Word level
		private int m_FocusedLine; // used in Line and Word level
		private int m_FocusedWord; // used in Word level
		private string m_SearchWord;

		private HTMLTag [] m_Tags = new HTMLTag[5];

		private static int [] m_LineNumberOffsets = { 8, 6, 4, 5 }; // X offsets for: 1, 2-9, 10 + 12-xx, 11

		public BookEditingGump( HTMLBook book ) : this( book, 0, 0, 0, 0, 0, "" )
		{
		}

		public BookEditingGump( HTMLBook book, int page ) : this( book, page, 0, 0, 0, 0, "" )
		{
		}

		public BookEditingGump( HTMLBook book, int page, int level, int focusedPage, int focusedLine, int focusedWord, string searchWord ) : base( 0, 0 )
		{
			m_Book = book;
			m_Page = page;
			m_Level = level;
			m_FocusedPage = focusedPage;
			m_FocusedLine = focusedLine;
			m_FocusedWord = focusedWord;
			m_SearchWord = searchWord;
			if ( m_SearchWord.Length == 0 )
				m_SearchWord = "search...";

			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);

			IHTMLBookGump bookGump = Activator.CreateInstance(m_Book.Gump, m_Book) as IHTMLBookGump;
			string page1 = ConstructHTMLPage( page );
			string page2 = "";
			if ( page + 1 < m_Book.FormattedBookContent.Length )
				page2 = ConstructHTMLPage( page+1 );
			
			Rectangle2D bookGumpDimensions = bookGump.GetDimensions( page1, page2 );
			int width = bookGumpDimensions.Width;
			int height = bookGumpDimensions.Height;
			
			int startX = 8;
			int startY = 62;
			int innerGumpStartX = startX;
			int innerGumpStartY = 104;
			int yScaleAmount = 0;
			
			if ( width > 457 ) // center outside
			{
				startX = (width - 457)/2;
				innerGumpStartX = 0;
			}
			else // center inside
			{
				int diff = (457 - width)/2;
				innerGumpStartX += diff;
			}

			yScaleAmount = height;
			
			this.AddBackground(0+startX, 0+startY, 457, 64+yScaleAmount, 9270);
			this.AddBackground(0+startX, 52+startY+yScaleAmount, 457, 172, 9270);
			this.AddBackground(11+startX, 63+startY+yScaleAmount, 435, 149, 9350);
			
			bookGump.DrawToSurface( this, innerGumpStartX, innerGumpStartY, page1, page2, page );

			this.AddLabel(56+startX, 17+startY, 1125, "Editing Level:");
			this.AddButton(150+startX, 79, ( m_Level == 0 ? 1895 : 1896 ), 1895, (int)Buttons.Body, GumpButtonType.Reply, 0);
			this.AddLabel(172+startX, 79, ( m_Level == 0 ? 1125 : 1130 ), "Body");

			this.AddButton(210+startX, 79, ( m_Level == 1 ? 1895 : 1896 ), 1895, (int)Buttons.Page, GumpButtonType.Reply, 0);
			this.AddLabel(231+startX, 79, ( m_Level == 1 ? 1125 : 1130 ), "Page");

			this.AddButton(270+startX, 79, ( m_Level == 2 ? 1895 : 1896 ), 1895, (int)Buttons.Lines, GumpButtonType.Reply, 0);
			this.AddLabel(292+startX, 79, ( m_Level == 2 ? 1125 : 1130 ), "Lines");
			this.AddButton(330+startX, 79, ( m_Level == 3 ? 1895 : 1896 ), 1895, (int)Buttons.Words, GumpButtonType.Reply, 0);
			this.AddLabel(352+startX, 79, ( m_Level == 3 ? 1125 : 1130 ), "Words");

			this.AddPage(1);

			List<HTMLTag> tags = new List<HTMLTag>();
			switch ( m_Level )
			{
				case 0:	// BODY
				{
					this.AddLabel(210+startX, 67+startY+yScaleAmount, 634, "BODY");

					foreach ( HTMLTag tag in m_Book.HTMLContent.Body )
						tags.Add( tag );

					break;
				}

				case 1:	// PAGE
				{
					this.AddLabel(210+startX, 67+startY+yScaleAmount, 634, "PAGE");
					this.AddLabel(22+startX, 94+startY+yScaleAmount, 635, "Page:");
					this.AddButton(98+startX, 95+startY+yScaleAmount, ( m_FocusedPage == 0 ? 1895 : 1896 ), 1895, (int)Buttons.LeftPage, GumpButtonType.Reply, 0);
					this.AddLabel(120+startX, 95+startY+yScaleAmount, ( m_FocusedPage == 0 ? 635 : 632 ), "Left");
					this.AddButton(158+startX, 95+startY+yScaleAmount, ( m_FocusedPage == 1 ? 1895 : 1896 ), 1895, (int)Buttons.RightPage, GumpButtonType.Reply, 0);
					this.AddLabel(180+startX, 95+startY+yScaleAmount, ( m_FocusedPage == 1 ? 635 : 632 ), "Right");

					List<HTMLTag> bodyTags = m_Book.HTMLContent.Body; // add page first, then fill blanks with body tags
					List<HTMLTag> pageTags = m_Book.HTMLContent.GetPageTags( m_Page + m_FocusedPage );

					foreach ( HTMLTag pageTag in pageTags )
						tags.Add( pageTag );

					foreach ( HTMLTag tag in bodyTags )
					{
						bool found = false;
						foreach ( HTMLTag pageTag in pageTags )
						{
							if ( tag.Type == pageTag.Type )
							{
								found = true;
								break;
							}
						}

						if (!found) // type is not overriden in page tags
							tags.Add( tag );
					}

					break;
				}

				case 2:	// LINES
				{
					this.AddLabel(210+startX, 67+startY+yScaleAmount, 634, "LINES");
					this.AddLabel(22+startX, 94+startY+yScaleAmount, 635, "Line:");

					int x = 59;
					int y = 90;
					for ( int i = 1; i <= m_Book.MaxLines * ( page + 1 < m_Book.FormattedBookContent.Length ? 2 : 1); i++ )
					{
						if ( i == 19 )
						{
							startY += 20;
							x = 59;
						}
						
						int actualLine, actualPage, lineColor, xOffset;
						actualPage = ( i > m_Book.MaxLines ? 1 : 0 );
						actualLine = i - actualPage*m_Book.MaxLines - 1;
						lineColor = ( actualPage == m_FocusedPage && actualLine == m_FocusedLine ? 36 : 635 );

						this.AddButton(x+startX, y+startY+yScaleAmount, 9792, 9792, 100 + ( i-1 ), GumpButtonType.Reply, 0); // id offset for line buttons is 100 + lineIndex

						if ( i == 1 )
							xOffset = m_LineNumberOffsets[0];
						else if ( i > 1 && i < 10 )
							xOffset = m_LineNumberOffsets[1];
						else if ( i == 11 )
							xOffset = m_LineNumberOffsets[3];
						else
							xOffset = m_LineNumberOffsets[2];

						this.AddLabel(x + xOffset + startX, y + 3+startY+yScaleAmount, lineColor, "" + i);
						x += 20;
					}

					// add line first, then fill blanks with page & body tags
					List<HTMLTag> lineTags = m_Book.HTMLContent.GetLineTags( m_Page + m_FocusedPage, m_FocusedLine );
					List<HTMLTag> bodyTags = m_Book.HTMLContent.Body;
					List<HTMLTag> pageTags = m_Book.HTMLContent.GetPageTags( m_Page + m_FocusedPage );

					foreach ( HTMLTag lineTag in lineTags )
						tags.Add( lineTag );

					foreach ( HTMLTag pageTag in pageTags )
					{
						bool found = false;
						foreach ( HTMLTag tag in tags )
						{
							if ( pageTag.Type == tag.Type )
							{
								found = true;
								break;
							}
						}

						if (!found) // type is not overriden in line tags
							tags.Add( pageTag );
					}

					foreach ( HTMLTag bodyTag in bodyTags )
					{
						bool found = false;
						foreach ( HTMLTag tag in tags )
						{
							if ( bodyTag.Type == tag.Type )
							{
								found = true;
								break;
							}
						}

						if (!found) // type is not overriden in page tags
							tags.Add( bodyTag );
					}

					break;
				}

				case 3:	// WORDS
				{
					this.AddLabel(210+startX, 67+startY+yScaleAmount, 634, "WORDS");
					this.AddLabel(22+startX, 94+startY+yScaleAmount, 635, "Word:");

					// add word first, then fill blanks with line, page & body tags
					List<HTMLTag> wordTags = m_Book.HTMLContent.GetWordTags( m_Page + m_FocusedPage, m_FocusedLine, m_FocusedWord );
					List<HTMLTag> lineTags = m_Book.HTMLContent.GetLineTags( m_Page + m_FocusedPage, m_FocusedLine );
					List<HTMLTag> bodyTags = m_Book.HTMLContent.Body;
					List<HTMLTag> pageTags = m_Book.HTMLContent.GetPageTags( m_Page + m_FocusedPage );

					foreach ( HTMLTag wordTag in wordTags )
						tags.Add( wordTag );

					foreach ( HTMLTag lineTag in lineTags )
					{
						bool found = false;
						foreach ( HTMLTag tag in tags )
						{
							if ( lineTag.Type == tag.Type )
							{
								found = true;
								break;
							}
						}

						if (!found) // type is not overriden in word tags
							tags.Add( lineTag );
					}

					foreach ( HTMLTag pageTag in pageTags )
					{
						bool found = false;
						foreach ( HTMLTag tag in tags )
						{
							if ( pageTag.Type == tag.Type )
							{
								found = true;
								break;
							}
						}

						if (!found) // type is not overriden in line tags
							tags.Add( pageTag );
					}

					foreach ( HTMLTag bodyTag in bodyTags )
					{
						bool found = false;
						foreach ( HTMLTag tag in tags )
						{
							if ( bodyTag.Type == tag.Type )
							{
								found = true;
								break;
							}
						}

						if (!found) // type is not overriden in page tags
							tags.Add( bodyTag );
					}

					break;
				}
			}

			// every level shares these
			foreach ( HTMLTag tag in tags )
				m_Tags[(int)tag.Type] = tag;

			int font = 1;
			if ( m_Tags[(int)TagType.Font] != null )
			{
				if ( m_Tags[(int)TagType.Font].Name == Tag.LargeFont )
					font = 2;
				else if ( m_Tags[(int)TagType.Font].Name == Tag.SmallFont )
					font = 0;
			}

			this.AddLabel(22+startX, 114+startY+yScaleAmount, 635, "Font Size:");
			this.AddButton(98+startX, 115+startY+yScaleAmount, (font == 0 ? 1895 : 1896), 1895, (int)Buttons.Small, GumpButtonType.Reply, 0);
			this.AddLabel(120+startX, 115+startY+yScaleAmount, (font == 0 ? 635 : 632), "Small");
			this.AddButton(158+startX, 115+startY+yScaleAmount, (font == 1 ? 1895 : 1896), 1895, (int)Buttons.Medium, GumpButtonType.Reply, 0);
			this.AddLabel(180+startX, 115+startY+yScaleAmount, (font == 1 ? 635 : 632), "Medium");
			this.AddButton(230+startX, 114+startY+yScaleAmount, (font == 2 ? 1895 : 1896), 1895, (int)Buttons.Large, GumpButtonType.Reply, 0);
			this.AddLabel(252+startX, 114+startY+yScaleAmount, (font == 2 ? 635 : 632), "Large");

			string color = "111111";
			if ( m_Tags[(int)TagType.Color] != null )
				color = m_Tags[(int)TagType.Color].Value;

			this.AddLabel(22+startX, 134+startY+yScaleAmount, 635, "Font Color:");
			this.AddTextEntry(114+startX, 134+startY+yScaleAmount, 60, 20, 632, (int)Buttons.HexColorEntry, color);
			this.AddLabel(100+startX, 134+startY+yScaleAmount, 632, "#");

			int style = 0;
			if ( m_Tags[(int)TagType.Italic] != null )
			{
				if ( m_Tags[(int)TagType.Bold] != null )
					style = 3;
				else
					style = 1;
			}
			else if ( m_Tags[(int)TagType.Bold] != null )
				style = 2;
			
			this.AddLabel(22+startX, 154+startY+yScaleAmount, 635, "Font Style:");
			this.AddButton(98+startX, 155+startY+yScaleAmount, ( style == 0 ? 1895 : 1896 ), 1895, (int)Buttons.None, GumpButtonType.Reply, 0);
			this.AddLabel(120+startX, 155+startY+yScaleAmount, (style == 0 ? 635 : 632), "None");
			this.AddButton(158+startX, 155+startY+yScaleAmount, ( style == 1 ? 1895 : 1896 ), 1895, (int)Buttons.Italic, GumpButtonType.Reply, 0);
			this.AddLabel(180+startX, 155+startY+yScaleAmount, (style == 1 ? 635 : 632), "Italic");
			this.AddButton(220+startX, 155+startY+yScaleAmount, ( style == 2 ? 1895 : 1896 ), 1895, (int)Buttons.Bold, GumpButtonType.Reply, 0);
			this.AddLabel(242+startX, 155+startY+yScaleAmount, (style == 2 ? 635 : 632), "Bold");
			this.AddButton(277+startX, 155+startY+yScaleAmount, ( style == 3 ? 1895 : 1896 ), 1895, (int)Buttons.ItalicBold, GumpButtonType.Reply, 0);
			this.AddLabel(299+startX, 155+startY+yScaleAmount, (style == 3 ? 635 : 632), "Italic & Bold");

			
			if ( m_Level == 0 || m_Level == 1 ) // body & page only
			{
				int align = 0;
				if ( m_Tags[(int)TagType.Alignment] != null )
				{
					if ( m_Tags[(int)TagType.Alignment].Name == Tag.CenterAlign )
						align = 1;
				}

				this.AddLabel(22+startX, 174+startY+yScaleAmount, 635, "Alignment:");
				this.AddButton(98+startX, 175+startY+yScaleAmount, ( align == 0 ? 1895 : 1896 ), 1895, (int)Buttons.Left, GumpButtonType.Reply, 0);
				this.AddLabel(120+startX, 175+startY+yScaleAmount, (align == 0 ? 635 : 632), "Left");
				this.AddButton(158+startX, 175+startY+yScaleAmount, ( align == 1 ? 1895 : 1896 ), 1895, (int)Buttons.Center, GumpButtonType.Reply, 0);
				this.AddLabel(180+startX, 175+startY+yScaleAmount, (align == 1 ? 635 : 632), "Center");
			}

			if ( m_Level == 3 ) // word search
			{
				this.AddImage(88+startX, 93+startY+yScaleAmount, 2445);
				this.AddTextEntry(94+startX, 95+startY+yScaleAmount, 102, 16, 632, (int)Buttons.WordSearch, m_SearchWord);
				this.AddButton(202+startX, 97+startY+yScaleAmount, 2224, 2224, (int)Buttons.NextWord, GumpButtonType.Reply, 0);
				this.AddButton(63+startX, 97+startY+yScaleAmount, 2223, 2223, (int)Buttons.PreviousWord, GumpButtonType.Reply, 0);
			}
		}

		public enum Buttons
		{
			NextPage = 1,
			PreviousPage = 2,
			Body,
			Lines,
			Words,
			Page,

			HexColorEntry,
			None,
			Italic,
			Bold,
			ItalicBold,
			Left,
			Center,
			Small,
			Medium,
			Large,

			LeftPage,
			RightPage,

			WordSearch,
			NextWord,
			PreviousWord
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			if ( info.ButtonID < 0 )
				return;
			
			if ( m_Book.RootParent != from )
			{
				from.SendMessage( "The book must be on your person in order to edit it." );
				return;
			}

			if (info.TextEntries[0] != null)
			{
				string color = info.TextEntries[0].Text;
				if ( color.Length != 6 )
					from.SendMessage( "Only hex color entries allowed, range 111111 - FFFFFF" );
				else
				{
					string fixedColor = "";
					for( int i = 0; i < color.Length; i++ )
					{
						if (color[i] != '0')
							fixedColor += color[i];
						else
							fixedColor += '1';
					}

					m_Tags[(int)TagType.Color] = new ColorTag();
					m_Tags[(int)TagType.Color].Value = fixedColor;
				}
			}

			if ( m_Level == 3 && info.TextEntries[1] != null ) // word to search for
			{
				string word = info.TextEntries[1].Text;
				if ( word.IndexOf( ' ' ) != -1 )
					from.SendMessage( "You may only enter one word to search for." );
				else
					m_SearchWord = word;
			}

			if ( info.ButtonID == 0 ) // close
			{
				UpdateHTML();
				m_Book.HTMLContent.UpdateCache();
				return;
			}

			else if ( info.ButtonID >= 100 && info.ButtonID < 200 ) // line change
			{
				int newLine = info.ButtonID - 100;
				if ( !( newLine > ( m_Book.MaxLines * (m_Page + 1 < m_Book.FormattedBookContent.Length ? 2 : 1) - 1 ) ) ) // within limits. otherwise, redraw anyway
				{
					UpdateHTML();
					m_FocusedPage = ( newLine >= m_Book.MaxLines ? 1 : 0 );
					m_FocusedLine = newLine - m_FocusedPage*m_Book.MaxLines;
				}
			}
	
			else
			{
				switch ( info.ButtonID )
				{
					// HTML buttons
					case (int)Buttons.Small:
					{
						m_Tags[(int)TagType.Font] = new SmallFontTag();
						UpdateHTML();
						break;
					}

					case (int)Buttons.Large:
					{
						m_Tags[(int)TagType.Font] = new LargeFontTag();
						UpdateHTML();
						break;
					}

					case (int)Buttons.Medium:
					{
						m_Tags[(int)TagType.Font] = new MediumFontTag();
						UpdateHTML();
						break;
					}

					case (int)Buttons.None:
					{
						m_Tags[(int)TagType.Italic] = null;
						m_Tags[(int)TagType.Bold] = null;
						UpdateHTML();
						break;
					}

					case (int)Buttons.Italic:
					{
						m_Tags[(int)TagType.Italic] = new ItalicTag();
						m_Tags[(int)TagType.Bold] = null;
						UpdateHTML();
						break;
					}

					case (int)Buttons.Bold:
					{
						m_Tags[(int)TagType.Bold] = new BoldTag();
						m_Tags[(int)TagType.Italic] = null;
						UpdateHTML();
						break;
					}

					case (int)Buttons.ItalicBold:
					{
						m_Tags[(int)TagType.Bold] = new BoldTag();
						m_Tags[(int)TagType.Italic] = new ItalicTag();
						UpdateHTML();
						break;
					}

					case (int)Buttons.Left:
					{
						if ( m_Level == 0 || m_Level == 1 )
						{
							m_Tags[(int)TagType.Alignment] = new LeftAlignTag();
							UpdateHTML();
						}
						break;
					}

					case (int)Buttons.Center:
					{
						if ( m_Level == 0 || m_Level == 1 )
						{
							m_Tags[(int)TagType.Alignment] = new CenterAlignTag();
							UpdateHTML();
						}
						break;
					}

					// Other buttons

					case (int)Buttons.NextPage:
					{
						UpdateHTML();
						if ( m_Page + 2 >= m_Book.FormattedBookContent.Length )
							return;
						m_Page += 2;
						break;
					}
				
					case (int)Buttons.PreviousPage:
					{
						UpdateHTML();
						if ( m_Page - 2 < 0 )
							return;
						m_Page -= 2;
						break;
					}

					case (int)Buttons.LeftPage:
					{
						if ( m_Level == 1 )
						{
							UpdateHTML();
							m_FocusedPage = 0;
						}
						break;
					}

					case (int)Buttons.RightPage:
					{
						if ( m_Level == 1 && m_Page + 1 < m_Book.FormattedBookContent.Length )
						{
							UpdateHTML();
							m_FocusedPage = 1;
						}
						break;
					}

					case (int)Buttons.PreviousWord: // searches backwards
					{
						UpdateHTML();
						// current line is handled somewhat differently, as it takes into account the focusedword offset
						int wordNum = -1;
						int index = -1;
						while ( ++wordNum != m_FocusedWord )
						{
							if ( index+1 >= m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].Length )
							{
								index = -1;
								break;
							}

							index = m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].IndexOf( " ", index+1 );
							if ( index == -1 ) // stop processing, nothing to be found here
								break;
						}

						int matchIndex = -1;
						if ( index > 1 ) // we still need to search the rest
						{
							matchIndex = m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].LastIndexOf( m_SearchWord, index-1 );
							Console.WriteLine( matchIndex );
							if ( matchIndex != -1 ) // we found the word we're looking for
							{
								wordNum = 0;
								for ( int i = 0; i < m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].Length; i++ )
								{
									i = m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].IndexOf( " ", i );
									if ( i == -1 )
										break;
									else if ( i >= matchIndex )
										break;

									wordNum++;
								}
								m_FocusedWord = wordNum; // since this word is on the same line as the previously focused word, we needn't change anything else
							}	
						}
						if ( matchIndex == -1 ) // search the remaining lines on this page
						{
							for ( int lineNum = m_FocusedLine-1; lineNum >= 0; lineNum-- )
							{
								matchIndex = m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[lineNum].LastIndexOf( m_SearchWord, m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[lineNum].Length-1 );
								if ( matchIndex != -1 )
								{
									m_FocusedLine = lineNum;
									break;
								}
							}

							if ( matchIndex == -1 && m_FocusedPage == 1 ) // if we're not on the left page, search the left page as well
							{
								for ( int lineNum = m_Book.FormattedBookContent[m_Page].Lines.Length-1; lineNum >= 0; lineNum-- )
								{
									matchIndex = m_Book.FormattedBookContent[m_Page].Lines[lineNum].LastIndexOf( m_SearchWord, m_Book.FormattedBookContent[m_Page].Lines[lineNum].Length-1 );
									if ( matchIndex != -1 )
									{
										m_FocusedPage = 0;
										m_FocusedLine = lineNum;
										break;
									}
								}
							}

							if ( matchIndex != -1 ) // found the word, now calculate its (space) index
							{
								wordNum = 0;
								for ( int i = 0; i < m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].Length; i++ )
								{
									i = m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].IndexOf( " ", i );
									if ( i == -1 )
										break;
									else if ( i >= matchIndex )
										break;

									wordNum++;
								}
								m_FocusedWord = wordNum; // we've already set the line and page focus
							}
						}
						break;
					}

					case (int)Buttons.NextWord: // searches forward
					{
						UpdateHTML();
						// current line is handled somewhat differently, as it takes into account the focusedword offset
						int wordNum = -1;
						int index = -1;
						while ( ++wordNum != m_FocusedWord+1 )
						{
							if ( index+1 >= m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].Length )
							{
								index = -1;
								break;
							}

							index = m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].IndexOf( " ", index+1 );
							if ( index == -1 ) // stop processing, nothing to be found here
								break;
						}
						
						int matchIndex = -1;
						if ( index != -1 ) // we still need to search the rest
						{
							matchIndex = m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].IndexOf( m_SearchWord, index+1 );
							if ( matchIndex != -1 ) // we found the word we're looking for
							{
								// we can reuse the wordNum we've calculated thus far and add to it
								for ( int i = index+1; i < m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].Length; i++ )
								{
									i = m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].IndexOf( " ", i );
									if ( i == -1 )
										break;
									else if ( i >= matchIndex )
										break;

									wordNum++;
								}
								m_FocusedWord = wordNum; // since this word is on the same line as the previously focused word, we needn't change anything else
							}	
						}
						if ( matchIndex == -1 ) // search the remaining lines on this page
						{
							for ( int lineNum = m_FocusedLine+1; lineNum < m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines.Length; lineNum++ )
							{
								matchIndex = m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[lineNum].IndexOf( m_SearchWord, 0 );
								if ( matchIndex != -1 )
								{
									m_FocusedLine = lineNum;
									break;
								}
							}

							if ( matchIndex == -1 && m_FocusedPage == 0 && m_Page + 1 < m_Book.FormattedBookContent.Length ) // if we're not on the right page, search the right page as well
							{
								for ( int lineNum = 0; lineNum < m_Book.FormattedBookContent[1 + m_Page].Lines.Length; lineNum++ )
								{
									matchIndex = m_Book.FormattedBookContent[1 + m_Page].Lines[lineNum].IndexOf( m_SearchWord, 0 );
									if ( matchIndex != -1 )
									{
										m_FocusedPage = 1;
										m_FocusedLine = lineNum;
										break;
									}
								}
							}

							if ( matchIndex != -1 ) // found the word, now calculate its (space) index
							{
								wordNum = 0;
								for ( int i = 0; i < m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].Length; i++ )
								{
									i = m_Book.FormattedBookContent[m_FocusedPage + m_Page].Lines[m_FocusedLine].IndexOf( " ", i );
									if ( i == -1 )
										break;
									else if ( i >= matchIndex )
										break;

									wordNum++;
								}
								m_FocusedWord = wordNum; // we've already set the line and page focus
							}
						}
						break;
					}

					case (int)Buttons.Body:
					{
						UpdateHTML();
						m_Level = m_FocusedPage = m_FocusedLine = m_FocusedWord = 0;
						break;
					}

					case (int)Buttons.Page:
					{
						UpdateHTML();
						m_Level = 1;
						m_FocusedPage = m_FocusedLine = m_FocusedWord = 0;
						break;
					}

					case (int)Buttons.Lines:
					{
						UpdateHTML();
						m_Level = 2;
						m_FocusedPage = m_FocusedLine = m_FocusedWord = 0;
						break;
					}

					case (int)Buttons.Words:
					{
						UpdateHTML();
						m_Level = 3;
						m_FocusedPage = m_FocusedLine = m_FocusedWord = 0;
						break;
					}
				}
			}

			from.SendGump( new BookEditingGump( m_Book, m_Page, m_Level, m_FocusedPage, m_FocusedLine, m_FocusedWord, m_SearchWord ) );
		}

		private void UpdateHTML()
		{
			List<HTMLTag> newTags = new List<HTMLTag>();
			for ( int i=0; i < m_Tags.Length; i++ )
				if ( m_Tags[i] != null )
					newTags.Add(m_Tags[i]);

			switch ( m_Level )
			{
				case 0: // body
				{
					m_Book.HTMLContent.SetBodyHTML( newTags );
					break;
				}

				case 1: // page
				{
					m_Book.HTMLContent.SetPageHTML( newTags, m_Page + m_FocusedPage );
					break;
				}

				case 2: // lines
				{
					m_Book.HTMLContent.SetLineHTML( newTags, m_Page + m_FocusedPage, m_FocusedLine );
					break;
				}

				case 3: // words
				{
					m_Book.HTMLContent.SetWordHTML( newTags, m_Page + m_FocusedPage, m_FocusedLine, m_FocusedWord );
					break;
				}
			}
			
			m_Book.FixStyling();
		}

		// create the html page, as it is not yet updated in the HTMLContent cache
		private string ConstructHTMLPage( int page )
		{
			string ret = "";
			for ( int k = 0; k < m_Book.FormattedBookContent[page].Lines.Length; k++ )
			{
				if ( k == 0 )
					ret += m_Book.HTMLContent.OpenPageAlignmentTags( page );
				
				if ( !m_Book.HTMLContent.PageContainsHTML( page ) && !( m_Level == 3 && m_Page + m_FocusedPage == page && m_FocusedLine == k ) )
				{
					ret += m_Book.HTMLContent.OpenLineTags( page, k );
					if ( m_Level == 2 && m_Page + m_FocusedPage == page && m_FocusedLine == k ) // line level, underline current line
						ret += "<U>";
					
					string line = m_Book.FormattedBookContent[page].Lines[k];
					ret += line;
					if ( m_Level == 2 && m_Page + m_FocusedPage == page && m_FocusedLine == k )
						ret += "</U>";
					ret += m_Book.HTMLContent.CloseLineTags( page, k );
					
					if ( line.IndexOf( '<' ) == -1 ) // don't add line break if the user already supplied one
						ret += "<BR>";
				}
				else
				{
					string openLineTags = m_Book.HTMLContent.OpenLineTags( page, k );
					string closeLineTags = m_Book.HTMLContent.CloseLineTags( page, k );
					bool addLineBreak = false;

					string underLineOpen = "";
					string underLineClose = "";
					if ( m_Level == 2 && m_Page + m_FocusedPage == page && m_FocusedLine == k )
					{
						underLineOpen = "<U>";
						underLineClose = "</U>";
					}
					
					string line = m_Book.FormattedBookContent[page].Lines[k];
					if ( line.IndexOf( '<' ) == -1 )
						addLineBreak = true;

					if ( !m_Book.HTMLContent.LineContainsHTMLWords( page, k ) && !( m_FocusedLine == k && m_Page + m_FocusedPage == page && m_Level == 3 ) )
						ret += openLineTags + underLineOpen + line + underLineClose + closeLineTags;
					else // handle individual words
					{
						string[] pieces = m_Book.FormattedBookContent[page].Lines[k].Split( new char[] { ' ' } );
						for ( int j = 0; j < pieces.Length; j++ )
						{
							string opentags = m_Book.HTMLContent.OpenWordTags( page, k, j ) + underLineOpen;
							string closewordUL = "";
							if ( m_Level == 3 && m_Page + m_FocusedPage == page && m_FocusedLine == k && m_FocusedWord == j )
							{
								opentags += "<U>";
								closewordUL = "</U>";
							}

							ret += opentags + pieces[j] + closewordUL + underLineClose + m_Book.HTMLContent.CloseWordTags( page, k, j );
							if ( j != pieces.Length - 1 )
								ret += underLineOpen + " " + underLineClose;
						}
					}
					
					if ( addLineBreak )
						ret += "<BR>";
				}
			}

			return ret;
		}
	}
}
