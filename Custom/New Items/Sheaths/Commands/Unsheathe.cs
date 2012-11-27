using System;
using Server.Items;

namespace Server.Commands
{
	public class Unsheathe
	{
		public static void Initialize()
		{
			CommandSystem.Register( "Unsheathe", AccessLevel.Player, new CommandEventHandler( Unsheathe_OnCommand ) );
		}

		[Usage( "Unsheathe back|waist" )]
		[Description( "Unsheathes the weapon in the specified slot." )]
		private static void Unsheathe_OnCommand( CommandEventArgs e )
		{
			if( e.Mobile.HasTrade )
			{
				e.Mobile.SendMessage( "You cannot unsheathe a weapon while you have a trade window open." );
				return;
			}
			
			if ( e.Length >= 1 )
			{
				string position = e.Arguments[0].ToLower();
				SheathedItem item;
				if ( position == "waist" )
					item = e.Mobile.FindItemOnLayer( Layer.Waist ) as SheathedItem;
				else
					item = e.Mobile.FindItemOnLayer( Layer.Talisman ) as SheathedItem;
				
				if ( item != null )
					item.Unsheathe( e.Mobile );
			}
			else
				e.Mobile.SendMessage( "Usage: .unsheathe back|waist" );
		}
	}
}
