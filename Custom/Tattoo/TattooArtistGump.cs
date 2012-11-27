using System;
using Server;
using Server.Gumps;
using Server.Network;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Prompts;

namespace Server.Gumps
{
	public class TattooArtistGump : Gump
	{
		private PlayerMobile m_Target;
		private PlayerMobile m_Artist;
		public static KeyValuePair<int, int>[] TattooArray = {
			// format is GUMP ID, ITEM ID
			new KeyValuePair<int, int>(50017, 11412),
			new KeyValuePair<int, int>(50018, 0x2C95),
			new KeyValuePair<int, int>(50019, 0x2C96),
			new KeyValuePair<int, int>(50020, 0x2C97)
		};
		
		public TattooArtistGump(PlayerMobile target, PlayerMobile artist) : base( 0, 0 )
		{
			m_Target = target;
			m_Artist = artist;
			
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			
			this.AddPage(0);
			
			this.AddBackground(81, 75, 298, 249, 2620);
			this.AddBackground(87, 84, 286, 231, 9350);
			this.AddLabel(185, 56, 1125, "Tattoo Artist");
			
			int y = 0;
			for (int i=0; i<TattooArray.Length; i++)
			{
				this.AddImage(36 + (i%5)*50, 40 + y*50, TattooArray[i].Key);
				this.AddButton(114 + (i%5)*50, 130 + y*50, 2094, 2095, 10+i, GumpButtonType.Reply, 0);
				if (i%5 == 4)
					y++;
			}
			
			this.AddLabel(88, 325, 337, "Remove current tattoo");
			this.AddButton(72, 330, 2103, 2104, (int)Buttons.Remove, GumpButtonType.Reply, 0);
			this.AddLabel(282, 324, 337, "Dye current tattoo");
			this.AddButton(267, 329, 2103, 2104, (int)Buttons.Dye, GumpButtonType.Reply, 0);

		}
		
		public enum Buttons
		{
			Remove = 1,
			Dye = 2
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			if ( info.ButtonID == 0 ) // close
				return;
			else
			{
				if (!m_Artist.InRange( m_Target.Location, 1 ))
				{
					m_Artist.SendMessage( "You are too far away." );
					return;
				}
				Tattoo tattoo = m_Target.FindItemOnLayer( Layer.Unused_xF ) as Tattoo;
				Item item = m_Target.FindItemOnLayer( Layer.Unused_xF );
				if ( info.ButtonID == (int)Buttons.Remove )
				{
					if ( tattoo != null )
					{
						tattoo.Delete();
						m_Artist.SendMessage( "You remove their tattoo." );
					}
					else
						m_Artist.SendMessage( "They don't have a tattoo!" );
				}
				else if ( info.ButtonID == (int)Buttons.Dye )
				{
					if ( tattoo != null )
					{
						if ( m_Artist.Feats.GetFeatLevel(FeatList.TattooArtist) > 1 )
							m_Artist.SendGump( new DyeTattooGump( m_Artist, 0, m_Target ) );
						else
							m_Artist.SendMessage( "You don't know how to do that!" );
					}
					else
						m_Artist.SendMessage( "They don't have a tattoo!" );
				}
				else
				{
					if ( tattoo == null && item == null )
					{
						int index = info.ButtonID - 10;
						if ( index < 0 || index >= TattooArray.Length )
							return;
						int newId = TattooArray[index].Value;
						Tattoo newTattoo = new Tattoo( newId );
						m_Target.EquipItem(newTattoo);
						newTattoo.Movable = false;
						from.SendMessage( "You apply a new tattoo." );
						if ( m_Artist.Feats.GetFeatLevel(FeatList.TattooArtist) > 2 )
						{
							from.SendMessage( "Enter the name for the tattoo: (ESC to cancel)" );
							from.Prompt = new TattooPrompt( newTattoo );
						}
					}
					else
						from.SendMessage( "They already have a tattoo! Remove it first." );
				}
			}
			
			return;
		}
		
		private class TattooPrompt : Prompt
		{
			private Tattoo m_Tattoo;
			public TattooPrompt( Tattoo tattoo )
			{
				m_Tattoo = tattoo;
			}
	
			public override void OnResponse( Mobile from, string text )
			{
				m_Tattoo.Name = text;
				from.SendMessage( "You set the name for the tattoo. You can also set the description by using .look on the tattoo." );
			}
		}
	}
}