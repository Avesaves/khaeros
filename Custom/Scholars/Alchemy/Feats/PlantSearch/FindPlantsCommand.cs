using System;
using Server;

namespace Server.Commands
{
	public class FindPlantsCommand
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "FindPlants", AccessLevel.Player, new CommandEventHandler( FindPlants_OnCommand ) );
		}

		[Usage( "FindPlants" )]
		[Description( "Allows you to search the surrounding area for plants." )]
		private static void FindPlants_OnCommand( CommandEventArgs e )
		{
			if( e.Mobile.Skills[SkillName.HerbalLore].Base > 0 )
				PlantSearch.OnUse( e.Mobile );
		}
	}
}
