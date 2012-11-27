using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	interface IHasQuality
	{
		int Quality{ get; set; }
	}
}