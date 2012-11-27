using System;
using System.Xml;
using Server;
using Server.Mobiles;

namespace Server.Regions
{
	public class SanctuaryRegion : BaseRegion
	{
		public SanctuaryRegion( Mobile m, Rectangle3D rect ) : base( null, m.Map, m.Region, rect )
		{
		}

		public override bool AllowHarmful( Mobile from, Mobile target ) 
		{
			if (Contains(from.Location) && Contains(target.Location)) // only when both are inside the region
				return true;
			else
				return false;
		}
	}
}
