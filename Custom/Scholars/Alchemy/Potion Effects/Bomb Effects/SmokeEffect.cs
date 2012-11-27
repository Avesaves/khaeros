using System;
using System.Collections.Generic;
using Server;

namespace Server.Items
{
	public class SmokeEffect : CustomPotionEffect
	{
		public override string Name{ get{ return "Smoke"; } }
		private const double Divisor = 0.30; // 30 second smoke at 100 intensity

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
		}

		public override bool CanDrink( Mobile mobile )
		{
			return true;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
			Server.Effects.PlaySound( loc, map, 0x22F );
			if ( itemSource is BombPotion )
			{
				BombPotion bomb = itemSource as BombPotion;
				int delay = (int)(intensity * Divisor);
				if ( delay <= 0 )
					delay = 1;
				TimeSpan time = TimeSpan.FromSeconds( delay );

				List<Point3D> circlePoints = CircleHelper.CircleMidpoint( loc.X, loc.Y, loc.Z, bomb.ExplosionRange, true );
				Point3D eye = new Point3D( loc );
				eye.Z += 14;
				foreach( Point3D point in circlePoints )
				{
					Point3D target = new Point3D(point);
					target.Z += 14;
					if ( map.LineOfSight( eye, target ) )
					{
						SmokeTile tile = new SmokeTile( time+TimeSpan.FromSeconds( Utility.RandomDouble()*5 ) );
						tile.MoveToWorld( point, map );
						tile.AddCurrentOccupants();
					}
				}
			}
		}
	}

	public class SmokeTile : Item
	{
		private List<Mobile> m_List = new List<Mobile>(); // keep track of those standing on the item
		[Constructable]
		public SmokeTile( TimeSpan delay ) : base( 14120 )
		{
			Movable = false;
			new InternalTimer( this, delay ).Start();
		}

		public SmokeTile( Serial serial ) : base( serial )
		{
		}
		
		public void AddCurrentOccupants()
		{
			foreach ( object o in GetMobilesInRange( 0 ) )
			{
				Mobile mob = o as Mobile;
				if ( mob != null && !m_List.Contains( mob ) && !mob.Hidden )
				{
					m_List.Add( mob );
					mob.Hidden = true;
					mob.AllowedStealthSteps = 2;
				}
			}
		}

		public void BeforeRemoved()
		{
			foreach ( Mobile m in m_List )
				m.UseSkill( SkillName.Hiding );
		}

		public override bool HandlesOnMovement { get { return true; } }

		public override void OnMovement( Mobile m, Point3D oldLocation ) // due to hiding going haywire by using OnMoveOver
		{
			if ( m.Location == Location && !m_List.Contains( m ) && !m.Hidden ) // stepped on it
			{
				m_List.Add( m );
				m.Hidden = true;
				m.AllowedStealthSteps = 2;
			}
		}

		public override bool OnMoveOff( Mobile m )
		{
			if ( m_List.Contains( m ) )
				m_List.Remove( m );

			return true;
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

		private class InternalTimer : Timer
		{
			private SmokeTile m_Smoke;

			public InternalTimer( SmokeTile smoke, TimeSpan delay ) : base( delay )
			{
				Priority = TimerPriority.OneSecond;

				m_Smoke = smoke;
			}

			protected override void OnTick()
			{
				m_Smoke.BeforeRemoved();
				m_Smoke.Delete();
			}
		}
	}
}
