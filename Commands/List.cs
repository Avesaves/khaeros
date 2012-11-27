using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.FeatInfo;

namespace Server.Commands
{
	public class ListCommand
	{
		public static void Initialize()
		{
			CommandSystem.Register( "List", AccessLevel.Player, new CommandEventHandler( List_OnCommand ) );
		}

		[Usage( "List <collection>" )]
		[Description( "Prints a selected collection of things to the screen in list format." )]
		public static void List_OnCommand( CommandEventArgs arg )
		{
			//arg.Mobile.SendMessage( "DEBUG: arg.ArgString.ToLower() was " + arg.ArgString.ToLower() );

			if ( arg.Length != 1 )
			{
				arg.Mobile.SendMessage( ".list <collection>" );
			}
			else if ( arg.ArgString.ToLower().Equals("feats") )
			{
				PlayerMobile pm = (PlayerMobile)arg.Mobile;
				foreach(KeyValuePair<FeatList, BaseFeat> kvp in pm.Feats.FeatDictionary)
					if (kvp.Value.Level > 0)
						pm.SendMessage( kvp.Value.Name + " " + kvp.Value.Level );
			}
			else
			{
				arg.Mobile.SendMessage( "Unknown collection type." );
			}
		}
	}
}
