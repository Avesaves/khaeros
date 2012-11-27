using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Spells;
using System.Collections;
using Server.Commands;
using Server.Misc;

namespace Server.Items
{
    public class Tattoo : Item
    {
        public Tattoo( int itemid ) : base( itemid )
        {
            Stackable = false;
            Weight = 1.0;
            Name = "tattoo";
            Hue = 0;
			Layer = Layer.Unused_xF;
        }
  
        public Tattoo( Serial serial )
            : base( serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int) 0 );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
        }
    }
}
