using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Factions;

namespace Server
{
	public class SpeedInfo
	{
		// Should we use the new method of speeds?
		private static bool Enabled = true;

		private double m_ActiveSpeed;
		private double m_PassiveSpeed;
		private Type[] m_Types;

		public double ActiveSpeed
		{
			get{ return m_ActiveSpeed; }
			set{ m_ActiveSpeed = value; }
		}

		public double PassiveSpeed
		{
			get{ return m_PassiveSpeed; }
			set{ m_PassiveSpeed = value; }
		}

		public Type[] Types
		{
			get{ return m_Types; }
			set{ m_Types = value; }
		}

		public SpeedInfo( double activeSpeed, double passiveSpeed, Type[] types )
		{
			m_ActiveSpeed = activeSpeed;
			m_PassiveSpeed = passiveSpeed;
			m_Types = types;
		}

		public static bool Contains( object obj )
		{
			if ( !Enabled )
				return false;

			if ( m_Table == null )
				LoadTable();

			SpeedInfo sp = (SpeedInfo)m_Table[obj.GetType()];

			return ( sp != null );
		}

		public static bool GetSpeeds( object obj, ref double activeSpeed, ref double passiveSpeed )
		{
			if ( !Enabled )
				return false;

			if ( m_Table == null )
				LoadTable();

			SpeedInfo sp = (SpeedInfo)m_Table[obj.GetType()];

			if ( sp == null )
				return false;

			activeSpeed = sp.ActiveSpeed;
			passiveSpeed = sp.PassiveSpeed;

			return true;
		}

		private static void LoadTable()
		{
			m_Table = new Hashtable();

			for ( int i = 0; i < m_Speeds.Length; ++i )
			{
				SpeedInfo info = m_Speeds[i];
				Type[] types = info.Types;

				for ( int j = 0; j < types.Length; ++j )
					m_Table[types[j]] = info;
			}
		}

		private static Hashtable m_Table;

