using System;
using Server;
using Server.Gumps;
using Server.Engines.Poisoning;
using Server.Engines.Craft;

namespace Server.Items
{
	public class MixingSet : BaseTool
	{
		public override CraftSystem CraftSystem{ get{ return DefAlchemy.CraftSystem; } } // not used

		[Constructable]
		public MixingSet() : base( 6237 )
		{
			Weight = 1.0;
			Name = "Mixing Set";
		}

		[Constructable]
		public MixingSet( int uses ) : base( uses, 6237 )
		{
			Weight = 1.0;
			Name = "Mixing Set";
		}

		public MixingSet( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( UsesRemaining > 0 )
			{
				if ( RootParent == from )
				{
					from.CloseGump( typeof( PoisoningGump ) );
					from.SendGump( new PoisoningGump( new PoisoningCraftState( from, this ) ) );
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
