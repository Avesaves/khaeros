using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Prompts;

namespace Server.Commands
{
	public class Online
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Online", AccessLevel.Player, new CommandEventHandler( Online_OnCommand ) );
			CommandSystem.Register( "WhoIs", AccessLevel.Player, new CommandEventHandler( WhoIs_OnCommand ) );
		}
		
		[Usage( "Online" )]
        [Description( "Adds/Removes yourself from the list of characters online." )]
        private static void Online_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;

    		if( m.DisplayGuildTitle )
    		{
    			m.SendMessage( "You remove yourself from the Online list." );
    			m.DisplayGuildTitle = false;
    		}
    		
    		else
    		{
    			m.SendMessage( "You have been added to the Online list." );
    			m.DisplayGuildTitle = true;
    		}
        }
        
        [Usage( "WhoIs" )]
        [Description( "Shows a list of characters available for RP." )]
        private static void WhoIs_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	List<string> list = new List<string>();

        	foreach( NetState state in NetState.Instances )
			{
				PlayerMobile player = state.Mobile as PlayerMobile;

				if( state.Mobile != null && state.Mobile != m && state.Mobile is PlayerMobile && player.DisplayGuildTitle && player.Name != null && player.Name.Length > 0 )
					list.Add( player.Name );
			}
        	
        	if( list.Count > 0 )
        	{
	        	m.SendMessage( "List of online characters:" );

	        	foreach( string st in list )
	        		m.SendMessage( st );
	        	
        	}
        	
        	else
        		m.SendMessage( "There are currently no players visibly online." );
        }        




			}
		}
	

