using System;
using Server;

namespace Server.Items
{
	// intensity = chance to detect hidden creature
	public class FlashEffect : CustomPotionEffect
	{
		public override string Name{ get{ return "Flash"; } }

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( intensity < 0 )
				return;
			
			if ( to.Hidden )
			{
				if ( (intensity * 0.01) > Utility.RandomDouble() )
				{
					to.SendMessage( "The flash reveals your position!" );
					to.RevealingAction();
				}
			}
		}

		public override bool CanDrink( Mobile mobile )
		{
			return true;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
			Server.Effects.PlaySound( loc, map, 283 + Utility.Random( 4 ) ); // boom
			Server.Effects.SendLocationEffect( loc, map, 0x37B9, 16 );
			LightSource light = new LightSource();
			light.MoveToWorld( loc, map );
			Timer.DelayCall( TimeSpan.FromMilliseconds( 500 ), new TimerStateCallback( RemoveLightSource ), light ); // 0.5 sec delay

		}

		private static void RemoveLightSource( object state )
		{
			Item item = state as Item;
			if ( item != null && !item.Deleted )
				item.Delete();
		}
	}
}
