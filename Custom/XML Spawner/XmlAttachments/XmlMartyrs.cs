using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Engines.XmlSpawner2
{
    public class XmlMartyrs : XmlAttachment
    {
        private TimeSpan m_Duration = TimeSpan.FromSeconds(30.0);       // default 30 sec duration
        private int m_Value = 1;       // default value of 1

        [CommandProperty(AccessLevel.GameMaster)]
        public int Value { get { return m_Value; } set { m_Value = value; } }


        // These are the various ways in which the message attachment can be constructed.  
        // These can be called via the [addatt interface, via scripts, via the spawner ATTACH keyword.
        // Other overloads could be defined to handle other types of arguments

        // a serial constructor is REQUIRED
        public XmlMartyrs(ASerial serial)
            : base(serial)
        {
        }

        [Attachable]
        public XmlMartyrs()
        {
        }

        [Attachable]
        public XmlMartyrs(int value)
        {
            m_Value = value;
        }

        [Attachable]
        public XmlMartyrs(int value, double duration)
        {
            m_Value = value;
            m_Duration = TimeSpan.FromSeconds(duration);
        }

        public override void OnAttach()
        {
            base.OnAttach();

            Timer.DelayCall(m_Duration, new TimerCallback(Delete));
        }
        
        public override void Serialize ( GenericWriter writer ) 
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize ( GenericReader reader ) 
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			
			Delete();
		}
    }
}
