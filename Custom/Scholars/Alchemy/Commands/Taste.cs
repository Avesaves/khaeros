using System;
using Server;
using Server.Targeting;
using Server.Gumps;
using Server.Items;

namespace Server.Commands
{
	public class Taste
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "Taste", AccessLevel.Player, new CommandEventHandler( Taste_OnCommand ) );
		}

		private class TasteTarget : Target
		{
			public TasteTarget( ) : base( 1, true, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Item )
				{
					Item item = o as Item;
					if ( item.RootParent != from )
						from.SendMessage( "Your tongue isn't that long. The object needs to be in your pack." );
					else
					{
						if ( item is DrinkPotion )
						{
							DrinkPotion potion = item as DrinkPotion;
							int tastingDifficulty = potion.Effects.Count;
							// difficulty is (alchemy + herbal lore) / 200, so at 100.0 at each skill, the person can ID a potion consisting of 10 effects
							if ( ( ( from.Skills[SkillName.Alchemy].Fixed + from.Skills[(SkillName)36].Fixed ) / 200 ) >= tastingDifficulty )
							{
								from.SendMessage( "You taste the substance with the tip of your tongue and successfully determine its composition." );
								from.SendGump( new PotionTastingGump( potion, from ) );
							}
							else
							{
								from.SendMessage( "The taste of this substance is far too complex for you to make anything sensible out of it." );
							}
						}
						else
							from.SendMessage( "Can't taste that." );
					}
				}
				else
				{
					from.SendMessage( "That would be inappropriate." );
				}
			}
		}

		[Usage( "Taste" )]
		[Description( "Allows you to taste things - such as potions - to determine their properties." )]
		private static void Taste_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "What would you like to taste?" );
			e.Mobile.Target = new TasteTarget();
		}
	}
}
