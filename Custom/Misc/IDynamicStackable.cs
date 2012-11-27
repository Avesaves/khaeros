using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	interface IDynamicStackable
	{
		void AcceptedStack( Mobile from, Item otherItem );
	}
}