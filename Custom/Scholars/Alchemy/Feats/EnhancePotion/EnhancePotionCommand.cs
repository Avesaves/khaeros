using System;
using Server;
using Server.Targeting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Commands
{
	public class EnhancePotion
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			CommandSystem.Register( "EnhancePotion", AccessLevel.Player, new CommandEventHandler( EnhancePotion_OnCommand ) );
		}

		private class EnhancePotionTarget : Target
		{
			public EnhancePotionTarget( ) : base( 1, true, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is DrinkPotion )
				{
					DrinkPotion potion = o as DrinkPotion;
					if ( potion.RootParent != from )
						from.SendMessage( "The potion must be in your pack." );
					else if ( potion.Enhanced )
						from.SendMessage( "This potion has already been enhanced." );
					else if ( potion.Effects.Count == 0 )
						from.SendMessage( "There is no substance present to enhance." );
					else
					{
						if ( Utility.RandomBool() ) // 50% chance to succeed
						{
							double bonusFactor;
							
							bonusFactor = 1 + ( 0.2 * ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.EnhancePotion) ); // 30% increase, change this to accomodate feat levels
							
							CustomEffect[] keyarray = new CustomEffect[potion.Effects.Count];
							int k = 0;
							foreach ( KeyValuePair<CustomEffect, int> kvp in potion.Effects )
								keyarray[k++] = kvp.Key;
							foreach ( CustomEffect key in keyarray )
								potion.Effects[key] = (int)(potion.Effects[key] * bonusFactor);
	
							potion.Enhanced = true;
							from.SendMessage( "You successfully enhance the potion." );
						}
						else
						{
							Bottle emptybottle = new Bottle();
							from.AddToBackpack( emptybottle );
							potion.Consume( 1 );
							from.SendMessage( "You have failed in your attempt at enhancing the potion, and have thus rendered it worthless." );
						}
					}
				}
				else
					from.SendMessage( "You can only enhance drinkable potions." );
			}
		}

		[Usage( "EnhancePotion" )]
		[Description( "Allows you to enhance drinkable potions." )]
		private static void EnhancePotion_OnCommand( CommandEventArgs e )
		{
			e.Mobile.SendMessage( "What would you like to attempt to enhance?" );
			e.Mobile.Target = new EnhancePotionTarget();
		}
	}
}
