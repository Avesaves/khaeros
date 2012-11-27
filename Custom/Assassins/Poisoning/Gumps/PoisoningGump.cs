using Server;
using Server.Items;
using Server.Engines.Craft;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using System;

namespace Server.Gumps
{
	public class PoisoningGump : Gump
	{
		private PoisoningCraftState m_CraftState;
		public PoisoningGump( PoisoningCraftState craftstate ) : base( 0, 0 )
		{
			m_CraftState = craftstate;
			
			this.Closable=true;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			
			this.AddBackground(453, 38, 203, 324, 2620);
			this.AddBackground(453, 38, 203, 44, 2620);
			this.AddBackground(50, 33, 416, 367, 9270);
			this.AddHtml( 63, 336, 392, 52, m_CraftState.LastMessage, (bool)true, (bool)false);
			this.AddLabel(234, 46, 59, "Poisoning");
			this.AddHtml( 473, 88, 175, 261, m_CraftState.HTMLEffects(), (bool)false, (bool)false);
			this.AddLabel(523, 51, 56, "Toxin Effects");
			this.AddImage(411, 11, 1417, 2720);
			
			int[] x = new int[]{ 87, 92 }; // button, ingredient
			int[] y = new int[]{ 80, 89 };
			for ( int i = 0; i<10; i++ )
			{
				this.AddButton(x[0], y[0], 9800, 9800, 500+i, GumpButtonType.Reply, 0);
				if ( m_CraftState.GumpComponents[i].Graphic > 0 )
					this.AddItem(x[1], y[1], m_CraftState.GumpComponents[i].Graphic, m_CraftState.GumpComponents[i].Hue);
				x[0]+=60; x[1]+=60;
				if ( ((i+1) % 5) == 0 ) // new line
				{
					x[0] = 87; x[1] = 92;
					y[0]+=60; y[1]+=60;
				}
			}
			
			this.AddLabel(97, 261, 55, "Choose bottle type");
			this.AddButton(78, 263, 2117, 2118, (int)Buttons.ChooseBottle, GumpButtonType.Reply, 0);
			this.AddLabel(97, 281, 55, "Read Recipe");
			this.AddButton(78, 283, 2117, 2118, (int)Buttons.ReadRecipe, GumpButtonType.Reply, 0);
			this.AddLabel(327, 261, 55, "Attempt craft");
			this.AddButton(308, 263, 2117, 2118, (int)Buttons.AttemptCraft, GumpButtonType.Reply, 0);
			this.AddLabel(327, 281, 55, "Write Recipe");
			this.AddButton(308, 283, 2117, 2118, (int)Buttons.WriteRecipe, GumpButtonType.Reply, 0);
			this.AddLabel(198, 309, 70, "Success chance: " + (int)(100*m_CraftState.CraftChance()) + "%");
			this.AddItem(436, 37, m_CraftState.BottleID);
			this.AddImage(92, 89, 3982);
			this.AddLabel(77, 214, 55, "Toxin name:");
			this.AddImage(152, 212, 1141);
			this.AddTextEntry(163, 214, 251, 14, 0, (int)Buttons.ToxinName, m_CraftState.Name);
			this.AddImageTiled(60, 200, 396, 3, 96);
		}

		public enum Buttons
		{
			ChooseBottle = 2,
			AttemptCraft,
			ToxinName,
			ReadRecipe,
			WriteRecipe
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
					m_CraftState.Name = info.TextEntries[0].Text;
			}

			if ( info.ButtonID >= 500 && info.ButtonID < 510 ) // ingredients
			{
				int index = info.ButtonID - 500;
				from.Target = new PickIngredientTarget( m_CraftState, index );
				from.SendMessage( 35, "Target an ingredient to use, or target yourself to empty that slot. Press ESC to cancel." );
			}
			switch ( info.ButtonID )
			{
				case (int)Buttons.ChooseBottle: 
				{
					from.CloseGump( typeof(PoisoningPickBottleGump) );
					from.SendGump( new PoisoningPickBottleGump( m_CraftState ) );
					break;
				}

				case (int)Buttons.ReadRecipe: 
				{
					from.Target = new PickFormulaReadTarget( m_CraftState );
					from.SendMessage( 35, "Target the recipe that you wish to read." );
					break;
				}

				case (int)Buttons.WriteRecipe: 
				{
					from.Target = new PickFormulaWriteTarget( m_CraftState );
					from.SendMessage( 35, "Target something to write the recipe on." );
					break;
				}
				
				case (int)Buttons.AttemptCraft: 
				{
					if ( !m_CraftState.AttemptCraft() )
						from.PlaySound( 0x242 ); // grind grind
					else
						from.PlaySound( 0x240 ); // pour
					from.SendGump( new PoisoningGump( m_CraftState ) );
					break;
				}
			}
		}
		
		private class PickFormulaReadTarget : Target
		{
			private PoisoningCraftState m_CraftState;

			public PickFormulaReadTarget( PoisoningCraftState craftstate ) : base( 15, false, TargetFlags.None )
			{
				m_CraftState = craftstate;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				PoisoningCraftState newState = new PoisoningCraftState( from, m_CraftState.Tool );
				if ( targ is BaseRecipe )
				{
					if ( newState.ReadRecipe( targ as BaseRecipe ) )
						from.SendGump( new PoisoningGump( newState ) );
					else
					{
						m_CraftState.LastMessage = newState.LastMessage;
						from.SendGump( new PoisoningGump( m_CraftState ) );
					}
				}
				else
				{
					from.SendMessage( "That's not a valid recipe." );
					from.SendGump( new PoisoningGump( m_CraftState ) );
				}
			}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				from.SendGump( new PoisoningGump( m_CraftState ) );
			}
		}

		private class PickFormulaWriteTarget : Target
		{
			private PoisoningCraftState m_CraftState;

			public PickFormulaWriteTarget( PoisoningCraftState craftstate ) : base( 15, false, TargetFlags.None )
			{
				m_CraftState = craftstate;
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
						ToxinRecipe recipe = new ToxinRecipe();
						from.AddToBackpack( recipe );
						if ( m_CraftState.WriteRecipe( recipe ) )
							formula.Delete();
						else
							recipe.Delete();
					}
				}

				else
					from.SendMessage( 35, "You can't write on that." );

				from.SendGump( new PoisoningGump( m_CraftState ) );
			}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				from.SendGump( new PoisoningGump( m_CraftState ) );
			}
		}

		private class PickIngredientTarget : Target
		{
			private PoisoningCraftState m_CraftState;
			private int m_Index;
			public PickIngredientTarget( PoisoningCraftState craftstate, int index ) : base( 15, false, TargetFlags.None )
			{
				m_CraftState = craftstate;
				m_Index = index;
			}

			protected override void OnTarget( Mobile from, object targ )
			{
				if ( targ == from ) // targeted self, clear the slot
					m_CraftState.RemoveComponent( m_Index );
				else
					m_CraftState.AddComponent( targ as Item, m_Index );

				from.SendGump( new PoisoningGump( m_CraftState ) );
			}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				from.SendGump( new PoisoningGump( m_CraftState ) );
			}
		}
	}
}
