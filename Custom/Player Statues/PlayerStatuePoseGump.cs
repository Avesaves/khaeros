using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Gumps
{
	public class PlayerStatuePoseGump : Gump
	{
		private PlayerMadeStatue m_Statue;
		private Mobile m_Mobile;

		public PlayerStatuePoseGump( Mobile from, PlayerMadeStatue statue) : base( 20, 30 )
		{
			m_Statue = statue;
			m_Mobile = from;

			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(0, 0, 150, 250, 9200);
			this.AddLabel(35, 10, 1149, @"Change Pose");
			this.AddButton(15, 50, 2117, 2118, (int)Buttons.Ready, GumpButtonType.Reply, 0);
			this.AddLabel(45, 50, 0, @"Ready");
			this.AddButton(15, 80, 2117, 2118, (int)Buttons.Fighting, GumpButtonType.Reply, 0);
			this.AddLabel(45, 80, 0, @"Fighting");
			this.AddButton(15, 110, 2117, 2118, (int)Buttons.AllPraise, GumpButtonType.Reply, 0);
			this.AddLabel(45, 110, 0, @"All Praise Me");
			this.AddButton(15, 140, 2117, 2118, (int)Buttons.Casting, GumpButtonType.Reply, 0);
			this.AddLabel(45, 140, 0, @"Casting");
			this.AddButton(15, 170, 2117, 2118, (int)Buttons.Hips, GumpButtonType.Reply, 0);
			this.AddLabel(45, 170, 0, @"Hands On Hips");
			this.AddButton(15, 200, 2117, 2118, (int)Buttons.Salute, GumpButtonType.Reply, 0);
			this.AddLabel(45, 200, 0, @"Salute");

		}
		
		public enum Buttons
		{
			Exit,
			Ready,
			Fighting,
			AllPraise,
			Casting,
			Hips,
			Salute,
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

					case (int)Buttons.Ready:
					{
							m_Statue.Pose = StatuePoses.Ready;
							break;
					}

					case (int)Buttons.Fighting:
					{
							m_Statue.Pose = StatuePoses.Fighting;
							break;
					}

					case (int)Buttons.AllPraise:
					{
							m_Statue.Pose = StatuePoses.AllPraiseMe;
							break;
					}

					case (int)Buttons.Casting:
					{
							m_Statue.Pose = StatuePoses.Casting;
							break;
					}

					case (int)Buttons.Hips:
					{
							m_Statue.Pose = StatuePoses.HandsOnHips;
							break;
					}

					case (int)Buttons.Salute:
					{
							m_Statue.Pose = StatuePoses.Salute;
							break;
					}
				}
			}
	}
}
