using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Mobiles;

namespace Server.Gumps
{
    public class ChosenDeityGump : Gump
    {
        public static void Initialize()
        {
            CommandSystem.Register( "Ambition", AccessLevel.Player, new CommandEventHandler(ChosenDeity_OnCommand) );
        }

        [Usage( "Ambition" )]
        [Description( "Makes a call to your custom gump." )]
        public static void ChosenDeity_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.ChosenDeity != ChosenDeity.None )
            {
            	string deity = m.ChosenDeity.ToString();
            	
            	if( m.ChosenDeity == ChosenDeity.Mahtet )
            		deity = "Survival";
            	
            	m.SendMessage( "You have already chosen this as your central ambition." );
            }
            
            else
            {
            	if ( m.HasGump(typeof(ChosenDeityGump)) )
                	m.CloseGump( typeof(ChosenDeityGump) );
            	
            	m.SendGump( new ChosenDeityGump(m) );
        	}
        }

        public ChosenDeityGump( PlayerMobile from ) : base( 0, 0 )
        {
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			from.CloseGump( typeof(ChosenDeityGump) );
			AddPage( 0 );
			AddBackground( 42, 23, 202, 331, 9270 );
			AddBackground( 58, 39, 169, 298, 3500 );
			AddLabel( 101, 51, 2010, @"Main Ambition" );
			AddButton( 72, 81, (from.ChosenDeity == ChosenDeity.Arianthynt ? 9723 : 9720), 9722, (int)Buttons.Arianthynt, GumpButtonType.Reply, 0 );
			AddButton( 72, 116, (from.ChosenDeity == ChosenDeity.Xipotec ? 9723 : 9720), 9722, (int)Buttons.Xipotec, GumpButtonType.Reply, 0 );
			AddButton( 72, 151, (from.ChosenDeity == ChosenDeity.Mahtet ? 9723 : 9720), 9722, (int)Buttons.Mahtet, GumpButtonType.Reply, 0 );
			AddButton( 72, 186, (from.ChosenDeity == ChosenDeity.Xorgoth ? 9723 : 9720), 9722, (int)Buttons.Xorgoth, GumpButtonType.Reply, 0 );
			AddButton( 72, 221, (from.ChosenDeity == ChosenDeity.Ohlm ? 9723 : 9720), 9722, (int)Buttons.Ohlm, GumpButtonType.Reply, 0 );
			AddButton( 72, 256, (from.ChosenDeity == ChosenDeity.Elysia ? 9723 : 9720), 9722, (int)Buttons.Elysia, GumpButtonType.Reply, 0 );
			AddButton( 72, 291, (from.ChosenDeity == ChosenDeity.None ? 9723 : 9720), 9722, (int)Buttons.None, GumpButtonType.Reply, 0 );
			AddLabel( 107, 86, 0, @"Knowledge" );
			AddLabel( 107, 121, 0, @"Wealth" );
			AddLabel( 107, 156, 0, @"Survival" );
			AddLabel( 107, 191, 0, @"Power" );
			AddLabel( 107, 226, 0, @"Honor" );
			AddLabel( 107, 261, 0, @"Love" );
			AddLabel( 107, 296, 0, @"None for now" );
        }

        public enum Buttons
		{
        	Close,
			Arianthynt,
			Xipotec,
			Mahtet,
			Xorgoth,
			Ohlm,
			Elysia,
			None,
		}


        public override void OnResponse( NetState sender, RelayInfo info )
        {
        	if( sender.Mobile == null || sender.Mobile.Deleted || !(sender.Mobile is PlayerMobile) )
        		return;
        	
            PlayerMobile from = sender.Mobile as PlayerMobile;

            switch(info.ButtonID)
            {
                case (int)Buttons.Arianthynt:
				{
					from.ChosenDeity = ChosenDeity.Arianthynt;
					break;
				}
				case (int)Buttons.Xipotec:
				{
					from.ChosenDeity = ChosenDeity.Xipotec;
					break;
				}
				case (int)Buttons.Mahtet:
				{
					from.ChosenDeity = ChosenDeity.Mahtet;
					break;
				}
				case (int)Buttons.Xorgoth:
				{
					from.ChosenDeity = ChosenDeity.Xorgoth;
					break;
				}
				case (int)Buttons.Ohlm:
				{
					from.ChosenDeity = ChosenDeity.Ohlm;
					break;
				}
				case (int)Buttons.Elysia:
				{
					from.ChosenDeity = ChosenDeity.Elysia;
					break;
				}
				case (int)Buttons.None:
				{
					from.ChosenDeity = ChosenDeity.None;
					break;
				}
            }
            
            if( info.ButtonID > 0 )
            	from.SendGump( new ChosenDeityGump(from) );
        }
    }
}
