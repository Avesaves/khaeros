using System;
using Server;

namespace Server.Items
{
	public class HealthDecreaseEffect : StatPoisonEffect
	{
		public override string Name{ get{ return "Health Decrease"; } }
		public override StatType Stat { get { return StatType.HitsMax; } }
	}
}
