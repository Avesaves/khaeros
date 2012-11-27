using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Gumps
{
	public class ConfirmDeleteStashGump : Gump
	{
		private Mobile m_From;
		private Stash m_Stash;

		public ConfirmDeleteStashGump( Mobile from, Stash stash ) : base( 50, 50 )
		{
			m_From = from;
			m_Stash = stash;

			m_From.CloseGump( typeof( ConfirmDeleteStashGump ) );

			AddPage( 0 );

			AddBackground( 0, 0, 270, 120, 5054 );
			AddBackground( 10, 10, 250, 100, 3000 );

			AddHtml( 20, 15, 230, 60, "Are you sure you want do delete your stash?", true, true );
			AddButton( 20, 80, 4005, 4007, 2, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 55, 80, 75, 20, 1011011, false, false ); // CONTINUE

			AddButton( 135, 80, 4005, 4007, 1, GumpButtonType.Reply, 0 );
			AddHtmlLocalized( 170, 80, 75, 20, 1011012, false, false ); // CANCEL
		}

		public override void OnResponse( Server.Network.NetState sender, RelayInfo info )
		{
			if ( info.ButtonID == 2 )
			{
				if ( m_Stash != null && !m_Stash.Deleted && m_Stash.Owner == m_From )
				{
					m_From.SendMessage( "You destroy the stash." );
            		m_Stash.Delete();
				}
			}
		}
	}
}
