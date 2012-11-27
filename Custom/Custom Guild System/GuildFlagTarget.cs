using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Targeting;

namespace Server.Misc
{		
	public class GuildFlagTarget : Target
    {
		private GuildFlag m_flag;
		
        public GuildFlagTarget( GuildFlag flag )
            : base( 8, false, TargetFlags.None )
        {
			m_flag = flag;
        }

        protected override void OnTarget( Mobile m, object obj )
        {
        	if( m == null || m.Deleted || m_flag == null || m_flag.Deleted )
        		return;
        	
        	if( obj != null && obj is CustomGuildStone )
        		m_flag.TryToRecharge( m, (CustomGuildStone)obj );
            
        	else
            	m.SendMessage( "Invalid target." );
    	}
    }
}
