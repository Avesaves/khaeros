using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.Engines.Craft
{
	public class DefBlacksmithy : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Blacksmith;	}
		}

		public override int GumpTitleNumber
		{
			get { return 1044002; } // <CENTER>BLACKSMITHY MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefBlacksmithy();

				return m_CraftSystem;
			}
		}

		public override CraftECA ECA{ get{ return CraftECA.ChanceMinusSixtyToFourtyFive; } }
		
		public override double GetChanceAtMin( CraftItem item )
		{
			return 0.5; // 50%
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

		private DefBlacksmithy() : base( 1, 1, 8.00 )// base( 1, 2, 1.7 )
		{
			/*
			
			base( MinCraftEffect, MaxCraftEffect, Delay )
			
			MinCraftEffect	: The minimum number of time the mobile will play the craft effect
			MaxCraftEffect	: The maximum number of time the mobile will play the craft effect
			Delay			: The delay between each craft effect
			
			Example: (3, 6, 1.7) would make the mobile do the PlayCraftEffect override
			function between 3 and 6 time, with a 1.7 second delay each time.
			
			*/ 
		}

		private static Type typeofAnvil = typeof( AnvilAttribute );
		private static Type typeofForge = typeof( ForgeAttribute );

		public static void CheckAnvilAndForge( Mobile from, int range, out bool anvil, out bool forge )
		{
			anvil = false;
			forge = false;

			Map map = from.Map;

			if ( map == null )
				return;

			IPooledEnumerable eable = map.GetItemsInRange( from.Location, range );

			foreach ( Item item in eable )
			{
				Type type = item.GetType();

				bool isAnvil = ( type.IsDefined( typeofAnvil, false ) || item.ItemID == 4015 || item.ItemID == 4016 || item.ItemID == 0x2DD5 || item.ItemID == 0x2DD6 );
				bool isForge = ( type.IsDefined( typeofForge, false ) || item.ItemID == 4017 || (item.ItemID >= 6522 && item.ItemID <= 6569) || item.ItemID == 0x2DD8 );

				if ( isAnvil || isForge )
				{
					if ( (from.Z + 16) < item.Z || (item.Z + 16) < from.Z || !from.InLOS( item ) )
						continue;

					anvil = anvil || isAnvil;
					forge = forge || isForge;

					if ( anvil && forge )
						break;
				}
			}

			eable.Free();

			for ( int x = -range; (!anvil || !forge) && x <= range; ++x )
			{
				for ( int y = -range; (!anvil || !forge) && y <= range; ++y )
				{
					Tile[] tiles = map.Tiles.GetStaticTiles( from.X+x, from.Y+y, true );

					for ( int i = 0; (!anvil || !forge) && i < tiles.Length; ++i )
					{
						int id = tiles[i].ID & 0x3FFF;

						bool isAnvil = ( id == 4015 || id == 4016 || id == 0x2DD5 || id == 0x2DD6 );
						bool isForge = ( id == 4017 || (id >= 6522 && id <= 6569) || id == 0x2DD8 );

						if ( isAnvil || isForge )
						{
							if ( (from.Z + 16) < tiles[i].Z || (tiles[i].Z + 16) < from.Z || !from.InLOS( new Point3D( from.X+x, from.Y+y, tiles[i].Z + (tiles[i].Height/2) + 1 ) ) )
								continue;

							anvil = anvil || isAnvil;
							forge = forge || isForge;
						}
					}
				}
			}
		}

		public override int CanCraft( Mobile from, BaseTool tool, Type itemType )
		{
			if ( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			else if ( !BaseTool.CheckTool( tool, from ) )
				return 1048146; // If you have a tool equipped, you must use that tool.
			else if ( !BaseTool.CheckAccessible( tool, from ) )
				return 1044263; // The tool must be on your person to use.
			else if ( tool is SmithHammer && tool.Parent != from )
				return 1063533; // A smith hammer needs to be equipped before it can be used.
			
			if ( tool == null || tool.Deleted || tool.UsesRemaining < 0 )
				return 1044038; // You have worn out your tool!
			
			else if( from is PlayerMobile && itemType != null )
			{
				PlayerMobile m = from as PlayerMobile;
				
		/*		if( itemType == typeof( SplintedMailGorget ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SplintedMailChest ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SplintedMailLegs ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SplintedMailArms ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( RunicBuckler ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( RoundShield ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( NotchedShield ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Claymore ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Falcata ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Billhook ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ArmingSword ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HeavyDoubleAxe ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BattleHammer ) && !TestRace( m, Nation.Southern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SpikedChest ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( EagleHelm ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( RoundedFaceShield ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TallFaceShield ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Kutar ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Tepatl ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Flamberge ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HaftedAxe ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Glaive ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PrimitiveSpear ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( WesternMace ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SpikedMace ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HookedClub ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ScaleArmorLegs ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ScaleArmorArms ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ScaleArmorChest ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Scimitar ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HeavyKhopesh ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Kukri ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Falchion ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Khopesh ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Shamshir ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Sabre ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Talwar ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Tabarzin ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Macana ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( CrescentBlade ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoneScythe ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BarbarianAxe ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BarbarianHeavyAxe ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoneSword ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoneSpear ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BoneAxe ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulBladedBoneStaff ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BarbarianSpear ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( WarFork ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BarbarianScepter ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BarbarianMace ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HalfPlateArms ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HalfPlateGorget ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HalfPlateLegs ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HalfPlateSabatons ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HalfPlateChest ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HalfPlateGloves ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ScaleArmorHelmet ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.

                else if( itemType == typeof( Helmet ) && !TestRace( m, Nation.Southern, Nation.Tirebladd ) )
                    return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HornedHelm ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HornedPlateHelm ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( WingedHelm ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( DragonKiteShield ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Broadsword ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HandAndAHalfSword ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnateAxe ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BroadAxe ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BeardedDoubleAxe ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HeavyBattleAxe ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( WingedAxe ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ThrowingAxe ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Angon ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HeavyWarMace ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HeavyBattleHammer ) && !TestRace( m, Nation.Tirebladd ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnatePlateChest ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnatePlateArms ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnatePlateLegs ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnatePlateGorget ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnatePlateGloves ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnateHelm ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnatePlateHelm ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnateNorseHelm ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( OrnateKiteShield ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Gladius ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( CavalrySword ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( BastardSword ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HorsemanAxe ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Lance ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HorsemanMace ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HorsemanWarhammer ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TwoHandedMaul ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( HeavyMaul ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Bascinet ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( WoodenKiteShield ) && !TestRace( m, Nation.Mhordul, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KiteShield ) && !TestRace( m, Nation.Northern ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PriestStaff ) && !TestRace( m, Nation.Haluaroc ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ShamanStaff ) && !TestRace( m, Nation.Western ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PlateSabatons ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PlateLegs ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PlateChest ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PlateArms ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PlateGloves ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PlateGorget ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PlateHelm ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( CloseHelm ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( NorseHelm ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Barbute ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Cervelliere ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
					*/
			}

			bool anvil, forge;
			CheckAnvilAndForge( from, 2, out anvil, out forge );

			if ( anvil && forge )
				return 0;

			return 1044267; // You must be near an anvil and a forge to smith items.
		}

		public override void PlayCraftEffect( Mobile from )
		{
			// no animation, instant sound
			new InternalTimer( from, 0 ).Start();
		}

		// Delay to synchronize the sound with the hit on the anvil
		private class InternalTimer : Timer
		{
			private Mobile m_From;
			private int m_turn;

			public InternalTimer( Mobile from, int turn ) : base( TimeSpan.FromSeconds( 0.7 ) )
			{
				if ( from.Body.Type == BodyType.Human && !from.Mounted )
					from.Animate( 9, 5, 1, true, false, 0 );
				
				m_From = from;
				m_turn = turn;
			}

			protected override void OnTick()
			{
				m_From.PlaySound( 0x2A );
				
				if( m_turn < 4 )
					new InternalTimer2( m_From, m_turn + 1 ).Start();
			}
		}
		
		private class InternalTimer2 : Timer
		{
			private Mobile m_From;
			private int m_turn;

			public InternalTimer2( Mobile from, int turn ) : base( TimeSpan.FromSeconds( 0.8 ) )
			{
				m_From = from;
				m_turn = turn;
			}

			protected override void OnTick()
			{
				new InternalTimer( m_From, m_turn ).Start();
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
			#region Ringmail
			AddCraft( typeof( RingmailGloves ), 1011076, "ringmail gloves", 10.0, 20.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( RingmailLegs ), 1011076, "ringmail legs", 20.0, 30.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( RingmailArms ), 1011076, "ringmail arms", 15.0, 25.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			
			AddCraft( typeof( RingmailGorget ), 1011076, "ringmail gorget", 5.0, 15.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			
			AddCraft( typeof( RingmailChest ), 1011076, "ringmail chest", 20.0, 40.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( ScaleArmorLegs ), 1011076, "scale armor legs", 80.0, 120.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( ScaleArmorArms ), 1011076, "scale armor arms", 80.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( ScaleArmorChest ), 1011076, "scale armor chest", 80.0, 120.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			
			#endregion

			#region Chainmail
			AddCraft( typeof( ChainCoif ), 1011077, "chain coif", 15.0, 65.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( ChainLegs ), 1011077, "chain legs", 35.0, 85.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			
			AddCraft( typeof( ChainGorget ), 1011077, "chain gorget", 20.0, 70.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( ChainGloves ), 1011077, "chain gloves", 25.0, 75.0, typeof( CopperIngot ), 1044036, 9, 1044037 );
			AddCraft( typeof( ChainArms ), 1011077, "chain arms", 30.0, 80.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			
			AddCraft( typeof( ChainChest ), 1011077, "chain chest", 40.0, 90.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			
			AddCraft( typeof( SplintedMailGorget ), 1011077, "splinted mail gorget", 80.0, 120.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( SplintedMailLegs ), 1011077, "splinted mail legs", 80.0, 120.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( SplintedMailChest ), 1011077, "splinted mail chest", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( SplintedMailArms ), 1011077, "splinted mail arms", 80.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );


			#endregion

			int index = -1;

			#region Platemail
			AddCraft( typeof( PlateArms ), 1011078, "plate arms", 75.0, 115.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( PlateGloves ), 1011078, "plate gloves", 70.0, 110.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( PlateGorget ), 1011078, "plate gorget", 65.0, 105.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( PlateLegs ), 1011078, "plate legs", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( PlateChest ), 1011078, "plate chest", 80.0, 120.0, typeof( CopperIngot ), 1044036, 25, 1044037 );
			AddCraft( typeof( PlateSabatons ), 1011078, "plate sabatons", 80.0, 120.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( FemalePlateChest ), 1011078, "female plate chest", 75.0, 115.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( HalfPlateArms ), 1011078, "half-plate arms", 80.0, 120.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( HalfPlateGloves ), 1011078, "half-plate gloves", 80.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( HalfPlateGorget ), 1011078, "half-plate gorget", 80.0, 120.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( HalfPlateLegs ), 1011078, "half-plate legs", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( HalfPlateChest ), 1011078, "half-plate chest", 80.0, 120.0, typeof( CopperIngot ), 1044036, 25, 1044037 );
			AddCraft( typeof( HalfPlateSabatons ), 1011078, "half-plate sabatons", 80.0, 120.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( OrnatePlateArms ), 1011078, "ornate plate arms", 80.0, 120.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( OrnatePlateGloves ), 1011078, "ornate plate gloves", 80.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( OrnatePlateGorget ), 1011078, "ornate plate gorget", 80.0, 120.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( OrnatePlateLegs ), 1011078, "ornate plate legs", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( OrnatePlateChest ), 1011078, "ornate plate chest", 80.0, 120.0, typeof( CopperIngot ), 1044036, 25, 1044037 );
			AddCraft( typeof( AG_HorseBardingAddonDeed ), 1011078, "horse barding", 80.0, 120.0, typeof( CopperIngot ), 1044036, 80, 1044037 );

			#endregion

			#region Helmets			
			AddCraft( typeof( CloseHelm ), 1011079, "close helm", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( NorseHelm ), 1011079, "norse helm", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( PlateHelm ), 1011079, "plate helm", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( Cervelliere ), 1011079, "cervelliere", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( Sallet ), 1011079, "sallet", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( SkullCapHelmet ), 1011079, "skull cap helm", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( Barbute ), 1011079, "barbute", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( EagleHelm ), 1011079, "eagle helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( ScaleArmorHelmet ), 1011079, "scale armor helmet", 60.0, 100.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( HornedPlateHelm ), 1011079, "horned plate helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( HornedHelm ), 1011079, "horned helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( WingedHelm ), 1011079, "winged helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( Bascinet ), 1011079, "bascinet", 10.0, 60.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( OrnateHelm ), 1011079, "ornate helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( OrnateNorseHelm ), 1011079, "ornate norse helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( OrnatePlateHelm ), 1011079, "ornate plate helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );

			#endregion

			#region Shields
			AddCraft( typeof( Buckler ), 1011080, "buckler", 0.0, 20.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( BronzeShield ), 1011080, "bronze shield", 0.0, 65.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( HeaterShield ), 1011080, "heater shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( MetalShield ), 1011080, "metal shield", 0.0, 80.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( RunicBuckler ), 1011080, "runic buckler", 80.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( RoundShield ), 1011080, "round shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( NotchedShield ), 1011080, "notched shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( DragonKiteShield ), 1011080, "dragon kite shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( OrnateKiteShield ), 1011080, "ornate kite shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 25, 1044037 );
			AddCraft( typeof( KiteShield ), 1011080, "kite shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			
			#endregion

			#region Bladed
			AddCraft( typeof( HandScythe ), 1011081, "hand scythe", 35.0, 85.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( Cutlass ), 1011081, "cutlass", 25.0, 75.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( Dagger ), 1011081, "dagger", 0.0, 20.0, typeof( CopperIngot ), 1044036, 3, 1044037 );
			AddCraft( typeof( DualDaggers ), 1011081, "dual daggers", 80.0, 120.0, typeof( CopperIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( Kryss ), 1011081, "kryss", 35.0, 85.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( Longsword ), 1011081, "longsword", 40.0, 80.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( Shortsword ), 1011081, "shortsword", 20.0, 70.0, typeof( CopperIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( Gladius ), 1011081, "gladius", 20.0, 70.0, typeof( CopperIngot ), 1044036, 6, 1044037 );			
			AddCraft( typeof( Machete ), 1011081, "machete", 20.0, 70.0, typeof( CopperIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( SkinningKnife ), 1011081, "skinning knife", 0.0, 20.0, typeof( CopperIngot ), 1044036, 3, 1044037 );
			AddCraft( typeof( Cleaver ), 1011081, "cleaver", 10.0, 30.0, typeof( CopperIngot ), 1044036, 3, 1044037 );
			AddCraft( typeof( ButcherKnife ), 1011081, "butcher knife", 0.0, 20.0, typeof( CopperIngot ), 1044036, 3, 1044037 );
			AddCraft( typeof( Greatsword ), 1011081, "greatsword", 60.0, 100.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( Rapier ), 1011081, "rapier", 70.0, 110.0, typeof( CopperIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( DoubleBladedStaff ), 1011081, "double bladed staff", 70.0, 110.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( Claymore ), 1011081, "claymore", 80.0, 120.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( Falcata ), 1011081, "falcata", 70.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( Billhook ), 1011081, "billhook", 40.0, 80.0, typeof( CopperIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( ArmingSword ), 1011081, "arming sword", 80.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( Kutar ), 1011081, "kutar", 60.0, 100.0, typeof( CopperIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( Flamberge ), 1011081, "flamberge", 70.0, 120.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( Scimitar ), 1011081, "scimitar", 30.0, 80.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( HeavyKhopesh ), 1011081, "heavy khopesh", 75.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( Kukri ), 1011081, "kukri", 70.0, 120.0, typeof( CopperIngot ), 1044036, 4, 1044037 );
			AddCraft( typeof( Falchion ), 1011081, "falchion", 70.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( Khopesh ), 1011081, "khopesh", 70.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( Shamshir ), 1011081, "shamshir", 50.0, 100.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( Sabre ), 1011081, "sabre", 70.0, 120.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( Talwar ), 1011081, "talwar", 70.0, 120.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( Broadsword ), 1011081, "broadsword", 50.0, 80.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( HandAndAHalfSword ), 1011081, "hand-and-a-half sword", 80.0, 120.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( CavalrySword ), 1011081, "cavalry sword", 70.0, 100.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( BastardSword ), 1011081, "bastard sword", 90.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
            		AddCraft( typeof( DualSwords ), 1011081, "dual swords", 75.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );

			#endregion

			#region Axes
			AddCraft( typeof( Axe ), 1011082, "axe", 35.0, 85.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( BattleAxe ), 1011082, "battle axe", 30.0, 80.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( WarAxe ), 1011082, "war axe", 40.0, 90.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( TwoHandedAxe ), 1011082, "two-handed axe", 30.0, 80.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( LargeBattleAxe ), 1011082, "large battle axe", 30.0, 80.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( DualPicks ), 1011082, "dual picks", 80.0, 120.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( HeavyDoubleAxe ), 1011082, "heavy double axe", 75.0, 115.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( HaftedAxe ), 1011082, "hafted axe", 35.0, 85.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( Tabarzin ), 1011082, "tabarzin", 35.0, 85.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( OrnateAxe ), 1011082, "ornate axe", 80.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( BeardedDoubleAxe ), 1011082, "bearded double axe", 60.0, 100.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( BroadAxe ), 1011082, "broad axe", 80.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( HeavyBattleAxe ), 1011082, "heavy battle axe", 50.0, 100.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( WingedAxe ), 1011082, "winged axe", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( ThrowingAxe ), 1011082, "throwing axe", 50.0, 100.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( HorsemanAxe ), 1011082, "horseman's axe", 30.0, 80.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			
			#endregion

			#region Pole Arms
			AddCraft( typeof( Bardiche ), 1011083, "bardiche", 30.0, 80.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( Halberd ), 1011083, "halberd", 40.0, 90.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( Pike ), 1011083, "pike", 45.0, 95.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( ShortSpear ), 1011083, "short spear", 45.0, 95.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( Scythe ), 1011083, "scythe", 40.0, 90.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( Spear ), 1011083, "spear", 50.0, 100.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( Pitchfork ), 1011083, "pitchfork", 35.0, 85.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( Glaive ), 1011083, "glaive", 40.0, 90.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( WarFork ), 1011083, "war fork", 40.9, 90.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( Angon ), 1011083, "angon", 40.0, 90.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( Lance ), 1011083, "lance", 50.0, 100.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
				
			#endregion

			#region Bashing
			AddCraft( typeof( HammerPick ), 1011084, "hammer pick", 35.0, 85.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( Mace ), 1011084, "mace", 15.0, 65.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( Maul ), 1011084, "maul", 20.0, 70.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( WarMace ), 1011084, "war mace", 30.0, 80.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( WarHammer ), 1011084, "war hammer", 35.0, 85.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( LightHammer ), 1011084, "light hammer", 70.0, 100.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( FlangedMace ), 1011084, "flanged mace", 80.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( ShamanStaff ), 1011084, "shaman's staff", 80.0, 120.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( PriestStaff ), 1011084, "priest's staff", 80.0, 120.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( BattleHammer ), 1011084, "battle hammer", 50.0, 90.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( SpikedMace ), 1011084, "spiked mace", 40.0, 90.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( HookedClub ), 1011084, "hooked club", 40.0, 90.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( Macana ), 1011084, "macana", 40.0, 90.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( HeavyWarMace ), 1011084, "heavy war mace", 60.0, 100.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( HeavyBattleHammer ), 1011084, "heavy battle hammer", 50.0, 90.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( HorsemanMace ), 1011084, "horseman's mace", 40.0, 80.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( HorsemanWarhammer ), 1011084, "horseman's war hammer", 50.0, 90.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( TwoHandedMaul ), 1011084, "two-handed maul", 60.0, 100.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( HeavyMaul ), 1011084, "heavy maul", 70.0, 110.0, typeof( CopperIngot ), 1044036, 20, 1044037 );

			#endregion
			
 			SetSubRes( typeof( CopperIngot ), 1044025 );
/*			AddSubRes( typeof( CopperIngot ),		1044025, 00.0, 1044036, 1044267 ); */
			AddSubRes( typeof( BronzeIngot ),		1044026, 60.0, 1044036, 1044267 );
			AddSubRes( typeof( IronIngot ),			1044022, 30.0, 1044036, 1044268 );
			AddSubRes( typeof( SilverIngot ),		1044028, 90.0, 1044036, 1044268 );
			AddSubRes( typeof( GoldIngot ),			1044027, 85.0, 1044036, 1044268 );
			AddSubRes( typeof( ObsidianIngot ),		1044029, 95.0, 1044036, 1044268 );
			AddSubRes( typeof( SteelIngot ),		1044030, 99.0, 1044036, 1044268 );
			AddSubRes( typeof( StarmetalIngot ),	1044024, 99.0, 1044036, 1044268 );
			//AddSubRes( typeof( ElectrumIngot ),		1044023, 85.0, 1044036, 1044268 );
			Resmelt = true;
			Repair = true;
			MarkOption = true;
		}
	}

	public class ForgeAttribute : Attribute
	{
		public ForgeAttribute()
		{
		}
	}

	public class AnvilAttribute : Attribute
	{
		public AnvilAttribute()
		{
		}
	}
}
