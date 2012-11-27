using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Craft
{
	public class PoisoningCraftState : CraftState
	{
		public override Type ToolType { get { return typeof( MixingSet ); } }
		private int m_BottleID;
		private string m_Name;
		private int m_PoisonDuration;
		private int m_ActingSpeed;
		private int m_Corrosivity;
		private int m_Duration;
		private int m_TotalEffects;
		
		private Dictionary<PoisonEffectEnum, int> m_PoisonEffects = new Dictionary<PoisonEffectEnum, int>();
		
		public string Name { get { return m_Name; } set { m_Name = value; } }
		public int BottleID { get { return m_BottleID; } set { m_BottleID = value; } }

		public PoisoningCraftState( Mobile crafter, BaseTool tool ) : base( crafter, tool, 10 )
		{
			m_BottleID = 3626;
			m_Name = "unknown potion";
			m_PoisonDuration = 0;
			m_ActingSpeed = 0;
			m_Corrosivity = 0;
			m_Duration = Toxin.MaxToxinDuration;
			m_TotalEffects = 0;
		}
		
		public override bool IsValidComponent( Item item )
		{
			return (item is IToxinIngredient);
		}
		
		public string HTMLEffects()
		{
			string ret = "<CENTER><BASEFONT COLOR=#5bae0a>";
			foreach ( KeyValuePair<PoisonEffectEnum, int> kvp in m_PoisonEffects ) 
			{
				ret += PoisonEffect.GetEffect( kvp.Key ).Name;
				ret += " (" + kvp.Value + ")";
				ret += "<BR>";
			}
			
			ret += "Corrosivity: " + m_Corrosivity + "<BR>";
			
			int duration = m_PoisonDuration;
			int seconds = duration % 60;
			int minutes = ((duration-=seconds) / 60) % 60;
			int hours = ((duration-=minutes*60) / 3600) % 24;
			int days = ((duration-=hours*3600) / 86400);
		
			string dur = (m_PoisonDuration < 0 ? "-" : "" ) + (days > 0 ? days + "d " : "") + (hours > 0 ? hours + "h " : "") +
					(minutes > 0 ? minutes + "m " : "") + (seconds > 0 ? seconds + "s" : "" );
			if ( dur == "" )
				dur = "0";
			ret += "Poison Duration: " + dur + "<BR>";
			ret += "Poison Acting Speed: " + m_ActingSpeed + "<BR>";
			

			return ret;
		}

		public override void Update()
		{
			if ( Components.Count == 0 )
			{
				m_PoisonDuration = m_ActingSpeed = m_Corrosivity = m_Duration = m_TotalEffects = 0;
				m_PoisonEffects = new Dictionary<PoisonEffectEnum, int>();
				return;
			}
			int avgSpeed, avgCorrosivity, avgDuration, totalEffects, totalComponents;
			avgSpeed = avgCorrosivity = avgDuration = totalEffects = totalComponents = 0;
			m_PoisonEffects = new Dictionary<PoisonEffectEnum, int>();
			
			foreach ( KeyValuePair<Type, int> kvp in Components ) 
			{
				Item instance = (Item)Activator.CreateInstance(kvp.Key);
				if (!(instance is IToxinIngredient))
					continue;
				
				IToxinIngredient ingredient = instance as IToxinIngredient;
				int amount = kvp.Value;
				totalComponents += amount;

				for (int i=1; i<=amount; i++) 
				{
					foreach ( KeyValuePair<PoisonEffectEnum, int> kvp2 in ingredient.Effects )
					{
						if ( !m_PoisonEffects.ContainsKey( kvp2.Key ) )
							m_PoisonEffects[kvp2.Key] = 0;
						
						int val = (int)(kvp2.Value * (1.0/i)); // diminishing returns, 100% for first, 50% for second, 33% for third...
						m_PoisonEffects[kvp2.Key] += val;
						totalEffects += val;
					}

					avgCorrosivity += ingredient.Corrosivity;
					avgDuration += ingredient.ToxinDuration;
					avgSpeed += ingredient.ToxinActingSpeed;
				}
			}

			m_ActingSpeed = avgSpeed / totalComponents;
			m_Corrosivity = avgCorrosivity / totalComponents;
			m_PoisonDuration = avgDuration / totalComponents;
			m_TotalEffects = totalEffects;
		}

		public override double CraftChance()
		{
			if ( Components.Count == 0 || m_TotalEffects == 0 )
				return 0.0;
			double skill = Crafter.Skills[SkillName.Poisoning].Value;
			double a = skill;
			if ( a == 0.0 ) // divide by zero
				return 0.0;
			double chance = 1.0/(m_TotalEffects/a);
			
			if ( chance > 1.0 )
				chance = 1.0;
			else if ( chance < 0.0 )
				chance = 0.0;
			
			return chance;
		}
		
		public override bool ConsumeResources()
		{
			Bottle emptyBottle = Crafter.Backpack.FindItemByType( typeof( Bottle ) ) as Bottle;
			if ( emptyBottle == null )
			{
				LastMessage = "You lack an empty bottle.";
				return false;
			}
			else
				return base.ConsumeResources();
		}
		
		public override void OnAfterCrafted( Item craftedItem )
		{
			Bottle emptyBottle = Crafter.Backpack.FindItemByType( typeof( Bottle ) ) as Bottle;
			if ( emptyBottle != null )
			{
				object bottleParent = emptyBottle.Parent;
				if ( !( bottleParent is BaseContainer ) || !((BaseContainer)bottleParent).TryDropItem( Crafter, craftedItem, false ) )
					Crafter.AddToBackpack( craftedItem );
				
				emptyBottle.Consume( 1 );
			}
		}
		
		public override Item CraftItem()
		{
			Toxin potion = new Toxin( m_BottleID );
			foreach ( KeyValuePair<PoisonEffectEnum, int> kvp in m_PoisonEffects )
				potion.AddEffect( kvp.Key, kvp.Value );
			potion.Duration = m_Duration;
			potion.Corrosivity = m_Corrosivity;
			potion.PoisonDuration = m_PoisonDuration;
			potion.ActingSpeed = m_ActingSpeed;
			potion.Name = m_Name;
			
			if( Crafter != null && Crafter is PlayerMobile )
				AwardXPCP( (PlayerMobile)Crafter );
			
			return potion;
		}
		
		private void AwardXPCP( PlayerMobile m )
		{
			m.Crafting = true;	
			Misc.LevelSystem.AwardMinimumXP( m, 3 );
			m.Crafting = false;
		}

		public override bool WriteRecipe( BaseRecipe recipe )
		{
			if ( !(recipe is ToxinRecipe) )
			{
				LastMessage = "That is not a valid toxin recipe.";
				return false;
			}
			
			if ( base.WriteRecipe( recipe ) )
			{
				ToxinRecipe trecipe = recipe as ToxinRecipe;
				trecipe.BottleID = m_BottleID;
				trecipe.ToxinName = m_Name;
				trecipe.Name = "toxin recipe for: " + m_Name;
				return true;
			}
			else 
				return false;
		}

		public override bool ReadRecipe( BaseRecipe recipe )
		{
			if ( !(recipe is ToxinRecipe) )
			{
				LastMessage = "That is not a valid toxin recipe.";
				return false;
			}
			
			if ( base.ReadRecipe( recipe ) )
			{
				ToxinRecipe trecipe = recipe as ToxinRecipe;
				m_BottleID = trecipe.BottleID;
				m_Name = trecipe.ToxinName;
				return true;
			}
			else 
				return false;
		}
	}
}
