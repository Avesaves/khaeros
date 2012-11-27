using System;
using Server.Network;
using Server.Prompts;

namespace Server.Engines.Help
{
	public class PagePrompt : Prompt
	{
		private PageType m_Type;

		public PagePrompt( PageType type )
		{
			m_Type = type;
		}

		public override void OnCancel( Mobile from )
		{
			from.SendLocalizedMessage( 501235, "", 0x35 ); // Help request aborted.
		}

		public override void OnResponse( Mobile from, string text )
		{
			from.SendMessage( 0x35, "The next available Overseer will assist you as soon as possible." );

			PageQueue.Enqueue( new PageEntry( from, text, m_Type ) );
		}
	}
}
