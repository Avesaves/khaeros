using System;
using Server;
using System.Collections.Generic;

namespace Server.Items
{
	interface IOilIngredient
	{
		bool CanUse( Mobile mobile );
		int Duration{ get; }
		int Corrosivity{ get; }
	}
}
