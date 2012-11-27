using System;
using Server;
using Server.Network;
using Server.Engines.XmlSpawner2;
using Server.Commands;
using Server.Mobiles;

namespace Server.Gumps
{
	public class FloatingIconGump : Gump
	{
		private IconEntry m_Entry;
		public FloatingIconGump( IconEntry entry ) : base( entry.Location.X, entry.Location.Y )
		{
			m_Entry = entry;
			this.Closable = false;
			this.Disposable = false;
			this.Dragable = false;
			this.Resizable = false;
			this.AddPage( 0 );
			this.AddBackground( 0, 0, 64, 64, 9270 );
			this.AddButton( 10, 10, m_Entry.IconID, m_Entry.IconID, 1, GumpButtonType.Reply, 0 );
		}
	
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			PlayerMobile from = sender.Mobile as PlayerMobile;
			if ( info.ButtonID == 0 )
			{
				return;
			}
			else
			{
				foreach ( string cmd in m_Entry.Commands )
				{
					if ( !String.IsNullOrEmpty( cmd ) )
					{
						from.DoSpeech( cmd, new int[] {}, MessageType.Regular, from.SpeechHue );
						//CommandSystem.Handle( from, cmd );
					}
				}
				m_Entry.SpawnGump( from );
			}
		}
	}
}
