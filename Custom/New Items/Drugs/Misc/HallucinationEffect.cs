using System; 
using System.Collections; 
using Server.Multis; 
using Server.Items; 
using Server.Network; 
using Server.Targeting;
using Server.Mobiles;

namespace Server.Items 
{ 
	public class HallucinationEffect
	{
		private static Hashtable m_Table = new Hashtable();

		public static bool IsHallucinating( PlayerMobile m )
		{
			return m_Table.Contains( m );
		}

		public static void BeginHallucinating( PlayerMobile m, int duration )
		{
			Timer t = (Timer)m_Table[m];

			if ( t != null )
				t.Stop();

			t = new HallucinationTimer( m, duration );

			m_Table[m] = t;

			t.Start();
		}

		public static void EndHallucination( PlayerMobile m )
		{
			Timer t = (Timer)m_Table[m];

			if ( t != null )
				t.Stop();

			m_Table.Remove( m );
			//m.SendMessage( "The hallucinogenic substance's effect wanes." );
		}

		public static void SendHallucinationItem( PlayerMobile hallucinator, IEntity e, int itemID, int speed, int duration, int renderMode, int hue )
		{
			Map map = e.Map;

			if ( map != null && hallucinator != null )
			{
				Packet regular = null;
				NetState state = hallucinator.NetState;
				if ( state != null )
				{
					hallucinator.ProcessDelta();

					/*switch ( Utility.Random( 16 ))
					{
						case 0: hallucinator.FixedParticles( 0x373A, 10, 15, 5018, EffectLayer.Waist ); break;
						case 1: hallucinator.FixedParticles( 0x374A, 10, 15, 5018, EffectLayer.Head ); break;
						case 2: hallucinator.FixedParticles( 0x375A, 10, 15, 5018, EffectLayer.Waist ); break;
						case 3: hallucinator.FixedParticles( 0x376A, 10, 15, 5018, EffectLayer.Head ); break;
						default: break;
					}*/

					regular = new LocationEffect( e, itemID, speed, duration, renderMode, hue );

					state.Send( regular );
				}
			}
		}

		public class HallucinationTimer : Timer 
		{ 
			private PlayerMobile m_Hallucinator;
			private int m_Duration;
			//Timespan between visual changes
			public HallucinationTimer( PlayerMobile from, int duration ) : base( TimeSpan.FromSeconds( 1 ), TimeSpan.FromSeconds( 1 ) ) 
			{ 
				Priority = TimerPriority.OneSecond; 
				m_Hallucinator = from;
				m_Duration = duration;
			} 

			protected override void OnTick() 
			{
				m_Duration -= 1;
				if ( m_Duration <= 0 )
				{
					EndHallucination( m_Hallucinator );
					return;
				}
				
				int hue = Utility.Random( 2, 1200 );

				IPooledEnumerable eable = m_Hallucinator.GetItemsInRange( 10 );
				int i = 0;
				foreach ( Item item in eable )
				{
					if ( i > 5 )
						break;
					if ( item.Visible )
					{
						SendHallucinationItem( m_Hallucinator, item, item.ItemID, 5, 5000 , 4410, hue );
						i++;
					}
				}
				eable.Free();
				IEntity entity = new Entity( Server.Serial.Zero, new Point3D( m_Hallucinator.X + Utility.Random(-4, 4), m_Hallucinator.Y  + Utility.Random(-4, 4), m_Hallucinator.Z  + Utility.Random(0, 10) ), m_Hallucinator.Map );
				SendHallucinationItem( m_Hallucinator, entity, 2444+Utility.Random(15370-2444), 5, 5000, 4410, hue );
				this.Interval = this.Delay = TimeSpan.FromSeconds( 1 );
			} 
		}
	}
}
