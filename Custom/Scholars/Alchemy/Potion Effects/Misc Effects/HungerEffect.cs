using System;
using Server;
using Server.Misc;

namespace Server.Items
{
	public class HungerEffect : CustomPotionEffect
	{
		public override string Name{ get{ return "Hunger"; } }
		private const double Divisor = 0.20; // 20 hunger (full restoration) at 100 intensity

		public override void ApplyEffect( Mobile to, Mobile source, int intensity, Item itemSource )
		{
			if ( source != to && intensity < 0 )
				source.DoHarmful( to );

			if ( intensity < 0 )
			{
				int amount = (int)(BasePotion.Scale( to, intensity*-1 ) * Divisor);
				if ( amount <= 0)
					amount = 1;

				int tmp = to.Hunger - amount;
				if ( tmp < 0 )
				{
					to.Hunger = 0;
					tmp*=-1;
					to.Hits-=(int)(tmp*1.5);
					to.SendMessage( "You're starving!" );
				}
				else
					to.Hunger -= amount;
			}
			else if ( to.Hunger >= 20 )
				return;
			else
			{
				int amount = (int)(BasePotion.Scale( to, intensity ) * Divisor);
				if ( amount <= 0)
					amount = 1;
				if ( to.Hunger + amount > 20 )
					to.Hunger = 20;
				else
					to.Hunger += amount;
			}
			FoodDecayTimer.CalculatePenalty( to );
		}

		public override bool CanDrink( Mobile mobile )
		{
			return mobile.Hunger < 20;
		}

		public override void OnExplode( Mobile source, Item itemSource, int intensity, Point3D loc, Map map )
		{
		}
	}
}
