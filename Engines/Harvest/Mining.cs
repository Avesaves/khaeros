using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections;
using Server.Misc;

namespace Server.Engines.Harvest
{
	public class Mining : HarvestSystem
	{
		private static Mining m_System;

		public static Mining System
		{
			get
			{
				if ( m_System == null )
					m_System = new Mining();

				return m_System;
			}
		}

		private HarvestDefinition m_OreAndStone, m_Sand;

		public HarvestDefinition OreAndStone
		{
			get{ return m_OreAndStone; }
		}

		public HarvestDefinition Sand
		{
			get{ return m_Sand; }
		}

		private Mining()
		{
			HarvestResource[] res;
			HarvestVein[] veins;

			#region Mining for ore and stone
			HarvestDefinition oreAndStone = m_OreAndStone = new HarvestDefinition();

			// Resource banks are every 8x8 tiles
			oreAndStone.BankWidth = 4;
			oreAndStone.BankHeight = 4;

			// Every bank holds from 10 to 34 ore
			oreAndStone.MinTotal = 8;
			oreAndStone.MaxTotal = 25;

			// A resource bank will respawn its content every 10 to 20 minutes
			oreAndStone.MinRespawn = TimeSpan.FromMinutes( 10.0 );
			oreAndStone.MaxRespawn = TimeSpan.FromMinutes( 20.0 );

			// Skill checking is done on the Mining skill
			oreAndStone.Skill = SkillName.Mining;

			// Set the list of harvestable tiles
			oreAndStone.Tiles = m_MountainAndCaveTiles;

			// Players must be within 2 tiles to harvest
			oreAndStone.MaxRange = 2;

			// One ore per harvest action
			oreAndStone.ConsumedPerHarvest = 1;
			oreAndStone.ConsumedPerFeluccaHarvest = 1;

			// The digging effect
			oreAndStone.EffectActions = new int[]{ 11 };
			oreAndStone.EffectSounds = new int[]{ 0x125, 0x126 };
			oreAndStone.EffectCounts = new int[]{ 1 };
			oreAndStone.EffectDelay = TimeSpan.FromSeconds( 1.6 );
			oreAndStone.EffectSoundDelay = TimeSpan.FromSeconds( 0.9 );

			oreAndStone.NoResourcesMessage = 503040; // There is no metal here to mine.
			oreAndStone.DoubleHarvestMessage = 503042; // Someone has gotten to the metal before you.
			oreAndStone.TimedOutOfRangeMessage = 503041; // You have moved too far away to continue mining.
			oreAndStone.OutOfRangeMessage = 500446; // That is too far away.
			oreAndStone.FailMessage = 503043; // You loosen some rocks but fail to find any useable ore.
			oreAndStone.PackFullMessage = 1010481; // Your backpack is full, so the ore you mined is lost.
			oreAndStone.ToolBrokeMessage = 1044038; // You have worn out your tool!

			res = new HarvestResource[]
				{
					new HarvestResource( 00.0, 00.0, 50.0, 1007075, typeof( IronOre ),		typeof( Coal ),				typeof( Bat ) ),
					new HarvestResource( 00.0, 00.0, 50.0, 1007073, typeof( TinOre ),			typeof( Granite ),			typeof( Bat ) ),
					new HarvestResource( 60.0, 40.0, 100.0, 1007072, typeof( CopperOre ),			typeof( Granite ),			typeof( Bat ) ),
					new HarvestResource( 85.0, 50.0, 100.0, 1007078, typeof( SilverOre ),		typeof( Granite ),			typeof( Bat ) ),
                    new HarvestResource( 95.0, 55.0, 100.0, 1007077, typeof( GoldOre ),			typeof( Granite ),			typeof( Bat ) ),
                   
				};

			veins = new HarvestVein[]
				{
					new HarvestVein( 45.0, 0.5, res[0], res[0] ),  // Copper
					new HarvestVein( 30.0, 0.5, res[1], res[0] ),  // Tin
					new HarvestVein( 24.0, 0.5, res[2], res[0] ),  // Iron
					new HarvestVein( 0.9, 0.5, res[3], res[0] ),   // Silver
					new HarvestVein( 0.1, 0.5, res[4], res[0] ),   // Gold
				};

			oreAndStone.Resources = res;
			oreAndStone.Veins = veins;

			Definitions.Add( oreAndStone );
			#endregion

			#region Mining for sand
			HarvestDefinition sand = m_Sand = new HarvestDefinition();

			// Resource banks are every 8x8 tiles
			sand.BankWidth = 2;
			sand.BankHeight = 2;

			// Every bank holds from 6 to 12 sand
			sand.MinTotal = 10;
			sand.MaxTotal = 18;

			// A resource bank will respawn its content every 10 to 20 minutes
			sand.MinRespawn = TimeSpan.FromMinutes( 10.0 );
			sand.MaxRespawn = TimeSpan.FromMinutes( 20.0 );

			// Skill checking is done on the Mining skill
			sand.Skill = SkillName.Mining;

			// Set the list of harvestable tiles
			sand.Tiles = m_SandTiles;

			// Players must be within 2 tiles to harvest
			sand.MaxRange = 2;

			// One sand per harvest action
			sand.ConsumedPerHarvest = 1;
			sand.ConsumedPerFeluccaHarvest = 1;

			// The digging effect
			sand.EffectActions = new int[]{ 11 };
			sand.EffectSounds = new int[]{ 0x125, 0x126 };
			sand.EffectCounts = new int[]{ 1 };
			sand.EffectDelay = TimeSpan.FromSeconds( 1.6 );
			sand.EffectSoundDelay = TimeSpan.FromSeconds( 0.9 );

			sand.NoResourcesMessage = 1044629; // There is no sand here to mine.
			sand.DoubleHarvestMessage = 1044629; // There is no sand here to mine.
			sand.TimedOutOfRangeMessage = 503041; // You have moved too far away to continue mining.
			sand.OutOfRangeMessage = 500446; // That is too far away.
			sand.FailMessage = 1044630; // You dig for a while but fail to find any of sufficient quality for glassblowing.
			sand.PackFullMessage = 1044632; // Your backpack can't hold the sand, and it is lost!
			sand.ToolBrokeMessage = 1044038; // You have worn out your tool!

			res = new HarvestResource[]
				{
					new HarvestResource( 0.0, 0.0, 50.0, 1044631, typeof( Sand ) )
				};

			veins = new HarvestVein[]
				{
					new HarvestVein( 0.0, 0.0, res[0], null )
				};

			sand.Resources = res;
			sand.Veins = veins;

			Definitions.Add( sand );
			#endregion
		}

