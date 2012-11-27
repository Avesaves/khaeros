using System;
using Server;
using System.Collections.Generic;

namespace Server.Items
{
	public class ExplosionEffect : CustomPotionEffect
	{
		public override string Name{ get{ return "Explosion"; } }
		private const double Divisor = 0.30; // 30 damage at 100 intensity

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( intensity < 0 )
				return;

			AOS.Damage( to, source, BasePotion.Scale( to, (int)( intensity * Divisor ) ), 0, 75, 0, 0, 0, 25, 0, 0 ); // 75% fire, 25% phys
		}

		public override bool CanDrink( Mobile mobile )
		{
			return true;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
			Server.Effects.PlaySound( loc, map, 283 + Utility.Random( 4 ) );
			if ( itemSource is BombPotion )
			{
				BombPotion bomb = itemSource as BombPotion;
				List<Point3D> circlePoints = CircleHelper.CircleMidpoint( loc.X, loc.Y, loc.Z, bomb.ExplosionRange, true );
				Point3D eye = new Point3D( loc );
				eye.Z += 14;
				foreach( Point3D point in circlePoints )
				{
					Point3D target = new Point3D(point);
					target.Z += 14;
					if ( map.LineOfSight( eye, target ) )
						Server.Effects.SendLocationEffect( point, map, 0x36BD, 20 );
				}
			}
		}
	}
}
