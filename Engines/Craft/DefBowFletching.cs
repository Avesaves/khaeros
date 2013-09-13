using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.Engines.Craft
{
	public class DefBowFletching : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Fletching; }
		}

		public override int GumpTitleNumber
		{
			get { return 1044006; } // <CENTER>BOWCRAFT AND FLETCHING MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefBowFletching();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.5; // 50%
		}

		private DefBowFletching() : base( 1, 1, 4.00 )// base( 1, 2, 1.7 )
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
				if( itemType == typeof( WarBow ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( FlatBow ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnateLongBow ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( CompositeRecurveBow ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ShortBow ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BlowGunDarts ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BlowGun ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Hijazi ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoneBow ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( RecurveLongBow ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( RepeatingCrossbow ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( LongBow ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
					*/
			}

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			// no animation
			//if ( from.Body.Type == BodyType.Human && !from.Mounted )
			//	from.Animate( 33, 5, 1, true, false, 0 );

			from.PlaySound( 0x55 );
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

		public override CraftECA ECA{ get{ return CraftECA.FiftyPercentChanceMinusTenPercent; } }

		public override void InitCraftList()
		{
			int index = -1;

			// Materials
			AddCraft( typeof( Kindling ), 1044457, 1023553, 0.0, 00.0, typeof( Log ), 1044041, 1, 1044351 );

			index = AddCraft( typeof( Shaft ), 1044457, 1027124, 0.0, 25.0, typeof( Log ), 1044041, 1, 1044351 );
			SetUseAllRes( index, true );

			// Ammunition
			index = AddCraft( typeof( Arrow ), 1044565, 1023903, 0.0, 25.0, typeof( Shaft ), 1044560, 1, 1044561 );
			AddRes( index, typeof( Feather ), 1044562, 1, 1044563 );
			SetUseAllRes( index, true );

			index = AddCraft( typeof( Bolt ), 1044565, 1027163, 0.0, 25.0, typeof( Shaft ), 1044560, 1, 1044561 );
			AddRes( index, typeof( Feather ), 1044562, 1, 1044563 );
			SetUseAllRes( index, true );
			
			// Weapons
			AddCraft( typeof( Bow ), 1044566, 1025042, 0.0, 40.0, typeof( Log ), 1044041, 12, 1044351 );
			AddCraft( typeof( CompositeShortbow ), 1044566, "composite shortbow", 80.0, 80.0, typeof( Log ), 1044041, 12, 1044351 );
			AddCraft( typeof( CompositeBow ), 1044566, 1029922, 70.0, 80.0, typeof( Log ), 1044041, 14, 1044351 );
			AddCraft( typeof( CompositeLongbow ), 1044566, "composite longbow", 70.0, 80.0, typeof( Log ), 1044041, 16, 1044351 );
			AddCraft( typeof( Crossbow ), 1044566, 1023919, 60.0, 80.0, typeof( Log ), 1044041, 14, 1044351 );
			AddCraft( typeof( HeavyCrossbow ), 1044566, 1025117, 70.0, 80.0, typeof( Log ), 1044041, 16, 1044351 );
		
			AddCraft( typeof( WarBow ), 1044566, "war bow", 80.0, 80.0, typeof( Log ), 1044041, 18, 1044351 );
			AddCraft( typeof( CompositeRecurveBow ), 1044566, "composite recurve bow", 60.0, 80.0, typeof( Log ), 1044041, 10, 1044351 );
			AddCraft( typeof( FlatBow ), 1044566, "flatbow", 60.0, 80.0, typeof( Log ), 1044041, 10, 1044351 );
			AddCraft( typeof( OrnateLongBow ), 1044566, "ornate longbow", 60.0, 80.0, typeof( Log ), 1044041, 12, 1044351 );
			
			AddCraft( typeof( ShortBow ), 1044566, "shortbow", 60.0, 80.0, typeof( Log ), 1044041, 10, 1044351 );

			AddCraft( typeof( Hijazi ), 1044566, "hijazi", 60.0, 80.0, typeof( Log ), 1044041, 12, 1044351 );
			
			AddCraft( typeof( RecurveLongBow ), 1044566, "recurve longbow", 60.0, 80.0, typeof( Log ), 1044041, 14, 1044351 );

			AddCraft( typeof( RepeatingCrossbow ), 1044566, "repeating crossbow", 60.0, 80.0, typeof( Log ), 1044041, 16, 1044351 );
			AddCraft( typeof( LongBow ), 1044566, "longbow", 60.0, 80.0, typeof( Log ), 1044041, 14, 1044351 );

			SetSubRes( typeof( Log ), 1063506 );
			
			AddSubRes( typeof( Log ),				1063506, 00.0, 1044036, 1044267 );
			AddSubRes( typeof( YewLog ),			1063507, 60.0, 1044036, 1044268 );
			AddSubRes( typeof( RedwoodLog ),		1063508, 70.0, 1044036, 1044268 );
			AddSubRes( typeof( AshLog ),			1063509, 80.0, 1044036, 1044268 );
			AddSubRes( typeof( GreenheartLog ),		1063510, 90.0, 1044036, 1044268 );
			
			MarkOption = true;
			Repair = Core.AOS;
		}
	}
}
