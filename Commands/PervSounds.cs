using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Commands
{
	public class PervSoundsCommand
	{

		public static void Initialize()
		{
			CommandSystem.Register( "PervSounds", AccessLevel.Player, new CommandEventHandler( PervSounds_OnCommand ) );
		}

		[Usage( "PervSounds" )]
		[Description( "Plays various perverted sound effects." )]
		public static void PervSounds_OnCommand( CommandEventArgs e )
		{
			Mobile m = e.Mobile;
			m.SendGump( new PervSoundsGump( m ) );

		}

	}

	public class PervSoundsGump : Gump
	{
		private Mobile m_Owner;
		public Mobile Owner{ get{ return m_Owner; } set{ m_Owner = value; } }

		public PervSoundsGump(Mobile owner) : base( 10, 10 )
		{
			owner.CloseGump( typeof( PervSoundsGump ) );
			m_Owner = owner;

			Closable=true;
			Disposable=false;
			Dragable=true;
			Resizable=false;
			AddPage(0);
			AddBackground(222, 114, 115, 434, 9200);
			AddButton(231, 124, 2714, 2714, 1, GumpButtonType.Reply, 0);
			AddButton(231, 156, 2714, 2714, 2, GumpButtonType.Reply, 0);
			AddButton(231, 189, 2714, 2714, 3, GumpButtonType.Reply, 0);
			AddButton(231, 225, 2714, 2714, 4, GumpButtonType.Reply, 0);
			AddButton(231, 257, 2714, 2714, 5, GumpButtonType.Reply, 0);
			AddButton(231, 290, 2714, 2714, 6, GumpButtonType.Reply, 0);
			AddButton(231, 322, 2714, 2714, 7, GumpButtonType.Reply, 0);
			AddButton(231, 354, 2714, 2714, 8, GumpButtonType.Reply, 0);
			AddButton(231, 386, 2714, 2714, 9, GumpButtonType.Reply, 0);
			AddButton(231, 418, 2714, 2714, 10, GumpButtonType.Reply, 0);
			AddButton(231, 450, 2714, 2714, 11, GumpButtonType.Reply, 0);
			AddButton(231, 482, 2714, 2714, 12, GumpButtonType.Reply, 0);			
			AddButton(231, 514, 2714, 2714, 13, GumpButtonType.Reply, 0);
			AddLabel(258, 124, 55, @"Aaagnh!");
			AddLabel(258, 156, 55, @"Nnnghh!");
			AddLabel(258, 192, 55, @"Uungh!");
			AddLabel(258, 226, 55, @"Nnghfff!");
			AddLabel(258, 258, 55, @"Unnffh.");
			AddLabel(258, 290, 55, @"Uuuaahhh!!");
			AddLabel(258, 322, 55, @"Euaaaghhhhl!!");
			AddLabel(258, 354, 55, @"Feels good.");
			AddLabel(258, 386, 55, @"Eohhnnnghh!");
			AddLabel(258, 418, 55, @"Pleeease!");
			AddLabel(258, 450, 55, @"Halppmeee!");
			AddLabel(258, 482, 55, @"Ouehhhh...");
			AddLabel(258, 514, 55, @"Nnnghh.");			
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			int soundValue = 0;

			switch( info.ButtonID )
			{
				case 0:
					from.CloseGump( typeof( PervSoundsGump ) );
					return;
				case 1:
/* Aaagnh! */				if (from.Female)
						soundValue = 0x14B;
					else
						soundValue = 0x157;
					break;

				case 2:
/* Nnnghh! */				if (from.Female)
						soundValue = 0x14C;
					else
						soundValue = 0x155;
					break;

				case 3:
/* Uungh! */				if (from.Female)
						soundValue = 0x14F;
					else
						soundValue = 0x159;
					break;

				case 4:
/* Nnghfff! */				if (from.Female)
						soundValue = 0x14D;
					else
						soundValue = 0x156;
					break;

				case 5:
/* Unnffh. */				if (from.Female)
						soundValue = 0x14E;
					else
						soundValue = 0x158;
					break;

				case 6:
/* Uuuaahhh!! */			if (from.Female)
						soundValue = 0x151;
					else
						soundValue = 0x15A;
					break;

				case 7:
/* Euaaaghhhhl!! */			if (from.Female)
						soundValue = 0x150;
					else
						soundValue = 0x15D;
					break;

				case 8:
/* Feels good. */			if (from.Female)
						soundValue = 0x53B;
					else
						soundValue = 0x53E;
					break;

				case 9:
/* Eohhnnnghh! */			if (from.Female)
						soundValue = 0x53C;
					else
						soundValue = 0x53F;
					break;

				case 10:
/* Pleeease! */				if (from.Female)
						soundValue = 0x555;
					else
						soundValue = 0x55A;
					break;

				case 11:
/* Halppmeee!  */			if (from.Female)
						soundValue = 0x556;
					else
						soundValue = 0x55B;
					break;

				case 12:
/* Ouehhhh... */			if (from.Female)
						soundValue = 0x544;
					else
						soundValue = 0x54D;
					break;

				case 13:
/* Nnnghh. */				if (from.Female)
						soundValue = 0x546;
					else
						soundValue = 0x434;
					break;
			}

			if (soundValue != 0)
				from.PlaySound(soundValue);

			from.SendGump( new PervSoundsGump( from ) );
		}
	}
}
