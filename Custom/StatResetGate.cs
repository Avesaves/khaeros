using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Items
{
	public class StatResetGate : Item
	{
		[Constructable]
		public StatResetGate() : base( 0xF6C )
		{
			Movable = false;
			Name = "Stat Reset Gate";
            Hue = 2101;
		}

		public StatResetGate( Serial serial ) : base( serial ) 
		{
		}

		public override bool OnMoveOver( Mobile m )
		{
		    PlayerMobile mobile = m as PlayerMobile;
		    mobile.RawDex = 10;
		    mobile.RawStr = 10;
		    mobile.RawInt = 10;
		    mobile.StatPoints = 690;
		    mobile.HitsMax = 10;
		    mobile.StamMax = 10;
		    mobile.ManaMax = 10;

		    return true;
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
