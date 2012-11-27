using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Alchemy
{
	public class BrewingState
	{
		// gump display stuff
		private int[] m_IngredientPictures = new int [10];
		private Type[] m_IngredientTypes = new Type [10];
		private int[] m_IngredientHues = new int [10];

		public int[] IngredientPictures{ get{ return m_IngredientPictures; } }
		public Type[] IngredientTypes{ get{ return m_IngredientTypes; } }
		public int[] IngredientHues{ get{ return m_IngredientHues; } }

		// crafting logic
		private Dictionary<Type, int> m_Ingredients = new Dictionary<Type, int>(); // item, amount
		private Mobile m_Brewer;
		private Dictionary<CustomEffect, int> m_PotionEffects = new Dictionary<CustomEffect, int>(); // effectID, intensity
		private int m_PotionBooster;
		private int m_Range;
		private int m_Corrosivity;
		private int m_Duration;
		private bool m_InstantExplosion;
		private double m_TotalEffects;
		private string m_Name = "unknown potion";
		private PotionType m_Type;
		private int m_Bottle;
		private BaseTool m_Tool;

		public int Bottle{ get{ return m_Bottle; } set { m_Bottle = value; } }
		public string Name{ get{ return m_Name; } set{ m_Name = value; } }
		public PotionType Type{ get{ return m_Type; } set{ m_Type = value; } }
		public Mobile Brewer{ get{ return m_Brewer; } }
		public BaseTool Tool{ get{ return m_Tool; } set{ m_Tool = value; } }

		public BrewingState( Mobile brewer, BaseTool tool )
		{
			m_Brewer = brewer;
			m_Tool = tool;
			m_Type = PotionType.Drink; // default potion type
			m_Bottle = 3626;
		}

		public void AddIngredient( Type item ) { AddIngredient( item, false ); }

		public void AddIngredient( Type item, bool skipupdate )
		{
			if ( m_Ingredients.ContainsKey( item ) )
				m_Ingredients[item]++;
			else
				m_Ingredients[item] = 1;

			if (!skipupdate)
				UpdateEffects();
		}

		public void RemoveIngredient( Type item ) { RemoveIngredient( item, false ); }

		public void RemoveIngredient( Type item, bool skipupdate )
		{
			if ( !m_Ingredients.ContainsKey( item ) )
				return;

			if ( m_Ingredients[item] > 1 )
				m_Ingredients[item]--;
			else
				m_Ingredients.Remove( item );

			if (!skipupdate)
				UpdateEffects();
		}

		public void UpdateEffects()	// also updates difficulty
		{
			if ( m_Ingredients.Count == 0 )
			{
				m_PotionEffects = new Dictionary<CustomEffect, int>();
				m_PotionBooster = m_Duration = m_Corrosivity = m_Range = m_Duration = 0;
				m_TotalEffects = 0;
				m_InstantExplosion = false;
				return;
			}

			m_PotionEffects.Clear();
			bool instantExplosion = true;
			int totalherbs=0;
			double potionbooster = 1.0;
			int range = 0;
			int corrosivity = 0;
			int duration = 0;

			foreach ( KeyValuePair<Type, int> kvp in m_Ingredients ) 
			{
				Item instance = (Item)Activator.CreateInstance(kvp.Key);
				if (!(instance is IAlchemyIngredient))
					continue;
				
				IAlchemyIngredient ingredient = instance as IAlchemyIngredient;
				int amount = kvp.Value;
				totalherbs += amount;

				for (int i=1; i<=amount; i++) 
				{
					foreach ( KeyValuePair<CustomEffect, int> kvp2 in ingredient.Effects )
					{
						if ( !m_PotionEffects.ContainsKey( kvp2.Key ) )
							m_PotionEffects[kvp2.Key] = 0;
						
						m_PotionEffects[kvp2.Key] += (int)(kvp2.Value * (1.0/i));	// diminishing returns, 100% for first, 50% for second, 33% for third...
					}
                    if (instance is IDrinkIngredient && m_Type == PotionType.Drink)
                        potionbooster += (((IDrinkIngredient)instance).PotionBooster * 0.01) * (1.0 / i); // also diminishing returns for potion boosters
                    else if (instance is IBombIngredient && m_Type == PotionType.Bomb)
                    {
                        range += ((IBombIngredient)instance).Range; // add up range of all ingredients together
                        // if any ingredient has InstantEffect = false, then the potion will NOT be instant
                        if (instantExplosion == true && ((IBombIngredient)instance).InstantEffect == false)
                            instantExplosion = false;
                    }
                    else if (instance is IOilIngredient && m_Type == PotionType.Oil)
                    {
                        corrosivity += ((IOilIngredient)instance).Corrosivity; // add up corrosivity of all ingredients together
                        duration += ((IOilIngredient)instance).Duration;
                    }
				}
			}

			CustomEffect[] keyarray = new CustomEffect [m_PotionEffects.Count]; // store keys to amplify later
			int k = 0;
			foreach ( KeyValuePair<CustomEffect, int> kvp in m_PotionEffects ) // build the keyarray, as we can't change values during enumeration
				keyarray[k++] = kvp.Key;
			
			m_TotalEffects = 0;

			for ( int j = 0; j < k; j++ ) // amplify values
			{
				m_PotionEffects[keyarray[j]] = (int)(m_PotionEffects[keyarray[j]] * potionbooster);

				if ( m_PotionEffects[keyarray[j]] < 0 && m_Type == PotionType.Drink && ((PlayerMobile)m_Brewer).Feats.GetFeatLevel(FeatList.LowerSideEffects) > 0 )
				{
					double reduction = 0.1 * ((PlayerMobile)m_Brewer).Feats.GetFeatLevel(FeatList.LowerSideEffects); // adjust for different levels
					m_PotionEffects[keyarray[j]] -= (int)(m_PotionEffects[keyarray[j]] * reduction);
				}
					
				if ( m_PotionEffects[keyarray[j]] > 0 || ( m_Type == PotionType.Bomb || m_Type == PotionType.Oil ) )	
				{ // only positive effects count towards difficulty increase, unless it's a bomb/oil in which case, both do.
					m_TotalEffects+=Math.Abs( m_PotionEffects[keyarray[j]] );
				}
			}

			if ( m_Type == PotionType.Drink )
				m_PotionBooster = (int)(potionbooster - 1.0);
			else if ( m_Type == PotionType.Bomb )
			{
				m_Range = range / totalherbs;
				m_InstantExplosion = instantExplosion;
			}
			else if ( m_Type == PotionType.Oil )
			{
				m_Corrosivity = corrosivity / totalherbs;
				m_Duration = duration / totalherbs;
			}
				
		}

		public double CraftChance()
		{
			if ( m_Ingredients.Count == 0 )
				return 0.0;

			double a = m_Brewer.Skills[SkillName.Alchemy].Value;
			
			if ( m_Type == PotionType.Drink )
				a *= 1.9;
			else if ( m_Type == PotionType.Bomb )
				a *= 1.5;
			else
				a *= 1.2;
			
			double chance = (a - 0.3*m_TotalEffects) / 100.0;
			if ( chance > 1.0 )
				chance = 1.0;
			else if ( chance < 0.00 )
				chance = 0.00;

			return chance;
		}

		public string HTMLEffects()
		{
			string ret = "<CENTER><BASEFONT COLOR=#5bae0a>";
			foreach ( KeyValuePair<CustomEffect, int> kvp in m_PotionEffects ) 
			{
				if ( kvp.Value > 0 || m_Type == PotionType.Bomb || m_Type == PotionType.Oil )
				{
					ret += CustomPotionEffect.GetEffect( kvp.Key ).Name;
					ret += " (" + kvp.Value + ")";
					ret += "<BR>";
				}
			}

			if ( m_PotionBooster > 0 )
				ret += "Potion Booster (" + m_PotionBooster + ")<BR>";

			if ( m_Type == PotionType.Bomb )
			{
				ret += "Radius: " + m_Range + "<BR>";
				ret += "Explosion: " + ( m_InstantExplosion ? "Instant" : "Delayed" ) + "<BR>";
			}
			else if ( m_Type == PotionType.Oil )
			{
				ret += "Corrosivity: " + m_Corrosivity + "<BR>";
				
				int duration = m_Duration;
				int seconds = duration % 60;
				int minutes = ((duration-=seconds) / 60) % 60;
				int hours = ((duration-=minutes*60) / 3600) % 24;
				int days = ((duration-=hours*3600) / 86400);
			
				string dur = (m_Duration < 0 ? "-" : "" ) + (days > 0 ? days + "d " : "") + (hours > 0 ? hours + "h " : "") +
						(minutes > 0 ? minutes + "m " : "") + (seconds > 0 ? seconds + "s" : "" );
				ret += "Duration: " + dur + "<BR>";
			}
			

			return ret;
		}

		public string HTMLSideEffects()
		{
			string ret = "<CENTER><BASEFONT COLOR=#BF2121>";
			foreach ( KeyValuePair<CustomEffect, int> kvp in m_PotionEffects ) 
			{
				if (kvp.Value < 0)
				{
					ret += CustomPotionEffect.GetEffect( kvp.Key ).Name;
					ret += " (" + kvp.Value + ")";
					ret += "<BR>";
				}
			}

			if ( m_PotionBooster < 0 )
				ret += "Potion Booster (" + m_PotionBooster + ")<BR>";

			return ret;
		}
		
		public int GetFinalHue() // hue of the most prevalent ingredient
		{
			if ( m_Ingredients.Count == 0 )
				return 0;

			KeyValuePair<Type, int> prevalentIngredient = new KeyValuePair<Type, int>( null, 0 );
			foreach ( KeyValuePair<Type, int> kvp in m_Ingredients )
			{
				if ( kvp.Value >= prevalentIngredient.Value )
						prevalentIngredient = kvp;
			}

			Item instance = (Item)Activator.CreateInstance( prevalentIngredient.Key );
			return instance.Hue;
		}

		public int AttemptCraft()
		{
			Bottle emptyBottle = m_Brewer.Backpack.FindItemByType( typeof( Bottle ) ) as Bottle;
			if ( m_Ingredients.Count == 0 )
				return -3;	// there's nothing to craft

			else if ( emptyBottle == null )
				return -5;	// the bottle must be in your pack

			else if ( Type == PotionType.Bomb && m_Range < 0 )
				return -6;	// not enough bomb in this bomb potion, MISTAR
			else if ( Type == PotionType.Oil && m_Duration <= 0 )
				return -7;

			object bottleParent = emptyBottle.Parent;
			Type[] types = new Type[m_Ingredients.Count];
			int[] amounts = new int[m_Ingredients.Count];
			int i = 0;
			
			foreach ( KeyValuePair<Type, int> kvp in m_Ingredients ) // build the ingredients list so we can remove them faster
			{
				types[i] = kvp.Key;
				amounts[i] = kvp.Value;
				i++;
			}

			if ( m_Brewer.Backpack.ConsumeTotal( types, amounts ) != -1 ) // couldn't find them
				return -1; // not enough resources

			if ( CraftChance() > Utility.RandomDouble() )
			{
				CustomPotion potion;

				if ( Type == PotionType.Bomb )
				{
					potion = new BombPotion( m_Bottle );
					((BombPotion)potion).ExplosionRange = m_Range;
					((BombPotion)potion).InstantExplosion = m_InstantExplosion;
				}
				else if ( Type == PotionType.Oil )
				{
					potion = new OilPotion( m_Bottle );
					((OilPotion)potion).Duration = m_Duration;
					((OilPotion)potion).Corrosivity = m_Corrosivity;
				}
				else
					potion = new DrinkPotion( m_Bottle );

				emptyBottle.Consume( 1 );
				potion.Name = m_Name;
				//potion.Hue = GetFinalHue(); // This has been disabled since the ingredient hues look funny on bottles

                foreach (KeyValuePair<CustomEffect, int> kvp in m_PotionEffects)
                {
                    if (kvp.Value != 0)
                    {
                        if (m_Brewer is PlayerMobile && (kvp.Key.GetType() == typeof(MadnessEffect) || kvp.Key.GetType() == typeof(ConfusionEffect) || kvp.Key.GetType() == typeof(OintmentEffect)))
                        {
                            PlayerMobile pm = m_Brewer as PlayerMobile;
                            int effectBonus = 0;
                            switch (pm.Feats.GetFeatLevel(FeatList.Pathology))
                            {
                                case 0: break;
                                case 1: break;
                                case 2: effectBonus = (int)(kvp.Value * 0.1); break;
                                case 3: effectBonus = (int)(kvp.Value * 0.3); break;
                                default: break;
                            }
                            potion.AddEffect(kvp.Key, kvp.Value + effectBonus);
                        }
                        else
                            potion.AddEffect(kvp.Key, kvp.Value);
                    }
                }

				if ( !( bottleParent is BaseContainer ) || !((BaseContainer)bottleParent).TryDropItem( m_Brewer, potion, false ) )
					m_Brewer.AddToBackpack( potion );

				return 1; // craft succeeded
			}

			else
				return -2; // craft failed
		}

		public bool WriteFormula( Item item )
		{
			if ( m_Ingredients.Count == 0 )
				return false;

			AlchemicalFormula formula = item as AlchemicalFormula;
			formula.Name = "alchemical formula for: " + m_Name;
			formula.PotionName = m_Name;
			formula.PotionType = m_Type;
			formula.Bottle = m_Bottle;
			Array.Copy( m_IngredientTypes, formula.IngredientTypes, formula.IngredientTypes.Length );

			m_Brewer.AddToBackpack( formula );

			return true;
		}

		public int ReadFormula( AlchemicalFormula formula )
		{
			m_Name = formula.PotionName;
			m_Type = formula.PotionType;
			m_Bottle = formula.Bottle;
			Array.Copy( formula.IngredientTypes, m_IngredientTypes, m_IngredientTypes.Length );

			for ( int i=0; i<m_IngredientTypes.Length; i++ ) 
			{
				if ( m_IngredientTypes[i] == null )
					continue;

				Item instance = (Item)Activator.CreateInstance(m_IngredientTypes[i]); // saves us some space
				if (!(instance is IAlchemyIngredient))
					return -2;

				IAlchemyIngredient alchemyingredient = instance as IAlchemyIngredient;

				switch ( (int)m_Type )
				{
					case (int)PotionType.Drink:
					{
						if ( !(instance is IDrinkIngredient) || !((IDrinkIngredient)instance).CanUse( m_Brewer ))
							return -1; // don't know how to use that ingredient
						break;
					}

					case (int)PotionType.Bomb:
					{
						if ( !(instance is IBombIngredient) || !((IBombIngredient)instance).CanUse( m_Brewer ))
							return -1; // don't know how to use that ingredient
						break;
					}

					case (int)PotionType.Oil:
					{
						if ( !(instance is IOilIngredient) || !((IOilIngredient)instance).CanUse( m_Brewer ))
							return -1; // don't know how to use that ingredient
						break;
					}
				}

				m_IngredientPictures[i] = instance.ItemID;
				m_IngredientHues[i] = instance.Hue;
				AddIngredient( m_IngredientTypes[i], true ); // skip update
			}

			UpdateEffects();
			return 1; // success
		}
	}
}
