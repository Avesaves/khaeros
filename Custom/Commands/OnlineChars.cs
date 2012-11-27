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
	public class OnlineChars
	{
		public static void Initialize()
		{
			CommandSystem.Register( "OnlineChars", AccessLevel.Player, new CommandEventHandler( OnlineChars_OnCommand ) );
		}
        
        [Usage( "OnlineChars" )]
        [Description( "Shows a list of characters online." )]
        private static void OnlineChars_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	List<string> list = new List<string>();

        	foreach( NetState state in NetState.Instances )
			{
				PlayerMobile player = state.Mobile as PlayerMobile;

				if( state.Mobile != null && state.Mobile != m && state.Mobile is PlayerMobile && player.Name != null && player.Name.Length > 0 )
					list.Add( player.Name );
			}
        	
        	if( list.Count > 0 )
        	{
		
	        	m.SendMessage( "" + list.Count + " other characters online, including:" );

	        	foreach( string st in list )
	        		m.SendMessage( st );
        	}
        	
        	else
        		m.SendMessage( "There are currently no other players online." );
        }        
	}
}
