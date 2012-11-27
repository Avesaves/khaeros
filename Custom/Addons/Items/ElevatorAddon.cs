using System;
using Server;

namespace Server.Items
{
	public class ElevatorAddon : BaseAddon
	{
		//public override BaseAddonDeed Deed{ get{ return new AbbatoirDeed(); } }

		[Constructable]
		public ElevatorAddon()
		{
			AddComponent( new AddonComponent( 0x917 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 0x917 ),  0, -1, 0 );
			AddComponent( new AddonComponent( 0x917 ),  1, -1, 0 );
			AddComponent( new AddonComponent( 0x917 ), -1,  0, 0 );
			AddComponent( new AddonComponent( 0x917 ),  0,  0, 0 );
			AddComponent( new AddonComponent( 0x917 ),  1,  0, 0 );
			AddComponent( new AddonComponent( 0x917 ), -1,  1, 0 );
			AddComponent( new AddonComponent( 0x917 ),  0,  1, 0 );
			AddComponent( new AddonComponent( 0x917 ),  1,  1, 0 );
		}

		public ElevatorAddon( Serial serial ) : base( serial )
		{
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
