using System;
using Server;
using System.Collections.Generic;

namespace Server.Items
{
	interface IAlchemyIngredient
	{
		 KeyValuePair<CustomEffect, int>[] Effects { get; }
	}
}
