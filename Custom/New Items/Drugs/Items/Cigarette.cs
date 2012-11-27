using System;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public class Cigarette : BaseSmokable
	{
		
		[Constructable]
		public Cigarette() : base( 12636, 3 )
		{
			Hue = 2965;
			Name = "cigarette";
			Weight = 0.1;
			ContentType = ContentType.Tobacco;
		}

		public Cigarette( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick ( Mobile from )
		{
			if ( RootParent == from && from is PlayerMobile )
			{
				if ( ContentRemaining > 0 )
				{
					OnSmoke( from );
                    XmlAddiction.Fix(from, "Tobacco");
					if ( --ContentRemaining <= 0 )
						Delete();
				}
				else
					from.SendMessage( "There's nothing left to smoke." );
			}
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
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
