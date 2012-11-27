using System;
using Server.Items;

namespace Server.Commands
{
	public class Sheathe
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Sheathe", AccessLevel.Player, new CommandEventHandler( Sheathe_OnCommand ) );
		}

		[Usage( "Sheathe left|right back|waist" )]
		[Description( "Sheathes the weapon in the specified hand either behind your back or on your waist." )]
		private static void Sheathe_OnCommand( CommandEventArgs e )
		{
			if( e.Mobile.HasTrade )
			{
				e.Mobile.SendMessage( "You cannot sheathe a weapon while you have a trade window open." );
				return;
			}
			
			if ( e.Length >= 2 )
			{
				string hand = e.Arguments[0].ToLower();
				string position = e.Arguments[1].ToLower();
				SheathedItem.Sheathe( e.Mobile, hand == "right", position == "back" );
			}
			else
			{
				e.Mobile.SendMessage( "Usage: .sheathe right back" );
				e.Mobile.SendMessage( "Usage: .sheathe left back" );
				e.Mobile.SendMessage( "Usage: .sheathe right waist" );
				e.Mobile.SendMessage( "Usage: .sheathe left waist" );
			}
		}
	}
}
