using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
    public class CustomSpellBookGump : Gump
    {
    	private PlayerMobile m = null;
    	private CustomSpellBook book = null;
    	private int index;
    	
        public CustomSpellBookGump( PlayerMobile from, CustomSpellBook spellbook, int position ) : base( 0, 0 )
        {
        	if( from != null )
        	{
	        	from.CloseGump( typeof( CustomSpellBookGump ) );
	        	m = from;
        	}
        	
        	if( position < 0 )
        		position = 0;
        	
        	index = position;
        	
            this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);
			AddImage(192, 182, 2220);
			
			if( spellbook != null )
			{
				book = spellbook;
				
				if( book.Spells.Count > index )
				{
					if( index > 0 )
						AddButton(242, 190, 2205, 2205, 1, GumpButtonType.Reply, 0);
					
					AddLabel(252, 235, 2010, book.Spells[index].CustomName);
					AddLabel(280, 289, 2983, @"Check Stats");
					AddLabel(280, 314, 2983, @"Copy to Scroll");
					AddLabel(280, 339, 2983, @"Remove Spell");
					AddButton(334, 228, book.Spells[index].IconID, book.Spells[index].IconID, 3, GumpButtonType.Reply, 0);
					AddButton(260, 292, 30008, 30009, 4, GumpButtonType.Reply, 0);
					AddButton(260, 317, 30008, 30009, 5, GumpButtonType.Reply, 0);
					AddButton(260, 342, 30008, 30009, 6, GumpButtonType.Reply, 0);
				}
				
				if( book.Spells.Count > (index +1) )
				{
					if( book.Spells.Count > (index +2) )
						AddButton(513, 190, 2206, 2206, 2, GumpButtonType.Reply, 0);
					
					AddLabel(409, 235, 2010, book.Spells[index + 1].CustomName);
					AddLabel(437, 289, 2983, @"Check Stats");
					AddLabel(437, 314, 2983, @"Copy to Scroll");
					AddLabel(437, 339, 2983, @"Remove Spell");
					AddButton(491, 228, book.Spells[index + 1].IconID, book.Spells[index + 1].IconID, 7, GumpButtonType.Reply, 0);
					AddButton(417, 292, 30008, 30009, 8, GumpButtonType.Reply, 0);
					AddButton(417, 317, 30008, 30009, 9, GumpButtonType.Reply, 0);
					AddButton(417, 342, 30008, 30009, 10, GumpButtonType.Reply, 0);
				}
			}
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if( from == null || m == null || book == null || from != m )
            	return;
            
            if( !book.IsChildOf(from.Backpack) )
            	return;
            
            if( info.ButtonID == 1 && index > 0 )
            	index -= 2;
            
            if( info.ButtonID == 2 && book.Spells.Count > (index + 2) )
            	index += 2;
            
            if( info.ButtonID > 2 && info.ButtonID < 7 && book.Spells.Count > index )
            {
            	if( info.ButtonID == 3 )
            		CustomSpellScroll.CastCustomMageSpell( m, book.Spells[index] );
            	
            	if( info.ButtonID == 4 )
            		CustomSpellScroll.GetCustomMageSpellStats( m, book.Spells[index] );
            	
            	if( info.ButtonID == 5 )
            		m.Target = new CustomSpellScrollCopyTarget( m, book.Spells[index] );
            	
            	if( info.ButtonID == 6 )
            	{
            		m.SendMessage( "You have removed " + book.Spells[index].CustomName + " from your spell book." );
            		book.Spells.RemoveAt( index );
            		index = 0;
            	}
            }
            
            if( info.ButtonID > 6 && info.ButtonID < 11 && book.Spells.Count > (index + 1) )
            {
            	if( info.ButtonID == 7 )
            		CustomSpellScroll.CastCustomMageSpell( m, book.Spells[index + 1] );
            	
            	if( info.ButtonID == 8 )
            		CustomSpellScroll.GetCustomMageSpellStats( m, book.Spells[index + 1] );
            	
            	if( info.ButtonID == 9 )
            		m.Target = new CustomSpellScrollCopyTarget( m, book.Spells[index + 1] );
            	
            	if( info.ButtonID == 10 )
            	{
            		m.SendMessage( "You have removed " + book.Spells[index + 1].CustomName + " from your spell book." );
            		book.Spells.RemoveAt( (index + 1) );
            		index = 0;
            	}
            }
            
            if( info.ButtonID > 0 && book.Spells.Count > 0 )
            	m.SendGump( new CustomSpellBookGump( m, book, index ) );
        }
    }
}
