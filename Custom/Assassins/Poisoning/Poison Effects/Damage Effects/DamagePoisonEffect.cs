using System;
using Server;

namespace Server.Items
{
	public enum Stat
	{
		Health,
		Stamina,
		Mana
	}
	public abstract class DamagePoisonEffect : PoisonEffect
	{
		public virtual Stat Stat { get { return Stat.Health; } }
		private const double Divisor = 0.10; // 10 point damage at 100 intensity

		public override void ApplyPoison( Mobile to, Mobile source, int intensity )
		{
			int amount = (int)( intensity * Divisor );
			if ( amount == 0 )
				amount = 1;
			switch ( (int)Stat )
			{
				case (int)Stat.Health:
				{
					to.Damage( amount, source );
					break;
				}
				
				case (int)Stat.Stamina:
				{
					to.Stam-=amount;
					break;
				}
				
				case (int)Stat.Mana:
				{
					to.Mana-=amount;
					break;
				}
			}
		}
		
		public override void CureEffect( Mobile mobile ) // can't be cured, as there's nothing to cure -- it just stops
		{
		}
	}
}
