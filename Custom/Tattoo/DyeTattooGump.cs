using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;
using Server.Targeting;

namespace Server.Gumps
{
	public class DyeTattooGump : Gump
	{
		private int[] hues = new int[9];
		private int page;
		private PlayerMobile m_Target;
		
		public DyeTattooGump( PlayerMobile m, int currentPage, PlayerMobile target )
			: base( 0, 0 )
		{
			m_Target = target;
			m.CloseGump( typeof( DyeTattooGump ) );
			page = currentPage;
			
			if( page == 0 )
				page = 1;
			
			SetHues( page, m );
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(54, 31, 400, 383, 9270);
			this.AddBackground(71, 192, 364, 202, 3500);
            this.AddImage(183, 50, 29);
			this.AddImage(4, 10, 10440);
			this.AddImage(423, 10, 10441);
			
			this.AddButton(396, 130, 4005, 4007, 102, GumpButtonType.Reply, 0);
            this.AddButton(359, 130, 4014, 4016, 101, GumpButtonType.Reply, 0);
										   
			this.AddLabel(181, 46, 2010, "Tattoo Dying Menu");
			this.AddButton(404, 46, 1150, 1152, 0, GumpButtonType.Reply, 0);
			
			if( hues[0] >= 0 )
			{
				this.AddButton(111, 253, 1209, 1210, 1, GumpButtonType.Reply, 0);
				this.AddImage(134, 245, 255, Math.Max( 0, hues[0] -1 ) );
			}
			
			if( hues[1] >= 0 )
			{
				this.AddButton(111, 311, 1209, 1210, 2, GumpButtonType.Reply, 0);
				this.AddImage(134, 303, 255, Math.Max( 0, hues[1] -1 ) );
			}
				
			if( hues[2] >= 0 )
			{
				this.AddButton(171, 253, 1209, 1210, 3, GumpButtonType.Reply, 0);
				this.AddImage(194, 245, 255, Math.Max( 0, hues[2] -1 ) );
			}
				
			if( hues[3] >= 0 )
			{
				this.AddButton(171, 311, 1209, 1210, 4, GumpButtonType.Reply, 0);
				this.AddImage(194, 303, 255, Math.Max( 0, hues[3] -1 ) );
			}
	
			if( hues[4] >= 0 )
			{
				this.AddButton(231, 253, 1209, 1210, 5, GumpButtonType.Reply, 0);
				this.AddImage(254, 245, 255, Math.Max( 0, hues[4] -1 ) );
			}
				
			if( hues[5] >= 0 )
			{
				this.AddButton(231, 311, 1209, 1210, 6, GumpButtonType.Reply, 0);
				this.AddImage(254, 303, 255, Math.Max( 0, hues[5] -1 ) );
			}
					
			if( hues[6] >= 0 )
			{
				this.AddButton(291, 253, 1209, 1210, 7, GumpButtonType.Reply, 0);
				this.AddImage(314, 245, 255, Math.Max( 0, hues[6] -1 ) );
			}
				
			if( hues[7] >= 0 )
			{
				this.AddButton(291, 311, 1209, 1210, 8, GumpButtonType.Reply, 0);
				this.AddImage(314, 303, 255, Math.Max( 0, hues[7] -1 ) );
			}
				
			if( hues[8] >= 0 )
			{
				this.AddButton(351, 253, 1209, 1210, 9, GumpButtonType.Reply, 0);
				this.AddImage(374, 245, 255, Math.Max( 0, hues[8] -1 ) );
			}
				
			if( hues[9] >= 0 )
			{
				this.AddButton(351, 311, 1209, 1210, 10, GumpButtonType.Reply, 0);
				this.AddImage(374, 303, 255, Math.Max( 0, hues[9] -1 ) );
			}
		
		}
		
		private void SetHues( int page, PlayerMobile m )
		{
			switch( page )
			{
				case 0:
				{
					hues = new int[]{-1,-1,-1,-1,-1,-1,-1,-1,-1,-1};
					break;
				}
				case 1:
				{
					hues = new int[]{0,2984,2993,2974,2973,2972,2971,2970,2969,2968};
					break;
				}
				case 2:
				{
					hues = new int[]{2967,2958,2952,2948,2946,2943,2942,2930,2927,2904};
					break;
				}
				case 3:
				{
					hues = new int[]{2900,2876,2712,2693,2690,2681,2678,2653,2642,2613};
					break;
				}
				case 4:
				{
					hues = new int[]{1162,37,217,45,134,576,-1,-1,-1,-1};
					break;
				}
			}
		}
		
		private void HandleChange( bool forward, PlayerMobile m )
		{
			if( page < 3 && forward && 3 >= (page + 1) )
				page++;
			
			else if( page > 0 && !forward )
				page--;
			
			else if( page == 3 && forward )
				page++;
			
			else if( page == 0 && !forward )
			{
				page = 4;
			}
			
			else
				page = 0;
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			PlayerMobile m = sender.Mobile as PlayerMobile;
			
			if ( m == null || info.ButtonID == 0 )
				return;

			if( info.ButtonID < 11 )
			{
				Tattoo tattoo = m_Target.FindItemOnLayer( Layer.Unused_xF ) as Tattoo;
				if ( tattoo != null )
				{
					if (!m.InRange( m_Target.Location, 1 ))
					{
						m.SendMessage( "You are too far away." );
						return;
					}
					tattoo.Hue = hues[info.ButtonID -1];
					tattoo.InvalidateProperties();
				}
			}
			
			else
				HandleChange( info.ButtonID == 102, m );	

			m.SendGump( new DyeTattooGump( m, page, m_Target ) );
		}
	}
}
