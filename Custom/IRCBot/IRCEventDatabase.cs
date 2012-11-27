using System;
using System.Collections;
using System.Collections.Generic;

namespace Server.IRCBot
{
    public class IRCEventDatabase : Item
    {
        public static void Initialize()
        {
            if( Database == null )
                Database = new IRCEventDatabase();
        }

        private List<IRCEvent> m_Events = new List<IRCEvent>();
        public List<IRCEvent> Events { get { return m_Events; } set { m_Events = value; } }

        public static IRCEventDatabase Database = null;

        public IRCEventDatabase() : base( 1 )
        {
        }

        public IRCEventDatabase( Serial serial )
        {
        }

        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int)0 ); // version
            writer.Write( (int) Events.Count );

            for( int i = 0; i < Events.Count; i++ )
            {
                writer.Write( (string) Events[i].Name );
                writer.Write( (DateTime) Events[i].Date );
                writer.Write( (string) Events[i].Creator );
                writer.Write( (bool) Events[i].Deleted );
            }
        }

        public override void Deserialize( GenericReader reader )
        {
            base.Deserialize( reader );

            int version = reader.ReadInt();
            int count = reader.ReadInt();

            for( int i = 0; i < count; i++ )
            {
                string name = reader.ReadString();
                DateTime date = reader.ReadDateTime();
                string creator = reader.ReadString();
                
                if( !reader.ReadBool() )
                    Events.Add( new IRCEvent( name, date, creator ) );
            }

            Database = this;
        }
    }
}
