using System;
using Server;
using Server.Items;
using Server.Factions;
using Server.Targeting;
using Server.Mobiles;
using Server.Misc;

namespace Server.Engines.Craft 
{ 
	public class DefMasonry : CraftSystem 
	{ 
		public override SkillName MainSkill 
		{ 
			get{ return SkillName.Craftsmanship; } 
		} 

		public override int GumpTitleNumber 
		{ 
			get{ return 1044500; } // <CENTER>MASONRY MENU</CENTER> 
		} 

		private static CraftSystem m_CraftSystem; 

		public static CraftSystem CraftSystem 
		{ 
			get 
			{ 
				if ( m_CraftSystem == null ) 
					m_CraftSystem = new DefMasonry(); 

				return m_CraftSystem; 
			} 
		} 

		public override double GetChanceAtMin( CraftItem item ) 
		{ 
			return 0.0; // 0% 
		} 

		private DefMasonry() : base( 1, 1, 1.25 )// base( 1, 2, 1.7 ) 
		{ 
		} 

		public override bool RetainsColorFrom( CraftItem item, Type type )
		{
			return false;
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			PlayerMobile m = from as PlayerMobile;
			
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckTool( tool, from ) )
				return 1048146; // If you have a tool equipped, you must use that tool.
			else if ( !(from is PlayerMobile && ((PlayerMobile)from).Masonry) )
				return 1044633; // You havent learned stonecraft.
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.

			return 0;
		} 

		public override void PlayCraftEffect( Mobile from ) 
		{ 
			// no effects
			//if ( from.Body.Type == BodyType.Human && !from.Mounted ) 
			//	from.Animate( 9, 5, 1, true, false, 0 ); 
			//new InternalTimer( from ).Start(); 
		} 

		// Delay to synchronize the sound with the hit on the anvil 
		private class InternalTimer : Timer 
		{ 
			private Mobile m_From; 

			public InternalTimer( Mobile from ) : base( TimeSpan.FromSeconds( 0.7 ) ) 
			{ 
				m_From = from; 
			} 

			protected override void OnTick() 
			{ 
				m_From.PlaySound( 0x23D ); 
			} 
		} 

		public override int PlayEndingEffect( Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item ) 
		{ 
			if ( toolBroken ) 
				from.SendLocalizedMessage( 1044038 ); // You have worn out your tool 

			if ( failed ) 
			{ 
				if ( lostMaterial ) 
					return 1044043; // You failed to create the item, and some of your materials are lost. 
				else 
					return 1044157; // You failed to create the item, but no materials were lost. 
			} 
			else 
			{ 
				if ( quality == 0 ) 
					return 502785; // You were barely able to make this item.  It's quality is below average. 
				else if ( makersMark && quality == 5 ) 
					return 1044156; // You create an exceptional quality item and affix your maker's mark. 
				else if ( quality == 5 ) 
					return 1044155; // You create an exceptional quality item. 
				else             
					return 1044154; // You create the item. 
			} 
		} 

		public override void InitCraftList() 
		{ 
			// Decorations
			AddCraft( typeof( Vase ), 1044501, 1022888, 52.5, 102.5, typeof( Granite ), 1044514, 1, 1044513 );
			AddCraft( typeof( LargeVase ), 1044501, 1022887, 52.5, 102.5, typeof( Granite ), 1044514, 3, 1044513 );
			
			int index = AddCraft( typeof( SmallUrn ), 1044501, 1029244, 82.0, 132.0, typeof( Granite ), 1044514, 3, 1044513 );

			index = AddCraft( typeof( SmallTowerSculpture ), 1044501, 1029242, 82.0, 132.0, typeof( Granite ), 1044514, 3, 1044513 );


			// Furniture
			AddCraft( typeof( StoneChair ), 1044502, 1024635, 55.0, 105.0, typeof( Granite ), 1044514, 4, 1044513 );
			AddCraft( typeof( MediumStoneTableEastDeed ), 1044502, 1044508, 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
			AddCraft( typeof( MediumStoneTableSouthDeed ), 1044502, 1044509, 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
			AddCraft( typeof( LargeStoneTableEastDeed ), 1044502, 1044511, 55.0, 105.0, typeof( Granite ), 1044514, 9, 1044513 );
			AddCraft( typeof( LargeStoneTableSouthDeed ), 1044502, 1044512, 55.0, 105.0, typeof( Granite ), 1044514, 9, 1044513 );
            AddCraft( typeof( StatueSouth ), 1044050, "statue (south)", 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
            AddCraft( typeof( StatueSouth2 ), 1044050, "statue (south)", 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
            AddCraft( typeof( StatueNorth ), 1044050, "statue (north)", 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
            AddCraft( typeof( StatueWest ), 1044050, "statue (west)", 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
            AddCraft( typeof( StatueEast ), 1044050, "statue (east)", 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
            AddCraft( typeof( StatueEast2 ), 1044050, "statue (east)", 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
            AddCraft( typeof( StatueSouthEast ), 1044050, "statue (southeast)", 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
            AddCraft( typeof( BustSouth ), 1044050, "bust (south)", 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
            AddCraft( typeof( BustEast ), 1044050, "cbust (east)", 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
            AddCraft( typeof( StatuePegasus ), 1044050, "pegasus statue (south)", 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
            AddCraft( typeof( StatuePegasus2 ), 1044050, "pegasus statue (east)", 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
            AddCraft( typeof( StatuePegasus2 ), 1044050, "pegasus statue (east)", 55.0, 105.0, typeof( Granite ), 1044514, 6, 1044513 );
            AddCraft( typeof( GuardStatueEast ), 1044050, "guard statue (east)", 55.0, 105.0, typeof( Granite ), 1044514, 30, 1044513 );
            AddCraft( typeof( GuardStatueSouth ), 1044050, "guard statue (south)", 55.0, 105.0, typeof( Granite ), 1044514, 30, 1044513 );
            AddCraft( typeof( Pedestal ), 1044050, "pedestal", 55.0, 105.0, typeof( Granite ), 1044514, 10, 1044513 );
            AddCraft( typeof( LargePedestal ), 1044050, "large pedestal", 55.0, 105.0, typeof( Granite ), 1044514, 20, 1044513 );
			AddCraft( typeof( PlayerMadeStatueDeed ), 1044050, "personal statue", 55.0, 105.0, typeof( Granite ), 1044514, 50, 1044513 );
            
			SetSubRes( typeof( Granite ), 1044525 );

			/*AddSubRes( typeof( Granite ),			1044525, 00.0, 1044514, 1044526 );
			AddSubRes( typeof( CopperGranite ),		1044025, 75.0, 1044514, 1044527 );
			AddSubRes( typeof( BronzeGranite ),		1044026, 80.0, 1044514, 1044527 );
			AddSubRes( typeof( GoldGranite ),		1044027, 85.0, 1044514, 1044527 );
			AddSubRes( typeof( SilverGranite ),	    1044028, 90.0, 1044514, 1044527 );
			AddSubRes( typeof( ObsidianGranite ),	1044029, 95.0, 1044514, 1044527 );
			AddSubRes( typeof( SteelGranite ),  	1044030, 99.0, 1044514, 1044527 );*/
		}
	}
}
