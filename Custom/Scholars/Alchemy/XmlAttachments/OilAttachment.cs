using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.Alchemy;

namespace Server.Engines.XmlSpawner2 
{
	public class OilAttachment : XmlAttachment 
	{
		private Dictionary<CustomEffect, int> m_Effects;
		private int m_Duration;
		private WearOffTimer m_Timer;
		private int m_Corrosivity;
		private int m_MaxDuration;

		public int Duration { get { return m_Duration; } set { m_Duration = value; } }

		public OilAttachment ( ASerial serial ) : base( serial ) 
		{ 
		}

		public OilAttachment( Dictionary<CustomEffect, int> effects, int duration, int corrosivity ) 
		{
			m_Effects = effects;
			m_Duration = duration;
			m_MaxDuration = duration;
			m_Corrosivity = corrosivity;
		}

		public override void Serialize ( GenericWriter writer ) 
		{
			base.Serialize( writer );
			writer.Write( (int) 1 ); // version
				
			writer.Write( (int) m_MaxDuration );

			writer.Write( (int) m_Corrosivity );

			writer.Write( (int) m_Duration );

			if ( m_Effects != null )
			{
				writer.Write( (int) m_Effects.Count );

				foreach ( KeyValuePair<CustomEffect, int> kvp in m_Effects ) 
				{
					writer.Write( (int)kvp.Key );
					writer.Write( (int)kvp.Value );
				}
			}
			else
				writer.Write( (int)0 );
		}

		public override void Deserialize ( GenericReader reader ) 
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			
			switch ( version )
			{
				case 0:
				{
					m_Corrosivity = reader.ReadInt();
					m_Duration = reader.ReadInt();
					int c = reader.ReadInt();
					if ( c > 0 )
					{
						m_Effects = new Dictionary<CustomEffect, int>();
						for ( int i = 0; i < c; i++ )
							m_Effects.Add( (CustomEffect) reader.ReadInt(), reader.ReadInt() );
					}

					m_Timer = new WearOffTimer( this );
					m_Timer.Start();
					break;
				}
				
				case 1:
				{
					m_MaxDuration = reader.ReadInt();
					goto case 0;
				}
			}
		}

		public override void OnAttach() 
		{
			if ( !(AttachedTo is BaseMeleeWeapon) )
				this.Delete();
			base.OnAttach();

			(AttachedTo as BaseWeapon).InvalidateProperties();
			
			if ( m_Timer == null )
			{
				m_Timer = new WearOffTimer( this );
				m_Timer.Start();
			}
		}

		public void WearOff()
		{
			Item item = AttachedTo as Item;
			Delete();
			if ( item != null )
				item.InvalidateProperties();
			
		}
		
		public void LowerDuration()
		{
			if ( this is ToxinAttachment )
				m_Duration -= (int)( Toxin.MaxToxinDuration * 0.03 );
			else
				m_Duration -= (int)( m_MaxDuration * 0.03 ); // loss of 3% duration
		}

		public virtual void OnWeaponHit( Mobile defender, Mobile attacker ) // the oiled weapon wielded by attacker struck defender -> apply effects
		{
			if ( m_Duration <= 0 )
			{
				m_Timer.Stop();
				WearOff();
			}
			LowerDuration();
			foreach ( KeyValuePair<CustomEffect, int> kvp in m_Effects ) 
			{
				CustomPotionEffect effect = CustomPotionEffect.GetEffect( kvp.Key );
				if ( effect != null )
					effect.ApplyEffect( defender, attacker, kvp.Value, (Item)AttachedTo );
			}
		}

		public void OnTimerTick() // called every 5 seconds, as per WearOffTimer
		{
			BaseMeleeWeapon weapon = AttachedTo as BaseMeleeWeapon;
			if ( weapon != null )
			{
				if ( m_Corrosivity > 0 )
				{
					if ( 0.05 > Utility.RandomDouble() )
					{
						weapon.HitPoints -= m_Corrosivity;

						if ( weapon.HitPoints < 0 )
						{
							weapon.MaxHitPoints += weapon.HitPoints;
							weapon.HitPoints = 0;

							if ( weapon.MaxHitPoints <= 0 )
							{
								weapon.Delete();
								Delete();
							}
						}
					}
				}
				if ( !weapon.Deleted )
					weapon.InvalidateProperties(); // update time
			}
		}

		public override void OnDelete()
		{
			if ( m_Timer != null )
				m_Timer.Stop();
		}

		private class WearOffTimer : Timer
		{
			private OilAttachment m_Owner;

			public WearOffTimer( OilAttachment owner ) : base( TimeSpan.FromSeconds( 5.0 ), TimeSpan.FromSeconds( 5.0 ) )
			{
				m_Owner = owner;
				Priority = TimerPriority.FiveSeconds;
			}

			protected override void OnTick()
			{
				int ticks = m_Owner.Duration - 5;
				if ( ticks <= 0 )
					m_Owner.WearOff();
				else
				{
					m_Owner.Duration = ticks;
					m_Owner.OnTimerTick();
					this.Delay = this.Interval = TimeSpan.FromSeconds( 5.0 );
				}
			}
		}
	}
}
