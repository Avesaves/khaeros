using System;
using System.Collections.Generic;
using Server;
using Server.Regions;

namespace Server.Items
{
	public class StickyGooEffect : CustomPotionEffect
	{
		public override string Name{ get{ return "Sticky Goo"; } }
		private const double Divisor = 0.30; // 30 second goo at 100 intensity

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( to != source )
				source.DoHarmful( to );
		}

		public override bool CanDrink( Mobile mobile )
		{
			return true;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
			Server.Effects.PlaySound( loc, map, 456 + Utility.Random( 3 ) );
            int range = 5;

			if ( itemSource is BombPotion )
			    range = ((BombPotion)itemSource).ExplosionRange;

			int delay = (int)(intensity * Divisor);

			if ( delay <= 0 )
				delay = 1;

			TimeSpan time = TimeSpan.FromSeconds( delay );

            Point3D start = new Point3D( loc.X-range, loc.Y-range, loc.Z );
            Point3D end = new Point3D( loc.X+range, loc.Y+range, loc.Z );
			StickyGooRegion region = new StickyGooRegion( source, new Rectangle3D( 
				start.X, start.Y, start.Z, end.X - start.X + 1, end.Y - start.Y + 1, 1
			) );

			region.Register();
			Timer.DelayCall( time, new TimerStateCallback( ReleaseRegion ), region );

            List<Point3D> circlePoints = CircleHelper.CircleMidpoint( loc.X, loc.Y, loc.Z, range, true );
			Point3D eye = new Point3D( loc );
			eye.Z += 14;

			foreach( Point3D point in circlePoints )
			{
				Point3D target = new Point3D(point);
				target.Z += 14;
				if ( map.LineOfSight( eye, target ) )
					new StickyGooTile( time+TimeSpan.FromSeconds( Utility.RandomDouble()*5 ) ).MoveToWorld( point, map );
			}
		}

		private static void ReleaseRegion( object state )
		{
			((Region)state).Unregister();
		}
	}

	public class StickyGooRegion : BaseRegion
	{
		public StickyGooRegion( Mobile m, Rectangle3D rect ) : base( null, m.Map, m.Region, rect )
		{
		}
	}

	public class StickyGooTile : Item
	{
		// Item IDs: 4650 - 4655
		[Constructable]
		public StickyGooTile( TimeSpan delay ) : base( Utility.Random( 6 ) + 4650 )
		{
			Movable = false;
			Hue = 358;
			new InternalTimer( this, delay ).Start();
		}

		public StickyGooTile( Serial serial ) : base( serial )
		{
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
			private StickyGooTile m_Goo;

			public InternalTimer( StickyGooTile goo, TimeSpan delay ) : base( delay )
			{
				Priority = TimerPriority.OneSecond;

				m_Goo = goo;
			}

			protected override void OnTick()
			{
				m_Goo.Delete();
			}
		}
	}
}
