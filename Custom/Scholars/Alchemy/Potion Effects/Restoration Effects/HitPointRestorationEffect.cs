using System;
using Server;
using Server.Network;

namespace Server.Items
{
	public class HitPointRestorationEffect : CustomPotionEffect
	{
		private const double Divisor = 0.2; // 20 hitpoints healed at 100 intensity
		public override string Name{ get{ return "Hit Point Restoration"; } }

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource ) // to = source for drinkable potions
		{
			if ( intensity > 0 ) // heal
			{
				if ( to.CanBeginAction( typeof( HitPointRestorationEffect ) ) && to.Hits < to.HitsMax )
				{
					to.BeginAction( typeof( HitPointRestorationEffect ) );
					to.Heal( BasePotion.Scale( to, (int)( intensity * Divisor ) ) );

					Timer.DelayCall( TimeSpan.FromSeconds( 20 ), new TimerStateCallback( ReleaseHealLock ), to ); // 20 sec delay
				}
			}

			else // damage
				to.Damage( BasePotion.Scale( to, (int)( intensity * Divisor * -1 ) ), source );
		}

		public override bool CanDrink( Mobile mobile )
		{
			return mobile.CanBeginAction( typeof( HitPointRestorationEffect ) ) && mobile.Hits < mobile.HitsMax;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
		}

		private static void ReleaseHealLock( object state )
		{
			((Mobile)state).EndAction( typeof( HitPointRestorationEffect ) );
		}
	}
}
