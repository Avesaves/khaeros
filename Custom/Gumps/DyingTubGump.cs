using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;
using Server.Targeting;
using Server.Multis;

namespace Server.Gumps
{
	public class DyingTubGump : Gump
	{
        public static int[] SpecialHues = new int[] { 2995, 2799, 2643, 2830, 2734, 2725, 2582, 2829, 2831, 2802 };
		private int[] hues = new int[9];
		private int page;
		
		public DyingTubGump( PlayerMobile m, int currentPage )
			: base( 0, 0 )
		{
			m.CloseGump( typeof( DyingTubGump ) );
			page = currentPage;
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
										   
			this.AddLabel(185, 46, 2010, @"Advanced Dying Menu");
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
					hues = new int[]{1245,1149,1327,1133,1434,1509,1109,1110,1117,1636};
					break;
				}
				case 1:
				{
					hues = new int[]{0,1890,2989,2739,2764,2737,2604,2683,2743,2745};
					break;
				}
				case 2:
				{
					hues = new int[]{2581,2723,2749,2761,2587,2744,2935,2835,2738,2881};
					break;
				}
				case 3:
				{
					hues = new int[]{2605,2992,2932,2756,2657,2985,2600,2583,2598,2800};
					break;
				}
				case 4:
				{
					switch( m.Nation )
					{
						case Nation.Southern:
						{
							hues = new int[]{2591,2708,2746,2750,2740,2736,2753,2765,2877,2751};
							break;
						}
						case Nation.Western:
						{
							hues = new int[]{2708,1133,2621,2797,2740,2724,1899,2796,2864,2817};
							break;
						}
						case Nation.Haluaroc:
						{
							hues = new int[]{2759,2766,2585,2711,2877,2751,-1,-1,-1,-1};
							break;
						}
						case Nation.Mhordul:
						{
							hues = new int[]{2816,2886,2757,2795,2798,2801,2759,2766,2585,2711};
							break;
						}
						case Nation.Tirebladd:
						{
							hues = new int[]{1899,2796,2864,2817,2753,2765,-1,-1,-1,-1};
							break;
						}
						case Nation.Northern:
						{
							hues = new int[]{2707,2599,2621,2747,2753,2982,2816,2886,2757,2795};
							break;
						}
					}
					
					int racialdyes = m.Feats.GetFeatLevel(FeatList.RacialDyes);
					
					if( FreeHueing(m) )
						racialdyes = 3;
					
					if( racialdyes < 3 )
					{
						hues[4] = -1;
						hues[5] = -1;
					}
					
					if( racialdyes < 2 )
					{
						hues[2] = -1;
						hues[3] = -1;
					}
					
					if( racialdyes < 1 )
					{
						hues[0] = -1;
						hues[1] = -1;
					}
					break;
				}
                case 5:
                {
                    hues = SpecialHues;
                    break;
                }
			}
		}
		
		private class DyeTarget : Target
        {
			private int m_hue;
			
            public DyeTarget( PlayerMobile pm, int hue )
                : base( 8, false, TargetFlags.None )
            {
            	m_hue = hue;
                pm.SendMessage( 60, "Choose the piece of clothing you wish to dye." );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                PlayerMobile pm = m as PlayerMobile;

                for( int i = 0; i < SpecialHues.Length; i++ )
                {
                    if( SpecialHues[i] == m_hue )
                    {
                        if( !( obj is BaseClothing ) || ( ( (BaseClothing)obj ).Resource != CraftResource.Silk && 
                            ( (BaseClothing)obj ).Resource != CraftResource.Velvet && ( (BaseClothing)obj ).Resource != CraftResource.Satin ) )
                        {
                            m.SendMessage( "That hue can only be applied to clothing made out of silk, velvet or satin." );
                            return;
                        }
                    }
                }

                if( obj is BaseArmor || obj is OffsetableBook ) // books are leather too
                {
                	if( pm.Feats.GetFeatLevel(FeatList.LeatherDying) < 1 )
                	{
                		m.SendMessage( 60, "You still have not learnt how to use these dyes on leather." );
                		return;
                	}
					
					if ( obj is BaseArmor ) // only needed for armor
					{
						BaseArmor armor = obj as BaseArmor;
						
						if( armor.Resource != CraftResource.RegularLeather && armor.Resource != CraftResource.ScaledLeather &&
						   armor.Resource != CraftResource.ThickLeather && armor.Resource != CraftResource.BeastLeather )
						{
							m.SendMessage( 60, "You can only dye clothing that way." );
							return;
						}
					}
                }
                
                else if( !( obj is IDyable ) && !(obj is AddonComponent && ((AddonComponent)obj).Addon is IDyable) )
                {
            		m.SendMessage( 60, "That cannot be dyed this way." );
            		return;
                }
             
                if( obj is AddonComponent )
                {
                    AddonComponent component = obj as AddonComponent;
                    BaseHouse house = BaseHouse.FindHouseAt( component );

                    if( ( house == null || ( !house.IsOwner( m ) && !house.IsCoOwner( m ) ) ) && m.AccessLevel < AccessLevel.Counselor )
                    {
                        m.SendMessage( 60, "Only a house owner or co-owner can do that." );
                        return;
                    }
                }
            	
                ( (Item)obj ).Hue = m_hue;
                m.PlaySound( 0x23E );
        	}
        }
		
		private bool FreeHueing( PlayerMobile m )
		{
			foreach( Item item in m.GetItemsInRange(5) )
			{
				if( item is CreationChamberDyeTub )
					return true;
			}
			
			return false;
		}
		
		private void HandleChange( bool forward, PlayerMobile m )
		{
			int advdyeing = m.Feats.GetFeatLevel(FeatList.AdvancedDying);
			int racialdyes = m.Feats.GetFeatLevel(FeatList.RacialDyes);
			
			if( FreeHueing(m) )
			{
				advdyeing = 3;
				racialdyes = 3;
			}
			
			if( page < 3 && forward && advdyeing >= (page + 1) )
				page++;

            else if( page == 5 )
            {
                if( !forward )
                {
                    if( racialdyes > 0 )
                        page = 4;
                    else
                        page = advdyeing;
                }

                else
                    page = 0;
            }

            else if( page > 0 && !forward )
                page--;

            else if( page == 3 )
            {
                if( forward && racialdyes > 0 )
                    page++;

                else if( advdyeing > 2 )
                    page = 5;
            }

            else if( (page == 0 && !forward && advdyeing > 2) || ( page == 4 && forward && advdyeing > 2) )
                page = 5;

            else
                page = 0;
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			PlayerMobile m = sender.Mobile as PlayerMobile;
			
			if ( m == null || info.ButtonID == 0 )
				return;

			if( info.ButtonID < 11 )
				m.Target = new DyeTarget( m, hues[info.ButtonID -1] );
			
			else
				HandleChange( info.ButtonID == 102, m );	

			m.SendGump( new DyingTubGump( m, page ) );
		}
	}
}
