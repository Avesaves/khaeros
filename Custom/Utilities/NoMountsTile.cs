using System;
using Server;
using Server.Mobiles;

namespace Server.Items
{
	public class NoMountsTile : Item
	{
		[Constructable]
		public NoMountsTile() : base( 8479 )
		{
			Visible = false;
			Movable = false;
		}
		
		public override bool OnMoveOver( Mobile m )
		{
            if (m is BaseMount)
                return false;
            else if (m is WorkHorse)
                return false;
            else if (m.Mounted)
                return false;
            else
                return true;
		}
		
		public NoMountsTile( Serial serial ) : base( serial )
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