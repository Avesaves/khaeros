using System;
using Server;

namespace Server.Items
{
	public class ShrapnelEffect : CustomPotionEffect
	{
		public override string Name{ get{ return "Shrapnel"; } }
		private const double Divisor = 0.50; // 50 damage (pierce) at 100 intensity

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( intensity < 0 )
				return;

			AOS.Damage( to, source, (int)(intensity * Divisor), 0, 0, 0, 0, 0, 0, 0, 100 ); // 100% pierce
		}

		public override bool CanDrink( Mobile mobile )
		{
			return true;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
			//Server.Effects.PlaySound( loc, map, 0x3B9 );
			foreach ( Mobile m in map.GetMobilesInRange( loc, ((BombPotion)itemSource).ExplosionRange ) )
				if ( m != null )
					if ( map.LineOfSight( loc, m ) )
						m.MovingEffect( new Entity( Serial.Zero, loc, map ), 10244, 18, 1, false, false );
		}
	}
}
