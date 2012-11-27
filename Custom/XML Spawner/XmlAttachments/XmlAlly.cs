using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;

namespace Server.Engines.XmlSpawner2
{
    public class XmlAlly : XmlAttachment
    {
        private CreatureGroup m_AllyOf;

        public CreatureGroup AllyOf
        {
            get
            {
                return m_AllyOf;
            }
            set
            {
                m_AllyOf = value;
                Name = m_AllyOf.ToString();
            }
        }

        [Attachable]
        public XmlAlly(CreatureGroup CreatureGroup)
        {
            m_AllyOf = CreatureGroup;
            Name = m_AllyOf.ToString();
        }

                
        public XmlAlly(ASerial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); //version
            writer.Write((int)AllyOf);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            AllyOf = (CreatureGroup)reader.ReadInt();
        }
    }
}

