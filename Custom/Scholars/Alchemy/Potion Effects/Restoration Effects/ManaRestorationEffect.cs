using System;
using Server;

namespace Server.Items
{
	public class ManaRestorationEffect : CustomPotionEffect
	{
		public override string Name{ get{ return "Mana Restoration"; } }
		private const double Divisor = 0.20; // 20 mana at 100 intensity

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( intensity > 0 )
			{
				if ( to.CanBeginAction( typeof( ManaRestorationEffect ) ) && to.Mana < to.ManaMax )
				{
					to.Mana += BasePotion.Scale( to, (int)(intensity * Divisor) );
					to.BeginAction( typeof( ManaRestorationEffect ) );
					Timer.DelayCall( TimeSpan.FromSeconds( 10 ), new TimerStateCallback( ReleaseManaLock ), to ); // 10 sec delay
				}
			}
			else
			{
				to.Mana -= BasePotion.Scale( to, (int)(-1 * intensity * Divisor) );

				if ( source != to ) // if it was thrown or something
					source.DoHarmful( to );
			}
				
		}

		public override bool CanDrink( Mobile mobile )
		{
			return mobile.CanBeginAction( typeof( ManaRestorationEffect ) ) && mobile.Mana < mobile.ManaMax;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
		}

		private static void ReleaseManaLock( object state )
		{
			((Mobile)state).EndAction( typeof( ManaRestorationEffect ) );
		}
	}
}
