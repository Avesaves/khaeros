using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class SlayerGroup
	{
		private static SlayerEntry[] m_TotalEntries;
		private static SlayerGroup[] m_Groups;

		public static SlayerEntry[] TotalEntries
		{
			get{ return m_TotalEntries; }
		}

		public static SlayerGroup[] Groups
		{
			get{ return m_Groups; }
		}

		public static SlayerEntry GetEntryByName( SlayerName name )
		{
			int v = (int)name;

			if ( v >= 0 && v < m_TotalEntries.Length )
				return m_TotalEntries[v];

			return null;
		}

		public static SlayerName GetLootSlayerType( Type type )
		{
			for ( int i = 0; i < m_Groups.Length; ++i )
			{
				SlayerGroup group = m_Groups[i];
				Type[] foundOn = group.FoundOn;

				bool inGroup = false;

				for ( int j = 0; foundOn != null && !inGroup && j < foundOn.Length; ++j )
					inGroup = ( foundOn[j] == type );

				if ( inGroup )
				{
					int index = Utility.Random( 1 + group.Entries.Length );

					if ( index == 0 )
						return group.m_Super.Name;

					return group.Entries[index - 1].Name;
				}
			}

			return SlayerName.Silver;
		}

		static SlayerGroup()
		{
			SlayerGroup humanoid = new SlayerGroup();
			SlayerGroup undead = new SlayerGroup();
			SlayerGroup elemental = new SlayerGroup();
			SlayerGroup abyss = new SlayerGroup();
			SlayerGroup arachnid = new SlayerGroup();
			SlayerGroup reptilian = new SlayerGroup();
			SlayerGroup fey = new SlayerGroup();

			humanoid.Opposition = new SlayerGroup[]{ undead };
			humanoid.FoundOn = new Type[]{ typeof( Bird ) };
            humanoid.Super = new SlayerEntry(SlayerName.Repond, typeof(Bird), typeof( FireGiant ), typeof( HillGiant ), typeof( IceGiant ), typeof( StoneGiant ), typeof( StormGiant ));
			humanoid.Entries = new SlayerEntry[]
				{
					new SlayerEntry( SlayerName.OgreTrashing, typeof( Bird ) ),
					new SlayerEntry( SlayerName.OrcSlaying, typeof( Bird ) ),
					new SlayerEntry( SlayerName.TrollSlaughter, typeof( Bird ) )
				};

			undead.Opposition = new SlayerGroup[]{ humanoid };
            undead.Super = new SlayerEntry(SlayerName.Silver, typeof(Bird), typeof(BoneGolem), typeof(FamineSpirit), typeof(FleshGolem), typeof(LesserBoneGolem), typeof(LesserFleshGolem), typeof(SkeletalLord), typeof(SkeletalSoldier), typeof(Skeleton), typeof(Zombie));
			undead.Entries = new SlayerEntry[0];

			fey.Opposition = new SlayerGroup[]{ abyss };
			fey.Super = new SlayerEntry( SlayerName.Fey, typeof( Centaur ), typeof( MaleUnicorn ), typeof( Petal ), typeof( Pixie ), typeof( Satyr ), typeof( Sprite ), typeof( Unicorn ), typeof( Treefellow ), typeof( Treant ), typeof( ElderDryad ), typeof( Dryad ));
			fey.Entries = new SlayerEntry[0];

			elemental.Opposition = new SlayerGroup[]{ abyss };
			elemental.FoundOn = new Type[]{ typeof( Bird ) };
			elemental.Super = new SlayerEntry( SlayerName.ElementalBan, typeof( FireElemental ), typeof( AirElemental ), typeof( EarthElemental ), typeof( EnergyElemental ), typeof( WaterElemental ), typeof( AirElemental ), typeof( CrystalElemental ), typeof( LesserFireElemental ), typeof( LesserCrystalElemental ), typeof( LesserEarthElemental ), typeof( LesserFireElemental ), typeof( LesserEnergyElemental ), typeof( LesserWaterElemental ), typeof( Excremental ));
			elemental.Entries = new SlayerEntry[]
				{
					new SlayerEntry( SlayerName.BloodDrinking, typeof( Bird ) ),
					new SlayerEntry( SlayerName.EarthShatter, typeof( Bird ) ),
					new SlayerEntry( SlayerName.ElementalHealth, typeof( Bird ) ),
					new SlayerEntry( SlayerName.FlameDousing, typeof( Bird ) ),
					new SlayerEntry( SlayerName.SummerWind, typeof( Bird ) ),
					new SlayerEntry( SlayerName.Vacuum, typeof( Bird ) ),
					new SlayerEntry( SlayerName.WaterDissipation, typeof( Bird ) )
				};

			abyss.Opposition = new SlayerGroup[]{ elemental, fey };
			abyss.FoundOn = new Type[]{ typeof( Bird ) };

			if( Core.AOS )
			{
				abyss.Super = new SlayerEntry( SlayerName.Exorcism, typeof( Bird ) );
	
				abyss.Entries = new SlayerEntry[]
					{
						// Daemon Dismissal & Balron Damnation have been removed and moved up to super slayer on OSI.
						new SlayerEntry( SlayerName.GargoylesFoe, typeof( Bird ) ),
					};
			}
			else
			{
				abyss.Super = new SlayerEntry( SlayerName.Exorcism, typeof( Bird ) );

				abyss.Entries = new SlayerEntry[]
					{
						new SlayerEntry( SlayerName.DaemonDismissal, typeof( Bird ) ),
						new SlayerEntry( SlayerName.GargoylesFoe, typeof( Bird ) ),
						new SlayerEntry( SlayerName.BalronDamnation, typeof( Bird ) )
					};
			}

			arachnid.Opposition = new SlayerGroup[]{ reptilian };
			arachnid.FoundOn = new Type[]{ typeof( Bird ) };
            arachnid.Super = new SlayerEntry(SlayerName.ArachnidDoom, typeof(Bird), typeof(DuneDigger), typeof(CrawlingVermin), typeof(DesertCrawler), typeof(DireSpider), typeof(FireBeetle), typeof(GiantCentipede), typeof(HornedBeetle), typeof(BeetleLarva), typeof(PincerBeetle), typeof(RhinoBeetle), typeof(RuneBeetle), typeof(Snowdigger));
			arachnid.Entries = new SlayerEntry[]
				{
					new SlayerEntry( SlayerName.ScorpionsBane, typeof( Bird ) ),
					new SlayerEntry( SlayerName.SpidersDeath, typeof( Bird ) ),
					new SlayerEntry( SlayerName.Terathan, typeof( Bird ) )
				};

			reptilian.Opposition = new SlayerGroup[]{ arachnid };
			reptilian.FoundOn = new Type[]{ typeof( Bird ) };
			reptilian.Super = new SlayerEntry( SlayerName.ReptilianDeath, typeof( Bird ) );
			reptilian.Entries = new SlayerEntry[]
				{
					new SlayerEntry( SlayerName.DragonSlaying, typeof( BronzeDragon ),typeof( CopperDragon ),typeof( Dragon ),typeof( Wyvern ),typeof( GoldDragon ),typeof( IronDragon ),typeof( SilverDragon ),typeof( SkeletalDragon ), typeof( SteelDragon ) ),
					new SlayerEntry( SlayerName.LizardmanSlaughter, typeof( Bird ) ),
					new SlayerEntry( SlayerName.Ophidian, typeof( Bird ) ),
					new SlayerEntry( SlayerName.SnakesBane, typeof( Bird ) )
				};

			m_Groups = new SlayerGroup[]
				{
					humanoid,
					undead,
					elemental,
					abyss,
					arachnid,
					reptilian,
					fey
				};

			m_TotalEntries = CompileEntries( m_Groups );
		}

		private static SlayerEntry[] CompileEntries( SlayerGroup[] groups )
		{
			SlayerEntry[] entries = new SlayerEntry[28];

			for ( int i = 0; i < groups.Length; ++i )
			{
				SlayerGroup g = groups[i];

				g.Super.Group = g;

				entries[(int)g.Super.Name] = g.Super;

				for ( int j = 0; j < g.Entries.Length; ++j )
				{
					g.Entries[j].Group = g;
					entries[(int)g.Entries[j].Name] = g.Entries[j];
				}
			}

			return entries;
		}

		private SlayerGroup[] m_Opposition;
		private SlayerEntry m_Super;
		private SlayerEntry[] m_Entries;
		private Type[] m_FoundOn;

		public SlayerGroup[] Opposition{ get{ return m_Opposition; } set{ m_Opposition = value; } }
		public SlayerEntry Super{ get{ return m_Super; } set{ m_Super = value; } }
		public SlayerEntry[] Entries{ get{ return m_Entries; } set{ m_Entries = value; } }
		public Type[] FoundOn{ get{ return m_FoundOn; } set{ m_FoundOn = value; } }

		public bool OppositionSuperSlays( Mobile m )
		{
			for( int i = 0; i < Opposition.Length; i++ )
			{
				if ( Opposition[i].Super.Slays( m ) )
					return true;
			}

			return false;
		}

		public SlayerGroup()
		{
		}
	}
}
