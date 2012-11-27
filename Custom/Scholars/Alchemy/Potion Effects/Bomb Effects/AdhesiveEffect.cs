using System;
using Server;

namespace Server.Items
{
	public class AdhesiveEffect : CustomPotionEffect
	{
		public override string Name{ get{ return "Adhesive"; } }
		private const double Divisor = 0.15; // 15 seconds at 100 intensity

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( intensity < 0 )
				return;
			if ( source != to )
				source.DoHarmful( to );

			// check if cant move already...
			if ( to.CantWalk )
				return;
			else
			{
				TimeSpan duration;
				int seconds = (int)( intensity * Divisor );

				if ( seconds < 1 )
					seconds = 1;

				duration = TimeSpan.FromSeconds( seconds );
				to.CantWalk = true;
				Timer.DelayCall( duration, new TimerStateCallback( ReleaseMobile ), to );
				to.SendMessage( "Your feet have been glued to the ground!" );
			}
		}

		public override bool CanDrink( Mobile mobile )
		{
			return true;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
		}

		private static void ReleaseMobile( object state )
		{
			((Mobile)state).CantWalk = false;
		}
	}
}
