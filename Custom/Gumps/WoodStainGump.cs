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
	public class WoodStainGump : Gump
	{
		private int[] hues = new int[9];
		private int page;
		
		public WoodStainGump( PlayerMobile m, int currentPage )
			: base( 0, 0 )
		{
			m.CloseGump( typeof( WoodStainGump ) );
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
										   
			this.AddLabel(186, 46, 2010, @"Wood Staining Menu");
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
		
		public static bool IsWoodenItem( object obj )
		{
			if( obj is ICanBeStained || obj is BaseRanged || obj is WoodenKiteShield ||
			    obj is WoodenShield || obj is ClericCrook || obj is DruidStaff || obj is ProphetDiviningRod ||
			    obj is QuarterStaff || obj is GnarledStaff || obj is SpikedClub ||
			    obj is Boomerang || obj is BlackStaff || obj is BoneShield ||
			    obj is BoiledLeatherShield || obj is LeatherShield )
			{
				return true;
			}
			
			return false;
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
					hues = new int[]{0,1454,1832,2594,2587,1890,1899,1862,-1,-1};
					break;
				}
				case 2:
				{
					hues = new int[]{0,2796,2744,2737,2816,2810,2713,2720,-1,-1};
					break;
				}
				case 3:
				{
					hues = new int[]{0,2932,2798,2985,2797,2581,2761,2992,-1,-1};
					break;
				}
				case 4:
				{
					switch( m.Nation )
					{
						case Nation.Southern:
						{
							hues = new int[]{2683,2708,2582,1436,2745,2741,2764,2989,2835,2754};
							break;
						}
						case Nation.Western:
						{
							hues = new int[]{1133,2683,2657,2935,2724,2740,2881,1345,2585,2820};
							break;
						}
						case Nation.Haluaroc:
						{
							hues = new int[]{2732,2757,2711,2585,2835,2754,-1,-1,-1,-1};
							break;
						}
						case Nation.Mhordul:
						{
							hues = new int[]{2588,2795,2939,2798,2600,2886,-1,-1,-1,-1};
							break;
						}
						case Nation.Tirebladd:
						{
							hues = new int[]{2881,1345,2585,2820,2764,2989,-1,-1,-1,-1};
							break;
						}
						case Nation.Northern:
						{
							hues = new int[]{2935,1636,2604,2599,2621,2986,2588,2795,2939,2798};
							break;
						}
					}
					
					if( m.Feats.GetFeatLevel(FeatList.RacialStaining) < 3 )
					{
						hues[4] = -1;
						hues[5] = -1;
					}
					
					if( m.Feats.GetFeatLevel(FeatList.RacialStaining) < 2 )
					{
						hues[2] = -1;
						hues[3] = -1;
					}
					
					if( m.Feats.GetFeatLevel(FeatList.RacialStaining) < 1 )
					{
						hues[0] = -1;
						hues[1] = -1;
					}
					break;
				}
			}
		}
		
		private class WoodStainTarget : Target
        {
			private int m_hue;
			
            public WoodStainTarget( PlayerMobile pm, int hue )
                : base( 8, false, TargetFlags.None )
            {
            	m_hue = hue;
                pm.SendMessage( 60, "Choose the wooden object you wish to stain." );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                PlayerMobile pm = m as PlayerMobile;
                
                if( IsWoodenItem( obj ) )
                {
                    ( (Item)obj ).Hue = m_hue;
                    m.PlaySound( 0x23E );
                }
                
                else
                	pm.SendMessage( 60, "You can only stain wooden objects." );
        	}
        }
		
		private void HandleChange( bool forward, PlayerMobile m )
		{
			if( page < 3 && forward && m.Feats.GetFeatLevel(FeatList.WoodStaining) >= (page + 1) )
				page++;
			
			else if( page > 0 && !forward )
				page--;
			
			else if( page == 3 && forward && m.Feats.GetFeatLevel(FeatList.RacialStaining) > 0 )
				page++;
			
			else if( page == 0 && !forward )
			{
				if( m.Feats.GetFeatLevel(FeatList.RacialStaining) > 0 )
					page = 4;
				
				else
					page = m.Feats.GetFeatLevel(FeatList.WoodStaining);
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
				m.Target = new WoodStainTarget( m, hues[info.ButtonID -1] );
			
			else
				HandleChange( info.ButtonID == 102, m );	

			m.SendGump( new WoodStainGump( m, page ) );
		}
	}
}
