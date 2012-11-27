using System;
using Server;

namespace Server.Items
{
	public class IntelligenceDecreaseEffect : StatPoisonEffect
	{
		public override string Name{ get{ return "Intelligence Decrease"; } }
		public override StatType Stat { get { return StatType.Int; } }
	}
}
