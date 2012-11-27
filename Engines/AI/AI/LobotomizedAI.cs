using System;
using System.Collections;
using Server.Targeting;
using Server.Network;

// This AI does nothing

namespace Server.Mobiles
{
	public class LobotomizedAI : BaseAI
	{
		public LobotomizedAI(BaseCreature m) : base (m)
		{
		}

		public override bool DoActionWander()
		{
			return true;
		}

		public override bool DoActionGuard()
		{
			return true;
		}
	}
}
