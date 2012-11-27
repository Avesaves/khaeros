using System;
using Server;
using Server.Gumps;
using Server.Engines.Alchemy;
using Server.Engines.Craft;

namespace Server.Items
{
	public class AlchemyTool : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefAlchemy.CraftSystem; } } // not used

		[Constructable]
		public AlchemyTool() : base( 0xE9B )
		{
			Weight = 1.0;
		}

		[Constructable]
		public AlchemyTool( int uses ) : base( uses, 0xE9B )
		{
			Weight = 1.0;
		}

		public AlchemyTool( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( UsesRemaining > 0 )
			{
				if ( RootParent == from )
				{
					from.CloseGump( typeof( AlchemyGump ) );
					from.SendGump( new AlchemyGump( new BrewingState( from, this ) ) );
				}

				else
					from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
