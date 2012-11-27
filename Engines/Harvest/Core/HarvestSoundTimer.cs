using System;
using Server.Mobiles;
using Server.Misc;
using Server.Engines;

namespace Server.Engines.Harvest
{
	public class HarvestSoundTimer : Timer
	{
		private Mobile m_From;
		private Item m_Tool;
		private HarvestSystem m_System;
		private HarvestDefinition m_Definition;
		private object m_ToHarvest, m_Locked;
		private bool m_Last;
		private int m_Time;
		private Point3D m_Loc;

		public HarvestSoundTimer( Mobile from, Item tool, HarvestSystem system, HarvestDefinition def, object toHarvest, object locked, bool last, int time, Point3D loc ) : base( def.EffectSoundDelay )
		{
			m_From = from;
			m_Tool = tool;
			m_System = system;
			m_Definition = def;
			m_ToHarvest = toHarvest;
			m_Locked = locked;
			m_Last = last;
			m_Time = time;
			m_Loc = loc;
		}

		protected override void OnTick()
		{
			m_System.DoHarvestingSound( m_From, m_Tool, m_Definition, m_ToHarvest );
			PlayerMobile m = m_From as PlayerMobile;
			
			if( ( m_System is Mining && m_From is PlayerMobile && m_Time < ( 4 - m.Feats.GetFeatLevel(FeatList.AdvancedMining) ) ) || ( m_System is Lumberjacking && m_From is PlayerMobile && m_Time < ( 5 - m.Feats.GetFeatLevel(FeatList.AdvancedLumberjacking) ) ) )
			{
				new HarvestSoundTimer( m_From, m_Tool, m_System, m_Definition, m_ToHarvest, m_Locked, m_Last, m_Time + 1, m_Loc ).Start();
				m_System.DoHarvestingEffect( m_From, m_Tool, m_Definition, m.Map, m_Loc );
				return;
			}

			if ( m_Last )
				m_System.FinishHarvesting( m_From, m_Tool, m_Definition, m_ToHarvest, m_Locked );
		}
	}
}
