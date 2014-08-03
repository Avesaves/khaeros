using System;
using Server.Network;
using Server.TimeSystem;
using Server.Mobiles;

namespace Server.Misc
{
	public class LoginStats
	{
		public static void Initialize()
		{
			// Register our event handler
			EventSink.Login += new LoginEventHandler( EventSink_Login );
		}

		private static void EventSink_Login( LoginEventArgs args )
		{
			int userCount = NetState.Instances.Count;
			int itemCount = World.Items.Count;
			int mobileCount = World.Mobiles.Count;

			Mobile m = args.Mobile;
			
			m.SendMessage( 55, "Khaeros Shard: Welcome to our World");
			m.SendMessage( 55, "There {1} currently {2} player{3} online...",
				args.Mobile.Name,
				userCount == 1 ? "is" : "are",
				userCount, userCount == 1 ? "" : "s",
				itemCount, itemCount == 1 ? "" : "s",
				mobileCount, mobileCount == 1 ? "" : "s" );
				
			string text = "";		
			text = text + TimeSystem.Formatting.GetDescriptiveTime(m);
			
			m.SendMessage(55, text);
			
			//m.SendMessage( 33, "We hope you enjoy your time!");
            m.SendMessage(2832, "At night, the sky is streaked with falling stars....");


//******Edit: Added in********
            			if ( m.AccessLevel >= AccessLevel.Counselor )
            			{
                			Server.Engines.Help.PageQueue.Pages_OnCalled( m );
            			}
//******************************
		}
	}
}