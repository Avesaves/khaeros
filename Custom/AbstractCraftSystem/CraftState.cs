using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Craft
{
	/* A generic custom-component craft system */
	public class GumpComponent
	{
		private int m_Graphic;
		private Type m_Type;
		private int m_Hue;
		
		public int Graphic{ get{ return m_Graphic; } set { m_Graphic = value; } }
		public Type Type{ get{ return m_Type; } set { m_Type = value; } }
		public int Hue{ get{ return m_Hue; } set { m_Hue = value; } }
	}
	public abstract class CraftState
	{
		// gump display stuff
		private GumpComponent[] m_GumpComponents;
		public GumpComponent[] GumpComponents { get{ return m_GumpComponents; } }

		// crafting logic
		private Dictionary<Type, int> m_Components = new Dictionary<Type, int>(); // item, amount
		private Mobile m_Crafter;
		private BaseTool m_Tool;
		private string m_LastMessage;

		public Mobile Crafter { get{ return m_Crafter; } }
		public BaseTool Tool { get{ return m_Tool; } set{ m_Tool = value; } }
		public string LastMessage { get { return m_LastMessage; } set { m_LastMessage = value; } }
		public Dictionary<Type, int> Components { get { return m_Components; } }
		
		public virtual Type ToolType { get { return typeof( BaseTool ); } }

		public CraftState( Mobile crafter, BaseTool tool, int componentAmount )
		{
			m_Crafter = crafter;
			m_Tool = tool;
			m_GumpComponents = new GumpComponent[componentAmount];
			for ( int i = 0; i < componentAmount; i++ )
				m_GumpComponents[i] = new GumpComponent();
		}

		public bool AddComponent( Item item, int index ) { return AddComponent( item, index, false ); }
		public virtual bool AddComponent( Item item, int index, bool skipupdate )
		{
			if ( item == null )
			{
				m_LastMessage = "That's not a valid component.";
				return false;
			}
			Type type = item.GetType();
			if (!IsValidComponent( item ))
			{
				m_LastMessage = "You don't know how to make use of this component.";
				return false;
			}
		
			if ( GumpComponents[index].Type != null )
				RemoveComponent(index, true);
			
			if ( m_Components.ContainsKey( type ) )
				m_Components[type]++;
			else
				m_Components[type] = 1;
			
			GumpComponents[index].Type = type;
			GumpComponents[index].Graphic = item.ItemID;
			GumpComponents[index].Hue = item.Hue;

			if (!skipupdate)
			{
				m_LastMessage = "You successfully add the component.";
				Update();
			}
			
			return true;
		}

		public void RemoveComponent( int index ) { RemoveComponent( index, false ); }
		public virtual void RemoveComponent( int index, bool skipupdate )
		{
			Type type = GumpComponents[index].Type;
			if ( type == null )
				return;
			if ( !m_Components.ContainsKey( type ) )
				return;

			if ( m_Components[type] > 1 )
				m_Components[type]--;
			else
				m_Components.Remove( type );
			
			GumpComponents[index] = new GumpComponent();

			if (!skipupdate)
				Update();
		}
		
		public virtual bool IsValidComponent( Item item )
		{
			return true;
		}

		public virtual void Update()
		{
		}

		public virtual double CraftChance()
		{
			return 0.0;
		}
		
		public virtual int GetFinalHue() // hue of the most prevalent component
		{
			if ( m_Components.Count == 0 )
				return 0;

			KeyValuePair<Type, int> prevalentComponent = new KeyValuePair<Type, int>( null, 0 );
			foreach ( KeyValuePair<Type, int> kvp in m_Components )
			{
				if ( kvp.Value >= prevalentComponent.Value )
					prevalentComponent = kvp;
			}

			Item instance = (Item)Activator.CreateInstance( prevalentComponent.Key );
			return instance.Hue;
		}
		
		public virtual bool ConsumeResources()
		{
			Type[] types = new Type[m_Components.Count];
			int[] amounts = new int[m_Components.Count];
			int i = 0;
			
			foreach ( KeyValuePair<Type, int> kvp in m_Components ) // build the list so we can remove them faster
			{
				types[i] = kvp.Key;
				amounts[i] = kvp.Value;
				i++;
			}

			if ( m_Crafter.Backpack.ConsumeTotal( types, amounts ) != -1 ) // couldn't find them
			{
				LastMessage = "You lack the required resources.";
				return false; // couldn't consume them
			}
				
			return true;
		}
		
		public virtual void OnAfterCrafted( Item craftedItem )
		{
			craftedItem.Hue = GetFinalHue();
		}
		
		public virtual bool CanAttemptCraft()
		{
			if ( m_Components.Count == 0 )
			{
				m_LastMessage = "There is nothing to craft.";
				return false;
			}
			
			if ( m_Tool != null && (m_Tool.Deleted || m_Tool.UsesRemaining <= 0 || m_Tool.RootParent != m_Crafter) )
			{
				// lets see if we can find another tool in the player's pack
				Item[] items = m_Crafter.Backpack.FindItemsByType( ToolType );

				bool foundTool = false;
				foreach ( Item item in items )
				{
					BaseTool tool = item as BaseTool;
					if ( tool.UsesRemaining > 0 )
					{
						m_Tool = tool;
						foundTool = true;
						break;
					}
				}

				if ( !foundTool ) // could not find another tool
				{
					m_LastMessage = "You need the proper tool in your pack.";
					return false;
				}
			}
			
			return true;
		}
		
		public virtual Item CraftItem()
		{
			return null;
		}

		public bool AttemptCraft()
		{
			if ( !CanAttemptCraft() )
				return false;
			
			if ( !ConsumeResources() )
				return false;

			if ( m_Tool != null && --m_Tool.UsesRemaining < 1 )
				m_Tool.Delete();
			
			if ( CraftChance() > Utility.RandomDouble() )
			{
				Item item = CraftItem();
				OnAfterCrafted( item );
				
				m_Crafter.AddToBackpack( item );

				m_LastMessage = "You've successfully crafted the item.";
				return true; // craft succeeded
			}

			else
			{
				m_LastMessage = "You've failed your craft attempt and the resources have been lost.";
				return false;
			}
		}

		public virtual bool WriteRecipe( BaseRecipe recipe )
		{
			if ( recipe.RootParent != m_Crafter )
			{
				m_LastMessage = "That needs to be in your pack.";
				return false;
			}
			else if ( m_Components.Count == 0 )
			{
				m_LastMessage = "There are no components to write down.";
				return false;
			}

			recipe.ComponentTypes = new Type[m_GumpComponents.Length];
			for( int i = 0; i<m_GumpComponents.Length; i++ )
				recipe.ComponentTypes[i] = m_GumpComponents[i].Type;

			return true;
		}

		public virtual bool ReadRecipe( BaseRecipe recipe )
		{
			if ( recipe.ComponentTypes == null )
			{
				m_LastMessage = "This recipe is blank.";
				return false;
			}
			
			m_GumpComponents = new GumpComponent[recipe.ComponentTypes.Length];
			
			for ( int i=0; i<recipe.ComponentTypes.Length; i++ ) 
			{
				m_GumpComponents[i] = new GumpComponent();
				if ( recipe.ComponentTypes[i] == null )
					continue;

				Item instance = (Item)Activator.CreateInstance(recipe.ComponentTypes[i]);
				if ( !AddComponent( instance, i, true ) )
				{
					m_LastMessage = "You cannot use one or more of the listed components.";
					Update();
					return false;
				}
			}

			m_LastMessage = "You successfully add the components according to the recipe.";
			Update();
			return true; // success
		}
	}
}
