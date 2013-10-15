using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Multis;

namespace Server.Engines.Craft
{
	public class DefCarpentry : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Carpentry;	}
		}

		public override int GumpTitleNumber
		{
			get { return 1044004; } // <CENTER>CARPENTRY MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefCarpentry();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.5; // 50%
		}

		private DefCarpentry() : base( 1, 1, 4.00 )// base( 1, 1, 3.0 )
		{
		}
		
		public static bool TestRace( PlayerMobile m, Nation nation1 )
		{
			if( m.Nation == nation1 )
				return true;
			
			return false;
		}
		
		public static bool TestRace( PlayerMobile m, Nation nation1, Nation nation2 )
		{
			if( TestRace( m, nation1 ) || TestRace( m, nation2 ) )
				return true;
			
			return false;			
		}
		
		public static bool TestRace( PlayerMobile m, Nation nation1, Nation nation2, Nation nation3 )
		{
			if( TestRace( m, nation1 ) || TestRace( m, nation2 ) || TestRace( m, nation3 ) )
				return true;
			
			return false;
		}
		
		public static bool TestRace( PlayerMobile m, Nation nation1, Nation nation2, Nation nation3, Nation nation4 )
		{
			if( TestRace( m, nation1 ) || TestRace( m, nation2 ) || TestRace( m, nation3 ) || TestRace( m, nation4 ) )
				return true;
			
			return false;
		}
		
		public static bool TestRace( PlayerMobile m, Nation nation1, Nation nation2, Nation nation3, Nation nation4, Nation nation5 )
		{
			if( TestRace( m, nation1 ) || TestRace( m, nation2 ) || TestRace( m, nation3 ) || TestRace( m, nation4 ) || TestRace( m, nation5 ) )
				return true;
			
			return false;
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.
			
			else if( from is PlayerMobile && itemType != null )
			{
				PlayerMobile m = from as PlayerMobile;
				/*
				if( itemType == typeof( ShortMusicStand ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TallMusicStand ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Throne ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ShortCabinet ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TallCabinet ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FancyDrawer ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( LapHarp ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Harp ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Lute ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( WritingTable ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FancyArmoire ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Armoire ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SpikedClub ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoneStaff ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( DruidStaff ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ClericCrook ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ProphetDiviningRod ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MedicineManFetish ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Boomerang ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoiledLeatherShield ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( LeatherShield ) && !TestRace( m, Nation.Mhordul ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoatPart ) && m.Feats.GetFeatLevel(FeatList.Shipwright) < 1 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( SmallBoatDeed ) && m.Feats.GetFeatLevel(FeatList.Shipwright) < 1 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( MediumBoatDeed ) && m.Feats.GetFeatLevel(FeatList.Shipwright) < 2 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( LargeBoatDeed ) && m.Feats.GetFeatLevel(FeatList.Shipwright) < 3 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( SmallBoatDeed ) && m.Nation == Nation.Tirebladd )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MediumBoatDeed ) && m.Nation == Nation.Tirebladd )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( LargeBoatDeed ) && m.Nation == Nation.Tirebladd )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SouthernLowChair ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnateSouthernChair ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( CozySouthernChair ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SouthernBedEastDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SouthernBedSouthDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TallSouthernBedEastDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TallSouthernBedSouthDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SouthernDresserEastDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SouthernDresserSouthDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SouthernBenchEastDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SouthernBenchSouthDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FancySouthernTableEastDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FancySouthernTableSouthDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnateSouthernTableEastDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnateSouthernTableSouthDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SouthernWashBasinEastDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SouthernWashBasinSouthDeed ) && m.Nation != Nation.Southern )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SmallDragonBoatDeed ) && m.Feats.GetFeatLevel(FeatList.Shipwright) < 1 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( MediumDragonBoatDeed ) && m.Feats.GetFeatLevel(FeatList.Shipwright) < 2 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( LargeDragonBoatDeed ) && m.Feats.GetFeatLevel(FeatList.Shipwright) < 3 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( SmallDragonBoatDeed ) && m.Nation != Nation.Tirebladd )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MediumDragonBoatDeed ) && m.Nation != Nation.Tirebladd )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( LargeDragonBoatDeed ) && m.Nation != Nation.Tirebladd )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoneShield ) && m.Nation != Nation.Mhordul )
					return 1063491; // Your race cannot craft that item.
					*/
			}

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			// no animation
			//if ( from.Body.Type == BodyType.Human && !from.Mounted )
			//	from.Animate( 9, 5, 1, true, false, 0 );

			from.PlaySound( 0x23D );
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
			int index = -1;

			// Other Items
			index =	AddCraft( typeof( Board ),				1044294, 1027127,	 0.0,   0.0,	typeof( Log ), 1044466,  1, 1044465 );
			SetUseAllRes( index, true );
			AddCraft( typeof( BoatPart ),					1044294, "boat part",	30.0,  60.0,	typeof( Log ), 1044041,  20, 1044351 );
			index = AddCraft( typeof( Barrel ),				1044294, 1023703, 10.0, 45.0, typeof( Log ), 1044041, 5, 1044351 );
			AddRes( index, typeof( BarrelLid ), 1015103, 2, 502910 );
			AddRes( index, typeof( BarrelStaves ), 1015102, 2, 502910 );
			AddRes( index, typeof( BarrelHoops ), 1011228, 2, 502910 );
            index = AddCraft( typeof( Keg ), 1044294, "keg", 10.0, 45.0, typeof( Log ), 1044041, 2, 1044351 );
            AddRes( index, typeof( BarrelLid ), 1015103, 2, 502910 );
            AddRes( index, typeof( BarrelStaves ), 1015102, 2, 502910 );
            AddRes( index, typeof( BarrelHoops ), 1011228, 2, 502910 );
			AddCraft( typeof( Beam ),				1044294, "beam",	5.0,  25.0,	typeof( Log ), 1044041,  1, 1044351 );
			AddCraft( typeof( BarrelStaves ),				1044294, 1027857,	5.0,  25.0,	typeof( Log ), 1044041,  5, 1044351 );
			AddCraft( typeof( BarrelLid ),					1044294, 1027608,	10.0,  35.0,	typeof( Log ), 1044041,  4, 1044351 );
			AddCraft( typeof( ShortMusicStand ),			1044294, 1044313,	80.0, 105.9,	typeof( Log ), 1044041, 15, 1044351 );
			AddCraft( typeof( TallMusicStand ),				1044294, 1044315,	80.0, 105.0,	typeof( Log ), 1044041, 20, 1044351 );
			AddCraft( typeof( Easle ),						1044294, 1044317,	85.0, 110.0,	typeof( Log ), 1044041, 20, 1044351 );
			index = AddCraft( typeof( FishingPole ), Core.AOS ? 1044294 : 1044295, 1023519, 70.0, 95.0, typeof( Log ), 1044041, 5, 1044351 ); //This is in the categor of Other during AoS
			AddRes( index, typeof( Cloth ), 1044286, 5, 1044287 );
			AddCraft( typeof( Torch ),						1044294, "torch",	5.0, 20.0,		typeof( Log ), 1044041, 2, 1044351 );
			
			AddCraft( typeof( WoodenBear ),					1044294, "wooden bear",	5.0, 20.0,	typeof( Log ), 1044041, 3, 1044351 );
			AddCraft( typeof( WoodenRat ),					1044294, "wooden rat",	5.0, 20.0,	typeof( Log ), 1044041, 3, 1044351 );
			AddCraft( typeof( WoodenChicken ),				1044294, "wooden chicken",	5.0, 20.0,	typeof( Log ), 1044041, 3, 1044351 );
			AddCraft( typeof( WoodenDragon ),				1044294, "wooden dragon",	5.0, 20.0,	typeof( Log ), 1044041, 3, 1044351 );
			AddCraft( typeof( WoodenCrocodile ),			1044294, "wooden crocodile",	5.0, 20.0,	typeof( Log ), 1044041, 3, 1044351 );
			AddCraft( typeof( WoodenOgre ),					1044294, "wooden ogre",	5.0, 20.0,	typeof( Log ), 1044041, 3, 1044351 );
			AddCraft( typeof( WoodenPanther ),				1044294, "wooden panther",	5.0, 20.0,	typeof( Log ), 1044041, 3, 1044351 );
			AddCraft( typeof( WoodenCat ),					1044294, "wooden cat",	5.0, 20.0,	typeof( Log ), 1044041, 3, 1044351 );
			AddCraft( typeof( WoodenBird ),					1044294, "wooden bird",	5.0, 20.0,	typeof( Log ), 1044041, 3, 1044351 );
			AddCraft( typeof( WoodenDog ),					1044294, "wooden dog",	5.0, 20.0,	typeof( Log ), 1044041, 3, 1044351 );
			AddCraft( typeof( WoodenHorse ),				1044294, "wooden horse",	5.0, 20.0,	typeof( Log ), 1044041, 3, 1044351 );
			AddCraft( typeof( WoodenWolf ),					1044294, "wooden wolf",	5.0, 20.0,	typeof( Log ), 1044041, 3, 1044351 );
			
			// Furniture
			AddCraft( typeof( Nightstand ),					1044291, "nightstand",	40.0,  65.0,	typeof( Log ), 1044041,  17, 1044351 );
			AddCraft( typeof( Cot ),						1044291, "cot",	40.0,  65.0,	typeof( Log ), 1044041,  17, 1044351 );
			AddCraft( typeof( ShopCounter ),				1044291, "shop counter",	40.0,  65.0,	typeof( Log ), 1044041,  21, 1044351 );
			AddCraft( typeof( FootStool ),					1044291, 1022910,	10.0,  35.0,	typeof( Log ), 1044041,  9, 1044351 );
			AddCraft( typeof( BambooStool ),				1044291, "Bamboo Stool",	10.0,  35.0,	typeof( Log ), 1044041,  9, 1044351 );
			AddCraft( typeof( Stool ),						1044291, 1022602,	10.0,  35.0,	typeof( Log ), 1044041,  9, 1044351 );
			AddCraft( typeof( BambooChair ),				1044291, 1044300,	20.0,  45.0,	typeof( Log ), 1044041, 13, 1044351 );
			AddCraft( typeof( WoodenChair ),				1044291, 1044301,	20.0,  45.0,	typeof( Log ), 1044041, 13, 1044351 );
			AddCraft( typeof( FancyWoodenChairCushion ),	1044291, 1044302,	40.0,  65.0,	typeof( Log ), 1044041, 15, 1044351 );
			AddCraft( typeof( WoodenChairCushion ),			1044291, 1044303,	40.0,  65.0,	typeof( Log ), 1044041, 13, 1044351 );
			AddCraft( typeof( WoodenBench ),				1044291, 1022860,	50.0,  75.0,	typeof( Log ), 1044041, 17, 1044351 );
			AddCraft( typeof( WoodenThrone ),				1044291, 1044304,	50.0,  75.0,	typeof( Log ), 1044041, 17, 1044351 );
			AddCraft( typeof( Throne ),						1044291, 1044305,	75.0,  100.0,	typeof( Log ), 1044041, 19, 1044351 );
			AddCraft( typeof( WritingTable ),				1044291, 1022890,	65.0,  90.0,	typeof( Log ), 1044041, 17, 1044351 );
			AddCraft( typeof( YewWoodTable ),				1044291, 1044307,	65.0,  90.0,	typeof( Log ), 1044041, 23, 1044351 );
			AddCraft( typeof( LargeTable ),					1044291, 1044308,	85.0, 110.0,	typeof( Log ), 1044041, 27, 1044351 );
			AddCraft( typeof( BarPart ),					1044291, "Bar Part",	85.0, 110.0,	typeof( Log ), 1044041, 27, 1044351 );
			AddCraft( typeof( ClothTable ),					1044291, "cloth table",	85.0, 110.0,	typeof( Log ), 1044041, 27, 1044351 );
			AddCraft( typeof( LogTable ),					1044291, "log table",	85.0, 110.0,	typeof( Log ), 1044041, 27, 1044351 );
			AddCraft( typeof( RattanTable ),				1044291, "rattan table",	85.0, 110.0,	typeof( Log ), 1044041, 27, 1044351 );
			AddCraft( typeof( LargeFancyTable ),			1044291, "fancy table",	85.0, 110.0,	typeof( Log ), 1044041, 27, 1044351 );
			AddCraft( typeof( LargeOakTable ),				1044291, "oak table",	85.0, 110.0,	typeof( Log ), 1044041, 27, 1044351 );
			AddCraft( typeof( AlchemistTableEastAddon ),			1044291, "Alchemy Table East",	85.0, 110.0,	typeof( Log ), 1044041, 27, 1044351 );
			AddCraft( typeof( AlchemistTableSouthAddon ),			1044291, "Alchemy Table South",	85.0, 110.0,	typeof( Log ), 1044041, 27, 1044351 );
			
			AddCraft( typeof( ElegantLowTable ),			1044291, 1030265,	80.0, 105.0,	typeof( Log ), 1044041, 20, 1044351 );
			AddCraft( typeof( PlainLowTable ),				1044291, 1030266,	80.0, 105.0,	typeof( Log ), 1044041, 20, 1044351 );
			AddCraft( typeof( OrnateSouthernChair ),			1044291, "ornate Southern chair",	80.0, 105.0,	typeof( Log ), 1044041, 12, 1044351 );
			AddCraft( typeof( CozySouthernChair ),				1044291, "cozy Southern chair",	80.0, 105.0,	typeof( Log ), 1044041, 13, 1044351 );
			AddCraft( typeof( SouthernLowChair ),			1044291, "Southern low chair",	80.0, 105.0,	typeof( Log ), 1044041, 12, 1044351 );


			// Containers
			AddCraft( typeof( WoodenBox ),					1044292, 1023709,	20.0,  45.0,	typeof( Log ), 1044041, 10, 1044351 );
			AddCraft( typeof( SmallCrate ),					1044292, 1044309,	10.0,  35.0,	typeof( Log ), 1044041, 8 , 1044351 );
			AddCraft( typeof( MediumCrate ),				1044292, 1044310,	30.0,  55.0,	typeof( Log ), 1044041, 15, 1044351 );
			AddCraft( typeof( LargeCrate ),					1044292, 1044311,	45.0,  70.0,	typeof( Log ), 1044041, 18, 1044351 );
			AddCraft( typeof( WoodenChest ),				1044292, 1023650,	75.0,  100.0,	typeof( Log ), 1044041, 20, 1044351 );
			AddCraft( typeof( EmptyBookcase ),				1044292, 1022718,	30.0,  50.0,	typeof( Log ), 1044041, 25, 1044351 );
			AddCraft( typeof( FancyArmoire ),				1044292, 1044312,	85.0, 110.0,	typeof( Log ), 1044041, 35, 1044351 );
			AddCraft( typeof( Armoire ),					1044292, 1022643,	85.0, 110.0,	typeof( Log ), 1044041, 35, 1044351 );	
			AddCraft( typeof( WorkersBookcase ),			1044292, "Workers Bookcase",	30.0,  50.0,	typeof( Log ), 1044041, 25, 1044351 );
			index = AddCraft( typeof( TallCabinet ),	1044292, 1030261, 90.0, 110.0,	typeof( Log ), 1044041, 35, 1044351 );
			index = AddCraft( typeof( SideCabinet ),	1044292, "side cabinet", 90.0, 110.0,	typeof( Log ), 1044041, 35, 1044351 );
			index = AddCraft( typeof( ShortCabinet ),	1044292, 1030263, 90.0, 110.0,	typeof( Log ), 1044041, 35, 1044351 );

			index = AddCraft( typeof( MapleArmoire ),	1044292, 1030328, 90.0, 110.0,	typeof( Log ), 1044041, 40, 1044351 );

			index = AddCraft( typeof( CherryArmoire ),	1044292, 1030328, 90.0, 110.0,	typeof( Log ), 1044041, 40, 1044351 );
			
			AddCraft( typeof( ElegantArmoire ),				1044292, "elegant armoire",	85.0, 110.0,	typeof( Log ), 1044041, 25, 1044351 );	
			AddCraft( typeof( FullBookcase ),				1044292, "full bookcase",	45.0, 60.0,	typeof( Log ), 1044041, 25, 1044351 );	
			AddCraft( typeof( Drawer ),						1044292, "chest of drawers",	25.0, 40.0,	typeof( Log ), 1044041, 15, 1044351 );	
			AddCraft( typeof( FancyDrawer ),				1044292, "fancy chest of drawers",	45.0, 60.0,	typeof( Log ), 1044041, 15, 1044351 );	

			// Staves and Shields
			AddCraft( typeof( ClericCrook ), 1044295, "cleric's crook", 80.0, 105.0, typeof( Log ), 1044041, 12, 1044351 );
			AddCraft( typeof( ProphetDiviningRod ), 1044295, "prophet's divining rod", 80.0, 120.0, typeof( Log ), 1044041, 8, 1044351 );
			AddCraft( typeof( DruidStaff ), 1044295, "druid's staff", 80.0, 120.0, typeof( Log ), 1044041, 12, 1044351 );
			AddCraft( typeof( QuarterStaff ), 1044295, 1023721, 75.0, 90.0, typeof( Log ), 1044041, 12, 1044351 );
			AddCraft( typeof( GnarledStaff ), 1044295, 1025112, 80.0, 105.0, typeof( Log ), 1044041, 12, 1044351 );
			AddCraft( typeof( BlackStaff ), 1044295, "black staff", 80.0, 105.0, typeof( Log ), 1044041, 12, 1044351 );
			AddCraft( typeof( WoodenShield ), 1044295, 1027034, 50.0, 75.0, typeof( Log ), 1044041, 9, 1044351 );
			index = AddCraft( typeof( LeatherShield ), 1044295, "leather shield", 80.0, 120.0, typeof( Log ), 1044041, 4, 1044351 );
			AddRes( index, typeof( Leather ), 1044462, 4, 1044463 );
			index = AddCraft( typeof( BoiledLeatherShield ), 1044295, "boiled leather shield", 80.0, 120.0, typeof( Log ), 1044041, 4, 1044351 );
			AddRes( index, typeof( Leather ), 1044462, 4, 1044463 );
			AddCraft( typeof( WoodenKiteShield ), 1044295, "wooden kite shield", 60.0, 95.0, typeof( Log ), 1044041, 10, 1044351 );			
			index = AddCraft( typeof( SpikedClub ), 1044295, "spiked club", 55.0, 70.0, typeof( Log ), 1044041, 12, 1044351 );
			AddRes( index, typeof( IronIngot ), 1044036, 5, 1044037 );
			AddCraft( typeof( Club ), 1044295, "club", 45.0, 60.0, typeof( Log ), 1044041, 12, 1044351 );
			
		

			// Instruments
			index = AddCraft( typeof( LapHarp ), 1044293, 1023762, 65.0, 90.0, typeof( Log ), 1044041, 20, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 10, 1044287 );

			index = AddCraft( typeof( Harp ), 1044293, 1023761, 80.0, 105.0, typeof( Log ), 1044041, 35, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 15, 1044287 );
			
			index = AddCraft( typeof( Drums ), 1044293, 1023740, 60.0, 80.0, typeof( Log ), 1044041, 20, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 10, 1044287 );
			
			index = AddCraft( typeof( Lute ), 1044293, 1023763, 70.0, 95.0, typeof( Log ), 1044041, 25, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 10, 1044287 );
			
			index = AddCraft( typeof( Tambourine ), 1044293, 1023741, 60.0, 80.0, typeof( Log ), 1044041, 15, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 10, 1044287 );

			index = AddCraft( typeof( TambourineTassel ), 1044293, 1044320, 60.0, 80.0, typeof( Log ), 1044041, 15, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 15, 1044287 );	

			// Misc
			index = AddCraft( typeof( SmallBedSouthDeed ), 1044290, 1044321, 95.0, 115.0, typeof( Log ), 1044041, 100, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 100, 1044287 );
			index = AddCraft( typeof( SmallBedEastDeed ), 1044290, 1044322, 95.0, 115.0, typeof( Log ), 1044041, 100, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 100, 1044287 );
			index = AddCraft( typeof( LargeBedSouthDeed ), 1044290,1044323, 95.0, 115.0, typeof( Log ), 1044041, 150, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 150, 1044287 );
			index = AddCraft( typeof( LargeBedEastDeed ), 1044290, 1044324, 95.0, 115.0, typeof( Log ), 1044041, 150, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 150, 1044287 );
			AddCraft( typeof( DartBoardSouthDeed ), 1044290, 1044325, 15.0, 40.0, typeof( Log ), 1044041, 5, 1044351 );
			AddCraft( typeof( DartBoardEastDeed ), 1044290, 1044326, 15.0, 40.0, typeof( Log ), 1044041, 5, 1044351 );
			AddCraft( typeof( BallotBoxDeed ), 1044290, 1044327, 45.0, 70.0, typeof( Log ), 1044041, 5, 1044351 );

			AddCraft( typeof( PlayerBBEast ), 1044290, 1062420, 85.0, 110.0, typeof( Log ), 1044041, 50, 1044351 );
			AddCraft( typeof( PlayerBBSouth ), 1044290, 1062421, 85.0, 110.0, typeof( Log ), 1044041, 50, 1044351 );
			
			index = AddCraft( typeof( SouthernBedEastDeed ), 1044290, "Southern bed (east)", 95.0, 115.0, typeof( Log ), 1044041, 100, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 100, 1044287 );
			index = AddCraft( typeof( SouthernBedSouthDeed ), 1044290, "Southern bed (south)", 95.0, 115.0, typeof( Log ), 1044041, 100, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 100, 1044287 );
			index = AddCraft( typeof( TallSouthernBedEastDeed ), 1044290, "ornate Southern bed (east)", 95.0, 115.0, typeof( Log ), 1044041, 150, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 150, 1044287 );
			index = AddCraft( typeof( TallSouthernBedSouthDeed ), 1044290, "ornate Southern bed (south)", 95.0, 115.0, typeof( Log ), 1044041, 150, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 150, 1044287 );
			AddCraft( typeof( SouthernDresserEastDeed ), 1044290, "Southern dresser (east)", 95.0, 115.0, typeof( Log ), 1044041, 35, 1044351 );
			AddCraft( typeof( SouthernDresserSouthDeed ), 1044290, "Southern dresser (south)", 95.0, 115.0, typeof( Log ), 1044041, 35, 1044351 );
			AddCraft( typeof( SouthernBenchEastDeed ), 1044290, "Southern bench (east)", 95.0, 115.0, typeof( Log ), 1044041, 20, 1044351 );
			AddCraft( typeof( SouthernBenchSouthDeed ), 1044290, "Southern bench (south)", 95.0, 115.0, typeof( Log ), 1044041, 20, 1044351 );
			AddCraft( typeof( FancySouthernTableEastDeed ), 1044290, "fancy Southern table (east)", 95.0, 115.0, typeof( Log ), 1044041, 40, 1044351 );
			AddCraft( typeof( FancySouthernTableSouthDeed ), 1044290, "fancy Southern table (south)", 95.0, 115.0, typeof( Log ), 1044041, 40, 1044351 );
			AddCraft( typeof( OrnateSouthernTableEastDeed ), 1044290, "ornate Southern table (east)", 95.0, 115.0, typeof( Log ), 1044041, 40, 1044351 );
			AddCraft( typeof( OrnateSouthernTableSouthDeed ), 1044290, "ornate Southern table (south)", 95.0, 115.0, typeof( Log ), 1044041, 40, 1044351 );
			AddCraft( typeof( SouthernWashBasinEastDeed ), 1044290, "Southern wash basin (east)", 95.0, 115.0, typeof( Log ), 1044041, 50, 1044351 );
			AddCraft( typeof( SouthernWashBasinSouthDeed ), 1044290, "Southern wash basin (south)", 95.0, 115.0, typeof( Log ), 1044041, 50, 1044351 );

            AddCraft( typeof( RightPegleg ), 1044295, "pegleg (right)", 80.0, 120.0, typeof( Log ), 1044041, 8, 1044351 );
            AddCraft( typeof( LeftPegleg ), 1044295, "pegleg (left)", 80.0, 120.0, typeof( Log ), 1044041, 8, 1044351 );
            AddCraft( typeof( Pipe ), 1044295, "pipe", 80.0, 120.0, typeof( Log ), 1044041, 2, 1044351 );
			AddCraft( typeof( Pipe2 ), 1044295, "pipe", 80.0, 120.0, typeof( Log ), 1044041, 2, 1044351 );

			index = AddCraft( typeof( AG_TyreanFirePitAddonDeed ), 1044299, "Fire Pit", 75.0, 95.0, typeof( Log ), 1044041, 75, 1044351 );
			AddSkill( index, SkillName.Tinkering, 40.0, 45.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 135, 1044037 );				
			index = AddCraft( typeof( StoneFireplaceEastDeed ), 1044299, "Stone Fireplace East", 70.0, 95.0, typeof( Log ), 1044041, 85, 1044351 );
			AddSkill( index, SkillName.Tinkering, 50.0, 55.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 125, 1044037 );
			index = AddCraft( typeof( StoneFireplaceSouthDeed ), 1044299, "Stone Fireplace South", 70.0, 95.0, typeof( Log ), 1044041, 85, 1044351 );
			AddSkill( index, SkillName.Tinkering, 50.0, 55.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 125, 1044037 );
			index = AddCraft( typeof( GrayBrickFireplaceEastDeed ), 1044299, "Gray Brick Fireplace East", 70.5, 95.0, typeof( Log ), 1044041, 95, 1044351 );
			AddSkill( index, SkillName.Tinkering, 55.0, 60.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 130, 1044037 );
			index = AddCraft( typeof( GrayBrickFireplaceSouthDeed ), 1044299, "Gray Brick Fireplace South", 75.0, 95.0, typeof( Log ), 1044041, 95, 1044351 );
			AddSkill( index, SkillName.Tinkering, 55.0, 60.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 130, 1044037 );	
			index = AddCraft( typeof( SandstoneFireplaceEastDeed ), 1044299, "Sandstone Fireplace East", 80.0, 95.0, typeof( Log ), 1044041, 100, 1044351 );
			AddSkill( index, SkillName.Tinkering, 60.0, 65.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 135, 1044037 );
			index = AddCraft( typeof( SandstoneFireplaceSouthDeed ), 1044299, "Sandstone Fireplace South", 80.0, 95.0, typeof( Log ), 1044041, 100, 1044351 );
			AddSkill( index, SkillName.Tinkering, 60.0, 65.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 135, 1044037 );	
		
			
			// Blacksmithy
			index = AddCraft( typeof( SmallForgeDeed ), 1044296, 1044330, 75.0, 100.0, typeof( Log ), 1044041, 5, 1044351 );
			AddRes( index, typeof( IronIngot ), 1044036, 75, 1044037 );
			index = AddCraft( typeof( LargeForgeEastDeed ), 1044296, 1044331, 80.0, 105.0, typeof( Log ), 1044041, 5, 1044351 );
			AddRes( index, typeof( IronIngot ), 1044036, 100, 1044037 );
			index = AddCraft( typeof( LargeForgeSouthDeed ), 1044296, 1044332, 78.9, 103.9, typeof( Log ), 1044041, 5, 1044351 );
			AddRes( index, typeof( IronIngot ), 1044036, 100, 1044037 );
			index = AddCraft( typeof( AnvilEastDeed ), 1044296, 1044333, 75.0, 100.0, typeof( Log ), 1044041, 5, 1044351 );
			AddRes( index, typeof( IronIngot ), 1044036, 150, 1044037 );
			index = AddCraft( typeof( AnvilSouthDeed ), 1044296, 1044334, 75.0, 100.0, typeof( Log ), 1044041, 5, 1044351 );
			AddRes( index, typeof( IronIngot ), 1044036, 150, 1044037 );

			// Training
			index = AddCraft( typeof( TrainingDummyEastDeed ), 1044297, 1044335, 70.0, 95.0, typeof( Log ), 1044041, 55, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 60, 1044287 );
			index = AddCraft( typeof( TrainingDummySouthDeed ), 1044297, 1044336, 70.0, 95.0, typeof( Log ), 1044041, 55, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 60, 1044287 );
			index = AddCraft( typeof( PickpocketDipEastDeed ), 1044297, 1044337, 75.0, 100.0, typeof( Log ), 1044041, 65, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 60, 1044287 );
			index = AddCraft( typeof( PickpocketDipSouthDeed ), 1044297, 1044338, 75.0, 100.0, typeof( Log ), 1044041, 65, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 60, 1044287 );

			// Tailoring
			index = AddCraft( typeof( Dressform ), 1044298, 1044339, 65.1, 90.0, typeof( Log ), 1044041, 25, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 10, 1044287 );
			index = AddCraft( typeof( SpinningwheelEastDeed ), 1044298, 1044341, 75.0, 100.0, typeof( Log ), 1044041, 75, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 25, 1044287 );
			index = AddCraft( typeof( SpinningwheelSouthDeed ), 1044298, 1044342, 75.0, 100.0, typeof( Log ), 1044041, 75, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 25, 1044287 );
			index = AddCraft( typeof( LoomEastDeed ), 1044298, 1044343, 85.0, 110.0, typeof( Log ), 1044041, 85, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 25, 1044287 );
			index = AddCraft( typeof( LoomSouthDeed ), 1044298, 1044344, 85.0, 110.0, typeof( Log ), 1044041, 85, 1044351 );
			AddRes( index, typeof( Cloth ), 1044286, 25, 1044287 );
			AddCraft( typeof( DyeTub ), 1044298, "dye tub", 85.0, 110.0, typeof( Log ), 1044041, 5, 1044351 );

			// Cooking
			index = AddCraft( typeof( StoneOvenEastDeed ), 1044299, 1044345, 70.0, 95.0, typeof( Log ), 1044041, 85, 1044351 );
			AddSkill( index, SkillName.Tinkering, 50.0, 55.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 125, 1044037 );
			index = AddCraft( typeof( StoneOvenSouthDeed ), 1044299, 1044346, 70.0, 95.0, typeof( Log ), 1044041, 85, 1044351 );
			AddSkill( index, SkillName.Tinkering, 50.0, 55.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 125, 1044037 );
			index = AddCraft( typeof( FlourMillEastDeed ), 1044299, 1044347, 95.0, 105.0, typeof( Log ), 1044041, 100, 1044351 );
			AddSkill( index, SkillName.Tinkering, 50.0, 55.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 50, 1044037 );
			index = AddCraft( typeof( FlourMillSouthDeed ), 1044299, 1044348, 95.0, 105.0, typeof( Log ), 1044041, 100, 1044351 );
			AddSkill( index, SkillName.Tinkering, 50.0, 55.0 );
			AddRes( index, typeof( IronIngot ), 1044036, 50, 1044037 );
			AddCraft( typeof( PicnicBasket ), 1044299, "small picnic basket", 25.0, 45.0, typeof( Log ), 1044041, 4, 1044351 );
			AddCraft( typeof( MediumPicnicBasket ), 1044299, "picnic basket", 25.0, 45.0, typeof( Log ), 1044041, 5, 1044351 );
			AddCraft( typeof( BambooBasket ), 1044299, "bamboo basket", 25.0, 45.0, typeof( Log ), 1044041, 6, 1044351 );
			AddCraft( typeof( SmallBasket ), 1044299, "small basket", 25.0, 45.0, typeof( Log ), 1044041, 4, 1044351 );
			AddCraft( typeof( SquarishBasket ), 1044299, "squarish basket", 25.0, 45.0, typeof( Log ), 1044041, 4, 1044351 );
			AddCraft( typeof( Basket ), 1044299, "basket", 25.0, 45.0, typeof( Log ), 1044041, 6, 1044351 );
			AddCraft( typeof( BasketWithHandles ), 1044299, "basket with handles", 25.0, 45.0, typeof( Log ), 1044041, 6, 1044351 );
			AddCraft( typeof( BasketOfHerbs ), 1044299, "basket of herbs", 25.0, 45.0, typeof( Log ), 1044041, 6, 1044351 );
			AddCraft( typeof( FruitBasket ), 1044299, "fruit basket", 25.0, 45.0, typeof( Log ), 1044041, 6, 1044351 );
			AddCraft( typeof( Tray ), 1044299, "tray", 25.0, 45.0, typeof( Log ), 1044041, 6, 1044351 );
			
			AddCraft( typeof( WaterTroughEastDeed ), 1044299, 1044349, 95.0, 105.0, typeof( Log ), 1044041, 150, 1044351 );
			AddCraft( typeof( WaterTroughSouthDeed ), 1044299, 1044350, 95.0, 105.0, typeof( Log ), 1044041, 150, 1044351 );

			AddCraft( typeof( SmallBoatDeed ), 1044290, "small boat deed", 40.0, 80.0, typeof( BoatPart ), "Boat Parts", 1000, "You do not have enough boat parts for that." );
			AddCraft( typeof( MediumBoatDeed ), 1044290, "medium boat deed", 40.0, 80.0, typeof( BoatPart ), "Boat Parts", 1500, "You do not have enough boat parts for that." );
			AddCraft( typeof( LargeBoatDeed ), 1044290, "large boat deed", 40.0, 80.0, typeof( BoatPart ), "Boat Parts", 2000, "You do not have enough boat parts for that." );
			
			AddCraft( typeof( SmallDragonBoatDeed ), 1044290, "small dragon boat deed", 40.0, 80.0, typeof( BoatPart ), "Boat Parts", 1000, "You do not have enough boat parts for that." );
			AddCraft( typeof( MediumDragonBoatDeed ), 1044290, "medium dragon boat deed", 40.0, 80.0, typeof( BoatPart ), "Boat Parts", 1500, "You do not have enough boat parts for that." );
			AddCraft( typeof( LargeDragonBoatDeed ), 1044290, "large dragon boat deed", 40.0, 80.0, typeof( BoatPart ), "Boat Parts", 2000, "You do not have enough boat parts for that." );
			
			SetSubRes( typeof( Log ), 1063506 );
			
			AddSubRes( typeof( Log ),				1063506, 00.0, 1044041, 1063534 );
			AddSubRes( typeof( YewLog ),			1063507, 60.0, 1044041, 1063534 );
			AddSubRes( typeof( RedwoodLog ),		1063508, 70.0, 1044041, 1063534 );
			AddSubRes( typeof( AshLog ),			1063509, 80.0, 1044041, 1063534 );
			AddSubRes( typeof( GreenheartLog ),		1063510, 90.0, 1044041, 1063534 );
			
			MarkOption = true;
			Repair = Core.AOS;
		}
	}
}
