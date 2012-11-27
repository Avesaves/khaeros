using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Gumps
{
	public class PlayerStatueDirectionGump : Gump
	{
		private PlayerMadeStatue m_Statue;
		private Mobile m_Mobile;

		public PlayerStatueDirectionGump( Mobile from, PlayerMadeStatue statue ) : base( 20, 30 )
		{
			m_Statue = statue;
			m_Mobile = from;

			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(0, 0, 170, 240, 9200);
			this.AddLabel(30, 10, 1149, @"Change Direction");
			this.AddButton(60, 45, 4500, 4500, (int)Buttons.Up, GumpButtonType.Reply, 0);
			this.AddButton(60, 160, 4504, 4504, (int)Buttons.Down, GumpButtonType.Reply, 0);
			this.AddButton(100, 60, 4501, 4501, (int)Buttons.North, GumpButtonType.Reply, 0);
			this.AddButton(110, 100, 4502, 4502, (int)Buttons.Right, GumpButtonType.Reply, 0);
			this.AddButton(100, 140, 4503, 4503, (int)Buttons.East, GumpButtonType.Reply, 0);
			this.AddButton(20, 140, 4505, 4505, (int)Buttons.South, GumpButtonType.Reply, 0);
			this.AddButton(10, 100, 4506, 4506, (int)Buttons.Left, GumpButtonType.Reply, 0);
			this.AddButton(20, 60, 4507, 4507, (int)Buttons.West, GumpButtonType.Reply, 0);

		}
		
		public enum Buttons
		{
			Exit,
			Up,
			Down,
			North,
			Right,
			East,
			South,
			Left,
			West,
		}

			public override void OnResponse( NetState state, RelayInfo info )
			{
				Mobile from = state.Mobile;

				switch( info.ButtonID )
				{
					case (int)Buttons.Exit:
					{
							break;
					}

					case (int)Buttons.Up:
					{
							m_Statue.Direction = Direction.Up;
							break;
					}

					case (int)Buttons.Down:
					{
							m_Statue.Direction = Direction.Down;
							break;
					}

					case (int)Buttons.Left:
					{
							m_Statue.Direction = Direction.Left;
							break;
					}

					case (int)Buttons.Right:
					{
							m_Statue.Direction = Direction.Right;
							break;
					}

					case (int)Buttons.North:
					{
							m_Statue.Direction = Direction.North;
							break;
					}

					case (int)Buttons.South:
					{
							m_Statue.Direction = Direction.South;
							break;
					}

					case (int)Buttons.East:
					{
							m_Statue.Direction = Direction.East;
							break;
					}

					case (int)Buttons.West:
					{
							m_Statue.Direction = Direction.West;
							break;
					}
				}
			}
	}
}
