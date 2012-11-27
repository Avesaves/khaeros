using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Prompts;

namespace Server.Gumps
{
	public class PlayerStatueGump : Gump
	{
		private Mobile m_Mobile;
		private PlayerMadeStatue m_Statue;

		public PlayerStatueGump( Mobile from, PlayerMadeStatue statue ) : base( 20, 30 )
		{
			m_Mobile = from;
			m_Statue = statue;

			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(0, 0, 160, 250, 9200);
			this.AddLabel(20, 10, 1149, @"Character Statue");
			this.AddButton(10, 50, 2151, 2152, (int)Buttons.EngraveChange, GumpButtonType.Reply, 0);
			this.AddLabel(45, 55, 0, @"Change Engraving");
			
//			this.AddButton(10, 100, 2151, 2152, (int)Buttons.MaterialChange, GumpButtonType.Reply, 0);
//			this.AddLabel(45, 105, 0, @"Change Material");
//			this.AddButton(10, 150, 2151, 2152, (int)Buttons.DirectionChange, GumpButtonType.Reply, 0);
//			this.AddLabel(45, 155, 0, @"Change Direction");
//			this.AddButton(10, 200, 2151, 2152, (int)Buttons.PoseChange, GumpButtonType.Reply, 0);
//			this.AddLabel(45, 205, 0, @"Change Pose");
//			this.AddButton(10, 250, 2151, 2152, (int)Buttons.ReDeed, GumpButtonType.Reply, 0);
//			this.AddLabel(45, 255, 0, @"Re-Deed");
			
			this.AddButton(10, 100, 2151, 2152, (int)Buttons.DirectionChange, GumpButtonType.Reply, 0);
			this.AddLabel(45, 105, 0, @"Change Direction");
			this.AddButton(10, 150, 2151, 2152, (int)Buttons.PoseChange, GumpButtonType.Reply, 0);
			this.AddLabel(45, 155, 0, @"Change Pose");
			this.AddButton(10, 200, 2151, 2152, (int)Buttons.ReDeed, GumpButtonType.Reply, 0);
			this.AddLabel(45, 205, 0, @"Re-Deed");
		}
		
		public enum Buttons
		{
			Exit,
			EngraveChange,
//			MaterialChange,
			DirectionChange,
			PoseChange,
			ReDeed,
		}

		private class InternalPrompt : Prompt
		{
			private PlayerMadeStatue m_Statue;

			public InternalPrompt( PlayerMadeStatue statue )
			{
				m_Statue = statue;
			}

			public override void OnResponse( Mobile from, string text )
			{
				if ( m_Statue.Deleted )
					return;

				if ( from != m_Statue.Owner )
				{
					from.SendMessage("You do not own this statue.");
				}
				else
				{
					m_Statue.Engraving = Utility.FixHtml( text.Trim() );

					from.CloseGump( typeof( PlayerStatueGump ) );

					from.SendMessage( "You change the engraving on the statue." );
				}
			}

			public override void OnCancel( Mobile from )
			{
				from.SendMessage("You decide not to change the statues engraving.");
				from.CloseGump( typeof( PlayerStatueGump ) );
			}
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

					case (int)Buttons.EngraveChange:
					{
							from.SendMessage("Enter the engraving you wish to place on the statue:");
							from.Prompt = new InternalPrompt( m_Statue );
							break;
					}

//					case (int)Buttons.MaterialChange:
//					{
//							from.SendGump(new PlayerStatueMaterialGump( from, m_Statue ));
//							break;
//					}

					case (int)Buttons.DirectionChange:
					{
							from.SendGump(new PlayerStatueDirectionGump( from, m_Statue ));
							break;
					}

					case (int)Buttons.PoseChange:
					{
							from.SendGump(new PlayerStatuePoseGump( from, m_Statue ));
							break;
					}

					case (int)Buttons.ReDeed:
					{
							from.AddToBackpack(new PlayerMadeStatueDeed());
							m_Statue.Delete();
							break;
					}
				}
			}
	}
}
