using System;
using System.Collections;
using Server;

namespace Server.Items
{
	public class ManaRegenerationEffect : CustomPotionEffect
	{
		private const double Divisor = 0.30; // Modifies duration only: at 100 intensity, effect lasts 30 seconds
		private const int PointsPerSecond = 1; // Will restore/take this many points each second
		private static Hashtable m_RegenTable = new Hashtable();

		private static void ReleaseManaRegenLock( object state )
		{
			((Mobile)state).EndAction( typeof( ManaRegenerationEffect ) );
		}

		public override string Name{ get{ return "Mana Regeneration"; } }

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( source != to && intensity < 0 )
				source.DoHarmful( to );

			if ( intensity > 0 )
			{
				if ( to.CanBeginAction( typeof( ManaRegenerationEffect ) ) )
				{
					to.BeginAction( typeof( ManaRegenerationEffect ) );
					Timer.DelayCall( TimeSpan.FromSeconds( 20 ), new TimerStateCallback( ReleaseManaRegenLock ), to ); // 20 sec delay
				}

				else
					return;
			}

			BeginRegenerating( to, intensity, source );
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
		}

		public override bool CanDrink( Mobile mobile )
		{
			return mobile.CanBeginAction( typeof( ManaRegenerationEffect ) );
		}

		public static bool IsRegenerating( Mobile m )
		{
			return m_RegenTable.Contains( m );
		}

		public static void BeginRegenerating( Mobile m, int intensity, Mobile source )
		{
			RegenTimer t = (RegenTimer)m_RegenTable[m];

			if ( t != null )
			{
				t.Update( intensity );
				return;
			}

			t = new RegenTimer( m, intensity );

			m_RegenTable[m] = t;

			t.Start();
		}

		public static void StopRegenerating( Mobile m )
		{
			Timer t = (Timer)m_RegenTable[m];

			if ( t != null )
				t.Stop();

			m_RegenTable.Remove( m );
		}

		private class RegenTimer : Timer
		{
			private Mobile m_Mobile;
			private int m_Ticks;
			private int m_Duration;
			private bool m_Harmful;

			public RegenTimer( Mobile m, int intensity ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;

				if ( intensity > 0 )
				{
					intensity = BasePotion.Scale( m, intensity );
					m_Harmful = false;
				}

				else
				{
					intensity = BasePotion.Scale( m, intensity * -1 );
					m_Harmful = true;
				}

				m_Duration = (int)(intensity * Divisor);
				if ( m_Duration <= 0 )
					m_Duration = 1;

				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				++m_Ticks;
				if ( !m_Mobile.Alive )
				{
					StopRegenerating( m_Mobile);
					return;
				}
				
				if ( m_Harmful )
					m_Mobile.Mana -= PointsPerSecond;
				else
					m_Mobile.Mana += PointsPerSecond;

				if ( m_Ticks >= m_Duration )
					StopRegenerating( m_Mobile );
			}

			public void Update( int intensity )
			{
				bool harmful;

				if ( intensity > 0 )
				{
					intensity = BasePotion.Scale( m_Mobile, intensity );
					harmful = false;
				}

				else
				{
					intensity = BasePotion.Scale( m_Mobile, intensity * -1 );
					harmful = true;
				}

				int duration = (int)(intensity * Divisor);
				if ( duration <= 0 )
					duration = 1;

				if ( harmful == m_Harmful ) // same effect already applied, keep the stronger one
					m_Duration = Math.Max( m_Duration - m_Ticks, duration );
				else // one is harmful, the other isn't
				{
					int sum = ( m_Duration - m_Ticks ) - duration;
					if ( sum >= 0 )
						m_Duration -= duration;
					else
					{
						m_Duration = Math.Abs( sum );
						m_Harmful = !m_Harmful;
					}
				}
			}
		}
	}
}
