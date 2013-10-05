using System;
using Server;
using Server.Items;
using Server.Factions;
using Server.Targeting;
using Server.Mobiles;
using Server.Misc;

namespace Server.Engines.Craft
{
	public class DefTinkering : CraftSystem
	{
		public override SkillName MainSkill
		{
			get	{ return SkillName.Tinkering; }
		}

		public override int GumpTitleNumber
		{
			get { return 1044007; } // <CENTER>TINKERING MENU</CENTER>
		}

		private static CraftSystem m_CraftSystem;

		public static CraftSystem CraftSystem
		{
			get
			{
				if ( m_CraftSystem == null )
					m_CraftSystem = new DefTinkering();

				return m_CraftSystem;
			}
		}

		private DefTinkering() : base( 1, 1, 4.00 )// base( 1, 1, 3.0 )
		{
		}

		public override double GetChanceAtMin( CraftItem item )
		{
			if ( item.NameNumber == 1044258 || item.NameNumber == 1046445 ) // potion keg and faction trap removal kit
				return 0.5; // 50%

			return 0.0; // 0%
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
			else if ( itemType != null && ( itemType.IsSubclassOf( typeof( BaseFactionTrapDeed ) ) || itemType == typeof( FactionTrapRemovalKit ) ) && Faction.Find( from ) == null )
				return 1044573; // You have to be in a faction to do that.
			
			else if( from is PlayerMobile && itemType != null )
			{
				PlayerMobile m = from as PlayerMobile;
					
				if( itemType == typeof( FootTrap ) && m.Feats.GetFeatLevel(FeatList.NonLethalTraps) < 1 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( WeakBlackPowder ) && m.Feats.GetFeatLevel(FeatList.BlackPowder) < 1 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( BlackPowder ) && m.Feats.GetFeatLevel(FeatList.BlackPowder) < 2 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( PowerfulBlackPowder ) && m.Feats.GetFeatLevel(FeatList.BlackPowder) < 3 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( WateryOil ) && m.Feats.GetFeatLevel(FeatList.OilMaking) < 1 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( Oil ) && m.Feats.GetFeatLevel(FeatList.OilMaking) < 2 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( AdhesiveOil ) && m.Feats.GetFeatLevel(FeatList.OilMaking) < 3 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( BolaBall ) && m.Feats.GetFeatLevel(FeatList.NonLethalTraps) < 1 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( Bola ) && m.Feats.GetFeatLevel(FeatList.NonLethalTraps) < 1 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( Rope ) && m.Feats.GetFeatLevel(FeatList.RopeTrick) < 1 )
					return 1063492; // You lack the required feat.
				
				if( itemType == typeof( Gears ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ClockParts ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Hinge ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SextantParts ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ForkLeft ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ForkRight ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SpoonLeft ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( SpoonRight ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( AxleGears ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ClockLeft ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( ClockRight ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( Sextant ) && TestRace( m, Nation.Mhordul ) )
					return 1063491; // Your race cannot craft that item.
				
				else if( itemType == typeof( GoldBracelet ) && m.Feats.GetFeatLevel(FeatList.JewelryCrafting) < 1 )
					return 1063492; // You lack the required feat.

                else if( itemType == typeof( SilverBracelet ) && m.Feats.GetFeatLevel( FeatList.JewelryCrafting ) < 1 )
                    return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( SilverBeadNecklace ) && m.Feats.GetFeatLevel(FeatList.JewelryCrafting) < 2 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( GoldBeadNecklace ) && m.Feats.GetFeatLevel(FeatList.JewelryCrafting) < 2 )
					return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( GoldNecklace ) && m.Feats.GetFeatLevel(FeatList.JewelryCrafting) < 2 )
					return 1063492; // You lack the required feat.

                else if( itemType == typeof( SilverNecklace ) && m.Feats.GetFeatLevel( FeatList.JewelryCrafting ) < 2 )
                    return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( GoldEarrings ) && m.Feats.GetFeatLevel(FeatList.JewelryCrafting) < 3 )
					return 1063492; // You lack the required feat.

                else if( itemType == typeof( SilverEarrings ) && m.Feats.GetFeatLevel( FeatList.JewelryCrafting ) < 3 )
                    return 1063492; // You lack the required feat.

                else if( itemType == typeof( LargeGoldEarrings ) && m.Feats.GetFeatLevel( FeatList.JewelryCrafting ) < 3 )
                    return 1063492; // You lack the required feat.

                else if( itemType == typeof( LargeSilverEarrings ) && m.Feats.GetFeatLevel( FeatList.JewelryCrafting ) < 3 )
                    return 1063492; // You lack the required feat.
				
				else if( itemType == typeof( GoldRing ) && m.Feats.GetFeatLevel(FeatList.JewelryCrafting) < 3 )
					return 1063492; // You lack the required feat.

                else if( itemType == typeof( SilverRing ) && m.Feats.GetFeatLevel( FeatList.JewelryCrafting ) < 3 )
                    return 1063492; // You lack the required feat.

                else if( itemType == typeof( GoldAnkhNecklace ) && m.Feats.GetFeatLevel( FeatList.JewelryCrafting ) < 2 )
                    return 1063492; // You lack the required feat.

                else if( itemType == typeof( DelicateGoldBeadNecklace ) && m.Feats.GetFeatLevel( FeatList.JewelryCrafting ) < 2 )
                    return 1063492; // You lack the required feat.

                else if( itemType == typeof( ExtravagantGoldNecklace ) && m.Feats.GetFeatLevel( FeatList.JewelryCrafting ) < 2 )
                    return 1063492; // You lack the required feat.

                else if( itemType == typeof( LargeGoldNecklace ) && m.Feats.GetFeatLevel( FeatList.JewelryCrafting ) < 2 )
                    return 1063492; // You lack the required feat.

                else if( itemType == typeof( SilverSerpentNecklace ) && m.Feats.GetFeatLevel( FeatList.JewelryCrafting ) < 2 )
                    return 1063492; // You lack the required feat.

                else if( itemType == typeof( LargeSilverNecklace ) && m.Feats.GetFeatLevel( FeatList.JewelryCrafting ) < 2 )
                    return 1063492; // You lack the required feat.
			}

			return 0;
		}

		public override void PlayCraftEffect( Mobile from )
		{
			// no sound
			//from.PlaySound( 0x241 );
		}

		private static Type[] m_TinkerColorables = new Type[]
			{
				/*typeof( ForkLeft ), typeof( ForkRight ),
				typeof( SpoonLeft ), typeof( SpoonRight ),
				typeof( KnifeLeft ), typeof( KnifeRight ),
				typeof( Plate ),
				typeof( Goblet ), typeof( PewterMug ),
				typeof( KeyRing ),
				typeof( Candelabra ), typeof( Scales ),
				typeof( Key ), typeof( Globe ),
				typeof( Spyglass ), typeof( Lantern ),
				typeof( HeatingStand )*/
			};

		public override bool RetainsColorFrom( CraftItem item, Type type )
		{
			if ( !type.IsSubclassOf( typeof( BaseIngot ) ) )
				return false;

			type = item.ItemType;

			bool contains = false;

			for ( int i = 0; !contains && i < m_TinkerColorables.Length; ++i )
				contains = ( m_TinkerColorables[i] == type );

			return contains;
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

		public override bool ConsumeOnFailure( Mobile from, Type resourceType, CraftItem craftItem )
		{
			if ( resourceType == typeof( Silver ) )
				return false;

			return base.ConsumeOnFailure( from, resourceType, craftItem );
		}

		public void AddJewelrySet( GemType gemType, Type itemType, string gemName )
		{
			int offset = (int)gemType - 1;

			int index = AddCraft( typeof( GoldRing ), 1044049, gemName + " Gold Ring", 40.0, 90.0, typeof( GoldIngot ), 1044036, 1, 1044037 );
			AddRes( index, itemType, 1044231 + offset, 1, 1044240 );
			
			index = AddCraft( typeof( SilverRing ), 1044049, gemName + " Silver Ring", 40.0, 90.0, typeof( SilverIngot ), 1044036, 1, 1044037 );
			AddRes( index, itemType, 1044231 + offset, 1, 1044240 );

			index = AddCraft( typeof( GoldNecklace ), 1044049, gemName + " Gold Necklace", 40.0, 90.0, typeof( GoldIngot ), 1044036, 1, 1044037 );
			AddRes( index, itemType, 1044231 + offset, 1, 1044240 );
			
			index = AddCraft( typeof( SilverNecklace ), 1044049, gemName + " Silver Necklace", 40.0, 90.0, typeof( SilverIngot ), 1044036, 1, 1044037 );
			AddRes( index, itemType, 1044231 + offset, 1, 1044240 );

			index = AddCraft( typeof( GoldEarrings ), 1044049, gemName + " Gold Earrings", 40.0, 90.0, typeof( GoldIngot ), 1044036, 1, 1044037 );
			AddRes( index, itemType, 1044231 + offset, 1, 1044240 );
			
			index = AddCraft( typeof( SilverEarrings ), 1044049, gemName + " Silver Earrings", 40.0, 90.0, typeof( SilverIngot ), 1044036, 1, 1044037 );
			AddRes( index, itemType, 1044231 + offset, 1, 1044240 );
			
			index = AddCraft( typeof( LargeGoldEarrings ), 1044049, gemName + " Large Gold Earrings", 40.0, 90.0, typeof( GoldIngot ), 1044036, 1, 1044037 );
			AddRes( index, itemType, 1044231 + offset, 1, 1044240 );
			
			index = AddCraft( typeof( LargeSilverEarrings ), 1044049, gemName + " Large Silver Earrings", 40.0, 90.0, typeof( SilverIngot ), 1044036, 1, 1044037 );
			AddRes( index, itemType, 1044231 + offset, 1, 1044240 );

			index = AddCraft( typeof( GoldBeadNecklace ), 1044049, gemName + " Gold Bead Necklace", 40.0, 90.0, typeof( GoldIngot ), 1044036, 1, 1044037 );
			AddRes( index, itemType, 1044231 + offset, 1, 1044240 );

            index = AddCraft( typeof( GoldAnkhNecklace ), 1044049, gemName + " Gold Ankh Necklace", 40.0, 90.0, typeof( GoldIngot ), 1044036, 1, 1044037 );
            AddRes( index, itemType, 1044231 + offset, 1, 1044240 );

            index = AddCraft( typeof( DelicateGoldBeadNecklace ), 1044049, gemName + " Delicate Gold Bead Necklace", 40.0, 90.0, typeof( GoldIngot ), 1044036, 1, 1044037 );
            AddRes( index, itemType, 1044231 + offset, 1, 1044240 );

            index = AddCraft( typeof( ExtravagantGoldNecklace ), 1044049, gemName + " Extravagant Gold Necklace", 40.0, 90.0, typeof( GoldIngot ), 1044036, 1, 1044037 );
            AddRes( index, itemType, 1044231 + offset, 1, 1044240 );

            index = AddCraft( typeof( LargeGoldNecklace ), 1044049, gemName + " Large Gold Necklace", 40.0, 90.0, typeof( GoldIngot ), 1044036, 1, 1044037 );
            AddRes( index, itemType, 1044231 + offset, 1, 1044240 );
			
			index = AddCraft( typeof( SilverBeadNecklace ), 1044049, gemName + " Silver Bead Necklace", 40.0, 90.0, typeof( SilverIngot ), 1044036, 1, 1044037 );
			AddRes( index, itemType, 1044231 + offset, 1, 1044240 );

            index = AddCraft( typeof( SilverSerpentNecklace ), 1044049, gemName + " Silver Serpent Necklace", 40.0, 90.0, typeof( SilverIngot ), 1044036, 1, 1044037 );
            AddRes( index, itemType, 1044231 + offset, 1, 1044240 );

            index = AddCraft( typeof( LargeSilverNecklace ), 1044049, gemName + " Large Silver Necklace", 40.0, 90.0, typeof( SilverIngot ), 1044036, 1, 1044037 );
            AddRes( index, itemType, 1044231 + offset, 1, 1044240 );

			index = AddCraft( typeof( GoldBracelet ), 1044049, gemName + " Gold Bracelet", 40.0, 90.0, typeof( GoldIngot ), 1044036, 1, 1044037 );
			AddRes( index, itemType, 1044231 + offset, 1, 1044240 );
			
			index = AddCraft( typeof( SilverBracelet ), 1044049, gemName + " Silver Bracelet", 40.0, 90.0, typeof( SilverIngot ), 1044036, 1, 1044037 );
			AddRes( index, itemType, 1044231 + offset, 1, 1044240 );
		}

		public override void InitCraftList()
		{
			int index = -1;

			#region Wooden Items
			AddCraft( typeof( JointingPlane ), 1044042, 1024144, 5.0, 50.0, typeof( Log ), 1044041, 4, 1044351 );
			AddCraft( typeof( MouldingPlane ), 1044042, 1024140, 5.0, 50.0, typeof( Log ), 1044041, 4, 1044351 );
			AddCraft( typeof( SmoothingPlane ), 1044042, 1024146, 5.0, 50.0, typeof( Log ), 1044041, 4, 1044351 );
			AddCraft( typeof( ClockFrame ), 1044042, 1024173, 5.0, 50.0, typeof( Log ), 1044041, 6, 1044351 );
			AddCraft( typeof( Axle ), 1044042, 1024187, 5.0, 25.0, typeof( Log ), 1044041, 2, 1044351 );
			AddCraft( typeof( RollingPin ), 1044042, 1024163, 5.0, 50.0, typeof( Log ), 1044041, 2, 1044351 );

			#endregion

			#region Tools
			AddCraft( typeof( Scissors ), 1044046, 1023998, 5.0, 20.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( AlchemyTool  ), 1044046, "mortar and pestle", 5.0, 20.0, typeof( TinIngot ), 1042691, 3, 1044037 );
			AddCraft( typeof( MixingSet  ), 1044046, "mixing set", 5.0, 20.0, typeof( TinIngot ), 1042691, 3, 1044037 );
			AddCraft( typeof( Scorp ), 1044046, 1024327, 5.0, 20.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( TinkerTools ), 1044046, 1044164, 5.0, 20.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( Hatchet ), 1044046, 1023907, 5.0, 20.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( typeof( DrawKnife ), 1044046, 1024324, 5.0, 20.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( typeof( SewingKit ), 1044046, 1023997, 5.0, 20.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( Saw ), 1044046, 1024148, 5.0, 20.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( typeof( DovetailSaw ), 1044046, 1024136, 5.0, 20.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( typeof( Froe ), 1044046, 1024325, 5.0, 20.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( typeof( Shovel ), 1044046, 1023898, 5.0, 20.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( typeof( Hammer ), 1044046, 1024138, 5.0, 20.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( typeof( Tongs ), 1044046, 1024028, 5.0, 20.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( typeof( SmithHammer ), 1044046, 1025091, 5.0, 20.0, typeof( IronIngot ), 1044036, 2, 1044037 );
/*             AddCraft(typeof(BlackSmithingHammer), 1044046, "weapon smithing hammer", 5.0, 20.0, typeof(IronIngot), 1044036, 2, 1044037);
            AddCraft(typeof(WeaponDismantler), 1044046, "weapon dismantler", 5.0, 20.0, typeof(IronIngot), 1044036, 2, 1044037); */
			AddCraft( typeof( SledgeHammer ), 1044046, 1024021, 5.0, 20.0, typeof( IronIngot ), 1044036, 3, 1044037 );
			AddCraft( typeof( Inshave ), 1044046, 1024326, 5.0, 20.0, typeof( IronIngot ), 1044036, 2, 1044037 );
			AddCraft( typeof( Pickaxe ), 1044046, 1023718, 5.0, 20.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( typeof( Lockpick ), 1044046, 1025371, 5.0, 20.0, typeof( TinIngot ), 1042691, 1, 1044037 );
			AddCraft( typeof( Skillet ), 1044046, 1044567, 5.0, 20.0, typeof( TinIngot ), 1042691, 4, 1044037 );
			
			AddCraft( typeof( CooksCauldron ), 1044046, "cook's cauldron", 5.0, 20.0, typeof( TinIngot ), 1042691, 8, 1044037 );
			AddCraft( typeof( FryingPan ), 1044046, "frying pan", 5.0, 20.0, typeof( TinIngot ), 1042691, 4, 1044037 );
			AddCraft( typeof( BakersBoard ), 1044046, "baker's board", 5.0, 20.0, typeof( Log ), 1044041, 4, 1044351 );

            AddCraft( typeof( BrewersTools ), 1044046, "brewer's tools", 5.0, 20.0, typeof( TinIngot ), 1042691, 4, 1044037 );
            AddCraft( typeof( VinyardLabelMaker ), 1044046, "vinyard label maker", 5.0, 20.0, typeof( TinIngot ), 1042691, 4, 1044037 );

            AddCraft( typeof( WinecraftersTools ), 1044046, "winecrafter's tools", 5.0, 20.0, typeof( TinIngot ), 1042691, 4, 1044037 );
            AddCraft( typeof( BreweryLabelMaker ), 1044046, "brewery label maker", 5.0, 20.0, typeof( TinIngot ), 1042691, 4, 1044037 );
			
			AddCraft( typeof( FlourSifter ), 1044046, 1024158, 5.0, 20.0, typeof( TinIngot ), 1042691, 3, 1044037 );
			AddCraft( typeof( FletcherTools ), 1044046, 1044166, 5.0, 20.0, typeof( TinIngot ), 1042691, 3, 1044037 );
			AddCraft( typeof( MapmakersPen ), 1044046, 1044167, 5.0, 20.0, typeof( TinIngot ), 1042691, 1, 1044037 );
			AddCraft( typeof( ScribesPen ), 1044046, 1044168, 5.0, 20.0, typeof( TinIngot ), 1042691, 1, 1044037 );
            AddCraft( typeof( PaintsAndBrush ), 1044046, "paints and brush", 5.0, 20.0, typeof( TinIngot ), 1042691, 1, 1044037 );
			AddCraft( typeof( ClayHarvestingShovel ), 1044046, "clay harvesting shovel", 5.0, 20.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( typeof( PottersWheel ), 1044046, "potter's wheel", 5.0, 20.0, typeof( IronIngot ), 1044036, 20, 1044037 );
			AddCraft( typeof( Blowpipe ), 1044046, "blowpipe", 5.0, 20.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( typeof( Boline ), 1044046, "boline", 5.0, 20.0, typeof( IronIngot ), 1044036, 4, 1044037 );
			AddCraft( typeof( MalletAndChisel ), 1044046, "mallet and chisel", 5.0, 20.0, typeof( IronIngot ), 1044036, 6, 1044037 );
			AddCraft( typeof( Nails ), 1044046, "nails", 5.0, 20.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( TattooArtistTool ), 1044046, "tattoo artist's tool", 5.0, 20.0, typeof( TinIngot ), 1042691, 2, 1044037 );
            AddCraft(typeof(SealmakersTool), 1044046, "sealmaker's tool", 50.0, 75.0, typeof(TinIngot), 1042691, 4, 1044037);
			
            #endregion

			#region Parts
			AddCraft( typeof( Gears ), 1044047, 1024179, 5.0, 55.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( ClockParts ), 1044047, 1024175, 25.0, 75.0, typeof( TinIngot ), 1042691, 1, 1044037 );
			AddCraft( typeof( BarrelTap ), 1044047, 1024100, 35.0, 85.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( Springs ), 1044047, 1024189, 5.0, 55.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( SextantParts ), 1044047, 1024185, 30.0, 80.0, typeof( TinIngot ), 1042691, 4, 1044037 );
			AddCraft( typeof( BarrelHoops ), 1044047, 1024321, 5.0, 35.0, typeof( TinIngot ), 1042691, 5, 1044037 );
			AddCraft( typeof( Hinge ), 1044047, 1024181, 5.0, 55.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( BolaBall ), 1044047, 1023699, 0.0, 0.0, typeof( TinIngot ), 1042691, 10, 1044037 );
			
			#endregion

			#region Utensils
			AddCraft( typeof( SpoonLeft ), 1044048, 1044158, 5.0, 50.0, typeof( TinIngot ), 1042691, 1, 1044037 );
			AddCraft( typeof( SpoonRight ), 1044048, 1044159, 5.0, 50.0, typeof( TinIngot ), 1042691, 1, 1044037 );
			AddCraft( typeof( Plate ), 1044048, 1022519, 5.0, 50.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( ForkLeft ), 1044048, 1044160, 5.0, 50.0, typeof( TinIngot ), 1042691, 1, 1044037 );
			AddCraft( typeof( ForkRight ), 1044048, 1044161, 5.0, 50.0, typeof( TinIngot ), 1042691, 1, 1044037 );
			AddCraft( typeof( KnifeLeft ), 1044048, 1044162, 5.0, 50.0, typeof( TinIngot ), 1042691, 1, 1044037 );
			AddCraft( typeof( KnifeRight ), 1044048, 1044163, 5.0, 50.0, typeof( TinIngot ), 1042691, 1, 1044037 );
			AddCraft( typeof( Goblet ), 1044048, 1022458, 10.0, 60.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( PewterMug ), 1044048, 1024097, 10.0, 60.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( FoodPlate ), 1044048, "food plate", 10.0, 60.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			
			#endregion

			#region Misc
			AddCraft( typeof( KeyRing ), 1044050, 1024113, 10.0, 60.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( Candelabra ), 1044050, 1022599, 55.0, 105.0, typeof( TinIngot ), 1042691, 4, 1044037 );
			AddCraft( typeof( Scales ), 1044050, 1026225, 60.0, 110.0, typeof( TinIngot ), 1042691, 4, 1044037 );
			AddCraft( typeof( Key ), 1044050, 1024112, 20.0, 70.0, typeof( TinIngot ), 1042691, 3, 1044037 );
			AddCraft( typeof( Globe ), 1044050, 1024167, 55.0, 105.0, typeof( TinIngot ), 1042691, 4, 1044037 );
			AddCraft( typeof( Spyglass ), 1044050, 1025365, 60.0, 110.0, typeof( TinIngot ), 1042691, 4, 1044037 );
			AddCraft( typeof( Lantern ), 1044050, 1022597, 30.0, 80.0, typeof( TinIngot ), 1042691, 2, 1044037 );
			AddCraft( typeof( Brazier ), 1044050, "brazier", 30.0, 80.0, typeof( TinIngot ), 1042691, 4, 1044037 );
			AddCraft( typeof( BrazierTall ), 1044050, "tall brazier", 30.0, 80.0, typeof( TinIngot ), 1042691, 8, 1044037 );
			AddCraft( typeof( WallSconce ), 1044050, "wall sconce", 30.0, 80.0, typeof( TinIngot ), 1042691, 3, 1044037 );
			AddCraft( typeof( WallTorch ), 1044050, "wall torch", 30.0, 80.0, typeof( TinIngot ), 1042691, 3, 1044037 );
			AddCraft( typeof( HeatingStand ), 1044050, 1026217, 60.0, 110.0, typeof( TinIngot ), 1042691, 4, 1044037 );
            AddCraft( typeof( Tin ), 1044050, "tin coins", 0.0, 0.0, typeof( TinIngot ), 1042691, 10, 1044037 );
            AddCraft( typeof( Dyes ), 1044050, "dyes", 5.0, 20.0, typeof( Log ), 1044041, 2, 1044351 );
            AddCraft( typeof( MetalChest ), 1044050, "metal chest", 60.0, 110.0, typeof( TinIngot ), 1042691, 20, 1044037 );
            AddCraft( typeof( Safe ), 1044050, "safe", 60.0, 110.0, typeof( IronIngot ), 1044036, 50, 1044037 );
            index = AddCraft( typeof( WeakBlackPowder ), 1044050, "weak black powder", 15.0, 20.0, typeof( SulfurousAsh ), "Sulfurous Ash", 1, "You do not have enough sulfurous ash." );
			AddRes( index, typeof( Guano ), "Guano", 1, "You do not have enough guano to make that." );
			AddRes( index, typeof( Coal ), "Coal", 1, "You do not have enough coal to make that." );
			index = AddCraft( typeof( BlackPowder ), 1044050, "black powder", 15.0, 20.0, typeof( SulfurousAsh ), "Sulfurous Ash", 2, "You do not have enough sulfurous ash." );
			AddRes( index, typeof( Guano ), "Guano", 1, "You do not have enough guano to make that." );
			AddRes( index, typeof( Coal ), "Coal", 1, "You do not have enough coal to make that." );
			index = AddCraft( typeof( PowerfulBlackPowder ), 1044050, "powerful black powder", 15.0, 20.0, typeof( SulfurousAsh ), "Sulfurous Ash", 3, "You do not have enough sulfurous ash." );
			AddRes( index, typeof( Guano ), "Guano", 1, "You do not have enough guano to make that." );
			AddRes( index, typeof( Coal ), "Coal", 1, "You do not have enough coal to make that." );
			AddCraft( typeof( LowQualityFireworks ), 1044050, "low quality fireworks", 15.0, 20.0, typeof( WeakBlackPowder ), "Weak Black Powder", 1, "You do not have enough black powder." );
			AddCraft( typeof( MediumQualityFireworks ), 1044050, "medium quality fireworks", 15.0, 20.0, typeof( BlackPowder ), "Black Powder", 1, "You do not have enough black powder." );
			AddCraft( typeof( ExcellentQualityFireworks ), 1044050, "excellent quality fireworks", 15.0, 20.0, typeof( PowerfulBlackPowder ), "Powerful Black Powder", 1, "You do not have enough black powder." );
			AddCraft( typeof( WateryOil ), 1044050, "watery oil", 15.0, 20.0, typeof( Cotton ), "Cotton", 1, "You do not have enough cotton." );
			AddCraft( typeof( Oil ), 1044050, "oil", 15.0, 20.0, typeof( Cotton ), "Cotton", 2, "You do not have enough cotton." );
			AddCraft( typeof( AdhesiveOil ), 1044050, "adhesive oil", 15.0, 20.0, typeof( Cotton ), "Cotton", 3, "You do not have enough cotton." );

            AddCraft( typeof( SquareGlasses ), 1044050, "square glasses", 30.0, 80.0, typeof( TinIngot ), 1042691, 6, 1044037 );
            AddCraft( typeof( RoundGlasses ), 1044050, "round glasses", 30.0, 80.0, typeof( TinIngot ), 1042691, 6, 1044037 );
            AddCraft( typeof( ThickRoundGlasses ), 1044050, "thick round glasses", 30.0, 80.0, typeof( TinIngot ), 1042691, 6, 1044037 );
            AddCraft( typeof( FancyGlasses ), 1044050, "fancy glasses", 30.0, 80.0, typeof( TinIngot ), 1042691, 6, 1044037 );
            AddCraft( typeof( Monocle ), 1044050, "monocle", 30.0, 80.0, typeof( TinIngot ), 1042691, 6, 1044037 );
            AddCraft( typeof( FancyMonocle ), 1044050, "fancy monocle", 30.0, 80.0, typeof( TinIngot ), 1042691, 6, 1044037 );
            AddCraft( typeof( Hook ), 1044050, "hook", 30.0, 80.0, typeof( TinIngot ), 1042691, 10, 1044037 );

            AddCraft(typeof(CandleShort), 1044050, "short candle", 5.0, 20.0, typeof(Beeswax), "Beeswax", 2, "You do not have enough beeswax.");
            AddCraft(typeof(Candle), 1044050, "candle", 5.0, 20.0, typeof(Beeswax), "Beeswax", 3, "You do not have enough beeswax.");
            AddCraft(typeof(CandleLong), 1044050, "long candle", 5.0, 20.0, typeof(Beeswax), "Beeswax", 4, "You do not have enough beeswax.");
            AddCraft(typeof(CandleLarge), 1044050, "large candle", 5.0, 20.0, typeof(Beeswax), "Beeswax", 5, "You do not have enough beeswax.");
            index = AddCraft(typeof(CandleSkull), 1044050, "skull candle", 34.0, 80.0, typeof(Beeswax), "Beesax", 5, "You do not have enough beeswax.");
            AddRes(index, typeof(Bone), "Bone", 3, "You do not have enough bones.");

            index = AddCraft( typeof( Supplies ), 1044050, "supplies", 5.0, 20.0, typeof( CopperIngot ), 1044036, 25, 1044037 );
            AddRes( index, typeof( Leather ), 1044462, 25, 1044463 );
            AddRes( index, typeof( Log ), 1044041, 25, 1044351 );

			#endregion

			#region Jewelry
			AddJewelrySet( GemType.StarSapphire, typeof( StarSapphire ), "Star Sapphire" );
			AddJewelrySet( GemType.Emerald, typeof( Emerald ), "Emerald" );
			AddJewelrySet( GemType.Sapphire, typeof( Sapphire ), "Sapphire" );
			AddJewelrySet( GemType.Ruby, typeof( Ruby ), "Ruby" );
			AddJewelrySet( GemType.Citrine, typeof( Citrine ), "Citrine" );
			AddJewelrySet( GemType.Amethyst, typeof( Amethyst ), "Amethyst" );
			AddJewelrySet( GemType.Tourmaline, typeof( Tourmaline ), "Tourmaline" );
			AddJewelrySet( GemType.Amber, typeof( Amber ), "Amber" );
			AddJewelrySet( GemType.Diamond, typeof( Diamond ), "Diamond" );
			
			#endregion

			#region Multi-Component Items
			index = AddCraft( typeof( AxleGears ), 1044051, 1024177, 5.0, 20.0, typeof( Axle ), 1044169, 1, 1044253 );
			AddRes( index, typeof( Gears ), 1044254, 1, 1044253 );

			index = AddCraft( typeof( ClockParts ), 1044051, 1024175, 5.0, 20.0, typeof( AxleGears ), 1044170, 1, 1044253 );
			AddRes( index, typeof( Springs ), 1044171, 1, 1044253 );

			index = AddCraft( typeof( SextantParts ), 1044051, 1024185, 5.0, 20.0, typeof( AxleGears ), 1044170, 1, 1044253 );
			AddRes( index, typeof( Hinge ), 1044172, 1, 1044253 );

			index = AddCraft( typeof( ClockRight ), 1044051, 1044257, 5.0, 20.0, typeof( ClockFrame ), 1044174, 1, 1044253 );
			AddRes( index, typeof( ClockParts ), 1044173, 1, 1044253 );

			index = AddCraft( typeof( ClockLeft ), 1044051, 1044256, 5.0, 20.0, typeof( ClockFrame ), 1044174, 1, 1044253 );
			AddRes( index, typeof( ClockParts ), 1044173, 1, 1044253 );

			AddCraft( typeof( Sextant ), 1044051, 1024183, 5.0, 20.0, typeof( SextantParts ), 1044175, 1, 1044253 );

			index = AddCraft( typeof( Bola ), 1044051, 1046441, 0.0, 0.0, typeof( BolaBall ), 1046440, 4, 1042613 );
			AddRes( index, typeof( Leather ), 1044462, 3, 1044463 );
			
			#endregion

			#region Traps
			// Dart Trap
			index = AddCraft( typeof( DartTrapCraft ), 1044052, 1024396, 30.0, 80.0, typeof( IronIngot ), 1044036, 6, 1044037 );
			AddRes( index, typeof( Bolt ), 1044570, 1, 1044253 );

			// Poison Trap
			index = AddCraft( typeof( PoisonTrapCraft ), 1044052, 1044593, 30.0, 80.0, typeof( IronIngot ), 1044036, 6, 1044037 );
			AddRes( index, typeof( BasePoisonPotion ), 1044571, 1, 1044253 );

			// Explosion Trap
			index = AddCraft( typeof( ExplosionTrapCraft ), 1044052, 1044597, 55.0, 105.0, typeof( IronIngot ), 1044036, 6, 1044037 );
			AddRes( index, typeof( BaseExplosionPotion ), 1044569, 1, 1044253 );
			
			index = AddCraft( typeof( FootTrap ), 1044052, "foot trap", 0.0, 0.0, typeof( IronIngot ), 1044036, 6, 1044037 );
			index = AddCraft( typeof( Rope ), 1044052, "rope", 0.0, 0.0, typeof( Leather ), 1044462, 6, 1044463 );

			#endregion

			// Set the overridable material
//			SetSubRes( typeof( CopperIngot ), 1044022 );

			// Add every material you want the player to be able to choose from
			// This will override the overridable material
//			AddSubRes( typeof( IronIngot ),			1044022, 60.0, 1044036, 1044267 );
//			AddSubRes( typeof( CopperIngot ),		1044025, 00.0, 1044036, 1044268 );
//			AddSubRes( typeof( BronzeIngot ),		1044026, 30.0, 1044036, 1044268 );
//			AddSubRes( typeof( GoldIngot ),			1044027, 85.0, 1044036, 1044268 );
//			AddSubRes( typeof( SilverIngot ),		1044028, 90.0, 1044036, 1044268 );
//			AddSubRes( typeof( ObsidianIngot ),		1044029, 95.0, 1044036, 1044268 );
//			AddSubRes( typeof( SilverIngot ),		1044030, 99.0, 1044036, 1044268 );
			//
			SetSubRes( typeof( CopperIngot ), 1044025 );
			AddSubRes( typeof( CopperIngot ),		1044025, 00.0, 1044036, 1044268 );
			AddSubRes( typeof( BronzeIngot ),		1044026, 30.0, 1044036, 1044268 );
			AddSubRes( typeof( IronIngot ),			1044022, 00.0, 1044036, 1044267 );
			AddSubRes( typeof( SilverIngot ),		1044028, 90.0, 1044036, 1044268 );
			AddSubRes( typeof( GoldIngot ),			1044027, 85.0, 1044036, 1044268 );
			AddSubRes( typeof( ObsidianIngot ),		1044029, 95.0, 1044036, 1044268 );
			AddSubRes( typeof( SteelIngot ),		1044030, 99.0, 1044036, 1044268 );
			AddSubRes( typeof( StarmetalIngot ),	1044024, 99.0, 1044036, 1044268 );

			MarkOption = true;
			Repair = true;
			CanEnhance = false;
		}
	}

	public abstract class TrapCraft : CustomCraft
	{
		private LockableContainer m_Container;

		public LockableContainer Container{ get{ return m_Container; } }

		public abstract TrapType TrapType{ get; }

		public TrapCraft( Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool, int quality ) : base( from, craftItem, craftSystem, typeRes, tool, quality )
		{
		}

		private int Verify( LockableContainer container )
		{
			if ( container == null || container.KeyValue == 0 )
				return 1005638; // You can only trap lockable chests.
			if ( From.Map != container.Map || !From.InRange( container.GetWorldLocation(), 2 ) )
				return 500446; // That is too far away.
			if ( !container.Movable )
				return 502944; // You cannot trap this item because it is locked down.
			if ( !container.IsAccessibleTo( From ) )
				return 502946; // That belongs to someone else.
			if ( container.Locked )
				return 502943; // You can only trap an unlocked object.
			if ( container.TrapType != TrapType.None )
				return 502945; // You can only place one trap on an object at a time.

			return 0;
		}

		private bool Acquire( object target, out int message )
		{
			LockableContainer container = target as LockableContainer;

			message = Verify( container );

			if ( message > 0 )
			{
				return false;
			}
			else
			{
				m_Container = container;
				return true;
			}
		}

		public override void EndCraftAction()
		{
			From.SendLocalizedMessage( 502921 ); // What would you like to set a trap on?
			From.Target = new ContainerTarget( this );
		}

		private class ContainerTarget : Target
		{
			private TrapCraft m_TrapCraft;

			public ContainerTarget( TrapCraft trapCraft ) : base( -1, false, TargetFlags.None )
			{
				m_TrapCraft = trapCraft;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				int message;

				if ( m_TrapCraft.Acquire( targeted, out message ) )
					m_TrapCraft.CraftItem.CompleteCraft( m_TrapCraft.Quality, false, m_TrapCraft.From, m_TrapCraft.CraftSystem, m_TrapCraft.TypeRes, m_TrapCraft.Tool, m_TrapCraft );
				else
					Failure( message );
			}

			protected override void OnTargetCancel( Mobile from, TargetCancelType cancelType )
			{
				if ( cancelType == TargetCancelType.Canceled )
					Failure( 0 );
			}

			private void Failure( int message )
			{
				Mobile from = m_TrapCraft.From;
				BaseTool tool = m_TrapCraft.Tool;

				if ( tool != null && !tool.Deleted && tool.UsesRemaining > 0 )
					from.SendGump( new CraftGump( from, m_TrapCraft.CraftSystem, tool, message ) );
				else if ( message > 0 )
					from.SendLocalizedMessage( message );
			}
		}

		public override Item CompleteCraft( out int message )
		{
			message = Verify( this.Container );

			if ( message == 0 )
			{
				int trapLevel = (int)(From.Skills.Tinkering.Value / 10);

				Container.TrapType = this.TrapType;
				Container.TrapPower = trapLevel * 9;
				Container.TrapLevel = trapLevel;
				Container.TrapOnLockpick = true;

				message = 1005639; // Trap is disabled until you lock the chest.
			}

			return null;
		}
	}

	[CraftItemID( 0x1BFC )]
	public class DartTrapCraft : TrapCraft
	{
		public override TrapType TrapType{ get{ return TrapType.DartTrap; } }

		public DartTrapCraft( Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool, int quality ) : base( from, craftItem, craftSystem, typeRes, tool, quality )
		{
		}
	}

	[CraftItemID( 0x113E )]
	public class PoisonTrapCraft : TrapCraft
	{
		public override TrapType TrapType{ get{ return TrapType.PoisonTrap; } }

		public PoisonTrapCraft( Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool, int quality ) : base( from, craftItem, craftSystem, typeRes, tool, quality )
		{
		}
	}

	[CraftItemID( 0x370C )]
	public class ExplosionTrapCraft : TrapCraft
	{
		public override TrapType TrapType{ get{ return TrapType.ExplosionTrap; } }

		public ExplosionTrapCraft( Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool, int quality ) : base( from, craftItem, craftSystem, typeRes, tool, quality )
		{
		}
	}
}
