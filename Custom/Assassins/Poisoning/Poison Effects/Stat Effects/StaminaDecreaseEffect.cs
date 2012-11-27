using System;
using Server;

namespace Server.Items
{
	public class StaminaDecreaseEffect : StatPoisonEffect
	{
		public override string Name{ get{ return "Stamina Decrease"; } }
		public override StatType Stat { get { return StatType.StamMax; } }
	}
}
