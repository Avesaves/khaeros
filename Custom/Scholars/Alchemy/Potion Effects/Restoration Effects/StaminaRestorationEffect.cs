using System;
using Server;

namespace Server.Items
{
	public class StaminaRestorationEffect : CustomPotionEffect
	{
		public override string Name{ get{ return "Stamina Restoration"; } }
		private const double Divisor = 0.20; // 20 stamina at 100 intensity

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( intensity > 0 )
			{
				if ( to.CanBeginAction( typeof( StaminaRestorationEffect ) ) && to.Stam < to.StamMax )
				{
					to.Stam += BasePotion.Scale( to, (int)(intensity * Divisor) );
					to.BeginAction( typeof( StaminaRestorationEffect ) );
					Timer.DelayCall( TimeSpan.FromSeconds( 10 ), new TimerStateCallback( ReleaseStamLock ), to ); // 10 sec delay
				}
			}
			else
			{
				to.Stam -= BasePotion.Scale( to, (int)(-1 * intensity * Divisor) );

				if ( source != to ) // if it was thrown or something
					source.DoHarmful( to );
			}
				
		}

		public override bool CanDrink( Mobile mobile )
		{
			return mobile.CanBeginAction( typeof( StaminaRestorationEffect ) ) && mobile.Stam < mobile.StamMax;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
		}

		private static void ReleaseStamLock( object state )
		{
			((Mobile)state).EndAction( typeof( StaminaRestorationEffect ) );
		}
	}
}
