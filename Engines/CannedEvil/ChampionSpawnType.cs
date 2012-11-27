using System;
using Server;
using Server.Mobiles;

namespace Server.Engines.CannedEvil
{
	public enum ChampionSpawnType
	{
		Abyss,
		Arachnid,
		ColdBlood,
		ForestLord,
		VerminHorde,
		UnholyTerror,
		SleepingDragon
	}

	public class ChampionSpawnInfo
	{
		private string m_Name;
		private Type m_Champion;
		private Type[][] m_SpawnTypes;
		private string[] m_LevelNames;

		public string Name { get { return m_Name; } }
		public Type Champion { get { return m_Champion; } }
		public Type[][] SpawnTypes { get { return m_SpawnTypes; } }
		public string[] LevelNames { get { return m_LevelNames; } }

		public ChampionSpawnInfo( string name, Type champion, string[] levelNames, Type[][] spawnTypes )
		{
			m_Name = name;
			m_Champion = champion;
			m_LevelNames = levelNames;
			m_SpawnTypes = spawnTypes;
		}

		public static ChampionSpawnInfo[] Table{ get { return m_Table; } }

		private static readonly ChampionSpawnInfo[] m_Table = new ChampionSpawnInfo[]
			{
				new ChampionSpawnInfo( "Abyss", typeof( Bird ), new string[]{ "Foe", "Assassin", "Conqueror" }, new Type[][]	// Abyss
				{																											// Abyss
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) }
				} ),
				new ChampionSpawnInfo( "Arachnid", typeof( Bird ), new string[]{ "Bane", "Killer", "Vanquisher" }, new Type[][]	// Arachnid
				{																											// Arachnid
			        new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) }
				} ),
				new ChampionSpawnInfo( "Cold Blood", typeof( Bird ), new string[]{ "Blight", "Slayer", "Destroyer" }, new Type[][]	// Cold Blood
				{																											// Cold Blood
			    	new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) }
			                      	                      			
				} ),
				new ChampionSpawnInfo( "Forest Lord", typeof( Bird ), new string[]{ "Enemy", "Curse", "Slaughterer" }, new Type[][]	// Forest Lord
				{																											// Forest Lord
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) }						
				} ),
				new ChampionSpawnInfo( "Vermin Horde", typeof( Bird ), new string[]{ "Adversary", "Subjugator", "Eradicator" }, new Type[][]	// Vermin Horde
				{																											// Vermin Horde
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) }							
				} ),
				new ChampionSpawnInfo( "Unholy Terror", typeof( Bird ), new string[]{ "Scourge", "Punisher", "Nemesis" }, new Type[][]	// Unholy Terror
				{																											// Unholy Terror
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) }								
				} ),
				new ChampionSpawnInfo( "Sleeping Dragon", typeof( Bird ), new string[]{ "Rival", "Challenger", "Antagonist" } , new Type[][]
				{																											// Unholy Terror
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) },
					new Type[]{ typeof( Bird ) }
				} )
			};

		public static ChampionSpawnInfo GetInfo( ChampionSpawnType type )
		{
			int v = (int)type;

			if( v < 0 || v >= m_Table.Length )
				v = 0;

			return m_Table[v];
		}
	}
}
