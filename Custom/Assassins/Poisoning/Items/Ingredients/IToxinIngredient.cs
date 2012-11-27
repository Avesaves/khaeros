using System;
using Server;
using System.Collections.Generic;

namespace Server.Items
{
	interface IToxinIngredient
	{
		KeyValuePair<PoisonEffectEnum, int>[] Effects { get; }
		int Corrosivity{ get; }
		int ToxinActingSpeed{ get; }
		int ToxinDuration{ get; }
	}
}
