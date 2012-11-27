using Server.Engines.Craft;
using Server.Network;

namespace Server.Gumps
{
	public class PoisoningPickBottleGump : Gump
	{
		private static int[] Bottles = { 3621, 3622, 3624, 3625, 3626, 3627, 3628, 3835, 3836, 3620, 6215, 6216 };
		private PoisoningCraftState m_CraftState;
		public PoisoningPickBottleGump( PoisoningCraftState craftstate ) : base(0, 0)
		{
			m_CraftState = craftstate;
			
			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(80, 33, 264, 246, 9200);
			
			int[] x = { 94, 102, 114 }; // background, bottle, button
			int[] y = { 43, 79, 96 };
			for ( int i = 0; i < Bottles.Length; i++ )
			{		
				this.AddBackground(x[0], y[0], 53, 57, 5120);
				
				this.AddItem( x[0], y[0], Bottles[i] );
				
				this.AddButton(x[2], y[2], 2117, 2118, i+10, GumpButtonType.Reply, 0);
				
				x[0]+=60; x[1]+=60; x[2]+=60;
				
				if ( ((i+1) % 4) == 0 ) // new line
				{
					x[0]=94; x[1]=102; x[2]=114;
					y[0]+=75; y[1]+=75; y[2]+=75;
				}
			}
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			if ( info.ButtonID == 0 ) // close
			{
				from.SendGump( new PoisoningGump( m_CraftState ) );
				return;
			}
			
			else if ( info.ButtonID >= 10 )
			{
				int i = info.ButtonID - 10;
				if ( i >= 0 && i < Bottles.Length )
				{
					m_CraftState.BottleID = Bottles[i];
					from.SendGump( new PoisoningGump( m_CraftState ) );
				}
			}
		}
	}
}
