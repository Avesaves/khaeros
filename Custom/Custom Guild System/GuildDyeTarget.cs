using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Targeting;

namespace Server.Misc
{		
	public class GuildDyeTarget : Target
    {
		private int m_hue;
		private bool m_clothing;
		private CustomGuildStone m_guild;
		
		private int[] m_BasicClothing =  new int[]{1245,1149,1327,1133,1434,1509,1109,1110,1117,1636,0,1890,2989,2739,2764,2737,2604,2683,2743,2745,2581,2723,2749,2761,2587,2744,2935,2835,2738,2881,2605,2992,2932,2756,2657,2985,2600,2583,2598,2800};
		private int[] m_SouthernClothing = new int[]{2591,2708,2746,2750,2740,2736};
		private int[] m_WesternClothing = new int[]{2708,1133,2621,2797,2740,2724};
		private int[] m_HaluarocClothing = new int[]{2759,2766,2585,2711,2877,2751};
		private int[] m_MhordulClothing = new int[]{2816,2886,2757,2795,2798,2801};
		private int[] m_TirebladdClothing = new int[]{1899,2796,2864,2817,2753,2765};
		private int[] m_NorthernClothing = new int[]{2707,2599,2621,2747,2753,2982};
		
		private int[] m_BasicArmour =  new int[]{1245,1149,1327,1133,1434,1509,1109,1110,1117,1636,0,1890,2989,2739,2764,2737,2604,2683,2743,2745,2581,2723,2749,2761,2587,2744,2935,2835,2738,2881,2605,2992,2932,2756,2657,2985,2600,2583,2598,2800};
		private int[] m_SouthernArmour = new int[]{1454,2708,2683,2591,2745,2741};
		private int[] m_WesternArmour = new int[]{2935,2598,2711,2745,2605,2724};
		private int[] m_HaluarocArmour = new int[]{2720,2843,2711,2725,2751,2585};
		private int[] m_MhordulArmour = new int[]{2989,2404,2656,2757,2600,2795};
		private int[] m_TirebladdArmour = new int[]{2864,2739,2753,2583,2751,2765};
		private int[] m_NorthernArmour = new int[]{2869,2723,2599,2934,2621,2986};
		
        public GuildDyeTarget( PlayerMobile m, int hue, bool clothing, CustomGuildStone guild )
            : base( 8, false, TargetFlags.None )
        {
        	m_hue = hue;
        	m_clothing = clothing;
        	m_guild = guild;
        	
        	if( hue < 0 && CustomGuildStone.IsGuildLeader(m, guild) )
        		m.SendMessage( "Target an item of the desired colour for your guild's {0} colour.", clothing == true ? "clothing" : "armour" );
        	
        	else
        	{
	        	if( !clothing )
	            	m.SendMessage( "Choose the piece of armour you wish to enamel." );
	            
	        	else
	            	m.SendMessage( "Choose the piece of clothing you wish to dye." );
	        	
	        	if( CustomGuildStone.IsGuildLeader(m, guild) )
	        		m.SendMessage( "If you wish to reset your guild's {0} colour, target yourself.", clothing == true ? "clothing" : "armour" );
        	}
        }

        protected override void OnTarget( Mobile m, object obj )
        {
        	if( m == null || m.Deleted || m_guild == null || m_guild.Deleted )
        		return;
        	
        	if( m_hue < 0 )
        	{
                if(CustomGuildStone.IsGuildLeader((PlayerMobile)m, m_guild))
        		{
        			if( obj is Item )
        			{
        				int[] main = (m_clothing == true ? m_BasicClothing : m_BasicArmour);
        				int[] race = (m_clothing == true ? m_NorthernClothing : m_NorthernArmour);
        				PlayerMobile pm = m as PlayerMobile;
        				bool found = false;
        				
        				if( pm.Nation == Nation.Western )
        					race = (m_clothing == true ? m_WesternClothing : m_WesternArmour);
        				
        				else if( pm.Nation == Nation.Southern )
        					race = (m_clothing == true ? m_SouthernClothing : m_SouthernArmour);
        				
        				else if( pm.Nation == Nation.Haluaroc )
        					race = (m_clothing == true ? m_HaluarocClothing : m_HaluarocArmour);
        				
        				else if( pm.Nation == Nation.Mhordul )
        					race = (m_clothing == true ? m_MhordulClothing : m_MhordulArmour);
        				
        				else if( pm.Nation == Nation.Tirebladd )
        					race = (m_clothing == true ? m_TirebladdClothing : m_TirebladdArmour);
    					
        				for( int i = 0; i < main.Length; i++ )
        				{
        					if( ((Item)obj).Hue == main[i] )
        					{
        						found = true;
        						break;
        					}
        				}
        				
        				if( !found )
        				{
        					for( int i = 0; i < race.Length; i++ )
	        				{
	        					if( ((Item)obj).Hue == race[i] )
	        					{
	        						found = true;
	        						break;
	        					}
	        				}
        				}
        				
        				if( !found )
        				{
        					m.SendMessage( "That is not an acceptable {0} colour for your race.", m_clothing == true ? "clothing" : "armour" );
        					return;
        				}
        			}
        			
        			else
        			{
        				m.SendMessage( "Invalid target." );
        				return;
        			}
        			
        			if( !m_guild.OfficialGuild && (m_guild.Treasury == null || m_guild.Treasury.Deleted || !m_guild.Treasury.ConsumeTotal(typeof(Copper), 500)) )
        				m.SendMessage( "Your guild's treasury is either unexistant or does not have 500 copper coins in it." );
        			
        			else
        			{
        				if( m_clothing )
        					m_guild.ClothingHue = ((Item)obj).Hue;
        				
        				else
        					m_guild.ArmourHue = ((Item)obj).Hue;
        				
	        			m.SendMessage( "You have successfully chosen a new {0} colour for your guild.", m_clothing == true ? "clothing" : "armour" );
        			}
        			
        			return;
        		}
        	}
        	
        	else if( obj == m && CustomGuildStone.IsGuildLeader((PlayerMobile)m, m_guild) )
        	{
        		if( m_clothing )
        			m_guild.ClothingHue = -1;
        		
        		else
        			m_guild.ArmourHue = -1;
        		
        		m.SendMessage( "You have reset your guild's {0} colour.", m_clothing == true ? "clothing" : "armour" );
        		return;
        	}
        	
        	if( obj != null && obj is Item && !((Item)obj).Deleted )
            {
            	if( ((Item)obj).RootParentEntity != m )
                {
                	m.SendMessage( "The item must be in your possession." );
                	return;
                }
            	
            	if( (obj is BaseClothing && m_clothing) || (obj is BaseArmor && !m_clothing) )
            	{
            		((Item)obj).Hue = m_hue;
            		m.PlaySound( 0x23E );
            		return;
            	}
            }
            
            m.SendMessage( "Invalid target." );
    	}
    }
}
