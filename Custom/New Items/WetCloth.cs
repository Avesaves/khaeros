using System;
using Server;

namespace Server.Items
{
    public class WetCloth : UncutCloth
    {
        public WetCloth()
        {
            Name = "a wet cloth";
            Stackable = true;
        }

        public WetCloth(Serial serial)
            : base(serial)
        {

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}