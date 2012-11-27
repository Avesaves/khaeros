using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.Engines.Craft
{
	public class DefPottery : CraftSystem
	{
		public override SkillName MainSkill
		{
			get{ return SkillName.Craftsmanship; }
		}

		public override int GumpTitleNumber
		{
			get{ return 1063529; } // <CENTER>POTTERY MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefPottery();

				return m_CraftSystem;
			}
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.0; // 0%
		}

		private DefPottery() : base( 1, 1, 1.25 )// base( 1, 2, 1.7 )
		{
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !(from is PlayerMobile && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.Potter) > 1 ) )
				return 1063492; // You lack the required feat.

			bool anvil, forge;

			DefBlacksmithy.CheckAnvilAndForge( from, 2, out anvil, out forge );

			if ( forge )
				return 0;

			return 1044265; // You must be near a forge to blow glass.
		}

		public override void PlayCraftEffect( Mobile from )
		{
			from.PlaySound( 0x2B ); // bellows

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
				m_From.PlaySound( 0x2A );
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
				from.PlaySound( 0x41 ); // glass breaking

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
            AddCraft( typeof( EmptyJug ), 1063530, "empty jug of cider", 50.0, 102.5, typeof( Clay ), "clay", 4, "You don't have enough clay to make that." );
			AddCraft( typeof( Vase ), 1063530, "vase", 50.0, 102.5, typeof( Clay ), "clay", 3, "You don't have enough clay to make that." );
            AddCraft( typeof( LargeVase ), 1063530, "large vase", 60.0, 102.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
			AddCraft( typeof( FancyVase ), 1063530, "fancy vase", 70.0, 102.5, typeof( Clay ), "clay", 3, "You don't have enough clay to make that." );
            AddCraft( typeof( LargeFancyVase ), 1063530, "large fancy vase", 80.0, 102.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
            AddCraft( typeof( CeramicMug ), 1063530, "ceramic mug", 30.0, 102.5, typeof( Clay ), "clay", 1, "You don't have enough clay to make that." );
            //AddCraft( typeof( Jug ), 1063530, "jug", 40.0, 102.5, typeof( Clay ), "clay", 2, "You don't have enough clay to make that." );
            AddCraft( typeof( Basin ), 1063530, "basin", 20.0, 102.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
            AddCraft( typeof( TallVase ), 1063530, "tall vase", 85.0, 102.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
            AddCraft( typeof( OrnamentedVase ), 1063530, "ornamented vase", 90.0, 102.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
        	AddCraft( typeof( ClayStatueSouth ), 1063530, "clay statue (south)", 95.0, 102.5, typeof( Clay ), "clay", 9, "You don't have enough clay to make that." );
        	AddCraft( typeof( ClayStatueSouth2 ), 1063530, "clay statue (south)", 95.0, 102.5, typeof( Clay ), "clay", 9, "You don't have enough clay to make that." );
        	AddCraft( typeof( ClayStatueNorth ), 1063530, "clay statue (north)", 95.0, 102.5, typeof( Clay ), "clay", 9, "You don't have enough clay to make that." );
        	AddCraft( typeof( ClayStatueWest ), 1063530, "clay statue (west)", 95.0, 102.5, typeof( Clay ), "clay", 9, "You don't have enough clay to make that." );
        	AddCraft( typeof( ClayStatueEast ), 1063530, "clay statue (east)", 95.0, 102.5, typeof( Clay ), "clay", 9, "You don't have enough clay to make that." );
        	AddCraft( typeof( ClayStatueEast2 ), 1063530, "clay statue (east)", 95.0, 102.5, typeof( Clay ), "clay", 9, "You don't have enough clay to make that." );
        	AddCraft( typeof( ClayStatueSouthEast ), 1063530, "statue (southeast)", 95.0, 102.5, typeof( Clay ), "clay", 9, "You don't have enough clay to make that." );
        	AddCraft( typeof( BustSouth ), 1063530, "clay bust (south)", 95.0, 102.5, typeof( Clay ), "clay", 9, "You don't have enough clay to make that." );
        	AddCraft( typeof( BustEast ), 1063530, "clay bust (east)", 95.0, 102.5, typeof( Clay ), "clay", 9, "You don't have enough clay to make that." );
        	AddCraft( typeof( ClayStatuePegasus ), 1063530, "clay pegasus statue (south)", 95.0, 102.5, typeof( Clay ), "clay", 9, "You don't have enough clay to make that." );
        	AddCraft( typeof( ClayStatuePegasus2 ), 1063530, "clay pegasus statue (east)", 95.0, 102.5, typeof( Clay ), "clay", 9, "You don't have enough clay to make that." );
        	AddCraft( typeof( SmallClayTowerSculpture ), 1063530, "clay tower sculpture", 95.0, 102.5, typeof( Clay ), "clay", 9, "You don't have enough clay to make that." );
			AddCraft( typeof( PottedPlant1 ), 1063530, "potted plant (1)", 35.0, 55.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
			AddCraft( typeof( PottedPlant2 ), 1063530, "potted plant (2)", 35.0, 55.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
			AddCraft( typeof( PottedPlant3 ), 1063530, "potted plant (3)", 35.0, 55.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
			AddCraft( typeof( PottedCactus1 ), 1063530, "potted cactus (1)", 35.0, 55.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
			AddCraft( typeof( PottedCactus2 ), 1063530, "potted cactus (2)", 35.0, 55.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
			AddCraft( typeof( PottedCactus3 ), 1063530, "potted cactus (3)", 35.0, 55.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
			AddCraft( typeof( PottedCactus4 ), 1063530, "potted cactus (4)", 35.0, 55.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
			AddCraft( typeof( PottedCactus5 ), 1063530, "potted cactus (5)", 35.0, 55.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
			AddCraft( typeof( PottedCactus6 ), 1063530, "potted cactus (6)", 35.0, 55.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
			AddCraft( typeof( PottedTree1 ), 1063530, "potted tree (1)", 35.0, 55.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
			AddCraft( typeof( PottedTree2 ), 1063530, "potted tree (2)", 35.0, 55.5, typeof( Clay ), "clay", 6, "You don't have enough clay to make that." );
		}
	}
}
