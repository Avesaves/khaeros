using System;
using Server;
using Server.Network;
using Server.Items;
using System.Collections;

namespace Server.Items
{
    public class SGLocationAddon1East : BaseAddon
    {
        [Constructable]
        public SGLocationAddon1East ()
        {
            AddComponent(new AddonComponent(10907), 0, +4, +1);  //Altar Piece

			AddComponent(new AddonComponent(13833),  0, 0, 1);  //Stairs
			AddComponent(new AddonComponent(13834),  +1, 0, 0);  
			AddComponent(new AddonComponent(13836), -1, 0, 0);  
			AddComponent(new AddonComponent(13837),  0, -1, 0);  
			AddComponent(new AddonComponent(13835),  0, +1, 0);  
			
			AddComponent(new AddonComponent(13842),  +2, +2, 0);  //Pillars
			AddComponent(new AddonComponent(13842), -2, +2, 0);  
			AddComponent(new AddonComponent(13842),  -2, -2, 0);  
			AddComponent(new AddonComponent(13842),  +2, -2, +1);  
			
			AddComponent(new AddonComponent(12292),  +1, +1, 0);  //Tiles
			AddComponent(new AddonComponent(12293), -1, +1, 0);  
			AddComponent(new AddonComponent(12294),  -1, -1, 0);  
			AddComponent(new AddonComponent(12295),  +1, -1, 0);  
			
			AddComponent(new AddonComponent(6587),  +1, +1, 3);  //Braziers
			AddComponent(new AddonComponent(6587), -1, +1, 3);  
			AddComponent(new AddonComponent(6587),  -1, -1, 3);  
			AddComponent(new AddonComponent(6587),  +1, -1, 3);  
        }

        public SGLocationAddon1East(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    public class SGLocationLantern1East : AddonComponent
    {
        [Constructable]
        public SGLocationLantern1East() : base(2567)
        {
            Name = "A Torch";
            Light = LightType.Circle225;
        }

        public SGLocationLantern1East(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}