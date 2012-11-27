using System;
using Server;
using Server.Misc;

namespace Server.Items
{
	public class ThirstEffect : CustomPotionEffect
	{
		public override string Name{ get{ return "Thirst"; } }
		private const double Divisor = 0.20; // 20 thirst (full restoration) at 100 intensity

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( source != to && intensity < 0 )
				source.DoHarmful( to );

			if ( intensity < 0 )
			{
				int amount = (int)(BasePotion.Scale( to, intensity*-1 ) * Divisor);
				if ( amount <= 0)
					amount = 1;

				int tmp = to.Thirst - amount;
				if ( tmp < 0 )
				{
					to.Thirst = 0;
					tmp*=-1;
					to.Hits-=(int)(tmp*1.5);
					to.SendMessage( "You're dehydrated!" );
				}
				else
					to.Thirst -= amount;
			}
			else if ( to.Thirst >= 20 )
				return;
			else
			{
				int amount = (int)(BasePotion.Scale( to, intensity ) * Divisor);
				if ( amount <= 0)
					amount = 1;
				if ( to.Thirst + amount > 20 )
					to.Thirst = 20;
				else
					to.Thirst += amount;
			}
			
			FoodDecayTimer.CalculatePenalty( to );
		}

		public override bool CanDrink( Mobile mobile )
		{
			return mobile.Thirst < 20;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
		}
	}
}