		public override Type GetResourceType( Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource )
		{
			if ( def == m_OreAndStone )
			{
				if ( from is PlayerMobile && ((PlayerMobile)from).StoneMining && ((PlayerMobile)from).ToggleMiningStone && 0.1 > Utility.RandomDouble() )
					return resource.Types[1];
				
				return resource.Types[0];
			}

			return base.GetResourceType( from, tool, def, map, loc, resource );
		}

		public override bool CheckHarvest( Mobile from, Item tool )
		{
			if ( !base.CheckHarvest( from, tool ) )
				return false;
		    from.RevealingAction();

			if ( from.Mounted )
			{
				from.SendLocalizedMessage( 501864 ); // You can't mine while riding.
				return false;
			}
			
			if( tool is Pickaxe )
			{
				if( from.Weapon != tool )
				{
					from.SendMessage( "You need to equip your pickaxe in order to mine." );
					return false;
				}
			}
			
			else if ( from.IsBodyMod && !from.Body.IsHuman )
			{
				from.SendLocalizedMessage( 501865 ); // You can't mine while polymorphed.
				return false;
			}

			return true;
		}

		public override void SendSuccessTo( Mobile from, Item item, HarvestResource resource )
		{
		}
		
		public override bool Give( Mobile m, Item item, bool placeAtFeet )
		{
			PlayerMobile pm = m as PlayerMobile;
			
			if( m is PlayerMobile )
			{
				pm.Crafting = true;	
				LevelSystem.AwardMinimumXP( pm, 1 );
				pm.Crafting = false;
				Item controller = null;
				CopperOre ore = new CopperOre();
				
				if( pm.UniqueSpot != null )
				{
					if( World.FindItem( pm.UniqueSpot.Serial ) != null )
					{
						controller = World.FindItem( pm.UniqueSpot.Serial );
						
						if( controller is ResourceController && pm.InRange( controller, ( (ResourceController)controller ).Range ) )
						{
							ResourceController rescontroller = controller as ResourceController;
							double chance = 0;
							
							switch( rescontroller.Intensity )
							{
								case VeinIntensity.Low: chance = 4; break;
								case VeinIntensity.Average: chance = 6; break;
								case VeinIntensity.Full: chance = 8; break;
							}
							
							if( chance >= Utility.RandomMinMax( 1, 100 ) )
							{
                                if( m.AccessLevel > AccessLevel.Player )
                                    m.SendMessage( "Debug message: resource controller activated." );

								switch( rescontroller.ControlledResource )
								{
									case ControlledResource.Copper: item = ore; break;
									
									case ControlledResource.Tin:
									{
										TinOre newore = new TinOre();
										item = newore;
										break;
									}
									
									case ControlledResource.Iron:
									{
										if( pm.Skills[SkillName.Mining].Base >= 80 )
										{
											IronOre newore = new IronOre();
											item = newore;
										}
										break;
									}
									
									case ControlledResource.Obsidian:
									{
										ObsidianIngot newore = new ObsidianIngot();
										item = newore;
										item.Amount = 5;
										break;
									}
									
									case ControlledResource.Silver:
									{
										if( pm.Skills[SkillName.Mining].Base >= 85 )
										{
											SilverOre newore = new SilverOre();
											item = newore;
										}
										break;
									}
									
									case ControlledResource.Gold:
									{
										if( pm.Skills[SkillName.Mining].Base >= 95 )
										{
											GoldOre newore = new GoldOre();
											item = newore;
										}
										break;
									}
									
									case ControlledResource.Citrine:
									{
										if( pm.Skills[SkillName.Mining].Base >= 80 )
										{
											Citrine gem = new Citrine();
											item = gem;
										}
										break;
									}
									
									case ControlledResource.Tourmaline:
									{
										if( pm.Skills[SkillName.Mining].Base >= 80 )
										{
											Tourmaline gem = new Tourmaline();
											item = gem;
										}
										break;
									}
									
									case ControlledResource.Amethyst:
									{
										if( pm.Skills[SkillName.Mining].Base >= 90 )
										{
											Amethyst gem = new Amethyst();
											item = gem;
										}
										break;
									}
									
									case ControlledResource.Emerald:
									{
										if( pm.Skills[SkillName.Mining].Base >= 90 )
										{
											Emerald gem = new Emerald();
											item = gem;
										}
										break;
									}
									
									case ControlledResource.Ruby:
									{
										if( pm.Skills[SkillName.Mining].Base >= 90 )
										{
											Ruby gem = new Ruby();
											item = gem;
										}
										break;
									}
									
									case ControlledResource.Sapphire:
									{
										if( pm.Skills[SkillName.Mining].Base >= 85 )
										{
											Sapphire gem = new Sapphire();
											item = gem;
										}
										break;
									}
									
									case ControlledResource.StarSapphire:
									{
										if( pm.Skills[SkillName.Mining].Base >= 95 )
										{
											StarSapphire gem = new StarSapphire();
											item = gem;
										}
										break;
									}
									
									case ControlledResource.Diamond:
									{
										if( pm.Skills[SkillName.Mining].Base >= 95 )
										{
											Diamond gem = new Diamond();
											item = gem;
										}
										break;
									}

                                    case ControlledResource.Coal:
                                    {
                                        if( pm.Feats.GetFeatLevel( FeatList.Sculptor ) > 0 )
                                        {
                                            Coal coal = new Coal();
                                            item = coal;
                                        }
                                        break;
                                    }

                                    case ControlledResource.Cinnabar:
                                    {
                                        if (pm.Feats.GetFeatLevel(FeatList.GemHarvesting) > 2)
                                        {
                                            Cinnabar cinnabar = new Cinnabar();
                                            item = cinnabar;
                                        }
                                        break;
                                    }
								}
							}
						}
					}
				}
				
				if( item is Citrine || item is Tourmaline || item is Amethyst )
				{
					if( pm.Feats.GetFeatLevel(FeatList.GemHarvesting) < 1 || !pm.GemHarvesting)
						item = ore;
				}
				
				else if( item is Emerald || item is Ruby || item is Sapphire )
				{
					if( pm.Feats.GetFeatLevel(FeatList.GemHarvesting) < 2 )
						item = ore;
					
					if( !pm.GemHarvesting )
					{
						TinOre tin = new TinOre();
						item = tin;
					}
				}
				
				else if( item is StarSapphire || item is Diamond || item is Cinnabar)
				{
					if( pm.Feats.GetFeatLevel(FeatList.GemHarvesting) < 3 )
						item = ore;
					
					if( !pm.GemHarvesting )
					{
						IronOre iron = new IronOre();
						item = iron;
					}
				}
				
				else if( item is ObsidianIngot && pm.Feats.GetFeatLevel(FeatList.Obsidian) < 1 )
					item = ore;
			}
			
			if( m is PlayerMobile && pm.Feats.GetFeatLevel(FeatList.GemHarvesting) > 0 && pm.GemHarvesting && item is CopperOre && Utility.RandomMinMax( 1, 100 ) > 95 )
			{
				int roll = Utility.RandomMinMax( 1, 100 );
                if (roll > 95 && pm.Feats.GetFeatLevel(FeatList.GemHarvesting) > 2)
                    item = new Cinnabar();
				if( roll > 90 && pm.Feats.GetFeatLevel(FeatList.GemHarvesting) > 2)
					item = new Diamond();
				else if( roll > 80 && pm.Feats.GetFeatLevel(FeatList.GemHarvesting) > 2 )
					item = new StarSapphire();
				else if( roll > 70 && pm.Feats.GetFeatLevel(FeatList.GemHarvesting) > 1 )
					item = new Ruby();
				else if( roll > 59 && pm.Feats.GetFeatLevel(FeatList.GemHarvesting) > 1 )
					item = new Emerald();
				else if( roll > 48 && pm.Feats.GetFeatLevel(FeatList.GemHarvesting) > 1 )
					item = new Sapphire();
				else if( roll > 37 )
					item = new Amethyst();
				else if( roll > 20 )
					item = new Tourmaline();
				else
					item = new Citrine();
			}
			
			string orename = "some copper ore";

            if( item is TinOre )
                orename = "some tin ore";

            else if( item is IronOre )
                orename = "some iron ore";

            else if( item is ObsidianIngot )
                orename = "some obsidian";

            else if( item is SilverOre )
                orename = "some silver ore";

            else if( item is GoldOre )
                orename = "some gold ore";

            else if( item is Citrine )
                orename = "a citrine";

            else if( item is Tourmaline )
                orename = "a tourmaline";

            else if( item is Emerald )
                orename = "an emerald";

            else if( item is Amethyst )
                orename = "an amethyst";

            else if( item is Ruby )
                orename = "a ruby";

            else if( item is Sapphire )
                orename = "a sapphire";

            else if( item is StarSapphire )
                orename = "a star sapphire";

            else if( item is Diamond )
                orename = "a diamond";

            else if( item is Sand )
                orename = "some sand";

            else if( item is Coal )
                orename = "some coal";
			
			m.SendMessage( "You dig " + orename + " and put it in your backpack." );
			
			if ( m.PlaceInBackpack( item ) )
				return true;

			if ( !placeAtFeet )
				return false;

			Map map = m.Map;

			if ( map == null )
				return false;

			ArrayList atFeet = new ArrayList();

			foreach ( Item obj in m.GetItemsInRange( 0 ) )
				atFeet.Add( obj );

			for ( int i = 0; i < atFeet.Count; ++i )
			{
				Item check = (Item)atFeet[i];

				if ( check.StackWith( m, item, false ) )
					return true;
			}

			item.MoveToWorld( m.Location, map );
			return true;
		}

