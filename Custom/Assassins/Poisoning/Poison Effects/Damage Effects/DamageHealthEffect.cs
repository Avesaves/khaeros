using System;
using Server;

namespace Server.Items
{
	public class DamageHealthEffect : DamagePoisonEffect
	{
		public override string Name{ get{ return "Damage Health"; } }
		public override Stat Stat { get { return Stat.Health; } }
	}
}
