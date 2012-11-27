using System;
using Server;

namespace Server.Items
{
	interface ICanSkin
	{
		int RequiredSkinningLevel{ get; }
	}
}
