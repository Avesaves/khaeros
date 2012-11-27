using System;
using Server;

namespace Server.Items
{
	public abstract class StatPoisonEffect : PoisonEffect
	{
		private string ModName { get { return "[Poison] " + Stat.ToString() + " Offset"; } }
		public virtual StatType Stat { get { return StatType.Str; } }
		private const double Divisor = 0.40; // 40 point debuff at 100 intensity

		public override void ApplyPoison( Mobile to, Mobile source, int intensity )
		{
			int offset = (int)( intensity * Divisor );
			to.AddStatMod( new StatMod( Stat, ModName, -offset, TimeSpan.Zero ) );
		}
		
		public override void CureEffect( Mobile mobile )
		{
			mobile.RemoveStatMod( ModName );
		}
	}
}
