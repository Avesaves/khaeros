using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class MediumQualityFireworks : Item, IEasyCraft
	{
		private FireworksTimer m_Timer;
		
		[Constructable]
		public MediumQualityFireworks() : base( 3715 )
		{
			Name = "medium quality fireworks";
			Hue = 38;
			Weight = 15.0;
		}

		public MediumQualityFireworks( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick ( Mobile from )
		{
			if ( from.InRange( this.Location, 1 ) )
			{
				if ( !from.Mounted )
				{
					Server.Spells.SpellHelper.Turn( from, this );
					from.Animate( 32, 5, 1, true, false, 0 );
					from.PlaySound( 79 );

					if ( Utility.RandomDouble() > 0.95 )
					{
						if ( Utility.RandomBool() )
							from.SendMessage( "Mechanic: Somebody set up us the bomb." );
						else
							from.SendMessage( "Oh no, something went wrong!" );
						
						Server.Effects.PlaySound( this.Location, this.Map, 283 + Utility.Random( 4 ) );
						new FireEffect().OnExplode( from, this, 80, this.Location, this.Map );
						
						Delete();
					}

					else
					{
						m_Timer = new FireworksTimer( this );
						m_Timer.Start();
						from.SendMessage( "You ignite the fireworks." );
						Movable = false;
					}
				}
				else
					from.SendMessage( "You can't do that while mounted." );
			}
			else
				from.SendMessage( "You are too far away." );
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
		
		public override void OnDelete()
		{
			if ( m_Timer != null )
				m_Timer.Stop();
		}
		
		private class FireworksTimer : Timer
		{
			private MediumQualityFireworks m_Owner;
			private int m_Ticks;
			private bool m_IgniteCountdown;

			public FireworksTimer( MediumQualityFireworks owner ) : base( TimeSpan.FromSeconds( 3.0 ), TimeSpan.FromSeconds( 1.0 ) )
			{
				m_IgniteCountdown = true;
				m_Ticks = 0;
				m_Owner = owner;
				Priority = TimerPriority.TwoFiftyMS;
			}
			
			private void Fireworks()
			{
				Point3D ourLoc = m_Owner.GetWorldLocation();
				Map map = m_Owner.Map;
				Point3D startLoc = new Point3D( ourLoc.X, ourLoc.Y, ourLoc.Z + 10 );
				Point3D endLoc = new Point3D( startLoc.X + Utility.RandomMinMax( -2, 2 ), startLoc.Y + Utility.RandomMinMax( -2, 2 ), startLoc.Z + 32 );

				Effects.SendMovingEffect( new Entity( Serial.Zero, startLoc, map ), new Entity( Serial.Zero, endLoc, map ),
					0x36E4, 5, 0, false, false );

				Timer.DelayCall( TimeSpan.FromSeconds( 1.0 ), new TimerStateCallback( FinishLaunch ), new object[]{ endLoc, map } );
			}

			private void FinishLaunch( object state )
			{
				object[] states = (object[])state;

				Point3D endLoc = (Point3D)states[0];
				Map map = (Map)states[1];

				int hue = Utility.Random( 40 );

				if ( hue < 8 )
					hue = 0x66D;
				else if ( hue < 10 )
					hue = 0x482;
				else if ( hue < 12 )
					hue = 0x47E;
				else if ( hue < 16 )
					hue = 0x480;
				else if ( hue < 20 )
					hue = 0x47F;
				else
					hue = 0;

				if ( Utility.RandomBool() )
					hue = Utility.RandomList( 0x47E, 0x47F, 0x480, 0x482, 0x66D );

				int renderMode = Utility.RandomList( 0, 2, 3, 4, 5, 7 );

				Effects.PlaySound( endLoc, map, Utility.Random( 0x11B, 4 ) );
				Effects.SendLocationEffect( endLoc, map, 0x373A + (0x10 * Utility.Random( 4 )), 16, 10, hue, renderMode );
			}

			protected override void OnTick()
			{
				m_Ticks++;
				
				if ( m_IgniteCountdown)
				{
					if ( m_Ticks > 3 )
					{
						m_Ticks = 0;
						m_IgniteCountdown = false;
						Delay = TimeSpan.FromSeconds( 60.0 );
						Interval = TimeSpan.FromSeconds( 2.0 );
					}
					else
						m_Owner.PublicOverheadMessage( MessageType.Regular, 0x22, false, "" + (4-m_Ticks) );
				}
				else
				{
					if ( m_Ticks > 30 )
					{
						m_Owner.Delete();
						Stop();
					}
					else
						Fireworks();
				}
			}
		}
	}
}
