using System;
using Server;

namespace Server.Items
{
	public class ManaDecreaseEffect : StatPoisonEffect
	{
		public override string Name{ get{ return "Mana Decrease"; } }
		public override StatType Stat { get { return StatType.ManaMax; } }
	}
}
