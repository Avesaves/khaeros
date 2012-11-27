using System;

namespace Server.Factions
{
	public class Moonglow : Town
	{
		public Moonglow()
		{
			Definition =
				new TownDefinition(
					3,
					0x186C,
					"Moonglow",
					"Moonglow",
					new TextDefinition( 1011435, "MOONGLOW" ),
					new TextDefinition( 1011563, "TOWN STONE FOR MOONGLOW" ),
					new TextDefinition( 1041037, "The Faction Sigil Monolith of Moonglow" ),
					new TextDefinition( 1041407, "The Faction Town Sigil Monolith of Moonglow" ),
					new TextDefinition( 1041416, "Faction Town Stone of Moonglow" ),
					new TextDefinition( 1041398, "Faction Town Sigil of Moonglow" ),
					new TextDefinition( 1041389, "Corrupted Faction Town Sigil of Moonglow" ),
					new Point3D( 0, 0, 0 ),
					new Point3D( 0, 0, 0 ) );
		}
	}
}
