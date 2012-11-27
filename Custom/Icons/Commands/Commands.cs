using System;
using Server;
using Server.Targeting;
using Server.Gumps;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Network;
using Server.Mobiles;

namespace Server.Commands
{
	public class MyIcons
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "MyIcons", AccessLevel.Player, new CommandEventHandler( MyIcons_OnCommand ) );
		}

		[Usage( "MyIcons" )]
		[Description( "Opens a gump that allows you to set up macro icons." )]
		private static void MyIcons_OnCommand( CommandEventArgs e )
		{
			PlayerMobile mob = e.Mobile as PlayerMobile;
			if ( mob != null )
			{
				mob.CloseGump( typeof( MyIconsGump ) );
				mob.SendGump( new MyIconsGump( mob ) );
			}
		}
	}
}
