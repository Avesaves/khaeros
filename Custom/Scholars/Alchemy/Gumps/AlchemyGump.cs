using System;
using System.Drawing;
using System.Collections.Generic;
using Server;
using Server.Misc;
using Server.Network;
using Server.Targeting;
using Server.Engines.Alchemy;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
	class AlchemyGump : Gump
	{
		// new coords, uniform placement
		private static Point[] ItemCoordinates = {	new Point(160, 140), new Point(230, 150), new Point(298, 140), new Point(110, 228), new Point(232, 240),
													new Point(348, 228), new Point(152, 284), new Point(308, 284), new Point(184, 348), new Point(274, 348)};
		/*private static Point[] ItemCoordinates = {	new Point(160, 133), new Point(230, 143), new Point(297, 133), new Point(109, 220), new Point(232, 233),
													new Point(348, 220), new Point(152, 276), new Point(308, 276), new Point(184, 340), new Point(273, 340)};*/
		private BrewingState m_BrewingState;
		public AlchemyGump( BrewingState state ) : base( 50, 50 )
		{
			m_BrewingState = state;
			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;

			this.AddPage(0);
			this.AddBackground(457, 40, 228, 32, 2620);
			this.AddBackground(458, 65, 227, 455, 2620);
			if ( m_BrewingState.Type == PotionType.Drink ) // only drinks have side-effects
				this.AddBackground(456, 262, 228, 32, 2620);
			this.AddBackground(29, 464, 440, 56, 2620);

			this.AddImage(20, 27, 30500);

			// Bags start
			this.AddButton(160, 133, 9800, 9800, 10, GumpButtonType.Reply, 0);
			this.AddButton(230, 143, 9800, 9800, 11, GumpButtonType.Reply, 0);
			this.AddButton(297, 133, 9800, 9800, 12, GumpButtonType.Reply, 0);
			this.AddButton(109, 220, 9800, 9800, 13, GumpButtonType.Reply, 0);
			this.AddButton(232, 233, 9800, 9800, 14, GumpButtonType.Reply, 0);
			this.AddButton(348, 220, 9800, 9800, 15, GumpButtonType.Reply, 0);
			this.AddButton(152, 276, 9800, 9800, 16, GumpButtonType.Reply, 0);
			this.AddButton(308, 276, 9800, 9800, 17, GumpButtonType.Reply, 0);
			this.AddButton(184, 340, 9800, 9800, 18, GumpButtonType.Reply, 0);
			this.AddButton(273, 340, 9800, 9800, 19, GumpButtonType.Reply, 0);

			// Items start
			for (int i = 0; i < m_BrewingState.IngredientPictures.Length; i++)
			{
				if (m_BrewingState.IngredientPictures[i] > 0)
					this.AddItem(ItemCoordinates[i].X, ItemCoordinates[i].Y, m_BrewingState.IngredientPictures[i], m_BrewingState.IngredientHues[i]);
			}

			this.AddButton(410, 445, 30083, 30089, (int)Buttons.Brew, GumpButtonType.Reply, 0);
			this.AddLabel(432, 446, 48, "Brew");
			this.AddLabel(174, 445, 965, "Chance of success: " + ( (int) ( m_BrewingState.CraftChance()*100 ) ) + "%" );
			this.AddLabel(216, 50, 56, "ALCHEMY");
			this.AddButton(33, 445, 30083, 30089, (int)Buttons.Discard, GumpButtonType.Reply, 0);
			this.AddLabel(55, 446, 48, "Discard");
			this.AddButton(59, 88, 30083, 30089, (int)Buttons.Read, GumpButtonType.Reply, 0);
			this.AddLabel(81, 89, 48, "Read formula");
			this.AddButton(323, 88, 30083, 30089, (int)Buttons.Write, GumpButtonType.Reply, 0);
			this.AddLabel(345, 89, 48, "Write formula");
			this.AddButton(196, 74, 30083, 30089, (int)Buttons.ChooseBottle, GumpButtonType.Reply, 0);
			this.AddLabel(218, 75, 48, "Choose bottle");
			this.AddLabel(114, 419, 61, "Potion type: ");
			this.AddButton(154, 418, (m_BrewingState.Type == PotionType.Drink ? 30070 : 30066), 30070, (int)Buttons.Drink, GumpButtonType.Reply, 0);
			this.AddLabel(208, 420, 165, "Drink");
			this.AddButton(219, 418, (m_BrewingState.Type == PotionType.Bomb ? 30070 : 30066), 30070, (int)Buttons.Bomb, GumpButtonType.Reply, 0);
			this.AddLabel(273, 420, 165, "Bomb");
			this.AddHtml( 478, 77, 194, (m_BrewingState.Type == PotionType.Drink ? 181 : 429), m_BrewingState.HTMLEffects(), (bool)false, (bool)false);
			this.AddButton(284, 418, (m_BrewingState.Type == PotionType.Oil ? 30070 : 30066), 30070, (int)Buttons.Oil, GumpButtonType.Reply, 0);
			this.AddLabel(338, 420, 165, "Oil");
			this.AddTextEntry(219, 488, 240, 20, 935, (int)Buttons.TextEntry, m_BrewingState.Name);
			this.AddLabel(132, 488, 56, "Potion Name:");
			this.AddLabel(522, 48, 56, "Potion Effects");

			if ( m_BrewingState.Type == PotionType.Drink ) // only drinks have side-effects
			{
				this.AddLabel(505, 270, 137, "Potion Side-Effects");
				this.AddHtml( 478, 298, 194, 211, m_BrewingState.HTMLSideEffects(), (bool)false, (bool)false);
			}

			this.AddImage( 411, 9, 1417, 2591 );
			this.AddItem( 429, 30, m_BrewingState.Bottle );
		}

		public enum Buttons
		{
			Brew=1,
			Discard=2,
			Read=3,
			Write=4,
			Drink=5,
			Bomb=6,
			Oil=7,
			TextEntry=8,
			ChooseBottle=9,
		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			if ( info.ButtonID == 0 ) // close
				return;

			Mobile from = sender.Mobile;
			if ( info.TextEntries[0] != null )
			{
				string tmp = info.TextEntries[0].Text;
				if ( String.IsNullOrEmpty( tmp ) || tmp.IndexOf( '<' ) != -1 )
					from.SendMessage( 35, "That name is unacceptable. Please pick another name." );
				else
					m_BrewingState.Name = info.TextEntries[0].Text;
			}

			if ( info.ButtonID >= 10 && info.ButtonID < 20 ) // ingredients
			{
				int index = info.ButtonID - 10;
				from.Target = new PickIngredientTarget( m_BrewingState, index );
				from.SendMessage( 35, "Target an ingredient to use, or target yourself to empty that slot. Press ESC to cancel." );
			}

			switch ( info.ButtonID ) 
			{
				case (int)Buttons.Drink: 
				{
					if ( m_BrewingState.Type != PotionType.Drink )
					{
						m_BrewingState = new BrewingState( m_BrewingState.Brewer, m_BrewingState.Tool ); // dump the old one, since we're changing potion types
						m_BrewingState.Type = PotionType.Drink;
					}
					from.SendGump( new AlchemyGump( m_BrewingState ) );
					break;
				}

				case (int)Buttons.Bomb: 
				{
					if ( m_BrewingState.Type != PotionType.Bomb )
					{
						m_BrewingState = new BrewingState( m_BrewingState.Brewer, m_BrewingState.Tool ); // dump the old one, since we're changing potion types
						m_BrewingState.Type = PotionType.Bomb;
					}
					from.SendGump( new AlchemyGump( m_BrewingState ) );
					break;
				}

				case (int)Buttons.Oil: 
				{
					if ( m_BrewingState.Type != PotionType.Oil )
					{
						m_BrewingState = new BrewingState( m_BrewingState.Brewer, m_BrewingState.Tool ); // dump the old one, since we're changing potion types
						m_BrewingState.Type = PotionType.Oil;
					}
					from.SendGump( new AlchemyGump( m_BrewingState ) );
					break;
				}

				case (int)Buttons.Discard: 
				{
					m_BrewingState = new BrewingState( m_BrewingState.Brewer, m_BrewingState.Tool ); // start anew
					from.SendGump( new AlchemyGump( m_BrewingState ) );
					break;
				}

				case (int)Buttons.Brew: 
				{
					if ( m_BrewingState.Tool == null || m_BrewingState.Tool.Deleted || m_BrewingState.Tool.UsesRemaining <= 0 || 
						m_BrewingState.Tool.RootParent != m_BrewingState.Brewer)
					{
						// lets see if we can find another tool in the player's pack
						Item[] items = m_BrewingState.Brewer.Backpack.FindItemsByType( typeof( AlchemyTool ) );
						AlchemyTool newTool = null;
						foreach ( Item item in items )
						{
							if ( item is AlchemyTool )
							{
								AlchemyTool tool = item as AlchemyTool;
								if ( tool.UsesRemaining > 0 )
								{
									newTool = tool;
									break;
								}
							}
						}

						if ( newTool != null ) // found a new tool to use
							m_BrewingState.Tool = newTool;
						else // could not find another tool
						{
							m_BrewingState.Brewer.SendMessage( 35, "You need the proper tool in your pack in order to brew potions." );
							from.SendGump( new AlchemyGump( m_BrewingState ) );
							return;
						}
					}

					int returned = m_BrewingState.AttemptCraft();
					switch ( returned )
					{
						case -7: // oils only: negative duration value
						{
							from.SendMessage( 35, "The substance is solid. Add something liquid into the mixture." );
							break;
						}
						case -6: // bombs only: negative range value (not enough explosives)
						{
							from.SendMessage( 35, "There's not enough explosives for the bomb to be effective." );
							break;
						}
						case -5: // bottle not in pack
						{
							from.SendMessage( 35, "You need an empty bottle." );
							break;
						}

						case -3: // no ingredients on the gump
						{
							from.SendMessage( 35, "There's nothing to pour!" );
							break;
						}

						case -2: // FAIL, kek.
						{
							from.SendMessage( 35, "You attempt to brew the potion, but end up only wasting the ingredients." );
							from.PlaySound( 0x242 ); // grind grind
							m_BrewingState.Tool.UsesRemaining--;
							if ( m_BrewingState.Tool.UsesRemaining < 1 )
							{
								m_BrewingState.Tool.Delete();
								m_BrewingState.Brewer.SendMessage( "You have worn out your tool." );
							}
							
							AwardXPCP( ((PlayerMobile)m_BrewingState.Brewer) );
							break;
						}

						case -1: // can't find the ingredients in pack
						{
							from.SendMessage( 35, "You don't have the required ingredients." );
							break;
						}

						case 1: // success
						{
							from.SendMessage( 35, "You pour the liquid into the bottle." );
							from.PlaySound( 0x240 ); // pour
							m_BrewingState.Tool.UsesRemaining--;
							if ( m_BrewingState.Tool.UsesRemaining < 1 )
							{
								m_BrewingState.Tool.Delete();
								m_BrewingState.Brewer.SendMessage( "You have worn out your tool." );
							}
							AwardXPCP( ((PlayerMobile)m_BrewingState.Brewer) );
							break;
						}
					}

					from.SendGump( new AlchemyGump( m_BrewingState ) );

					break;
				}

				case (int)Buttons.ChooseBottle: 
				{
					from.CloseGump( typeof(PickBottleGump) );
					from.SendGump( new PickBottleGump( m_BrewingState ) );
					break;
				}

				case (int)Buttons.Read: 
				{
					from.Target = new PickFormulaReadTarget( m_BrewingState );
					from.SendMessage( 35, "Target the formula that you wish to read." );
					break;
				}

				case (int)Buttons.Write: 
				{
					from.Target = new PickFormulaWriteTarget( m_BrewingState );
					from.SendMessage( 35, "Target something to write the formula on." );
					break;
				}
			}
        }
		
		private void AwardXPCP( PlayerMobile m )
		{
			m.Crafting = true;	
			LevelSystem.AwardMinimumXP( m, 3 );
			m.Crafting = false;
		}

		private class PickFormulaReadTarget : Target
		{
			private BrewingState m_BrewingState;

			public PickFormulaReadTarget( BrewingState brewingstate ) : base( 15, false, TargetFlags.None )
			{
				m_BrewingState = brewingstate;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if ( targ is AlchemicalFormula )
				{
					AlchemicalFormula formula = targ as AlchemicalFormula;
					if ( formula.RootParent != from )
						from.SendMessage( 35, "The formula must be in your pack for you to get a good look at it." );
					else
					{
						BrewingState newState = new BrewingState( m_BrewingState.Brewer, m_BrewingState.Tool );
						int returned = newState.ReadFormula( formula );
						switch ( returned )
						{
							case -2: // one of the ingredients isn't IAlchemyIngredient
							{
								from.SendMessage( 35, "Broken formula." );
								break;
							}

							case -1: // doesn't know how to use an ingredient
							{
								from.SendMessage( 35, "You don't recognize some of the ingredients required by this formula." );
								break;
							}

							case 1: // success
							{
								m_BrewingState = newState;
								from.SendMessage( 35, "You successfully mix the ingredients according to the formula." );
								break;
							}
						}
					}
				}

				else
					from.SendMessage( 35, "That's not an alchemical formula." );

				from.SendGump( new AlchemyGump( m_BrewingState ) );
			}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				from.SendGump( new AlchemyGump( m_BrewingState ) );
			}
		}

		private class PickFormulaWriteTarget : Target
		{
			private BrewingState m_BrewingState;

			public PickFormulaWriteTarget( BrewingState brewingstate ) : base( 15, false, TargetFlags.None )
			{
				m_BrewingState = brewingstate;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if ( targ is AlchemicalFormula )
				{
					AlchemicalFormula formula = targ as AlchemicalFormula;
					if ( formula.RootParent != from )
						from.SendMessage( 35, "The object that you wish to write the formula on must be in your pack." );
					else if ( formula.PotionName != null )
						from.SendMessage( 35, "Something is already written on that formula, try a blank one." );
					else
					{
						if ( m_BrewingState.WriteFormula( formula ) )
							from.SendMessage( 35, "You scribble the formula down." );
						else
							from.SendMessage( 35, "There are no ingredients!" );
					}
				}

				else
					from.SendMessage( 35, "You can't write on that." );

				from.SendGump( new AlchemyGump( m_BrewingState ) );
			}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				from.SendGump( new AlchemyGump( m_BrewingState ) );
			}
		}

		private class PickIngredientTarget : Target
		{
			private BrewingState m_BrewingState;
			private int m_Index;
			public PickIngredientTarget( BrewingState brewingstate, int index ) : base( 15, false, TargetFlags.None )
			{
				m_BrewingState = brewingstate;
				m_Index = index;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if ( targ is Item && targ is IAlchemyIngredient )
				{
					IBombIngredient bombIngredient = targ as IBombIngredient;
					IDrinkIngredient drinkIngredient = targ as IDrinkIngredient;
					IOilIngredient oilIngredient = targ as IOilIngredient;

					Item item = targ as Item;

					if ( ((m_BrewingState.Type == PotionType.Drink) && drinkIngredient != null) ||  
							((m_BrewingState.Type == PotionType.Bomb) && bombIngredient != null) || 
							((m_BrewingState.Type == PotionType.Oil) && oilIngredient != null))
					{
						if ( ((m_BrewingState.Type == PotionType.Drink) && drinkIngredient.CanUse( from )) ||  
							((m_BrewingState.Type == PotionType.Bomb) && bombIngredient.CanUse( from )) || 
							((m_BrewingState.Type == PotionType.Oil) && oilIngredient.CanUse( from )))
						{
							if ( m_BrewingState.IngredientTypes[m_Index] != null )
								m_BrewingState.RemoveIngredient( m_BrewingState.IngredientTypes[m_Index], true ); // skip update, since we'll also add something here
							m_BrewingState.IngredientTypes[m_Index] = item.GetType();
							m_BrewingState.IngredientPictures[m_Index] = item.ItemID;
							m_BrewingState.IngredientHues[m_Index] = item.Hue;
							m_BrewingState.AddIngredient( item.GetType() ); // update now
						}

						else
							from.SendMessage(35, "You don't know how to use that ingredient.");
					}

					else
						from.SendMessage(35, "That ingredient cannot be used in this type of potion.");
				}

				else if ( targ == from ) // targeted self, clear the slot
				{
					if ( m_BrewingState.IngredientTypes[m_Index] != null ) 
					{
						m_BrewingState.IngredientPictures[m_Index] = 0;
						m_BrewingState.RemoveIngredient( m_BrewingState.IngredientTypes[m_Index] );
						m_BrewingState.IngredientTypes[m_Index] = null;
					}
				}

				else
					from.SendMessage(35, "That is not an alchemical ingredient.");

				from.SendGump( new AlchemyGump( m_BrewingState ) );
			}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				from.SendGump( new AlchemyGump( m_BrewingState ) );
			}
		}
	}
}
