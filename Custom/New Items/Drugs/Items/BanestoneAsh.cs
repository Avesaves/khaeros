using System;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
	public class BanestoneAsh : BaseSnortable
	{
		
		[Constructable]
		public BanestoneAsh() : base( 3983, 2 )
		{
			Hue = 2989;
			Name = "a pile of fine ash";
			Weight = 0.1;
			ContentType2 = ContentType2.Banestone;
		}

		public BanestoneAsh( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick ( Mobile from )
		{
			if ( RootParent == from && from is PlayerMobile )
			{
				if ( ContentRemaining > 0 )
				{
					OnSnort( from );
                    XmlAddiction.Fix(from, "Banestone");
					if ( --ContentRemaining <= 0 )
						Delete();
				}
				else
					from.SendMessage( "There's nothing left to snort." );
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
