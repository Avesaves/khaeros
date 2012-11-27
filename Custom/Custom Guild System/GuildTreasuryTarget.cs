using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Targeting;

namespace Server.Misc
{		
	public class GuildTreasuryTarget : Target
    {
		private CustomGuildStone g;
		
        public GuildTreasuryTarget( PlayerMobile m, CustomGuildStone guild )
            : base( 8, false, TargetFlags.None )
        {
        	g = guild;
        	m.SendMessage( "Target a safe to use as your guild's treasury." );
        }

        protected override void OnTarget( Mobile m, object obj )
        {
        	if( m == null || m.Deleted || g == null || g.Deleted )
        		return;
        	
        	if( obj is Safe )
            {
        		foreach( Item item in ((Item)obj).GetItemsInRange( 2 ) )
        			if( item == g )
        			{
        				m.SendMessage( "You have successfully assigned the targetted safe to be your guild's treasury." );
        				g.Treasury = (Safe)obj;
        				return;
        			}
        		
        		m.SendMessage( "That is too far away from your guildstone." );
            }
            
        	else
            	m.SendMessage( "Invalid target." );
    	}
    }
}
