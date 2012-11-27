using System;
using Server;

namespace Server.Items
{
	public class DamageStaminaEffect : DamagePoisonEffect
	{
		public override string Name{ get{ return "Damage Stamina"; } }
		public override Stat Stat { get { return Stat.Stamina; } }
	}
}
