using System;
using Server;
using Server.Targeting;
using Server.Gumps;
using Server.Items;

namespace Server.Commands
{
	public class DiscardLiquid
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "DiscardLiquid", AccessLevel.Player, new CommandEventHandler( DiscardLiquid_OnCommand ) );
		}

		private class DiscardLiquidTarget : Target
		{
			public DiscardLiquidTarget( ) : base( 1, true, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Item )
				{
					Item item = o as Item;
					if ( item.RootParent != from )
						from.SendMessage( "The object needs to be in your pack." );
					else
					{
						Item[] items;
						if ( !(item is BaseContainer) )
						{
							items = new Item[1];
							items[0] = item;
						}
						else
							items = ((BaseContainer)item).FindItemsByType( new Type[]{ typeof(BasePotion), typeof(BaseBeverage) }, false );
						
						foreach ( Item tmpItem in items )
						{
							if ( tmpItem is BasePotion )
							{
								BasePotion potion = tmpItem as BasePotion;
								Bottle emptybottle = new Bottle();
								from.AddToBackpack( emptybottle );
								potion.Delete();
							}
							else if ( tmpItem is BaseBeverage )
							{
								BaseBeverage beverage = tmpItem as BaseBeverage;
								if (beverage.Quantity > 0)
									beverage.Quantity = 0;
							}
							else
								from.SendMessage( "Can't empty that." );
						}
					}
				}
				else
				{
					from.SendMessage( "That's not a liquid container." );
				}
			}
		}

		[Usage( "DiscardLiquid" )]
		[Description( "Allows you to discard liquid currently present in containers such as bottles and jugs." )]
		private static void DiscardLiquid_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "Target the liquid container to empty, or target a bag to empty all liquid containers inside it." );
			e.Mobile.Target = new DiscardLiquidTarget();
		}
	}
}
