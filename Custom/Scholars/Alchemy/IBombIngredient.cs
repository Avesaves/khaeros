using System;
using Server;

namespace Server.Items
{
	interface IBombIngredient
	{
		int Range { get; } // range for this ingredient when going boom
		bool InstantEffect { get; } // does it immediately go splat when thrown, like goo?
		bool CanUse( Mobile mobile );
	}
}
