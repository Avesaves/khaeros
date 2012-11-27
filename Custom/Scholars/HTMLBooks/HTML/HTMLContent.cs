using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Items
{
	public class HTMLContent
	{
		private BookPageInfo[] m_CachedHTMLContent;
		private HTMLBook m_Book;
		private List<HTMLTag> m_Body;
		private Dictionary<int, List<HTMLTag>> m_Pages = new Dictionary<int, List<HTMLTag>>();
		private Dictionary<int, List<HTMLTag>> m_Lines = new Dictionary<int, List<HTMLTag>>(); // see GetLineIndex
		private Dictionary<int, List<HTMLTag>> m_Words = new Dictionary<int, List<HTMLTag>>(); // see GetWordIndex

		private Dictionary<int, int> m_HTMLPage = new Dictionary<int, int>(); // optimization: does the page have HTML lines or HTML words? Value is how many lines and words
		private Dictionary<int, int> m_HTMLLines = new Dictionary<int, int>(); // optimization: does the line have HTML words? Value is how many words. index = GetLineIndex

		/*	at these settings, 49 is the max. amount of lines per page and 199 words per line.
			due to the size of the integer type, the maximum amount of pages is 200,000			*/
		public static int GetLineIndex( int pageIndex, int lineIndex ) { return 10000*pageIndex + lineIndex; }
		public static int GetWordIndex( int pageIndex, int lineIndex, int wordIndex ) { return 10000*pageIndex + 200*lineIndex + wordIndex; }

		public BookPageInfo[] CachedHTMLContent { get { return m_CachedHTMLContent; } }

		public List<HTMLTag> Body { set { m_Body = value; } get { return m_Body; } }
		
		public Dictionary<int, int> HTMLPage { get { return m_HTMLPage; } set { m_HTMLPage = value; } }
		public Dictionary<int, int> HTMLLines { get { return m_HTMLLines; } set { m_HTMLLines = value; } }

		public Dictionary<int, List<HTMLTag>> WordsTable { set { m_Words = value; } get { return m_Words; } }
		public Dictionary<int, List<HTMLTag>> LinesTable { set { m_Lines = value; } get { return m_Lines; } }
		public Dictionary<int, List<HTMLTag>> PagesTable { set { m_Pages = value; } get { return m_Pages; } }

		public int CalculateCharacterCost( int page, int line, int word )
		{
			//	Returns Character Cost for a specific word, taking into account all html levels
			int cost = 0;
			HTMLTag fontTag = new MediumFontTag();
			HTMLTag boldTag = null;
			HTMLTag italicTag = null;
			// We have to loop through each of these since we don't know where they might be overridden
			foreach ( HTMLTag tag in Body )
			{
				if ( tag.Type == TagType.Font )
					fontTag = tag;
				else if ( tag.Type == TagType.Bold )
					boldTag = tag;
				else if ( tag.Type == TagType.Italic )
					italicTag = tag;
			}

			foreach ( HTMLTag tag in GetPageTags( page ) )
			{
				if ( tag.Type == TagType.Font )
					fontTag = tag;
				else if ( tag.Type == TagType.Bold )
					boldTag = tag;
				else if ( tag.Type == TagType.Italic )
					italicTag = tag;
			}

			foreach ( HTMLTag tag in GetLineTags( page, line ) )
			{
				if ( tag.Type == TagType.Font )
					fontTag = tag;
				else if ( tag.Type == TagType.Bold )
					boldTag = tag;
				else if ( tag.Type == TagType.Italic )
					italicTag = tag;
			}

			foreach ( HTMLTag tag in GetWordTags( page, line, word ) )
			{
				if ( tag.Type == TagType.Font )
					fontTag = tag;
				else if ( tag.Type == TagType.Bold )
					boldTag = tag;
				else if ( tag.Type == TagType.Italic )
					italicTag = tag;
			}
			if ( fontTag != null )
				cost += fontTag.CharacterCost;
			if ( boldTag != null )
				cost += boldTag.CharacterCost;
			if ( italicTag != null )
				cost += italicTag.CharacterCost;

			return cost;
		}

		public List<HTMLTag> GetPageTags( int page )
		{
			if ( m_Pages.ContainsKey( page ) )
				return m_Pages[page];
			else
				return new List<HTMLTag>();
		}

		public List<HTMLTag> GetLineTags( int pageIndex, int lineIndex )
		{
			int indx = GetLineIndex( pageIndex, lineIndex );
			if ( m_Lines.ContainsKey( indx ) )
				return m_Lines[indx];
			else
				return new List<HTMLTag>();
		}

		public List<HTMLTag> GetWordTags( int pageIndex, int lineIndex, int wordIndex )
		{
			int indx = GetWordIndex( pageIndex, lineIndex, wordIndex );
			if ( m_Words.ContainsKey( indx ) )
				return m_Words[indx];
			else
				return new List<HTMLTag>();
		}

		public bool LineContainsHTMLWords( int pageIndex, int lineIndex )
		{
			int index = GetLineIndex( pageIndex, lineIndex );
			return m_HTMLLines.ContainsKey(index);
		}

		public bool PageContainsHTML( int pageIndex ) // either words or lines on that page have HTML elements
		{
			return m_HTMLPage.ContainsKey( pageIndex );
		}

		public HTMLContent( int pages, int maxLines, HTMLBook book )
		{
			m_Book = book;
			m_CachedHTMLContent = new BookPageInfo[pages];

			for ( int i = 0; i < m_CachedHTMLContent.Length; i++ )
			{
				m_CachedHTMLContent[i] = new BookPageInfo();
				m_CachedHTMLContent[i].Lines = new string[maxLines];
			}

			// set default body styling
			m_Body = new List<HTMLTag>();
			HTMLTag tag = new ColorTag();
			tag.Value = "111111";	// #000000 (black) is invisible
			m_Body.Add( tag );
			tag = new LeftAlignTag();
			m_Body.Add( tag );
			tag = new MediumFontTag();
			m_Body.Add( tag );
		}
			
		public void UpdateCache() // updates the HTML cache with the content from the parent book
		{
			m_CachedHTMLContent = new BookPageInfo[m_Book.FormattedBookContent.Length];
			for( int i = 0; i < m_Book.FormattedBookContent.Length; i++ )
			{
				m_CachedHTMLContent[i] = new BookPageInfo();
				m_CachedHTMLContent[i].Lines = new string[m_Book.FormattedBookContent[i].Lines.Length];
				for ( int k = 0; k < m_Book.FormattedBookContent[i].Lines.Length; k++ )
				{
					m_CachedHTMLContent[i].Lines[k] = "";
					
					if ( k == 0 )
						m_CachedHTMLContent[i].Lines[k] += OpenPageAlignmentTags( i );
					
					if ( !PageContainsHTML( i ) )
					{
						if ( k == 0 )
							m_CachedHTMLContent[i].Lines[k] += OpenPageTags( i );
						
						m_CachedHTMLContent[i].Lines[k] += m_Book.FormattedBookContent[i].Lines[k];
						if ( m_Book.FormattedBookContent[i].Lines[k].IndexOf( '<' ) == -1 )
							m_CachedHTMLContent[i].Lines[k] += "<BR>";
						
						if ( k == m_Book.FormattedBookContent[i].Lines.Length-1 )
							m_CachedHTMLContent[i].Lines[k] += ClosePageTags( i );
					}
					else
					{
						if ( !LineContainsHTMLWords( i, k ) )
							m_CachedHTMLContent[i].Lines[k] += OpenLineTags( i, k ) + m_Book.FormattedBookContent[i].Lines[k] + CloseLineTags( i, k );

						else // handle individual words
						{
							string[] pieces = m_Book.FormattedBookContent[i].Lines[k].Split( new char[] { ' ' } );
							for ( int j = 0; j < pieces.Length; j++ )
							{
								m_CachedHTMLContent[i].Lines[k] += OpenWordTags( i, k, j ) + pieces[j] + CloseWordTags( i, k, j );
								if ( j != pieces.Length - 1 )
									m_CachedHTMLContent[i].Lines[k] += " ";
							}
						}

						if ( m_Book.FormattedBookContent[i].Lines[k].IndexOf( '<' ) == -1 )
							m_CachedHTMLContent[i].Lines[k] += "<BR>";
					}
				}
			}
		}

		// HTML tags will only be set if they differ from its parent tags
		public void SetWordHTML( List<HTMLTag> tags, int pageIndex, int lineIndex, int wordIndex )
		{
			List<HTMLTag> tagsToRemove = new List<HTMLTag>();
			bool foundType;
			int lineHashIndex = GetLineIndex( pageIndex, lineIndex );
			int wordHashIndex = GetWordIndex( pageIndex, lineIndex, wordIndex );
			List<HTMLTag> lineTags = new List<HTMLTag>();
			if ( m_Lines.ContainsKey( lineHashIndex ) )
				lineTags = m_Lines[lineHashIndex];

			List<HTMLTag> pageTags = new List<HTMLTag>();
			if ( m_Pages.ContainsKey( pageIndex ) )
				pageTags = m_Pages[pageIndex];

			foreach ( HTMLTag newWordTag in tags )
			{
				foundType = false;

				foreach ( HTMLTag lineTag in lineTags )
				{
					if ( newWordTag.Type == lineTag.Type )
					{
						foundType = true;
						if ( HTMLTag.CompareTags( newWordTag, lineTag ) ) // remove tags that are already defined on a larger scope
						{
							tagsToRemove.Add( newWordTag );
							break;
						}
					}
				}

				if ( !foundType ) // the html tag type was not present in the immediate parent (i.e. was not overriden), so it might still be present higher up
				{
					foreach ( HTMLTag pageTag in pageTags )
					{
						if ( newWordTag.Type == pageTag.Type )
						{
							foundType = true;
							if ( HTMLTag.CompareTags( newWordTag, pageTag ) )
							{
								tagsToRemove.Add( newWordTag );
								break;
							}
						}
					}

					if ( !foundType ) // we STILL haven't found the type tag, so let's check the last parent
					{
						foreach ( HTMLTag bodyTag in m_Body )
						{
							if ( HTMLTag.CompareTags( newWordTag, bodyTag ) ) // found the same tag in body
							{
								tagsToRemove.Add( newWordTag );
								break;
							}
						}
					}
				}
			}

			foreach ( HTMLTag toRemove in tagsToRemove )
				tags.Remove( toRemove );

			if ( tags.Count > 0 )
			{
				if ( !m_Words.ContainsKey( wordHashIndex ) ) // we're adding a new tag
				{
					if ( m_HTMLPage.ContainsKey( pageIndex ) )
						m_HTMLPage[pageIndex]++;
					else
						m_HTMLPage[pageIndex] = 1;

					if ( m_HTMLLines.ContainsKey( lineHashIndex ) )
						m_HTMLLines[lineHashIndex]++;
					else
						m_HTMLLines[lineHashIndex] = 1;
				}

				m_Words[wordHashIndex] = tags;
			}

			else if ( m_Words.ContainsKey( wordHashIndex ) ) // we're removing a tag
			{
				if ( m_HTMLPage.ContainsKey( pageIndex ) )
					if (--m_HTMLPage[pageIndex] <= 0)
						m_HTMLPage.Remove( pageIndex ); // no more tags there anyway

				if ( m_HTMLLines.ContainsKey( lineHashIndex ) )
					if (--m_HTMLLines[lineHashIndex] <= 0)
						m_HTMLLines.Remove( lineHashIndex ); // no more tags there anyway

				m_Words.Remove( wordHashIndex );
			}
		}

		public void SetLineHTML( List<HTMLTag> tags, int pageIndex, int lineIndex )
		{
			List<HTMLTag> tagsToRemove = new List<HTMLTag>();
			bool foundType;
			List<HTMLTag> pageTags = new List<HTMLTag>();
			if ( m_Pages.ContainsKey( pageIndex ) )
				pageTags = m_Pages[pageIndex];

			foreach ( HTMLTag newLineTag in tags )
			{
				foundType = false;

				foreach ( HTMLTag pageTag in pageTags )
				{
					if ( newLineTag.Type == pageTag.Type )
					{
						foundType = true;
						if ( HTMLTag.CompareTags( newLineTag, pageTag ) ) // remove tags that are already defined on a larger scope
						{
							tagsToRemove.Add( newLineTag );
							break;
						}
					}
				}

				if ( !foundType ) // the html tag type was not present in the immediate parent (i.e. was not overriden), so it might still be present higher up
				{
					foreach ( HTMLTag bodyTag in m_Body )
					{
						if ( HTMLTag.CompareTags( newLineTag, bodyTag ) ) // found the same tag in body
						{
							tagsToRemove.Add( newLineTag );
							break;
						}
					}
				}
			}

			foreach ( HTMLTag toRemove in tagsToRemove )
				tags.Remove( toRemove );

			int index = GetLineIndex( pageIndex, lineIndex );
			if ( tags.Count > 0 )
			{
				if ( !m_Lines.ContainsKey( index ) ) // we're adding a new tag
				{
					if ( m_HTMLPage.ContainsKey( pageIndex ) )
						m_HTMLPage[pageIndex]++;
					else
						m_HTMLPage[pageIndex] = 1;
				}

				m_Lines[index] = tags;
			}

			else if ( m_Lines.ContainsKey( index ) ) // we're removing a tag
			{
				if ( m_HTMLPage.ContainsKey( pageIndex ) )
					if (--m_HTMLPage[pageIndex] <= 0)
						m_HTMLPage.Remove( pageIndex ); // no more tags there anyway

				m_Lines.Remove( index );
			}
		}

		public void SetPageHTML( List<HTMLTag> tags, int pageIndex )
		{
			List<HTMLTag> tagsToRemove = new List<HTMLTag>();
			foreach ( HTMLTag newPageTag in tags )
			{
				foreach ( HTMLTag bodyTag in m_Body )
				{
					if ( HTMLTag.CompareTags( newPageTag, bodyTag ) ) // remove tags that are already defined on a larger scope
					{
						tagsToRemove.Add( newPageTag );
						break;
					}
				}
			}

			foreach ( HTMLTag toRemove in tagsToRemove )
				tags.Remove( toRemove );

			m_Pages[pageIndex] = tags;
		}

		public void SetBodyHTML( List<HTMLTag> tags )
		{
			m_Body = tags;
		}

		public string OpenWordTags( int pageIndex, int lineIndex, int wordIndex )
		{
			HTMLTag [] tags = new HTMLTag[5];
			string ret = "";
			foreach ( HTMLTag tag in m_Body )
				tags[(int)tag.Type] = tag;
			if ( m_Pages.ContainsKey( pageIndex ) )
				foreach ( HTMLTag tag in m_Pages[pageIndex] )
					tags[(int)tag.Type] = tag;
				
			int index = GetLineIndex( pageIndex, lineIndex );
			if ( m_Lines.ContainsKey( index ) )
				foreach ( HTMLTag tag in m_Lines[index] )
					tags[(int)tag.Type] = tag;
			
			index = GetWordIndex( pageIndex, lineIndex, wordIndex );
			if ( m_Words.ContainsKey( index ) )
				foreach ( HTMLTag tag in m_Words[index] )
					tags[(int)tag.Type] = tag;
				
			string basefont = ""; // basefont must always come first
			for ( int i = 0; i < tags.Length; i++ )
			{
				if ( tags[i] != null )
				{
					if ( tags[i].Type == TagType.Color )
						basefont = tags[i].OpenTag();
					else if ( tags[i].Type != TagType.Alignment )
						ret += tags[i].OpenTag();
				}
			}

			return basefont + ret;
		}

		public string CloseWordTags( int pageIndex, int lineIndex, int wordIndex )
		{
			HTMLTag [] tags = new HTMLTag[5];
			string ret = "";
			foreach ( HTMLTag tag in m_Body )
				tags[(int)tag.Type] = tag;
			if ( m_Pages.ContainsKey( pageIndex ) )
				foreach ( HTMLTag tag in m_Pages[pageIndex] )
					tags[(int)tag.Type] = tag;
				
			int index = GetLineIndex( pageIndex, lineIndex );
			if ( m_Lines.ContainsKey( index ) )
				foreach ( HTMLTag tag in m_Lines[index] )
					tags[(int)tag.Type] = tag;
			
			index = GetWordIndex( pageIndex, lineIndex, wordIndex );
			if ( m_Words.ContainsKey( index ) )
				foreach ( HTMLTag tag in m_Words[index] )
					tags[(int)tag.Type] = tag;
				
			for ( int i = tags.Length-1; i >=0; i-- )
				if ( tags[i] != null && tags[i].Type != TagType.Alignment )
					ret += tags[i].CloseTag();

			return ret;
		}
			
		public string OpenLineTags( int pageIndex, int lineIndex )
		{
			HTMLTag [] tags = new HTMLTag[5];
			string ret = "";
			foreach ( HTMLTag tag in m_Body )
				tags[(int)tag.Type] = tag;
			if ( m_Pages.ContainsKey( pageIndex ) )
				foreach ( HTMLTag tag in m_Pages[pageIndex] )
					tags[(int)tag.Type] = tag;
				
			int index = GetLineIndex( pageIndex, lineIndex );
			if ( m_Lines.ContainsKey( index ) )
				foreach ( HTMLTag tag in m_Lines[index] )
					tags[(int)tag.Type] = tag;
				
			string basefont = ""; // basefont must always come first
			for ( int i = 0; i < tags.Length; i++ )
			{
				if ( tags[i] != null )
				{
					if ( tags[i].Type == TagType.Color )
						basefont = tags[i].OpenTag();
					else if ( tags[i].Type != TagType.Alignment )
						ret += tags[i].OpenTag();
				}
			}

			return basefont + ret;
		}

		public string CloseLineTags( int pageIndex, int lineIndex )
		{
			HTMLTag [] tags = new HTMLTag[5];
			string ret = "";
			foreach ( HTMLTag tag in m_Body )
				tags[(int)tag.Type] = tag;
			if ( m_Pages.ContainsKey( pageIndex ) )
				foreach ( HTMLTag tag in m_Pages[pageIndex] )
					tags[(int)tag.Type] = tag;
				
			int index = GetLineIndex( pageIndex, lineIndex );
			if ( m_Lines.ContainsKey( index ) )
				foreach ( HTMLTag tag in m_Lines[index] )
					tags[(int)tag.Type] = tag;
				
			for ( int i = tags.Length-1; i >=0; i-- )
				if ( tags[i] != null && tags[i].Type != TagType.Alignment )
					ret += tags[i].CloseTag();

			return ret;
		}
		public string OpenPageAlignmentTags( int pageIndex )
		{
			HTMLTag tmptag = null;
			foreach ( HTMLTag tag in m_Body )
				if ( tag.Type == TagType.Alignment )
					tmptag = tag;
				
			if ( m_Pages.ContainsKey( pageIndex ) )
				foreach ( HTMLTag tag in m_Pages[pageIndex] )
					if ( tag.Type == TagType.Alignment )
						tmptag = tag;
					
			return ( tmptag == null ? "" : tmptag.OpenTag() );
		}

		public string OpenPageTags( int pageIndex )
		{
			HTMLTag [] tags = new HTMLTag[5];
			string ret = "";
			foreach ( HTMLTag tag in m_Body )
				tags[(int)tag.Type] = tag;
			if ( m_Pages.ContainsKey( pageIndex ) )
				foreach ( HTMLTag tag in m_Pages[pageIndex] )
					tags[(int)tag.Type] = tag;
				
			string basefont = ""; // basefont must always come first
			for ( int i = 0; i < tags.Length; i++ )
			{
				if ( tags[i] != null )
				{
					if ( tags[i].Type == TagType.Color )
						basefont = tags[i].OpenTag();
					else if ( tags[i].Type != TagType.Alignment )
						ret += tags[i].OpenTag();
				}
			}

			return basefont + ret;
		}

		public string ClosePageTags( int pageIndex )
		{
			HTMLTag [] tags = new HTMLTag[5];
			string ret = "";
			foreach ( HTMLTag tag in m_Body )
				tags[(int)tag.Type] = tag;
			if ( m_Pages.ContainsKey( pageIndex ) )
				foreach ( HTMLTag tag in m_Pages[pageIndex] )
					tags[(int)tag.Type] = tag;
				
			for ( int i = tags.Length-1; i >=0; i-- )
				if ( tags[i] != null && tags[i].Type != TagType.Alignment )
					ret += tags[i].CloseTag();

			return ret;
		}

		public string OpenBodyTags()
		{
			string ret = "";
			foreach ( HTMLTag tag in m_Body )
				ret += tag.OpenTag();

			return ret;
		}

		public string CloseBodyTags()
		{
			string ret = "";
			for ( int i = m_Body.Count-1; i >= 0; i-- )
					ret += m_Body[i].CloseTag();

			return ret;
		}

		public void Deserialize( GenericReader reader )
		{
			int version = reader.ReadInt();

			List<HTMLTag> list;
			int c, v, pageIndex, lineIndex, wordIndex;

			/*
				BODY
			*/
			list = new List<HTMLTag>();
			c = reader.ReadInt();
			for ( int i = 0; i < c; i++ )
			{
				HTMLTag tag = HTMLTag.CreateTagInstance( (Tag)reader.ReadInt() );
				if ( tag != null )
				{
					tag.Value = reader.ReadString();
					list.Add( tag );
				}
				else
					reader.ReadString(); // can't instantiate tag? just skip it.
			}

			SetBodyHTML( list );

			/*
				PAGES
			*/
			c = reader.ReadInt();
			for ( int i = 0; i < c; i++ )
			{
				list = new List<HTMLTag>();
				pageIndex = reader.ReadInt();
				v = reader.ReadInt();
				for ( int j = 0; j < v; j++ )
				{
					HTMLTag tag = HTMLTag.CreateTagInstance( (Tag)reader.ReadInt() );
					if ( tag != null )
					{
						tag.Value = reader.ReadString();
						list.Add( tag );
					}
					else
						reader.ReadString(); // can't instantiate tag? just skip it.
				}

				SetPageHTML( list, pageIndex );
			}

			/*
				LINES
			*/
			c = reader.ReadInt();
			for ( int i = 0; i < c; i++ )
			{
				list = new List<HTMLTag>();
				pageIndex = reader.ReadInt();
				lineIndex = reader.ReadInt();
				v = reader.ReadInt();
				for ( int j = 0; j < v; j++ )
				{
					HTMLTag tag = HTMLTag.CreateTagInstance( (Tag)reader.ReadInt() );
					if ( tag != null )
					{
						tag.Value = reader.ReadString();
						list.Add( tag );
					}
					else
						reader.ReadString(); // can't instantiate tag? just skip it.
				}

				SetLineHTML( list, pageIndex, lineIndex );
			}

			/*
				WORDS
			*/
			c = reader.ReadInt();
			for ( int i = 0; i < c; i++ )
			{
				list = new List<HTMLTag>();
				wordIndex = reader.ReadInt();
				pageIndex = reader.ReadInt();
				lineIndex = reader.ReadInt();
				v = reader.ReadInt();
				for ( int j = 0; j < v; j++ )
				{
					HTMLTag tag = HTMLTag.CreateTagInstance( (Tag)reader.ReadInt() );
					if ( tag != null )
					{
						tag.Value = reader.ReadString();
						list.Add( tag );
					}
					else
						reader.ReadString(); // can't instantiate tag? just skip it.
				}

				SetWordHTML( list, pageIndex, lineIndex, wordIndex );
			}
		}

		public void Serialize( GenericWriter writer )
		{
			writer.Write( (int)0 ); // version

			int pageIndex, lineIndex, wordIndex;

			/*
				BODY
			*/
			writer.Write( (int)m_Body.Count );
			foreach ( HTMLTag tag in m_Body )
			{
				writer.Write( (int) tag.Name );
				writer.Write( (string) tag.Value );
			}

			/*
				PAGES
			*/
			writer.Write( (int)m_Pages.Count );
			foreach ( KeyValuePair<int, List<HTMLTag>> kvp in m_Pages )
			{
				writer.Write( (int) kvp.Key );
				writer.Write( (int) kvp.Value.Count );
				foreach ( HTMLTag tag in kvp.Value )
				{
					writer.Write( (int) tag.Name );
					writer.Write( (string) tag.Value );
				}
			}

			/*
				LINES
			*/
			writer.Write( (int)m_Lines.Count );
			foreach ( KeyValuePair<int, List<HTMLTag>> kvp in m_Lines )
			{
				pageIndex = (int)( kvp.Key / 10000 );
				lineIndex = kvp.Key % 10000;
				writer.Write( (int) pageIndex );
				writer.Write( (int) lineIndex );
				writer.Write( (int) kvp.Value.Count );
				foreach ( HTMLTag tag in kvp.Value )
				{
					writer.Write( (int) tag.Name );
					writer.Write( (string) tag.Value );
				}
			}

			/*
				WORDS
			*/
			writer.Write( (int)m_Words.Count );
			foreach ( KeyValuePair<int, List<HTMLTag>> kvp in m_Words )
			{
				pageIndex = (int)( kvp.Key / 10000 );
				lineIndex = kvp.Key % 10000;
				wordIndex = lineIndex % 200;
				lineIndex = (int)( lineIndex / 200 );
				writer.Write( (int) wordIndex );
				writer.Write( (int) pageIndex );
				writer.Write( (int) lineIndex );
				writer.Write( (int) kvp.Value.Count );
				foreach ( HTMLTag tag in kvp.Value )
				{
					writer.Write( (int) tag.Name );
					writer.Write( (string) tag.Value );
				}
			}
		}
	}
}
