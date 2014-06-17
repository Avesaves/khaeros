using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Spells;
using System.Collections;
using Server.Commands;

namespace Server.Items
{
    public class SerpentBile : Item
    {
        private int m_Power;

        [CommandProperty(AccessLevel.GameMaster)]
        public int Power
        {
            get { return m_Power; }
            set { m_Power = value; }
        }

        [Constructable]
        public SerpentBile()
            : base(0xF0A)
        {
            Stackable = false;
            Weight = 1.0;
            Name = "A bottle of bile";

        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !(from is PlayerMobile) || from.Deleted || !from.Alive || from.Frozen || from.Paralyzed)
                return;

            PlayerMobile pm = from as PlayerMobile;


            if (pm.DayOfDeath - 4 <= -50)
            {
                pm.SendMessage("This just looks too unappetizing to eat right now.");
                return;
            }
            if (pm.Hunger > 19)
            {
                pm.SendMessage("You are too full to eat this ... thing!");
                return;
            }
            if (from.Backpack != null && this.ParentEntity == from.Backpack)
            {

                from.Emote("*Drinks some strange foul-smelling fluid");

                this.Delete();
                pm.DayOfDeath -= 4;
                pm.Hunger += 1; 
               // pm.WikiConfig = "fey";

                
            }

            else
                from.SendMessage("That needs to be in your backpack for you to use it.");
        }

        public SerpentBile(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write((int)m_Power);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Power = reader.ReadInt();
        }

      
    }
}