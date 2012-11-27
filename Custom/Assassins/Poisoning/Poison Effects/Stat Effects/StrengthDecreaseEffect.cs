using System;
using Server;

namespace Server.Items
{
	public class StrengthDecreaseEffect : StatPoisonEffect
	{
		public override string Name{ get{ return "Strength Decrease"; } }
		public override StatType Stat { get { return StatType.Str; } }
	}
}
