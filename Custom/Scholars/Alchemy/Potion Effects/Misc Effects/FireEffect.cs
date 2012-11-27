using System;
using System.Collections.Generic;
using Server;

namespace Server.Items
{
    public enum FireEffectType
    {
        Fire,
        Cold,
        Poison,
        Energy
    }    

	public class FireEffect : CustomPotionEffect
	{
        private FireEffectType m_FireEffectType;
        public FireEffectType FireEffectType{ get{ return m_FireEffectType; } set{ m_FireEffectType = value; } }

        public int GetFireDamage { get { return ( FireEffectType == FireEffectType.Fire ? 100 : 0 ); } }
        public int GetColdDamage { get { return ( FireEffectType == FireEffectType.Cold ? 100 : 0 ); } }
        public int GetPoisonDamage { get { return ( FireEffectType == FireEffectType.Poison ? 100 : 0 ); } }
        public int GetEnergyDamage { get { return ( FireEffectType == FireEffectType.Energy ? 100 : 0 ); } }

		public override string Name{ get{ return "Fire"; } }
		private const double Divisor = 0.15; // 15 second fire at 100 intensity, fire always does 20 points of damage per second

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			// only oils call this, bombs handle it via OnExplode
			if ( intensity < 0 )
				return;

            AOS.Damage( to, source, (int)( ( intensity * Divisor ) * 0.5 ), 0, GetFireDamage, GetColdDamage, GetPoisonDamage, GetEnergyDamage, 0, 0, 0 ); // 100% fire
		}

		public override bool CanDrink( Mobile mobile )
		{
			return true;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
			Server.Effects.PlaySound( loc, map, 0x22F );
			
			int radius = 2; // for anything that's calling this, and is not a bomb
			if ( itemSource is BombPotion )
			{
				BombPotion bomb = itemSource as BombPotion;
				radius = bomb.ExplosionRange;
			}
			
			int delay = (int)(intensity * Divisor);
			if ( delay <= 0 )
				delay = 1;
			TimeSpan time = TimeSpan.FromSeconds( delay );
				
			List<Point3D> circlePoints = CircleHelper.CircleMidpoint( loc.X, loc.Y, loc.Z, radius, true );
			
			Point3D eye = new Point3D( loc );
			eye.Z += 14;
			foreach( Point3D point in circlePoints )
			{
				Point3D target = new Point3D(point);
				target.Z += 14;
				if ( map.LineOfSight( eye, target ) )
				{
					FireTile tile = new FireTile( time+TimeSpan.FromSeconds( Utility.RandomDouble()*5 ), source, FireEffectType );
					tile.MoveToWorld( point, map );
					tile.AddCurrentOccupants();
				}
			}
		}
	}

	public class FireTile : Item
	{
        private FireEffectType m_FireEffectType;
        public FireEffectType FireEffectType { get { return m_FireEffectType; } set { m_FireEffectType = value; } }

        public int GetFireDamage { get { return ( FireEffectType == FireEffectType.Fire ? 100 : 0 ); } }
        public int GetColdDamage { get { return ( FireEffectType == FireEffectType.Cold ? 100 : 0 ); } }
        public int GetPoisonDamage { get { return ( FireEffectType == FireEffectType.Poison ? 100 : 0 ); } }
        public int GetEnergyDamage { get { return ( FireEffectType == FireEffectType.Energy ? 100 : 0 ); } }

        public int GetHue
        {
            get
            {
                if( FireEffectType == FireEffectType.Cold )
                    return 2972;
                if( FireEffectType == FireEffectType.Poison )
                    return 2597;
                if( FireEffectType == FireEffectType.Energy )
                    return 2833;
                return 0;
            }
        }

		// 14732 - 14751, 6571 - 6582
		private List<Mobile> m_List = new List<Mobile>(); // keep track of those standing on the item
		private InternalBurnTimer m_BurnTimer;
		private Mobile m_Owner;
        [Constructable]
        public FireTile( TimeSpan delay, Mobile owner ) : this( delay, owner, FireEffectType.Fire ) { }

        [Constructable]
        public FireTile( TimeSpan delay, Mobile owner, FireEffectType type ) : base( ( Utility.RandomBool() ? 14732 : 6571 ) )
        {
            FireEffectType = type;
            Hue = GetHue;
            m_Owner = owner;
            Movable = false;
            new InternalTimer( this, delay ).Start();
            m_BurnTimer = new InternalBurnTimer( this, TimeSpan.FromSeconds( 1 ), TimeSpan.FromSeconds( 1 ) );
            m_BurnTimer.Start();
        }

		public FireTile( Serial serial ) : base( serial )
		{
		}
		
		public void AddCurrentOccupants()
		{
			foreach ( object o in GetMobilesInRange( 0 ) )
			{
				Mobile mob = o as Mobile;
				if ( mob != null && !m_List.Contains( mob ) )
					m_List.Add( mob );
			}
		}
		
		public override bool OnMoveOff( Mobile m )
		{
			if ( m_List.Contains( m ) )
				m_List.Remove( m );

			return base.OnMoveOff( m );
		}
		
		public override bool OnMoveOver( Mobile m )
		{
			if ( !m_List.Contains( m ) )
				m_List.Add( m );

			return base.OnMoveOver( m );
		}

		public void Burn()
		{
			foreach ( Mobile mobile in m_List )
			{
				if ( mobile != null )
				{
                    AOS.Damage( mobile, m_Owner, 20, 0, GetFireDamage, GetColdDamage, GetPoisonDamage, GetEnergyDamage, 0, 0, 0 ); // 20 points of 100% fire damage
					Server.Effects.PlaySound( mobile.Location, mobile.Map, 477 );
				}
			}
		}

		public void BeforeRemoved()
		{
			if ( m_BurnTimer != null )
				m_BurnTimer.Stop();
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

			Delete();
		}

		private class InternalBurnTimer : Timer
		{
			private FireTile m_Fire;

			public InternalBurnTimer( FireTile fire, TimeSpan delay, TimeSpan interval ) : base( delay, interval )
			{
				Priority = TimerPriority.OneSecond;
				m_Fire = fire;
			}

			protected override void OnTick()
			{
				if ( m_Fire != null && !m_Fire.Deleted )
				{
					m_Fire.Burn();
					Delay = TimeSpan.FromSeconds(1);
				}
			}
		}

		private class InternalTimer : Timer
		{
			private FireTile m_Fire;

			public InternalTimer( FireTile fire, TimeSpan delay ) : base( delay )
			{
				Priority = TimerPriority.OneSecond;

				m_Fire = fire;
			}

			protected override void OnTick()
			{
				if ( m_Fire != null && !m_Fire.Deleted )
				{
					m_Fire.BeforeRemoved();
					m_Fire.Delete();
				}
			}
		}
	}
}
