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
				
				if( itemType == typeof( AlyrianChainGorget ) && !TestRace( m, Nation.Alyrian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AlyrianChainChest ) && !TestRace( m, Nation.Alyrian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AlyrianChainLegs ) && !TestRace( m, Nation.Alyrian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AlyrianChainArms ) && !TestRace( m, Nation.Alyrian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AlyrianBuckler ) && !TestRace( m, Nation.Alyrian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AlyrianRoundShield ) && !TestRace( m, Nation.Alyrian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AlyrianLeafShield ) && !TestRace( m, Nation.Alyrian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AlyrianClaymore ) && !TestRace( m, Nation.Alyrian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AlyrianSabre ) && !TestRace( m, Nation.Alyrian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AlyrianHandScythe ) && !TestRace( m, Nation.Alyrian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AlyrianLongsword ) && !TestRace( m, Nation.Alyrian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AlyrianTwoHandedAxe ) && !TestRace( m, Nation.Alyrian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AlyrianBattleHammer ) && !TestRace( m, Nation.Alyrian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AzhuranSpikedChainChest ) && !TestRace( m, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AzhuranHelm ) && !TestRace( m, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AzhuranRoundShield ) && !TestRace( m, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AzhuranKiteShield ) && !TestRace( m, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AzhuranShortsword ) && !TestRace( m, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AzhuranLongsword ) && !TestRace( m, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AzhuranBroadsword ) && !TestRace( m, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AzhuranAxe ) && !TestRace( m, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AzhuranBladedStaff ) && !TestRace( m, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AzhuranSpear ) && !TestRace( m, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AzhuranMace ) && !TestRace( m, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AzhuranWarMace ) && !TestRace( m, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AzhuranHookedClub ) && !TestRace( m, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarScaleLegs ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarScaleArms ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarScaleChest ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarScimitar ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarHeavyKhopesh ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarKukri ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarFalchion ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarKhopesh ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarThinScimitar ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarCrescentSword ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarLargeCrescentSword ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarAxe ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarWarMace ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulCrescentBlade ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulBoneScythe ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulAxe ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulHeavyBattleAxe ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulBoneSword ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulBoneSpear ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulBoneAxe ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulBladedBoneStaff ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulSpear ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulWarFork ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulScepter ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( MhordulMace ) && !TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanHalfPlateArms ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanHalfPlateGorget ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanHalfPlateLegs ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanHalfPlateSabatons ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanHalfPlateChest ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanHalfPlateGloves ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( KhemetarScaleHelmet ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.

                else if( itemType == typeof( Helmet ) && !TestRace( m, Nation.Alyrian, Nation.Tyrean ) )
                    return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanHornedHelm ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanHornedPlateHelm ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanWingedHelm ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanKiteShield ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanBroadsword ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanBastardSword ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanOrnateAxe ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanWarAxe ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanDoubleAxe ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanBattleAxe ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanWingedAxe ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanThrowingAxe ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanHarpoon ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanWarMace ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( TyreanBattleHammer ) && !TestRace( m, Nation.Tyrean ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianOrnatePlateChest ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianOrnatePlateArms ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianOrnatePlateLegs ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianOrnatePlateGorget ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianOrnatePlateGloves ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianOrnateHelm ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianOrnatePlateHelm ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianOrnateNorseHelm ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianOrnateKiteShield ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianGladius ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianBroadsword ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianBastardSword ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianWarAxe ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianLance ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianMace ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianWarHammer ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianMaul ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianHeavyMaul ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianBascinet ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( WoodenKiteShield ) && !TestRace( m, Nation.Mhordul, Nation.Azhuran ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( VhalurianMetalKiteShield ) && !TestRace( m, Nation.Vhalurian ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( PriestStaff ) && !TestRace( m, Nation.Khemetar ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ShamanStaff ) && !TestRace( m, Nation.Azhuran ) )
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
			AddCraft( typeof( KhemetarScaleLegs ), 1011076, "khemetar scale legs", 80.0, 120.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( KhemetarScaleArms ), 1011076, "khemetar scale arms", 80.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( KhemetarScaleChest ), 1011076, "khemetar scale chest", 80.0, 120.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			
			#endregion

			#region Chainmail
			AddCraft( typeof( ChainCoif ), 1011077, "chain coif", 15.0, 65.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( ChainLegs ), 1011077, "chain legs", 35.0, 85.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			
			AddCraft( typeof( ChainGorget ), 1011077, "chain gorget", 20.0, 70.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( ChainGloves ), 1011077, "chain gloves", 25.0, 75.0, typeof( CopperIngot ), 1044036, 9, 1044037 );
			AddCraft( typeof( ChainArms ), 1011077, "chain arms", 30.0, 80.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			
			AddCraft( typeof( ChainChest ), 1011077, "chain chest", 40.0, 90.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			
			AddCraft( typeof( AlyrianChainGorget ), 1011077, "alyrian chain gorget", 80.0, 120.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( AlyrianChainLegs ), 1011077, "alyrian chain legs", 80.0, 120.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( AlyrianChainChest ), 1011077, "alyrian chain chest", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( AlyrianChainArms ), 1011077, "alyrian chain arms", 80.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( AzhuranSpikedChainChest ), 1011077, "azhuran spiked chain chest", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );

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
			AddCraft( typeof( TyreanHalfPlateArms ), 1011078, "tyrean half-plate arms", 80.0, 120.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( TyreanHalfPlateGloves ), 1011078, "tyrean half-plate gloves", 80.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( TyreanHalfPlateGorget ), 1011078, "tyrean half-plate gorget", 80.0, 120.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( TyreanHalfPlateLegs ), 1011078, "tyrean half-plate legs", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( TyreanHalfPlateChest ), 1011078, "tyrean half-plate chest", 80.0, 120.0, typeof( CopperIngot ), 1044036, 25, 1044037 );
			AddCraft( typeof( TyreanHalfPlateSabatons ), 1011078, "tyrean half-plate sabatons", 80.0, 120.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( VhalurianOrnatePlateArms ), 1011078, "vhalurian ornate plate arms", 80.0, 120.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( VhalurianOrnatePlateGloves ), 1011078, "vhalurian ornate plate gloves", 80.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( VhalurianOrnatePlateGorget ), 1011078, "vhalurian ornate plate gorget", 80.0, 120.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( VhalurianOrnatePlateLegs ), 1011078, "vhalurian ornate plate legs", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( VhalurianOrnatePlateChest ), 1011078, "vhalurian ornate plate chest", 80.0, 120.0, typeof( CopperIngot ), 1044036, 25, 1044037 );
			AddCraft( typeof( AG_HorseBardingAddonDeed ), 1011078, "horse barding", 80.0, 120.0, typeof( CopperIngot ), 1044036, 80, 1044037 );

			#endregion

			#region Helmets			
			AddCraft( typeof( CloseHelm ), 1011079, "close helm", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( Helmet ), 1011079, "antlered helm", 35.0, 85.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( NorseHelm ), 1011079, "norse helm", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( PlateHelm ), 1011079, "plate helm", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( Cervelliere ), 1011079, "cervelliere", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( Sallet ), 1011079, "sallet", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( SkullCapHelmet ), 1011079, "skull cap helm", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( Barbute ), 1011079, "barbute", 60.0, 110.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( AzhuranHelm ), 1011079, "azhuran helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( KhemetarScaleHelmet ), 1011079, "khemetar scale helmet", 60.0, 100.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( TyreanHornedPlateHelm ), 1011079, "tyrean horned plate helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( TyreanHornedHelm ), 1011079, "tyrean horned helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( TyreanWingedHelm ), 1011079, "tyrean winged helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( VhalurianBascinet ), 1011079, "vhalurian bascinet", 10.0, 60.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( VhalurianOrnateHelm ), 1011079, "vhalurian ornate helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( VhalurianOrnateNorseHelm ), 1011079, "vhalurian ornate norse helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );
			AddCraft( typeof( VhalurianOrnatePlateHelm ), 1011079, "vhalurian ornate plate helm", 80.0, 120.0, typeof( CopperIngot ), 1044036, 15, 1044037 );

			#endregion

			#region Shields
			AddCraft( typeof( Buckler ), 1011080, "buckler", 0.0, 20.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( BronzeShield ), 1011080, "bronze shield", 0.0, 65.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( HeaterShield ), 1011080, "heater shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( MetalShield ), 1011080, "metal shield", 0.0, 80.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( AlyrianBuckler ), 1011080, "alyrian buckler", 80.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( AlyrianRoundShield ), 1011080, "alyrian round shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( AlyrianLeafShield ), 1011080, "alyrian leaf shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( AzhuranRoundShield ), 1011080, "azhuran round shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( AzhuranKiteShield ), 1011080, "azhuran kite shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( TyreanKiteShield ), 1011080, "tyrean kite shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( VhalurianOrnateKiteShield ), 1011080, "vhalurian ornate kite shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 25, 1044037 );
			AddCraft( typeof( VhalurianMetalKiteShield ), 1011080, "vhalurian metal kite shield", 80.0, 120.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			
			#endregion

			#region Bladed
			AddCraft( typeof( HandScythe ), 1011081, "hand scythe", 35.0, 85.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( Cutlass ), 1011081, "cutlass", 25.0, 75.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( Dagger ), 1011081, "dagger", 0.0, 20.0, typeof( CopperIngot ), 1044036, 3, 1044037 );
			AddCraft( typeof( DualDaggers ), 1011081, "dual daggers", 80.0, 120.0, typeof( CopperIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( Kryss ), 1011081, "kryss", 35.0, 85.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( Longsword ), 1011081, "longsword", 40.0, 80.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( Shortsword ), 1011081, "shortsword", 20.0, 70.0, typeof( CopperIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( Machete ), 1011081, "machete", 20.0, 70.0, typeof( CopperIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( SkinningKnife ), 1011081, "skinning knife", 0.0, 20.0, typeof( CopperIngot ), 1044036, 3, 1044037 );
			AddCraft( typeof( Cleaver ), 1011081, "cleaver", 10.0, 30.0, typeof( CopperIngot ), 1044036, 3, 1044037 );
			AddCraft( typeof( ButcherKnife ), 1011081, "butcher knife", 0.0, 20.0, typeof( CopperIngot ), 1044036, 3, 1044037 );
			AddCraft( typeof( Greatsword ), 1011081, "greatsword", 60.0, 100.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( Rapier ), 1011081, "rapier", 70.0, 110.0, typeof( CopperIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( DoubleBladedStaff ), 1011081, "double bladed staff", 70.0, 110.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( AlyrianClaymore ), 1011081, "alyrian claymore", 80.0, 120.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( AlyrianSabre ), 1011081, "alyrian sabre", 70.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( AlyrianHandScythe ), 1011081, "alyrian hand scythe", 40.0, 80.0, typeof( CopperIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( AlyrianLongsword ), 1011081, "alyrian longsword", 80.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( AzhuranShortsword ), 1011081, "azhuran shortsword", 60.0, 100.0, typeof( CopperIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( AzhuranLongsword ), 1011081, "azhuran longsword", 70.0, 120.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( AzhuranBroadsword ), 1011081, "azhuran broadsword", 70.0, 120.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( KhemetarScimitar ), 1011081, "khemetar scimitar", 30.0, 80.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( KhemetarHeavyKhopesh ), 1011081, "khemetar heavy khopesh", 75.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( KhemetarKukri ), 1011081, "khemetar kukri", 70.0, 120.0, typeof( CopperIngot ), 1044036, 4, 1044037 );
			AddCraft( typeof( KhemetarFalchion ), 1011081, "khemetar falchion", 70.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( KhemetarKhopesh ), 1011081, "khemetar khopesh", 70.0, 120.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( KhemetarThinScimitar ), 1011081, "khemetar thin scimitar", 50.0, 100.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( KhemetarCrescentSword ), 1011081, "khemetar crescent sword", 70.0, 120.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( KhemetarLargeCrescentSword ), 1011081, "khemetar large crescent sword", 70.0, 120.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( MhordulCrescentBlade ), 1011081, "mhordul crescent blade", 45.0, 95.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			index = AddCraft( typeof( MhordulBoneSword ), 1011081, "mhordul bone sword", 40.0, 90.0, typeof( CopperIngot ), 1044036, 2, 1044037 );
			AddRes( index, typeof( Bone ), 1049064, 6, 1049063 );
			AddCraft( typeof( TyreanBroadsword ), 1011081, "tyrean broadsword", 50.0, 80.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( TyreanBastardSword ), 1011081, "tyrean bastard sword", 80.0, 120.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( VhalurianGladius ), 1011081, "vhalurian gladius", 50.0, 80.0, typeof( CopperIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( VhalurianBroadsword ), 1011081, "vhalurian broadsword", 70.0, 100.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( VhalurianBastardSword ), 1011081, "vhalurian bastard sword", 90.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
            		AddCraft( typeof( DualSwords ), 1011081, "dual swords", 75.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );

			#endregion

			#region Axes
			AddCraft( typeof( Axe ), 1011082, "axe", 35.0, 85.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( BattleAxe ), 1011082, "battle axe", 30.0, 80.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( WarAxe ), 1011082, "war axe", 40.0, 90.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( TwoHandedAxe ), 1011082, "two-handed axe", 30.0, 80.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( LargeBattleAxe ), 1011082, "large battle axe", 30.0, 80.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( DualPicks ), 1011082, "dual picks", 80.0, 120.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( AlyrianTwoHandedAxe ), 1011082, "alyrian two-handed axe", 75.0, 115.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( AzhuranAxe ), 1011082, "azhuran axe", 35.0, 85.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( KhemetarAxe ), 1011082, "khemetar axe", 35.0, 85.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( MhordulAxe ), 1011082, "mhordul axe", 50.0, 100.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( MhordulHeavyBattleAxe ), 1011082, "mhordul heavy battle axe", 80.0, 120.0, typeof( CopperIngot ), 1044036, 22, 1044037 );
			index = AddCraft( typeof( MhordulBoneAxe ), 1011082, "mhordul bone axe", 40.0, 90.0, typeof( CopperIngot ), 1044036, 4, 1044037 );
			AddRes( index, typeof( Bone ), 1049064, 10, 1049063 );
			AddCraft( typeof( TyreanOrnateAxe ), 1011082, "tyrean ornate axe", 80.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( TyreanDoubleAxe ), 1011082, "tyrean double axe", 60.0, 100.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( TyreanWarAxe ), 1011082, "tyrean war axe", 80.0, 120.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( TyreanBattleAxe ), 1011082, "tyrean battle axe", 50.0, 100.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( TyreanWingedAxe ), 1011082, "tyrean winged axe", 80.0, 120.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( TyreanThrowingAxe ), 1011082, "tyrean throwing axe", 50.0, 100.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( VhalurianWarAxe ), 1011082, "vhalurian war axe", 30.0, 80.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			
			#endregion

			#region Pole Arms
			AddCraft( typeof( Bardiche ), 1011083, "bardiche", 30.0, 80.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( Halberd ), 1011083, "halberd", 40.0, 90.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( Pike ), 1011083, "pike", 45.0, 95.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( ShortSpear ), 1011083, "short spear", 45.0, 95.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( Scythe ), 1011083, "scythe", 40.0, 90.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( Spear ), 1011083, "spear", 50.0, 100.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( Pitchfork ), 1011083, "pitchfork", 35.0, 85.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( AzhuranBladedStaff ), 1011083, "azhuran bladed staff", 40.0, 90.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( AzhuranSpear ), 1011083, "azhuran spear", 40.0, 90.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( MhordulWarFork ), 1011083, "mhordul war fork", 40.9, 90.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( MhordulSpear ), 1011083, "mhordul spear", 50.0, 100.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			index = AddCraft( typeof( MhordulBoneSpear ), 1011083, "mhordul bone spear", 50.0, 100.0, typeof( CopperIngot ), 1044036, 2, 1044037 );
			AddRes( index, typeof( Bone ), 1049064, 10, 1049063 );
			index = AddCraft( typeof( MhordulBladedBoneStaff ), 1011083, "mhordul bladed bone staff", 40.0, 90.0, typeof( CopperIngot ), 1044036, 4, 1044037 );
			AddRes( index, typeof( Bone ), 1049064, 10, 1049063 );
			index = AddCraft( typeof( MhordulBoneScythe ), 1011083, "mhordul bone scythe", 40.0, 90.0, typeof( CopperIngot ), 1044036, 4, 1044037 );
			AddRes( index, typeof( Bone ), 1049064, 12, 1049063 );
			AddCraft( typeof( TyreanHarpoon ), 1011083, "tyrean harpoon", 40.0, 90.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( VhalurianLance ), 1011083, "vhalurian lance", 50.0, 100.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
				
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
			AddCraft( typeof( AlyrianBattleHammer ), 1011084, "alyrian battle hammer", 50.0, 90.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( AzhuranWarMace ), 1011084, "azhuran war mace", 40.0, 90.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( AzhuranHookedClub ), 1011084, "azhuran hooked club", 40.0, 90.0, typeof( CopperIngot ), 1044036, 10, 1044037 );
			AddCraft( typeof( AzhuranMace ), 1011084, "azhuran mace", 40.0, 90.0, typeof( CopperIngot ), 1044036, 8, 1044037 );
			AddCraft( typeof( KhemetarWarMace ), 1011084, "khemetar war mace", 40.0, 90.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( MhordulScepter ), 1011084, "mhordul scepter", 20.0, 70.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( MhordulMace ), 1011084, "mhordul mace", 40.0, 90.0, typeof( CopperIngot ), 1044036, 14, 1044037 );
			AddCraft( typeof( TyreanWarMace ), 1011084, "tyrean war mace", 60.0, 100.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( TyreanBattleHammer ), 1011084, "tyrean battle hammer", 50.0, 90.0, typeof( CopperIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( VhalurianMace ), 1011084, "vhalurian mace", 40.0, 80.0, typeof( CopperIngot ), 1044036, 12, 1044037 );
			AddCraft( typeof( VhalurianWarHammer ), 1011084, "vhalurian war hammer", 50.0, 90.0, typeof( CopperIngot ), 1044036, 16, 1044037 );
			AddCraft( typeof( VhalurianMaul ), 1011084, "vhalurian maul", 60.0, 100.0, typeof( CopperIngot ), 1044036, 18, 1044037 );
			AddCraft( typeof( VhalurianHeavyMaul ), 1011084, "vhalurian heavy maul", 70.0, 110.0, typeof( CopperIngot ), 1044036, 20, 1044037 );

			#endregion
			
			SetSubRes( typeof( CopperIngot ), 1044025 );
			AddSubRes( typeof( CopperIngot ),		1044025, 00.0, 1044036, 1044267 );
			AddSubRes( typeof( BronzeIngot ),		1044026, 30.0, 1044036, 1044268 );
			AddSubRes( typeof( IronIngot ),			1044022, 60.0, 1044036, 1044268 );
			AddSubRes( typeof( SilverIngot ),		1044028, 90.0, 1044036, 1044268 );
			AddSubRes( typeof( GoldIngot ),			1044027, 85.0, 1044036, 1044268 );
			AddSubRes( typeof( ObsidianIngot ),		1044029, 95.0, 1044036, 1044268 );
			AddSubRes( typeof( SteelIngot ),		1044030, 99.0, 1044036, 1044268 );
			AddSubRes( typeof( StarmetalIngot ),	1044024, 99.0, 1044036, 1044268 );
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
