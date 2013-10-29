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
	public class EnamelGump : Gump
	{
		private int[] hues = new int[9];
		private int page;
		
		public EnamelGump( PlayerMobile m, int currentPage )
			: base( 0, 0 )
		{
			m.CloseGump( typeof( EnamelGump ) );
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
										   
			this.AddLabel(181, 46, 2010, @"Armour Enameling Menu");
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
					hues = new int[]{0,2224,1899,1436,1890,1827,2407,1832,1609,1445};
					break;
				}
				case 2:
				{
					hues = new int[]{2932,2101,2588,2604,2737,2704,2796,2582,2835,2594};
					break;
				}
				case 3:
				{
					hues = new int[]{2878,2834,2992,2736,2657,2755,2980,2803,2820,2740};
					break;
				}
				case 4:
				{
					switch( m.Nation )
					{
						case Nation.Southern:
						{
							hues = new int[]{1454,2708,2683,2591,2745,2741,2751,2585,2751,2765};
							break;
						}
						case Nation.Western:
						{
							hues = new int[]{2935,2598,2711,2745,2605,2724,2864,2739,2753,2583};
							break;
						}
						case Nation.Haluaroc:
						{
							hues = new int[]{2720,2843,2711,2725,2751,2585,-1,-1,-1,-1};
							break;
						}
						case Nation.Mhordul:
						{
							hues = new int[]{2989,2739,2753,2583,2751,2795,-1,-1,-1,-1};
							break;
						}
						case Nation.Tirebladd:
						{
							hues = new int[]{2864,2739,2753,2583,2751,2765,-1,-1,-1,-1};
							break;
						}
						case Nation.Northern:
						{
							hues = new int[]{2869,2723,2599,2934,2621,2986,2404,2656,2757,2600};
							break;
						}
					}
					
					if( m.Feats.GetFeatLevel(FeatList.RacialEnameling) < 3 )
					{
						hues[4] = -1;
						hues[5] = -1;
					}
					
					if( m.Feats.GetFeatLevel(FeatList.RacialEnameling) < 2 )
					{
						hues[2] = -1;
						hues[3] = -1;
					}
					
					if( m.Feats.GetFeatLevel(FeatList.RacialEnameling) < 1 )
					{
						hues[0] = -1;
						hues[1] = -1;
					}
					break;
				}
			}
		}
		
		private class EnamelTarget : Target
        {
			private int m_hue;
			
            public EnamelTarget( PlayerMobile pm, int hue )
                : base( 8, false, TargetFlags.None )
            {
            	m_hue = hue;
                pm.SendMessage( 60, "Choose the piece of armour you wish to enamel." );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                PlayerMobile pm = m as PlayerMobile;
                
                if( (obj is BaseArmor && EnamelGump.IsMetal((BaseArmor)obj)) || ( obj is Item && ( (Item)obj ).ItemID == 4984 ) || ( obj is Horse && ( ( (Horse)obj ).IsPetFriend( m ) || ( (Horse)obj ).ControlMaster == m ) && ( (Horse)obj ).BodyValue == 284 ) )
                {
                	if( obj is Item )
                    	( (Item)obj ).Hue = m_hue;
                	
                	if( obj is Horse )
                		( (Horse)obj ).Hue = m_hue;
                	
                    m.PlaySound( 0x23E );
                    
                    return;
                }
                
                pm.SendMessage( 60, "You can only enamel metal armour pieces." );
        	}
        }
		
		public static bool IsMetal( BaseArmor ba )
		{
			if( ba is IBoneArmour )
				return false;
			
			if( ba.Resource == CraftResource.Copper || ba.Resource == CraftResource.Bronze || ba.Resource == CraftResource.Iron ||
			   ba.Resource == CraftResource.Silver || ba.Resource == CraftResource.Gold || ba.Resource == CraftResource.Steel ||
			   ba.Resource == CraftResource.Obsidian || ba.Resource == CraftResource.Starmetal || ba.Resource == CraftResource.Electrum )
				return true;
			
			return false;
		}
		
		private void HandleChange( bool forward, PlayerMobile m )
		{
			if( page < 3 && forward && m.Feats.GetFeatLevel(FeatList.ArmourEnameling) >= (page + 1) )
				page++;
			
			else if( page > 0 && !forward )
				page--;
			
			else if( page == 3 && forward && m.Feats.GetFeatLevel(FeatList.RacialEnameling) > 0 )
				page++;
			
			else if( page == 0 && !forward )
			{
				if( m.Feats.GetFeatLevel(FeatList.RacialEnameling) > 0 )
					page = 4;
				
				else
					page = m.Feats.GetFeatLevel(FeatList.ArmourEnameling);
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
				m.Target = new EnamelTarget( m, hues[info.ButtonID -1] );
			
			else
				HandleChange( info.ButtonID == 102, m );	

			m.SendGump( new EnamelGump( m, page ) );
		}
	}
}
