using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;

namespace Server.Engines.Harvest
{
	public class ClayHarvesting : HarvestSystem
	{
		private static ClayHarvesting m_System;

		public static ClayHarvesting System
		{
			get
			{
				if ( m_System == null )
					m_System = new ClayHarvesting();

				return m_System;
			}
		}

		private HarvestDefinition m_Definition;

		public HarvestDefinition Definition
		{
			get{ return m_Definition; }
		}

        private ClayHarvesting()
		{
			HarvestResource[] res;
			HarvestVein[] veins;

            #region ClayHarvesting
            HarvestDefinition clay = new HarvestDefinition();

			// Resource banks are every 4x3 tiles
			clay.BankWidth = 4;
			clay.BankHeight = 3;

			// Every bank holds from 20 to 45 piles of clay
			clay.MinTotal = 3;
			clay.MaxTotal = 9;

			// A resource bank will respawn its content every 20 to 30 minutes
			clay.MinRespawn = TimeSpan.FromMinutes( 20.0 );
			clay.MaxRespawn = TimeSpan.FromMinutes( 30.0 );

			// Skill checking is done on the Lumberjacking skill
			clay.Skill = SkillName.Mining;

			// Set the list of harvestable tiles
			clay.Tiles = m_ClayTiles;

			// Players must be within 2 tiles to harvest
			clay.MaxRange = 2;

			// Ten logs per harvest action
			clay.ConsumedPerHarvest = 3;
			clay.ConsumedPerFeluccaHarvest = 3;

			// The chopping effect
            clay.EffectActions = new int[] { 11 };
            clay.EffectSounds = new int[] { 0x125, 0x126 };
            clay.EffectCounts = new int[] { 1 };
            clay.EffectDelay = TimeSpan.FromSeconds( 1.6 );
            clay.EffectSoundDelay = TimeSpan.FromSeconds( 0.9 );

			clay.NoResourcesMessage = "There's not enough clay here to harvest."; // There's not enough wood here to harvest.
			clay.FailMessage = "You dig the ground for a while, but fail to find any useable clay."; // You hack at the tree for a while, but fail to produce any useable wood.
			clay.OutOfRangeMessage = 500446; // That is too far away.
			clay.PackFullMessage = "You can't place any more clay into your backpack."; // You can't place any wood into your backpack!
			clay.ToolBrokeMessage = "You broke your shovel."; // You broke your axe.

			res = new HarvestResource[]
				{
					new HarvestResource( 00.0, 00.0, 100.0, "You place some clay in your backpack.", typeof( Clay ) )
				};

			veins = new HarvestVein[]
				{
					new HarvestVein( 100.0, 0.0, res[0], null )
				};

			clay.Resources = res;
			clay.Veins = veins;

			m_Definition = clay;
			Definitions.Add( clay );
			#endregion
		}

		public override bool CheckHarvest( Mobile from, Item tool )
		{
			if ( !base.CheckHarvest( from, tool ) )
				return false;

			if ( tool.Parent != from.Backpack )
			{
				from.SendMessage( "You need a shovel in order to harvest clay." ); 
				return false;
			}

            if( from is PlayerMobile )
            {
                if( ( (PlayerMobile)from ).Feats.GetFeatLevel(FeatList.Potter) <= 0 )
                {
                    from.SendMessage( 60, "You need the Potter feat to do that." );
                    return false;
                }
            }

            if( from.Mounted )
            {
                from.SendMessage( "You can't dig while riding." ); // You can't mine while riding.
                return false;
            }

			return true;
		}

		public override bool CheckHarvest( Mobile from, Item tool, HarvestDefinition def, object toHarvest )
		{
			if ( !base.CheckHarvest( from, tool, def, toHarvest ) )
				return false;

			return true;
		}

		public override void OnBadHarvestTarget( Mobile from, Item tool, object toHarvest )
		{
			from.SendMessage( "You can't use a shovel on that." );
		}

		public static void Initialize()
		{
			Array.Sort( m_ClayTiles );
		}

		#region Tile lists
		private static int[] m_ClayTiles = new int[]
			{
				0x008E, 0x008F, 0x0090, 0x0091, 0x0092, 0x0093, 0x0094, 0x0095, 
                0x0096, 0x0097, 0x0098, 0x0099, 0x009A, 0x009B, 0x009B, 0x009C, 
                0x009D, 0x009E, 0x009F, 0x00A0, 0x00A1, 0x00A2, 0x00A3, 0x00A4,
                0x00A5, 0x00A6, 0x00A7, 0x0150, 0x0151, 0x0152, 0x0153, 0x0154,
                0x0155, 0x0156, 0x0157, 0x0158, 0x0159, 0x015A, 0x015B, 0x015C,
                0x02E6, 0x02E7, 0x02E8, 0x02E9, 0x02EA, 0x02EB, 0x02EC, 0x02ED,
                0x02EE, 0x02EF, 0x02F0, 0x02F1, 0x02F2, 0x02F3, 0x02F4, 0x02F5,
                0x02F6, 0x02F7, 0x02F8, 0x02F9, 0x02FA, 0x02FB, 0x02FC, 0x02FD,
                0x02FE, 0x02FF, 0x0306, 0x0307, 0x0308, 0x0309, 0x030A, 0x030B,
                0x030C, 0x030D, 0x030E, 0x030F, 0x0310, 0x0311, 0x0312, 0x0313,
                0x0314, 0x0315, 0x0316, 0x0317, 0x0318, 0x0319, 0x031A, 0x031B,
                0x031C, 0x031D, 0x031E, 0x031F, 0x3DC1, 0x3DC2, 0x3DC3, 0x3DC4,
                0x3DC5, 0x3DC6, 0x3DC7, 0x3DC8, 0x3DC9, 0x3DCA, 0x3DCB, 0x3DCC,
                0x3DCD, 0x3DCE, 0x3DCF, 0x3DD0, 0x3DD1, 0x3DD2, 0x3DD3, 0x3DD4,
                0x3DD5, 0x3DD6, 0x3DD7, 0x3DD8, 0x3DD9, 0x3DDA, 0x3DDB, 0x3DDC,
                0x3DDD, 0x3DDE, 0x3DDF, 0x3DE0, 0x3DE1, 0x3DE2, 0x3DE3, 0x3DE4,
                0x3DE5, 0x3DE6, 0x3DE7, 0x3DE8, 0x3DE9, 0x3DEA, 0x3DEB, 0x3DEC,
                0x3DED, 0x3DEE, 0x3DEF, 0x3DF0, 0x3DF1
			};
		#endregion
	}
}
