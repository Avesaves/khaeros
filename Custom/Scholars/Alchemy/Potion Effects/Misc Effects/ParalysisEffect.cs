using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class ParalysisEffect : CustomPotionEffect
	{
		public override string Name{ get{ return "Paralysis"; } }
		private const int Divisor = 40; // 4000 milliseconds (4 seconds) at 100 intensity

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( intensity < 0 || to.Paralyzed )
				return;

			if ( source != to )
				source.DoHarmful( to );

			to.SendMessage( "You are rendered helpless by the substance." );
			if( to is PlayerMobile )
			{
				( (PlayerMobile)to ).m_PetrifiedTimer = new GeneralizedParalyzeTimer( to, TimeSpan.FromMilliseconds( BasePotion.Scale( to, (int)( intensity * Divisor ) ) ) );
				( (PlayerMobile)to ).m_PetrifiedTimer.Start();
			}

			else if( to is BaseCreature )
			{
				( (BaseCreature)to ).m_PetrifiedTimer = new GeneralizedParalyzeTimer( to, TimeSpan.FromMilliseconds( BasePotion.Scale( to, (int)( intensity * Divisor ) ) ) );
				( (BaseCreature)to ).m_PetrifiedTimer.Start();
			}
		}

		public override bool CanDrink( Mobile mobile )
		{
			return true;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
			Server.Effects.PlaySound( loc, map, 0x3B9 );
		}
	}
	
	public class GeneralizedParalyzeTimer : Timer
    {
		private Mobile m_m;

		public GeneralizedParalyzeTimer( Mobile m, TimeSpan delay ) : base( delay )
		{
			m_m = m;
		}

		protected override void OnTick()
		{
			m_m.SendMessage( "You are able to move again." );
			
			if( m_m is PlayerMobile )
				( (PlayerMobile)m_m ).m_PetrifiedTimer = null;
			
			else if( m_m is BaseCreature )
				( (BaseCreature)m_m ).m_PetrifiedTimer = null;
		}
    }
}
