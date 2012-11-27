using System;
using Server;
using Server.Targeting;
using Server.Items;

namespace Server.Engines.Craft
{
	public class Resmelt
	{
		private static bool FailedToSmelt = true; //used for jewelry smelting
	
		public Resmelt()
		{
		}

		public static void Do( Mobile from, CraftSystem craftSystem, BaseTool tool )
		{
			int num = craftSystem.CanCraft( from, tool, null );

			if ( num > 0 )
			{
				from.SendGump( new CraftGump( from, craftSystem, tool, num ) );
			}
			else
			{
				from.Target = new InternalTarget( craftSystem, tool );
				from.SendLocalizedMessage( 1044273 ); // Target an item to recycle.
			}
		}
		
		public static bool SmeltJewelry( Mobile crafter, Item jewelry )
		{
			try {
            	bool anvil;
            	bool forge;
            	
            	Engines.Craft.DefBlacksmithy.CheckAnvilAndForge( crafter, 2, out anvil, out forge );
            	
            	if( !forge )
            	{
            		crafter.SendMessage( "You need to be standing near a forge to smelt jewelry." );
            		return false;
            	}			
			
				FailedToSmelt = true;
				jewelry.Delete();

				if (Utility.RandomDouble() < .37) {
					crafter.AddToBackpack( (((BaseJewel)jewelry).getGem()) );
					crafter.SendMessage("You were able to salvage a gemstone.");
					FailedToSmelt = false;
				}
				if (Utility.RandomDouble() < .37) {
					crafter.AddToBackpack( (((BaseJewel)jewelry).getMetal()) );
					crafter.SendMessage("You were able to recover some metal.");
					FailedToSmelt = false;
				}
				
				if (FailedToSmelt)
					crafter.SendMessage("You couldn't recover any materials from this one.");

				crafter.PlaySound( 0x2A );
				crafter.PlaySound( 0x240 );
				return true;
				}
				
				catch{
				}
				
				return false;
		}
		
		public static bool PublicResmelt( Mobile from, Item item, CraftResource resource, CraftSystem m_CraftSystem )
		{
			try
			{
			
				if ( CraftResources.GetType( resource ) != CraftResourceType.Metal )
					return false;

				CraftResourceInfo info = CraftResources.GetInfo( resource );

				if ( info == null || info.ResourceTypes.Length == 0 )
					return false;

				CraftItem craftItem = m_CraftSystem.CraftItems.SearchFor( item.GetType() );

				if ( craftItem == null || craftItem.Ressources.Count == 0 )
					return false;

				CraftRes craftResource = craftItem.Ressources.GetAt( 0 );

				if ( craftResource.Amount < 2 )
					return false; // Not enough metal to resmelt

				Type resourceType = info.ResourceTypes[0];
				Item ingot = (Item)Activator.CreateInstance( resourceType );

				if ( item is DragonBardingDeed || item is BaseArmor || item is BaseWeapon || item is BaseClothing )
					ingot.Amount = craftResource.Amount / 2;
				else
					ingot.Amount = 1;

				item.Delete();
				from.AddToBackpack( ingot );

				from.PlaySound( 0x2A );
				from.PlaySound( 0x240 );
				return true;
			}
			catch
			{
			}

			return false;
		}

		public static void PublicOnTarget( Mobile from, object targeted, CraftSystem m_CraftSystem, BaseTool m_Tool )
		{
			int num = m_CraftSystem.CanCraft( from, m_Tool, null );

			if ( num > 0 )
			{
				from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, num ) );
			}
			else
			{
				bool success = false;
				bool isStoreBought = false;

				if ( targeted is BaseArmor )
				{
					success = PublicResmelt( from, (BaseArmor)targeted, ((BaseArmor)targeted).Resource, m_CraftSystem );
					isStoreBought = false;
				}
				else if ( targeted is BaseWeapon )
				{
					success = PublicResmelt( from, (BaseWeapon)targeted, ((BaseWeapon)targeted).Resource, m_CraftSystem );
					isStoreBought = false;
				}
				else if ( targeted is DragonBardingDeed )
				{
					success = PublicResmelt( from, (DragonBardingDeed)targeted, ((DragonBardingDeed)targeted).Resource, m_CraftSystem );
					isStoreBought = false;
				}
				else if ( targeted is BaseJewel )
				{
					success = SmeltJewelry( from, (BaseJewel)targeted );
					isStoreBought = false;
				}

				if ( success )
					from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, isStoreBought ? 500418 : 1044270 ) ); // You melt the item down into ingots.
				else
					from.SendGump( new CraftGump( from, m_CraftSystem, m_Tool, 1044272 ) ); // You can't melt that down into ingots.
			}
		}

		private class InternalTarget : Target
		{
			private CraftSystem m_CraftSystem;
			private BaseTool m_Tool;

			public InternalTarget( CraftSystem craftSystem, BaseTool tool ) :  base ( 2, false, TargetFlags.None )
			{
				m_CraftSystem = craftSystem;
				m_Tool = tool;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				Resmelt.PublicOnTarget( from, targeted, m_CraftSystem, m_Tool );
			}
		}
	}
}
