using System;
using Server;

namespace Server.Items
{
	public class DamageManaEffect : DamagePoisonEffect
	{
		public override string Name{ get{ return "Damage Mana"; } }
		public override Stat Stat { get { return Stat.Mana; } }
	}
}