		private static SpeedInfo[] m_Speeds = new SpeedInfo[]
		{
			// Slow 
			new SpeedInfo( 0.3, 0.6, new Type[]
			{
				typeof( Snowdigger ),			typeof( EarthElemental ),	typeof( Ettin ),	
				typeof( Ogre ),					typeof( CrawlingVermin ),	
				typeof( OgreBrute ),			typeof( Xorn ),				typeof( Rat ),				
				typeof( Zombie ),			typeof( Walrus ),			
				typeof( CrystalElemental ),		typeof( FleshJelly ),
				typeof( MinotaurAbomination ),	typeof( DuneDigger ),		typeof( GrassSnake ),		
				typeof( GiantWolfSpider ),		typeof( AssassinSpider ),	typeof( DesertScorpion ),	
				typeof( FunnelWebSpider ),		typeof( LesserEarthElemental ),	typeof( LesserCrystalElemental ),
				typeof( HillGiant ),			typeof( StoneGiant ),		typeof( FireGiant ),	
				typeof( IceGiant ),				typeof( StormGiant ),	
				typeof( GelatinousBlob ),		typeof( BlackPudding ),		typeof( JellyOoze ),		
				typeof( GelatinousBlobSpawn ),	typeof( BlackPuddingSpawn ), typeof( JellyOozeSpawn ),
				typeof( Cyclops )
			} ),
			// Fast 
			new SpeedInfo( 0.2, 0.4, new Type[]
			{
				typeof( Sprite ),			typeof( AirElemental ),		typeof( Hag ),
				typeof( LesserAirElemental ), typeof( EnergyElemental ),	typeof( LesserEnergyElemental ),
				typeof( GoblinScavenger ),	typeof( GoblinArcher ),		typeof( HobgoblinRider ),
				typeof( HobgoblinWarrior ),	typeof( Troglin ),			typeof( TroglinWarrior ), 
				typeof( KoboldWorker ),		typeof( KoboldCleric ),		typeof( KoboldLord ),	
				typeof( KoboldWarrior ),	typeof( DireSpider ),		typeof( FamineSpirit ),
				typeof( SkeletalLord ),		typeof( SkeletalSoldier ),	typeof( Skeleton ),		
				typeof( Gorgon ),			typeof( Spectre ),			typeof( Ghost ),
				typeof( Banshee ),			typeof( Wraith ),			typeof( Dragon ), typeof( WorkHorse ),
				typeof( SpiritTotem ),		typeof( LesserSpiritTotem ),	typeof( GreaterSpiritTotem ),
				typeof( ServantOfOhlm ),	typeof( LesserServantOfOhlm ),	typeof( GreaterServantOfOhlm ),
				typeof( DivineProtector ),	typeof( LesserDivineProtector ), 	typeof( GreaterDivineProtector ),
				typeof( VolcanicGuardian ),	typeof( LesserVolcanicGuardian ), 	typeof( GreaterVolcanicGuardian ),
				typeof( Huorn ),			typeof( LesserHuorn ),			typeof( GreaterHuorn ),
				typeof( ClericScorpion ),			typeof( LesserClericScorpion ),			typeof( GreaterClericScorpion ),
                typeof( DesertScorpion )
				
			} ),
			// Very Fast 
			new SpeedInfo( 0.175, 0.350, new Type[]
			{
				typeof( Pixie ),			
				typeof( Jaguar ),			typeof( GallowayHorse ),		typeof( KudaHorse ),	
				typeof( BarbHorse ),	typeof( SteppeHorse ),		typeof( RuganHorse ),	
				typeof( RoseanHorse ),	typeof( WarHorse ),			typeof( DireWolf ),		
				typeof( Ridgeraptor ),		typeof( DireBear ),			typeof( ForestStrider ),
				typeof( GiantScarab ),		typeof( Petal ),			typeof( Unicorn ),	
				typeof( MaleUnicorn ),      typeof( Quaraphon ),		typeof( Wyvern ),
				typeof( HookHorror )
				
			} ),
			// Medium 
			new SpeedInfo( 0.25, 0.5, new Type[]
			{
				typeof( Alligator ),		typeof( Bird ),				typeof( CorvinusBear ),		
				typeof( FormianMyrmarch ), typeof( FormianWarrior ),	typeof( FormianWorker ),
				typeof( Boar ),				typeof( BradocsBear ),		typeof( Bull ),	
				typeof( BullFrog ),			typeof( Cat ),				typeof( Centaur ),		
				typeof( Chicken ),			typeof( Cougar ),			typeof( Cow ),
				typeof( Dog ),				typeof( Shark ),			typeof( Dolphin ),	
				typeof( Eagle ),			typeof( Beholder ),	
				typeof( Hydra ),			typeof( FireElemental ),	typeof( AmbusherSpider ),
				typeof( Gazer ),			typeof( GiantKingsnake ),	typeof( GiantRat ),	
				typeof( BoaConstrictor ),	typeof( GiantToad ),		typeof( Goat ),
				typeof( Gorilla ),			typeof( Elk ),				typeof( SouthernBear ),	
				typeof( Deer ),				typeof( Horse ),	
				typeof( LesserHomunculus ),	typeof( Rabbit ),		typeof( Kraken ),	
				typeof( Crocodile ),		typeof( KingCobra ),		typeof( BlackMamba ),
				typeof( MountainGoat ),		typeof( Wortling ),			
				typeof( Panther ),			typeof( Pig ),				typeof( WasteBear ),	
				typeof( SeaSerpent ),		typeof( WanderingSpirit ),	typeof( Sheep ),
				typeof( SkeletalDragon ),	
				typeof( Lynx ),				typeof( ConstrictingVine ),	
				typeof( DriderQueen ),		typeof( DriderWorker ),		typeof( DriderCleric ), 
				typeof( DriderWarrior ),	typeof( Timberwolf ),
				typeof( WaterElemental ),	typeof( WhippingVine ),		
				typeof( Octopod ),	
				typeof( BoneGolem ),		typeof( Devourer ),			typeof( FleshGolem ),
				typeof( Homunculus ),		typeof( LesserFleshGolem ),	
				typeof( LesserBoneGolem ),	typeof( Ettercap ),			typeof( VampireBat ),	
				typeof( DisplacerBeast ),	typeof( Sabretooth ),	
				typeof( ForestTroll ),		typeof( RuneBeetle ),		typeof( Drox ),	
				typeof( Flamingo ),			typeof( RedWolf ), 			typeof( ManedWolf ),
				typeof( Jackal ),					typeof( Snowbird ),	
				typeof( Cobra ),			typeof( Bushmaster ),		typeof( Gambol ),
				typeof( Viper ),			typeof( Bat ),				typeof( OakDryad ),
				typeof( YewDryad ),			typeof( RedwoodDryad ),		typeof( AshDryad ),	
				typeof( GreenheartDryad ),	typeof( ElderOakDryad ),	typeof( ElderYewDryad ),
				typeof( ElderRedwoodDryad ), typeof( ElderAshDryad ),	typeof( ElderGreenheartDryad ),	
				typeof( Satyr ),			typeof( OakTreefellow ), 	typeof( YewTreefellow ),
				typeof( RedwoodTreefellow ), typeof( AshTreefellow ),	typeof( GreenheartTreefellow ),
				typeof( OakTreant ),		typeof( YewTreant ),		typeof( RedwoodTreant ),
				typeof( AshTreant ),		typeof( GreenheartTreant ),	typeof( LesserFireElemental ), 	typeof( LesserWaterElemental ),
				typeof( CopperDragon ),		typeof( BronzeDragon ),		typeof( IronDragon ),
				typeof( SilverDragon ),		typeof( GoldDragon ),		typeof( SteelDragon ),	
				typeof( YuanTi ),			typeof( YuanTiWarrior ),	typeof( YuanTiMage ),	
				typeof( YuanTiPureblood ),	typeof( YuanTiAbomination ),	typeof( ArtifactBoneGolem ),
				typeof( SpiritSoldier ),	typeof( Spectre ),			typeof( Minotaur ),		
				typeof( MinotaurCleric ),	typeof( MinotaurWarrior ),	typeof( MinotaurBrute ),
				typeof( HornedBeetle ),		typeof( MountainTroll ),	typeof( CaveTroll ),
				typeof( Beastman ),			typeof( BeastmanLord ),		typeof( GoatmanWarrior ),
				typeof( GoatmanArcher ),	typeof( PlanarHorror ),		typeof( Gnoll ),
				typeof( Umberhulk ),		typeof( RustMonster ),		typeof( Chimera )
			} )
		};
	}
}