		public override bool CheckHarvest( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			if ( !base.CheckHarvest( from, tool, def, toHarvest ) )
				return false;

			if ( def == m_Sand && !( from is PlayerMobile && ((PlayerMobile)from).Feats.GetFeatLevel(FeatList.GlassBlower) > 0 ) )
			{
				OnBadHarvestTarget( from, tool, toHarvest );
				return false;
			}
			else if ( from.Mounted )
			{
				from.SendLocalizedMessage( 501864 ); // You can't mine while riding.
				return false;
			}
			else if ( from.IsBodyMod && !from.Body.IsHuman )
			{
				from.SendLocalizedMessage( 501865 ); // You can't mine while polymorphed.
				return false;
			}

			return true;
		}

		public override HarvestVein MutateVein( Mobile from, Item tool, HarvestDefinition def, HarvestBank bank, object toHarvest, HarvestVein vein )
		{
			if ( tool is GargoylesPickaxe && def == m_OreAndStone )
			{
				int veinIndex = Array.IndexOf( def.Veins, vein );

				if ( veinIndex >= 0 && veinIndex < (def.Veins.Length - 1) )
					return def.Veins[veinIndex + 1];
			}

			return base.MutateVein( from, tool, def, bank, toHarvest, vein );
		}

		private static int[] m_Offsets = new int[]
			{
				-1, -1,
				-1,  0,
				-1,  1,
				 0, -1,
				 0,  1,
				 1, -1,
				 1,  0,
				 1,  1
			};

