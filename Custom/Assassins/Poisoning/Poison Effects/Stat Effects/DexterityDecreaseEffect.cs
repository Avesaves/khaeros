using System;
using Server;

namespace Server.Items
{
	public class DexterityDecreaseEffect : StatPoisonEffect
	{
		public override string Name{ get{ return "Dexterity Decrease"; } }
		public override StatType Stat { get { return StatType.Dex; } }
	}
}
