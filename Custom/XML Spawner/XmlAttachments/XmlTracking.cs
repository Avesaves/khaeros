using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlTracking : XmlAttachment
    {
        private Mobile m_Target = null;

        [CommandProperty( AccessLevel.GameMaster )]
        public Mobile Target { get { return m_Target; } set { m_Target = value; } }

        // These are the various ways in which the message attachment can be constructed.  
        // These can be called via the [addatt interface, via scripts, via the spawner ATTACH keyword.
        // Other overloads could be defined to handle other types of arguments

        // a serial constructor is REQUIRED
        public XmlTracking( ASerial serial )
            : base( serial )
        {
        }

        [Attachable]
        public XmlTracking()
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );

            writer.Write( (int)0 );
            writer.Write( (Mobile)m_Target );
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
            m_Target = reader.ReadMobile();
        }
    }
}
