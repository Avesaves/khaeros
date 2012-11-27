using System;
using Server;
using System.Collections.Generic;

namespace Server.Items
{
	interface IDrinkIngredient
	{
		int PotionBooster { get; } // boosts overall effectiveness of the potion
		bool CanUse( Mobile mobile );
	}
}