		public override void OnHarvestFinished( Mobile from, Item tool, HarvestDefinition def, HarvestVein vein, HarvestBank bank, HarvestResource resource, object harvested )
		{
			if ( tool is GargoylesPickaxe && def == m_OreAndStone && 0.1 > Utility.RandomDouble() )
			{
				HarvestResource res = vein.PrimaryResource;

				if ( res == resource && res.Types.Length >= 3 )
				{
					try
					{
						Map map = from.Map;

						if ( map == null )
							return;

						BaseCreature spawned = Activator.CreateInstance( res.Types[2], new object[]{ 25 } ) as BaseCreature;

						if ( spawned != null )
						{
							int offset = Utility.Random( 8 ) * 2;

							for ( int i = 0; i < m_Offsets.Length; i += 2 )
							{
								int x = from.X + m_Offsets[(offset + i) % m_Offsets.Length];
								int y = from.Y + m_Offsets[(offset + i + 1) % m_Offsets.Length];

								if ( map.CanSpawnMobile( x, y, from.Z ) )
								{
									spawned.MoveToWorld( new Point3D( x, y, from.Z ), map );
									spawned.Combatant = from;
									return;
								}
								else
								{
									int z = map.GetAverageZ( x, y );

									if ( map.CanSpawnMobile( x, y, z ) )
									{
										spawned.MoveToWorld( new Point3D( x, y, z ), map );
										spawned.Combatant = from;
										return;
									}
								}
							}

							spawned.MoveToWorld( from.Location, from.Map );
							spawned.Combatant = from;
						}
					}
					catch
					{
					}
				}
			}
		}

