using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.Alchemy;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2 
{
	public class PoisonAttachment : XmlAttachment 
	{
		private Mobile m_Poisoner;
		private Dictionary<PoisonEffectEnum, int> m_Effects;
		private PoisonTimer m_Timer;
		private int m_Duration;
		private int m_ActingSpeed;
		private int m_PoisonStrength;
		
		public int PoisonStrength{ get{ return m_PoisonStrength; } }
		public Mobile Poisoner { get { return m_Poisoner; } }
        public Dictionary<PoisonEffectEnum, int> Effects { get { return m_Effects; } }
        public int Duration { get { return m_Duration; } }
        public int ActingSpeed { get { return m_ActingSpeed; } }

		public PoisonAttachment ( ASerial serial ) : base( serial ) 
		{ 
		}

		public PoisonAttachment( Dictionary<PoisonEffectEnum, int> effects, int duration, int actingspeed, Mobile poisoner ) 
		{
			m_Effects = effects;
			m_Duration = duration;
			m_ActingSpeed = actingspeed;
			m_Poisoner = poisoner;
			
			m_PoisonStrength = 0;
			foreach ( KeyValuePair<PoisonEffectEnum, int> kvp in m_Effects ) 
				m_PoisonStrength += kvp.Value;
		}

		public override void Serialize ( GenericWriter writer ) 
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize ( GenericReader reader ) 
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			
			Delete(); // currently active poisons aren't serialized
		}

		public override void OnAttach() 
		{
			Mobile mobile = AttachedTo as Mobile;
			if ( mobile == null )
				this.Delete();
			base.OnAttach();

			// update delta, i.e. poisoned bar color
			mobile.Delta( MobileDelta.Flags );
			
			if ( m_Timer == null )
			{
				m_Timer = new PoisonTimer( this );
				m_Timer.Start();
			}
		}

		public void WearOff()
		{
			Mobile mobile = AttachedTo as Mobile;
            if( mobile != null )
            {
                mobile.SendMessage( 83, "The poison seems to have worn off." );
                mobile.Delta( MobileDelta.Flags );
            }
			Delete();
		}

		public void OnPoisonTick( double tick ) // called on each tick by the timer
		{
			Mobile mobile = AttachedTo as Mobile;
			BaseCreature bc = mobile as BaseCreature;
			if ( mobile != null )
			{
				if ( !mobile.Alive )
					Delete();
				else if ( bc != null && bc.IsDeadPet )
					Delete();
				else
					foreach ( KeyValuePair<PoisonEffectEnum, int> kvp in m_Effects ) 
						PoisonEffect.GetEffect( kvp.Key ).ApplyEffect( mobile, m_Poisoner, (int)(kvp.Value*(tick/((double)m_Duration/(10.0/(double)m_ActingSpeed)))), null );
			}
		}

		public override void OnDelete()
		{
			RemovePoisonEffects();
			if ( m_Timer != null )
				m_Timer.Stop();
		}
		
		public void RemovePoisonEffects()
		{
			Mobile mob = AttachedTo as Mobile;
			if ( mob != null )
			{
				foreach ( KeyValuePair<PoisonEffectEnum, int> kvp in m_Effects ) 
					PoisonEffect.GetEffect( kvp.Key ).CureEffect( mob );
			}
		}

		private class PoisonTimer : Timer
		{
			private PoisonAttachment m_Owner;
			private int m_Ticks;
			private int m_MaxTicks;

			public PoisonTimer( PoisonAttachment owner ) : base( TimeSpan.FromSeconds( 10.0/((double)owner.m_ActingSpeed) ), TimeSpan.FromSeconds( 10.0/((double)owner.m_ActingSpeed) ) )
			{
				m_Ticks = 0;
				m_MaxTicks = (int)(owner.m_Duration/(10.0/((double)owner.m_ActingSpeed)));
				m_Owner = owner;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				int tick = m_Ticks++;
				if ( tick >= m_MaxTicks )
					m_Owner.WearOff();
				else
				{
					m_Owner.OnPoisonTick( tick );
					this.Delay = this.Interval = TimeSpan.FromSeconds( 10.0/((double)m_Owner.m_ActingSpeed) );
				}
			}
		}
	}
}
