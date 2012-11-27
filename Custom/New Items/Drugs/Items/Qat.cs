using System;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public class Qat : BaseChewable
	{
		
		[Constructable]
		public Qat() : base( 0x1E01, 2 )
		{
			Hue = 1454;
			Name = "qat";
			Weight = 0.2;
			Chewable = Chewable.Qat;
		}

		public Qat( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick ( Mobile from )
		{
			if ( RootParent == from && from is PlayerMobile )
			{
				if ( ChewableRemaining > 0 )
				{
					OnChew( from );
                    XmlAddiction.Fix(from, "Qat");
					if ( --ChewableRemaining <= 0 )
						Delete();
				}
				else
					from.SendMessage( "There's nothing left to chew." );
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