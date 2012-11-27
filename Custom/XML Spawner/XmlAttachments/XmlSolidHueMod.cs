using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlSolidHueMod : XmlAttachment
    {
        private int m_Hue;
        // These are the various ways in which the message attachment can be constructed.  
        // These can be called via the [addatt interface, via scripts, via the spawner ATTACH keyword.
        // Other overloads could be defined to handle other types of arguments

        // a serial constructor is REQUIRED
        public XmlSolidHueMod( ASerial serial )
            : base( serial )
        {
        }

        [Attachable]
        public XmlSolidHueMod( int hue, double seconds )
        {
            m_Hue = hue;
            Expiration = TimeSpan.FromSeconds( seconds );
        }

        public override void OnDelete()
        {
            base.OnDelete();

            // remove the mod
            if( AttachedTo is Mobile )
            {
                ( (Mobile)AttachedTo ).SolidHueOverride = -1;
            }
        }

        public override void OnAttach()
        {
            base.OnAttach();

            // apply the mod
            if( AttachedTo is Mobile )
                ( (Mobile)AttachedTo ).SolidHueOverride = m_Hue;
            else
                Delete();
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 ); // version
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();

            Delete();
        }
    }
}
