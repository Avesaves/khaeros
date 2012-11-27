using System;
using Server;
using Server.Network;
using Server.Items;

namespace Server.Gumps
{
	public class StackConfirmGump : Gump
	{
		private Item m_Item1;
		private Item m_Item2;
		
		public StackConfirmGump(string result, Item item1, Item item2) : base( 0, 0 )
		{
			m_Item1 = item1;
			m_Item2 = item2;
			
			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			
			this.AddPage(0);
			
			this.AddBackground(103, 27, 178, 208, 5120);
			this.AddBackground(110, 31, 163, 187, 3500);
			this.AddHtml( 124, 62, 136, 110, "Combining these two items will result in the following item:<BR><BR><CENTER>" + result, (bool)false, (bool)false);
			this.AddLabel(159, 38, 38, "WARNING");
			
			this.AddButton(120, 218, 249, 248, (int)Buttons.Accept, GumpButtonType.Reply, 0);
			this.AddButton(204, 218, 243, 241, (int)Buttons.Cancel, GumpButtonType.Reply, 0);

		}
		
		public enum Buttons
		{
			Cancel = 0,
			Accept
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			if ( info.ButtonID == 0 ) // close
				return;
			if ( m_Item1.RootParent != from || m_Item2.RootParent != from )
			{
				from.SendMessage( "The items you wish to combine must be in your backpack." );
				return;
			}
			
			if ( m_Item1 is IDynamicStackable ) 
			{
				if ( m_Item1.Amount + m_Item2.Amount > 60000 )
					from.SendMessage( "You cannot make a single stack of more than 60000 items." );
				else
					(m_Item1 as IDynamicStackable).AcceptedStack( from, m_Item2 );
				return;
			}
			else
			{
				from.SendMessage( "Error, one of the items is not a dynamic stackable item" );
				return;
			}
		}
	}
}