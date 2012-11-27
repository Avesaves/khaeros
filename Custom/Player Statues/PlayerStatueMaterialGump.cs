using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Gumps
{
	public class PlayerStatueMaterialGump : Gump
	{
		private PlayerMadeStatue m_Statue;
		private Mobile m_Mobile;

		public PlayerStatueMaterialGump( Mobile from, PlayerMadeStatue statue ) : base( 20, 30 )
		{
			m_Mobile = from;
			m_Statue = statue;

			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(0, 0, 240, 400, 9200);
			this.AddLabel(60, 10, 1149, @"Change Material");

			this.AddButton(20, 45, 1209, 1210, (int)Buttons.JadeX1, GumpButtonType.Reply, 0);
			this.AddLabel(40, 40, 0, @"Jade 1");
			this.AddButton(20, 115, 1209, 1210, (int)Buttons.Jade1, GumpButtonType.Reply, 0);
			this.AddLabel(40, 145, 0, @"Jade 4");
			this.AddButton(20, 185, 1209, 1210, (int)Buttons.Jade3, GumpButtonType.Reply, 0);
			this.AddLabel(40, 180, 0, @"Jade 5");
			this.AddButton(20, 80, 1209, 1210, (int)Buttons.Jade0, GumpButtonType.Reply, 0);
			this.AddLabel(40, 75, 0, @"Jade 2");
			this.AddButton(20, 150, 1209, 1210, (int)Buttons.Jade2, GumpButtonType.Reply, 0);
			this.AddLabel(40, 110, 0, @"Jade 3");

			this.AddButton(20, 255, 1209, 1210, (int)Buttons.BronzeX2, GumpButtonType.Reply, 0);
			this.AddLabel(40, 250, 0, @"Bronze 2");
			this.AddButton(20, 325, 1209, 1210, (int)Buttons.Bronze1, GumpButtonType.Reply, 0);
			this.AddLabel(40, 320, 0, @"Bronze 4");
			this.AddButton(20, 220, 1209, 1210, (int)Buttons.BronzeX1, GumpButtonType.Reply, 0);
			this.AddLabel(40, 215, 0, @"Bronze 1");
			this.AddButton(20, 290, 1209, 1210, (int)Buttons.Bronze0, GumpButtonType.Reply, 0);
			this.AddLabel(40, 285, 0, @"Bronze 3");
			this.AddButton(20, 360, 1209, 1210, (int)Buttons.Bronze2, GumpButtonType.Reply, 0);
			this.AddLabel(40, 355, 0, @"Bronze 5");
			this.AddButton(120, 45, 1209, 1210, (int)Buttons.Bronze3, GumpButtonType.Reply, 0);
			this.AddLabel(140, 40, 0, @"Bronze 6");

			this.AddButton(120, 80, 1209, 1210, (int)Buttons.AlabasterX1, GumpButtonType.Reply, 0);
			this.AddLabel(140, 75, 0, @"Alabaster 1");
			this.AddButton(120, 115, 1209, 1210, (int)Buttons.AlabasterMarbleX0, GumpButtonType.Reply, 0);
			this.AddLabel(140, 110, 0, @"Alabaster 2");
			this.AddButton(120, 150, 1209, 1210, (int)Buttons.AlabasterMarble1, GumpButtonType.Reply, 0);
			this.AddLabel(140, 145, 0, @"Alabaster 3");
			this.AddButton(120, 185, 1209, 1210, (int)Buttons.AlabasterMarble2, GumpButtonType.Reply, 0);
			this.AddLabel(140, 180, 0, @"Alabaster 4");
			this.AddButton(120, 220, 1209, 1210, (int)Buttons.AlabasterMarble3, GumpButtonType.Reply, 0);
			this.AddLabel(140, 215, 0, @"Alabaster 5");

			this.AddButton(120, 255, 1209, 1210, (int)Buttons.MarbleX1, GumpButtonType.Reply, 0);
			this.AddLabel(140, 250, 0, @"Marble 1");
			this.AddButton(120, 290, 1209, 1210, (int)Buttons.MarbleX2, GumpButtonType.Reply, 0);
			this.AddLabel(140, 285, 0, @"Marble 2");

			this.AddButton(120, 325, 1209, 1210, (int)Buttons.Granite1, GumpButtonType.Reply, 0);
			this.AddLabel(140, 320, 0, @"Granite 1");
			this.AddButton(120, 360, 1209, 1210, (int)Buttons.BloodStoneX1, GumpButtonType.Reply, 0);
			this.AddLabel(140, 355, 0, @"Granite 2");
		}
		
		public enum Buttons
		{
			Exit,
			JadeX1,
			Jade1,
			Jade3,
			BronzeX2,
			Bronze1,
			Jade0,
			Jade2,
			BronzeX1,
			Bronze0,
			Bronze2,
			Bronze3,
			AlabasterX1,
			AlabasterMarbleX0,
			AlabasterMarble1,
			AlabasterMarble2,
			AlabasterMarble3,
			MarbleX1,
			MarbleX2,
			Granite1,
			BloodStoneX1,
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

					case (int)Buttons.JadeX1:
					{
						m_Statue.Material = StatueMaterial.JadeX1;
						break;
					}

					case (int)Buttons.Jade0:
					{
						m_Statue.Material = StatueMaterial.Jade0;
						break;
					}

					case (int)Buttons.Jade1:
					{
						m_Statue.Material = StatueMaterial.Jade1;
						break;
					}

					case (int)Buttons.Jade2:
					{
						m_Statue.Material = StatueMaterial.Jade2;
						break;
					}

					case (int)Buttons.Jade3:
					{
						m_Statue.Material = StatueMaterial.Jade3;
						break;
					}

					case (int)Buttons.BronzeX1:
					{
						m_Statue.Material = StatueMaterial.BronzeX1;
						break;
					}

					case (int)Buttons.BronzeX2:
					{
						m_Statue.Material = StatueMaterial.BronzeX2;
						break;
					}

					case (int)Buttons.Bronze0:
					{
						m_Statue.Material = StatueMaterial.Bronze0;
						break;
					}

					case (int)Buttons.Bronze1:
					{
						m_Statue.Material = StatueMaterial.Bronze1;
						break;
					}

					case (int)Buttons.Bronze2:
					{
						m_Statue.Material = StatueMaterial.Bronze2;
						break;
					}

					case (int)Buttons.Bronze3:
					{
						m_Statue.Material = StatueMaterial.Bronze3;
						break;
					}

					case (int)Buttons.AlabasterX1:
					{
						m_Statue.Material = StatueMaterial.AlabasterX1;
						break;
					}

					case (int)Buttons.AlabasterMarbleX0:
					{
						m_Statue.Material = StatueMaterial.AlabasterMarbleX0;
						break;
					}

					case (int)Buttons.AlabasterMarble1:
					{
						m_Statue.Material = StatueMaterial.AlabasterMarble1;
						break;
					}

					case (int)Buttons.AlabasterMarble2:
					{
						m_Statue.Material = StatueMaterial.AlabasterMarble2;
						break;
					}

					case (int)Buttons.AlabasterMarble3:
					{
						m_Statue.Material = StatueMaterial.AlabasterMarble3;
						break;
					}

					case (int)Buttons.MarbleX1:
					{
						m_Statue.Material = StatueMaterial.MarbleX1;
						break;
					}

					case (int)Buttons.MarbleX2:
					{
						m_Statue.Material = StatueMaterial.MarbleX2;
						break;
					}

					case (int)Buttons.Granite1:
					{
						m_Statue.Material = StatueMaterial.Granite1;
						break;
					}

					case (int)Buttons.BloodStoneX1:
					{
						m_Statue.Material = StatueMaterial.BloodStoneX1;
						break;
					}
				}
			}
	}
}
