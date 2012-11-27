using System;
using System.Collections;
using System.Collections.Generic;
using Server.Commands;

namespace Server.IRCBot
{
    public class IRCEvent : object
    {
        private string m_Name;
        private DateTime m_Date;
        private string m_Creator;
        private bool m_Deleted;

        public string Name { get { return m_Name; } set { m_Name = value; } }
        public DateTime Date { get { return m_Date; } set { m_Date = value; } }
        public string Creator { get { return m_Creator; } set { m_Creator = value; } }
        public bool Deleted { get { return m_Deleted; } set { m_Deleted = value; } }

        public IRCEvent( string name, DateTime date, string creator )
        {
            Name = name;
            Date = date;
            Creator = creator;
        }
    }
}
