using System;
using System.Text;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.FeatInfo;

namespace Server.Items
{
	public enum Language
	{
		Common = 1,
		Northern,
		Mhordul,
		Western,
		Haluaroc,
		Tirebladd,
		Southern
	}

	public abstract class HTMLBook : BaseBook
	{
		public virtual int CharactersPerLineMax { get { return 25; } } // small font, no styles
		public virtual int MaxLines { get { return 8; } }
		public virtual Type Gump { get { return null; } }

        public string Seal { get { return m_Seal; } set { m_Seal = value; } }


		

		private HTMLContent m_HTMLContent;
		private Mobile m_SealedBy;
		private BookPageInfo[] m_FormattedBookContent;
		private bool m_RequiresFormatting; // html books can display less text per page and thus need to be formatted appropriately
		private string m_Cypher = "";

        private string m_Seal;
	
		private Language m_Language = Language.Common;
	
		public Mobile SealedBy { get { return m_SealedBy; } set { m_SealedBy = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public Language Language
		{
			get { return m_Language; }
			set { m_Language = value; InvalidateProperties(); }
		}
	
		[CommandProperty( AccessLevel.GameMaster )]
		public string Cypher
		{
			get { return m_Cypher; }
			set { m_Cypher = value; }
		}

		public bool RequiresFormatting { get { return m_RequiresFormatting; } set { m_RequiresFormatting = value; } }
		public HTMLContent HTMLContent { get { return m_HTMLContent; } set { m_HTMLContent = value; } }
		public BookPageInfo[] FormattedBookContent { get { return m_FormattedBookContent; } }

		[Constructable]
		public HTMLBook( int itemID ) : this( itemID, 20 )
		{
		}

		[Constructable]
		public HTMLBook( int itemID, int pageCount ) : base( itemID, pageCount, true )
		{
			m_HTMLContent = new HTMLContent( pageCount, MaxLines, this );
		}

		public HTMLBook( Serial serial ) : base( serial )
		{
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			base.GetProperties( list );

			if ( !String.IsNullOrEmpty( Author ) )
				list.Add( 1060659, "Author\t{0}", Author ); // ~1_val~: ~2_val~
				if (m_Language == Language.Haluaroc)
					list.Add( 1060658, "{0}\t{1}", "Language", "Ancient Script" ); // ~1_val~: ~2_val~
				else
				list.Add( 1060658, "{0}\t{1}", "Language", m_Language.ToString() ); // ~1_val~: ~2_val~
            if (!String.IsNullOrEmpty(m_Seal))
                list.Add(m_Seal);
		}

		public override void OnDoubleClick( Mobile from )
		{
			PlayerMobile m = from as PlayerMobile; 
			if ( String.IsNullOrEmpty( Title ) )
			{
				if ( !String.IsNullOrEmpty( Name ) )
					Title = Name;
				else
					Title = "a book";
			}
			
			if ( m_RequiresFormatting )
			{
				FixContent();
				HTMLContent.UpdateCache();
				m_RequiresFormatting = false;
			}

			if ( RootParent == from )
			{
				if ( Writable == false && m_SealedBy != null && m_SealedBy != from ) // not sealed by the same person, only allow viewing
				{
					if ( UnderstandsLanguage( m_Language, from ) && m.Feats.GetFeatLevel(FeatList.Linguistics) > 0 )
					{
						if ( Cypher.Length > 0 ) // encrypted
							from.SendGump( new EnterCypherGump( this ) );
						else
							SpawnGump( from );
					}
					else
						from.SendMessage( "It's all gibberish, as you don't know how to read the language." );
				}
				else
				{
					from.CloseGump( typeof( OpenBookGump ) );
					from.SendGump( new OpenBookGump( this ) ); // can also edit if its in his pack
				}
			}
			else if ( RootParent == null || !( RootParent is Mobile ) )
			{
				if ( from.InRange( GetWorldLocation(), 2 ) && from.InLOS( this ) )
				{
					if ( UnderstandsLanguage( m_Language, from ) && m.Feats.GetFeatLevel(FeatList.Linguistics) > 0 )
					{
						if ( Cypher.Length > 0 ) // encrypted
							from.SendGump( new EnterCypherGump( this ) );
						else
							SpawnGump( from );
					}
					else
						from.SendMessage( "It's all gibberish, as you don't know the language." );
				}
				else
					from.SendMessage( "You are too far away." );
			}
			else
				from.SendMessage( "That's not accessible." );
		}
		
		public virtual void SpawnGump( Mobile viewer )
		{
			viewer.SendGump( (Gump)Activator.CreateInstance(Gump, this) );
		}

		public virtual void FixContent()
		{
			string content = "";
			StringBuilder sb;
			for( int i = 0; i < Pages.Length; i++ )
			{
				for ( int k = 0; k < Pages[i].Lines.Length; k++ )
				{
					if ( Pages[i].Lines[k].Length < 1 )
						continue;

					sb = null;
					// make sure html tags are surrounded by spaces, but don't remove illegal html tags here, we'll do that later
					if ( Pages[i].Lines[k].IndexOf( '<' ) != -1 )
					{
						sb = new StringBuilder(Pages[i].Lines[k]);
						for ( int q = 1; q < sb.Length; q++ )
						{
							if ( sb[q] == '<' && sb[q-1] != ' ' )
								sb.Insert( q, ' ' );
							else if ( sb[q-1] == '>' && sb[q] != ' ' )
								sb.Insert( q, ' ' );
						}
					}
					if ( sb != null )
						content += sb.ToString();
					else
						content += Pages[i].Lines[k];
					
					if ( content[content.Length-1] != ' ' )
						content += ' ';
				}
			}

			string[] pieces = content.Split( new char[] { ' ' } );
			int page = 0;
			int j = 0;
			m_FormattedBookContent = new BookPageInfo[PagesCount];
			for ( int i = 0; i < m_FormattedBookContent.Length; i++ )
				m_FormattedBookContent[i] = new BookPageInfo();

			m_FormattedBookContent[page].Lines = new string[MaxLines];
			for ( int i = 0; i < MaxLines; i++ )
				m_FormattedBookContent[page].Lines[i] = "";

			bool skipLine = false;
			int lineFillFactor = 0;
			int textCost = new MediumFontTag().CharacterCost;
			
			foreach( string word in pieces )
			{
				if ( word.IndexOf( '<' ) != -1 )
				{
					if ( String.Compare( word, "<BR>", true ) != 0 ) // ignore case
						continue;	// do not want
					else
					{
						m_FormattedBookContent[page].Lines[j] += word;
						skipLine = true;	// avoids text screwups
					}
				}
					
				if ( lineFillFactor + word.Length + 1 + textCost > CharactersPerLineMax  || skipLine ) // count the space towards limit
				{
					j++;
					lineFillFactor = 0;
					if ( j >= MaxLines )
					{
						j = 0;
						page++;

						if ( page >= m_FormattedBookContent.Length )
						{
							// no more space in book
							return;
						}

						m_FormattedBookContent[page].Lines = new string[MaxLines];
						for ( int i = 0; i < MaxLines; i++ )
							m_FormattedBookContent[page].Lines[i] = "";
					}
				}
				
				if ( !skipLine )
				{
					m_FormattedBookContent[page].Lines[j] += word;
					m_FormattedBookContent[page].Lines[j] += ' ';
					lineFillFactor += 1 + textCost + word.Length;
				}
				else
					skipLine = false;
			}
		}

		public virtual void FixStyling()
		{
			// Recalculates the word positions due to style changes (font size, etc)
			// Reconstructs the content and then discards the old one
			int newPage = 0;
			int newLine = 0;
			int newLineFillFactor = 0;
			int newLineWordCount = 0;
			BookPageInfo[] newPages = new BookPageInfo[PagesCount];
			for ( int i = 0; i < newPages.Length; i++ )
				newPages[i] = new BookPageInfo();

			newPages[newPage].Lines = new string[MaxLines];
			for ( int i = 0; i < MaxLines; i++ )
				newPages[newPage].Lines[i] = "";

			// the new word hashtable with rearranged word positions
			Dictionary<int, List<HTMLTag>> newWordsTable = new Dictionary<int, List<HTMLTag>>();
			int CharacterCost;
			string[] splitWords;
			bool skipLine = false;

			if ( m_FormattedBookContent == null )
				m_FormattedBookContent = new BookPageInfo[0];
			for( int i = 0; i < m_FormattedBookContent.Length; i++ )
			{
				for ( int k = 0; k < m_FormattedBookContent[i].Lines.Length; k++ )
				{
					if ( m_FormattedBookContent[i].Lines[k].Length < 1 )
						continue;

					splitWords = m_FormattedBookContent[i].Lines[k].Split( new char[] { ' ' } );

					for( int z = 0; z < splitWords.Length; z++ )
					{
						if ( splitWords[z].Length < 1 )
							continue;
						else if ( splitWords[z].IndexOf( '<' ) != -1 ) // this can only be a <BR>
						{
							skipLine = true;
						
							newPages[newPage].Lines[newLine] += splitWords[z];
							
							List<HTMLTag> temp = m_HTMLContent.GetWordTags( i, k, z );

							if ( temp.Count > 0 ) 
							{
								newWordsTable[HTMLContent.GetWordIndex( newPage, newLine, newLineWordCount )] = temp;
								int newlh = HTMLContent.GetLineIndex( newPage, newLine );

								if ( HTMLContent.HTMLPage.ContainsKey( newPage ) ) // add tag
									HTMLContent.HTMLPage[newPage]++;
								else
									HTMLContent.HTMLPage[newPage] = 1;

								if ( HTMLContent.HTMLLines.ContainsKey( newlh ) )
									HTMLContent.HTMLLines[newlh]++;
								else
									HTMLContent.HTMLLines[newlh] = 1;
								
								if ( HTMLContent.HTMLPage.ContainsKey( i ) )
									if (--HTMLContent.HTMLPage[i] <= 0)
										HTMLContent.HTMLPage.Remove( i ); // no more tags there anyway

								int lh = HTMLContent.GetLineIndex( i, k );
								if ( HTMLContent.HTMLLines.ContainsKey( lh ) )
									if (--HTMLContent.HTMLLines[lh] <= 0)
										HTMLContent.HTMLLines.Remove( lh ); // no more tags there anyway
							}

							newLineWordCount++;
						}
						
						CharacterCost = m_HTMLContent.CalculateCharacterCost( i, k, z );
						if ( newLineFillFactor + splitWords[z].Length + CharacterCost + 1 > CharactersPerLineMax || skipLine ) // count the space towards limit
						{
							newLine++;
							newLineFillFactor = 0;
							newLineWordCount = 0;
							if ( newLine >= MaxLines )
							{
								newLine = 0;
								newPage++;

								if ( newPage >= newPages.Length )
								{
									// no more space in book
									return;
								}

								newPages[newPage].Lines = new string[MaxLines];
								for ( int g = 0; g < MaxLines; g++ )
									newPages[newPage].Lines[g] = "";
							}
						}

						if ( !skipLine )
						{
							newPages[newPage].Lines[newLine] += splitWords[z];
							
							List<HTMLTag> oldWordTags = m_HTMLContent.GetWordTags( i, k, z );

							if ( oldWordTags.Count > 0 )
							{
								int newlinehash = HTMLContent.GetLineIndex( newPage, newLine );
								newWordsTable[HTMLContent.GetWordIndex( newPage, newLine, newLineWordCount )] = oldWordTags;
								if ( HTMLContent.HTMLPage.ContainsKey( newPage ) ) // add tag
									HTMLContent.HTMLPage[newPage]++;
								else
									HTMLContent.HTMLPage[newPage] = 1;

								if ( HTMLContent.HTMLLines.ContainsKey( newlinehash ) )
									HTMLContent.HTMLLines[newlinehash]++;
								else
									HTMLContent.HTMLLines[newlinehash] = 1;
								
								if ( HTMLContent.HTMLPage.ContainsKey( i ) )
									if (--HTMLContent.HTMLPage[i] <= 0)
										HTMLContent.HTMLPage.Remove( i ); // no more tags there anyway

								int linehash = HTMLContent.GetLineIndex( i, k );
								if ( HTMLContent.HTMLLines.ContainsKey( linehash ) )
									if (--HTMLContent.HTMLLines[linehash] <= 0)
										HTMLContent.HTMLLines.Remove( linehash ); // no more tags there anyway
							}

							newLineWordCount++;
							newLineFillFactor += splitWords[z].Length + CharacterCost;

							newPages[newPage].Lines[newLine] += ' ';
							newLineFillFactor++; // for the space
						}
						else
							skipLine = false;
					}
				}
			}

			m_FormattedBookContent = newPages;
			m_HTMLContent.WordsTable = newWordsTable;
		}
		
		public static KnownLanguage LanguageToKnownLanguage( Language language )
		{
			KnownLanguage kl = KnownLanguage.Common;
			
			if( language == Language.Southern )
				kl = KnownLanguage.Southern;
			else if( language == Language.Western )
				kl = KnownLanguage.Western;
			else if( language == Language.Haluaroc )
				kl = KnownLanguage.Haluaroc;
			else if( language == Language.Mhordul )
				kl = KnownLanguage.Mhordul;
			else if( language == Language.Tirebladd )
				kl = KnownLanguage.Tirebladd;
			else if( language == Language.Northern )
				kl = KnownLanguage.Northern;
			
			return kl;
		}
		
		public static bool UnderstandsLanguage( Language language, Mobile mobile )
		{
			PlayerMobile m = mobile as PlayerMobile; 
			if( mobile == null || mobile.Deleted || !( mobile is PlayerMobile ) )
				return false;
			if (m.Feats.GetFeatLevel(FeatList.Linguistics) == 0 )
				return false;
			return PlayerMobile.KnowsLanguage( ((PlayerMobile)mobile), LanguageToKnownLanguage(language) );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			switch ( version )
			{
                case 2:
                {
                    m_Seal = reader.ReadString();
                    goto case 1;
                }
				case 1:
				{
					m_SealedBy = reader.ReadMobile();
					goto case 0;
				}
				
				case 0:
				{
					m_Cypher = reader.ReadString();
			
					m_Language = (Language)reader.ReadInt();
					
					FixContent();
					
					m_HTMLContent = new HTMLContent( PagesCount, MaxLines, this );
					m_HTMLContent.Deserialize( reader );
					
					FixStyling();
					
					m_HTMLContent.UpdateCache();
					break;
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)2 ); // version

            writer.Write( m_Seal );

			writer.Write( m_SealedBy );
				
			writer.Write( m_Cypher );
			
			writer.Write( (int)m_Language );

			if ( RequiresFormatting )
			{
				FixContent();
				RequiresFormatting = false;
			}

			if ( m_HTMLContent != null )
				m_HTMLContent.Serialize( writer );
		}
	}
}