		public override bool BeginHarvesting( Mobile from, Item tool )
		{
			if ( !base.BeginHarvesting( from, tool ) )
				return false;

			from.SendLocalizedMessage( 503033 ); // Where do you wish to dig?
			return true;
		}

		public override void OnBadHarvestTarget( Mobile from, Item tool, object toHarvest )
		{
			if ( toHarvest is LandTarget )
				from.SendLocalizedMessage( 501862 ); // You can't mine there.
			else
				from.SendLocalizedMessage( 501863 ); // You can't mine that.
		}

		#region Tile lists
		private static int[] m_MountainAndCaveTiles = new int[]
			{
				220, 221, 222, 223, 224, 225, 226, 227, 228, 229,
				230, 231, 236, 237, 238, 239, 240, 241, 242, 243,
				244, 245, 246, 247, 252, 253, 254, 255, 256, 257,
				258, 259, 260, 261, 262, 263, 268, 269, 270, 271,
				272, 273, 274, 275, 276, 277, 278, 279, 286, 287,
				288, 289, 290, 291, 292, 293, 294, 296, 296, 297,
				321, 322, 323, 324, 467, 468, 469, 470, 471, 472,
				473, 474, 476, 477, 478, 479, 480, 481, 482, 483,
				484, 485, 486, 487, 492, 493, 494, 495, 543, 544,
				545, 546, 547, 548, 549, 550, 551, 552, 553, 554,
				555, 556, 557, 558, 559, 560, 561, 562, 563, 564,
				565, 566, 567, 568, 569, 570, 571, 572, 573, 574,
				575, 576, 577, 578, 579, 581, 582, 583, 584, 585,
				586, 587, 588, 589, 590, 591, 592, 593, 594, 595,
				596, 597, 598, 599, 600, 601, 610, 611, 612, 613,

				1010, 1741, 1742, 1743, 1744, 1745, 1746, 1747, 1748, 1749,
				1750, 1751, 1752, 1753, 1754, 1755, 1756, 1757, 1771, 1772,
				1773, 1774, 1775, 1776, 1777, 1778, 1779, 1780, 1781, 1782,
				1783, 1784, 1785, 1786, 1787, 1788, 1789, 1790, 1801, 1802,
				1803, 1804, 1805, 1806, 1807, 1808, 1809, 1811, 1812, 1813,
				1814, 1815, 1816, 1817, 1818, 1819, 1820, 1821, 1822, 1823,
				1824, 1831, 1832, 1833, 1834, 1835, 1836, 1837, 1838, 1839,
				1840, 1841, 1842, 1843, 1844, 1845, 1846, 1847, 1848, 1849,
				1850, 1851, 1852, 1853, 1854, 1861, 1862, 1863, 1864, 1865,
				1866, 1867, 1868, 1869, 1870, 1871, 1872, 1873, 1874, 1875,
				1876, 1877, 1878, 1879, 1880, 1881, 1882, 1883, 1884, 1981,
				1982, 1983, 1984, 1985, 1986, 1987, 1988, 1989, 1990, 1991,
				1992, 1993, 1994, 1995, 1996, 1997, 1998, 1999, 2000, 2001,
				2002, 2003, 2004, 2028, 2029, 2030, 2031, 2032, 2033, 2100,
				2101, 2102, 2103, 2104, 2105,

				0x453B, 0x453C, 0x453D, 0x453E, 0x453F, 0x4540, 0x4541,
				0x4542, 0x4543, 0x4544,	0x4545, 0x4546, 0x4547, 0x4548,
				0x4549, 0x454A, 0x454B, 0x454C, 0x454D, 0x454E,	0x454F
			};

		private static int[] m_SandTiles = new int[]
			{
				22, 23, 24, 25, 26, 27, 28, 29, 30, 31,
				32, 33, 34, 35, 36, 37, 38, 39, 40, 41,
				42, 43, 44, 45, 46, 47, 48, 49, 50, 51,
				52, 53, 54, 55, 56, 57, 58, 59, 60, 61,
				62, 68, 69, 70, 71, 72, 73, 74, 75,

				286, 287, 288, 289, 290, 291, 292, 293, 294, 295,
				296, 297, 298, 299, 300, 301, 402, 424, 425, 426,
				427, 441, 442, 443, 444, 445, 446, 447, 448, 449,
				450, 451, 452, 453, 454, 455, 456, 457, 458, 459,
				460, 461, 462, 463, 464, 465, 642, 643, 644, 645,
				650, 651, 652, 653, 654, 655, 656, 657, 821, 822,
				823, 824, 825, 826, 827, 828, 833, 834, 835, 836,
				845, 846, 847, 848, 849, 850, 851, 852, 857, 858,
				859, 860, 951, 952, 953, 954, 955, 956, 957, 958,
				967, 968, 969, 970,

				1447, 1448, 1449, 1450, 1451, 1452, 1453, 1454, 1455,
				1456, 1457, 1458, 1611, 1612, 1613, 1614, 1615, 1616,
				1617, 1618, 1623, 1624, 1625, 1626, 1635, 1636, 1637,
				1638, 1639, 1640, 1641, 1642, 1647, 1648, 1649, 1650
			};
		#endregion
	}
}
