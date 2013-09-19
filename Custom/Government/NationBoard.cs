using System;
using Server.Prompts;
using Server.Mobiles;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;

namespace Server.Items
{
    [Flipable(0x1E5E, 0x1E5F)]
    public class NationBoard : Item
    {
        #region Properties

        private Nation m_Nation;
        private Point3D m_JailLocation;
        private Point3D m_SetFreeLocation;
        private TimeSpan m_JailTime;
        private List<string> m_KillByNameList;
        private List<string> m_ArrestByNameList;
        private List<string> m_KillByNationList;
        private List<string> m_ArrestByNationList;
        private List<string> m_KillByNationExceptionList;
        private List<string> m_ArrestByNationExceptionList;
        public static List<NationBoard> NationBoards = new List<NationBoard>();

        [CommandProperty(AccessLevel.GameMaster)]
        public bool DoDelete
        {
            get { return false; }
            set
            {
                if (value == true)
                    this.Delete();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Nation Nation
        {
            get { return m_Nation; }
            set { m_Nation = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D JailLocation
        {
            get { return m_JailLocation; }
            set { m_JailLocation = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D SetFreeLocation
        {
            get { return m_SetFreeLocation; }
            set { m_SetFreeLocation = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public TimeSpan JailTime
        {
            get { return m_JailTime; }
            set { m_JailTime = value; }
        }

        public List<string> KillByNameList
        {
            get { return m_KillByNameList; }
            set { m_KillByNameList = value; }
        }

        public List<string> ArrestByNameList
        {
            get { return m_ArrestByNameList; }
            set { m_ArrestByNameList = value; }
        }

        public List<string> KillByNationList
        {
            get { return m_KillByNationList; }
            set { m_KillByNationList = value; }
        }

        public List<string> ArrestByNationList
        {
            get { return m_ArrestByNationList; }
            set { m_ArrestByNationList = value; }
        }

        public List<string> KillByNationExceptionList
        {
            get { return m_KillByNationExceptionList; }
            set { m_KillByNationExceptionList = value; }
        }

        public List<string> ArrestByNationExceptionList
        {
            get { return m_ArrestByNationExceptionList; }
            set { m_ArrestByNationExceptionList = value; }
        }

        #endregion

        #region Construction Parameters

        [Constructable]
        public NationBoard()
            : base(0x1E5E)
        {
            Weight = 1.0;
            Name = "a Nation Board";
            m_KillByNameList = new List<string>();
            m_ArrestByNameList = new List<string>();
            m_KillByNationList = new List<string>();
            m_ArrestByNationList = new List<string>();
            m_KillByNationExceptionList = new List<string>();
            m_ArrestByNationExceptionList = new List<string>();

            NationBoards.Add(this);
        }

        #endregion

        #region OnDoubleClick

        public override void OnDoubleClick(Mobile from)
        {
            PlayerMobile pm = from as PlayerMobile;
            bool leaderAccess = false;
            bool officerAccess = false;

            if (this.Nation == null || this.Nation == Nation.None)
            {
                from.SendMessage("Nation has not been set for this Nation Board. Please contact staff.");
                return;
            }

            if (GovernmentEntity.Governments != null)
            {
                foreach (GovernmentEntity g in GovernmentEntity.Governments)
                {
                    if (g.Nation != null && g.Nation != Nation.None)
                    {
                        if (GovernmentEntity.IsGuildLeader(pm, g) && g.Nation == this.Nation)
                        {
                            leaderAccess = true;
                            break;
                        }

                        if (GovernmentEntity.IsGuildOfficer(pm, g) && g.Nation == this.Nation)
                            officerAccess = true;
                    }
                }
            }
            else
                from.SendMessage("There are currently no governments.");

            if (from.AccessLevel > AccessLevel.Player)
                leaderAccess = true;

            if(leaderAccess || officerAccess)
            {
                from.SendMessage("Type the number of one the following options:");
                from.SendMessage("01 AddKillByName");
                from.SendMessage("02 RemoveKillByName");
                from.SendMessage("03 DisplayKillByNameList");
                from.SendMessage("11 AddArrestByName");
                from.SendMessage("12 RemoveArrestByName");
                from.SendMessage("13 DisplayArrestByNameList");

                if (leaderAccess)
                {
                    from.SendMessage("21 AddKillByNation");
                    from.SendMessage("22 RemoveKillByNation");
                    from.SendMessage("23 DisplayKillByNationList");
                    from.SendMessage("31 AddKillByNationException");
                    from.SendMessage("32 RemoveKillByNationException");
                    from.SendMessage("33 DisplayKillByNationExceptionList");
                    from.SendMessage("41 AddArrestByNation");
                    from.SendMessage("42 RemoveArrestByNation");
                    from.SendMessage("43 DisplayArrestByNationList");
                    from.SendMessage("51 AddArrestByNationException");
                    from.SendMessage("52 RemoveArrestByNationException");
                    from.SendMessage("53 DisplayArrestByNationExceptionList");
                }

                from.Prompt = new NationBoardOptionsPrompt(this);
            }
            else
                from.SendMessage("You do not have access to this nation's board.");
        }

        #endregion

        #region Handling Methods

        public void KillByNameMethod(Mobile from, string text)
        {
            //ADDKILLBYNAME
            if (text == "01")
            {
                from.SendMessage("Enter the name of the person you wish to add to the killing list.");
                from.Prompt = new KillByNamePrompt(this, 1);
            }

            //REMOVEKILLBYNAME
            if (text == "02")
            {
                from.SendMessage("Enter the name of the person you wish to remove from the killing list.");
                from.Prompt = new KillByNamePrompt(this, 2);
            }

            //DISPLAYKILLBYNAMELIST
            if (text == "03")
            {
                if (KillByNameList.Count < 1)
                {
                    from.SendMessage("No one is targeted for execution at the moment.");
                }

                else
                {
                    from.SendMessage("To be attacked on sight:");

                    for (int i = 0; i < KillByNameList.Count; ++i)
                        from.SendMessage(KillByNameList[i]);
                }
            }
        }

        public void ArrestByNameMethod(Mobile from, string text)
        {
            //ADDARRESTBYNAME
            if (text == "11")
            {
                from.SendMessage("Enter the name of the person you wish to add to the arresting list.");
                from.Prompt = new ArrestByNamePrompt(this, 1);
            }

            //REMOVEARRESTBYNAME
            if (text == "12")
            {
                from.SendMessage("Enter the name of the person you wish to remove from the arresting list.");
                from.Prompt = new ArrestByNamePrompt(this, 2);
            }

            //DISPLAYARRESTBYNAMELIST
            if (text == "13")
            {
                if (ArrestByNameList.Count < 1)
                {
                    from.SendMessage("No one is targeted for prison at the moment.");
                }

                else
                {
                    from.SendMessage("To be arrested on sight:");

                    for (int i = 0; i < ArrestByNameList.Count; ++i)
                        from.SendMessage(ArrestByNameList[i]);
                }
            }
        }

        public void KillByNationMethod(Mobile from, string text)
        {
            //ADDKILLBYNATION
            if (text == "21")
            {
                from.SendMessage("Enter the name of the nation you wish to add to the killing list.");
                from.Prompt = new KillByNationPrompt(this, 1);
            }

            //REMOVEKILLBYNATION
            if (text == "22")
            {
                from.SendMessage("Enter the name of the nation you wish to remove from the killing list.");
                from.Prompt = new KillByNationPrompt(this, 2);
            }

            //DISPLAYKILLBYNATIONLIST
            if (text == "23")
            {
                if (KillByNationList.Count < 1)
                {
                    from.SendMessage("No nation is targeted for execution at the moment.");
                }

                else
                {
                    from.SendMessage("To be attacked on sight:");

                    for (int i = 0; i < KillByNationList.Count; ++i)
                        from.SendMessage(KillByNationList[i]);
                }
            }
        }

        public void ArrestByNationMethod(Mobile from, string text)
        {
            //ADDARRESTBYNATION
            if (text == "41")
            {
                from.SendMessage("Enter the name of the nation you wish to add to the arresting list.");
                from.Prompt = new ArrestByNationPrompt(this, 1);
            }

            //REMOVEARRESTBYNATION
            if (text == "42")
            {
                from.SendMessage("Enter the name of the nation you wish to remove from the arresting list.");
                from.Prompt = new ArrestByNationPrompt(this, 2);
            }

            //DISPLAYARRESTBYNATIONLIST
            if (text == "43")
            {
                if (ArrestByNationList.Count < 1)
                {
                    from.SendMessage("No nation is targeted for prison at the moment.");
                }

                else
                {
                    from.SendMessage("To be arrested on sight:");

                    for (int i = 0; i < ArrestByNationList.Count; ++i)
                        from.SendMessage(ArrestByNationList[i]);
                }
            }
        }

        public void KillByNationExceptionMethod(Mobile from, string text)
        {
            //ADDKILLBYNATIONEXCEPTION
            if (text == "31")
            {
                from.SendMessage("Enter the name of the person you wish to add to the killing exception list.");
                from.Prompt = new KillByNationExceptionPrompt(this, 1);
            }

            //REMOVEKILLBYNATIONEXCEPTION
            if (text == "32")
            {
                from.SendMessage("Enter the name of the person you wish to remove from the killing exception list.");
                from.Prompt = new KillByNationExceptionPrompt(this, 2);
            }

            //DISPLAYKILLBYNATIONEXCEPTIONLIST
            if (text == "33")
            {
                if (KillByNationExceptionList.Count < 1)
                {
                    from.SendMessage("No one is an exception when it comes to killing enemy personnel at the moment.");
                }

                else
                {
                    from.SendMessage("Not to be attacked:");

                    for (int i = 0; i < KillByNationExceptionList.Count; ++i)
                        from.SendMessage(KillByNationExceptionList[i]);
                }
            }
        }

        public void ArrestByNationExceptionMethod(Mobile from, string text)
        {
            //ADDARRESTBYNATIONEXCEPTION
            if (text == "51")
            {
                from.SendMessage("Enter the name of the person you wish to add to the arresting exception list.");
                from.Prompt = new ArrestByNationExceptionPrompt(this, 1);
            }

            //REMOVEARRESTBYNATIONEXCEPTION
            if (text == "52")
            {
                from.SendMessage("Enter the name of the person you wish to remove from the arresting exception list.");
                from.Prompt = new ArrestByNationExceptionPrompt(this, 2);
            }

            //DISPLAYARRESTBYNATIONEXCEPTIONLIST
            if (text == "53")
            {
                if (ArrestByNationExceptionList.Count < 1)
                {
                    from.SendMessage("No one is an exception when it comes to arresting enemy personnel at the moment.");
                }

                else
                {
                    from.SendMessage("Not to be arrested:");

                    for (int i = 0; i < ArrestByNationExceptionList.Count; ++i)
                        from.SendMessage(ArrestByNationExceptionList[i]);
                }
            }
        }

        #endregion

        #region Serialization and Deserialization

        public NationBoard(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);

            writer.Write((int)m_Nation);

            writer.Write((int)m_JailLocation.X);
            writer.Write((int)m_JailLocation.Y);
            writer.Write((int)m_JailLocation.Z);

            writer.Write((int)m_SetFreeLocation.X);
            writer.Write((int)m_SetFreeLocation.Y);
            writer.Write((int)m_SetFreeLocation.Z);

            writer.Write((TimeSpan)m_JailTime);

            //KILLBYNAME and ARRESTBYNAME serialization

            writer.Write(m_KillByNameList.Count);

            for (int i = 0; i < m_KillByNameList.Count; ++i)
                writer.Write(m_KillByNameList[i]);

            writer.Write(m_ArrestByNameList.Count);

            for (int i = 0; i < m_ArrestByNameList.Count; ++i)
                writer.Write(m_ArrestByNameList[i]);

            //KILLBYNATION and ARRESTBYNATION serialization

            writer.Write(m_KillByNationList.Count);

            for (int i = 0; i < m_KillByNationList.Count; ++i)
                writer.Write(m_KillByNationList[i]);

            writer.Write(m_ArrestByNationList.Count);

            for (int i = 0; i < m_ArrestByNationList.Count; ++i)
                writer.Write(m_ArrestByNationList[i]);

            //KILLBYNATIONEXCEPTION and ARRESTBYNATIONEXCEPTION serialization

            writer.Write(m_KillByNationExceptionList.Count);

            for (int i = 0; i < m_KillByNationExceptionList.Count; ++i)
                writer.Write(m_KillByNationExceptionList[i]);

            writer.Write(m_KillByNationExceptionList.Count);

            for (int i = 0; i < m_KillByNationExceptionList.Count; ++i)
                writer.Write(m_KillByNationExceptionList[i]);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Nation = (Nation)reader.ReadInt();

            m_JailLocation.X = reader.ReadInt();
            m_JailLocation.Y = reader.ReadInt();
            m_JailLocation.Z = reader.ReadInt();

            m_SetFreeLocation.X = reader.ReadInt();
            m_SetFreeLocation.Y = reader.ReadInt();
            m_SetFreeLocation.Z = reader.ReadInt();

            m_JailTime = reader.ReadTimeSpan();

            //KILLBYNAME deserialization

            int size = reader.ReadInt();

            m_KillByNameList = new List<string>(size);

            for (int i = 0; i < size; ++i)
            {
                string typeName = reader.ReadString();

                m_KillByNameList.Add(typeName);
            }

            //ARRESTBYNAME deserialization

            size = reader.ReadInt();

            m_ArrestByNameList = new List<string>(size);

            for (int i = 0; i < size; ++i)
            {
                string typeName = reader.ReadString();

                m_ArrestByNameList.Add(typeName);
            }

            //KILLBYNATION deserialization

            size = reader.ReadInt();

            m_KillByNationList = new List<string>(size);

            for (int i = 0; i < size; ++i)
            {
                string typeName = reader.ReadString();

                m_KillByNationList.Add(typeName);
            }

            //ARRESTBYNATION deserialization

            size = reader.ReadInt();

            m_ArrestByNationList = new List<string>(size);

            for (int i = 0; i < size; ++i)
            {
                string typeName = reader.ReadString();

                m_ArrestByNationList.Add(typeName);
            }

            //KILLBYNATIONEXCEPTION deserialization

            size = reader.ReadInt();

            m_KillByNationExceptionList = new List<string>(size);

            for (int i = 0; i < size; ++i)
            {
                string typeName = reader.ReadString();

                m_KillByNationExceptionList.Add(typeName);
            }

            //ARRESTBYNATIONEXCEPTION deserialization

            size = reader.ReadInt();

            m_ArrestByNationExceptionList = new List<string>(size);

            for (int i = 0; i < size; ++i)
            {
                string typeName = reader.ReadString();

                m_ArrestByNationExceptionList.Add(typeName);
            }

            NationBoards.Add(this);
        }

        public override void OnDelete()
        {
            if (NationBoards.Contains(this))
                NationBoards.Remove(this);
        }

        public static void KilledByName(Mobile from, string text)
        {
            from.SendMessage("You chose option " + text + ".");
        }

        public static void ArrestedByName(Mobile from, string text)
        {
            from.SendMessage("You chose option " + text + ".");
        }

        public static void KilledByNation(Mobile from, string text)
        {
            from.SendMessage("You chose option " + text + ".");
        }

        public static void ArrestedByNation(Mobile from, string text)
        {
            from.SendMessage("You chose option " + text + ".");
        }

        public static void KilledByNationException(Mobile from, string text)
        {
            from.SendMessage("You chose option " + text + ".");
        }

        public static void ArrestedByNationException(Mobile from, string text)
        {
            from.SendMessage("You chose option " + text + ".");
        }

        #endregion
    }

    #region Prompts

    public class KillByNamePrompt : Prompt
    {
        private NationBoard m_NationBoard;
        private int m_Option;

        public KillByNamePrompt(NationBoard board, int option)
        {
            m_NationBoard = board;
            m_Option = option;
        }

        public override void OnResponse(Mobile from, string text)
        {
            switch (m_Option)
            {
                case 1:
                    {
                        if (m_NationBoard.KillByNameList.Contains(text))
                        {
                            from.SendMessage(text + " is already targeted for execution.");
                            return;
                        }

                        m_NationBoard.KillByNameList.Add(text);
                        from.SendMessage("You have successfully added " + text + " to the killing list.");
                        return;
                    }

                case 2:
                    {
                        if (m_NationBoard.KillByNameList.Contains(text))
                        {
                            m_NationBoard.KillByNameList.Remove(text);
                            from.SendMessage("You have successfully removed " + text + " from the killing list.");
                            return;
                        }

                        from.SendMessage(text + " was already not targeted for execution.");
                        return;
                    }
            }
        }
    }

    public class ArrestByNamePrompt : Prompt
    {
        private NationBoard m_NationBoard;
        private int m_Option;

        public ArrestByNamePrompt(NationBoard board, int option)
        {
            m_NationBoard = board;
            m_Option = option;
        }

        public override void OnResponse(Mobile from, string text)
        {
            switch (m_Option)
            {
                case 1:
                    {
                        if (m_NationBoard.ArrestByNameList.Contains(text))
                        {
                            from.SendMessage(text + " is already targeted for prison.");
                            return;
                        }

                        m_NationBoard.ArrestByNameList.Add(text);
                        from.SendMessage("You have successfully added " + text + " to the arresting list.");
                        return;
                    }

                case 2:
                    {
                        if (m_NationBoard.ArrestByNameList.Contains(text))
                        {
                            m_NationBoard.ArrestByNameList.Remove(text);
                            from.SendMessage("You have successfully removed " + text + " from the arresting list.");
                            return;
                        }

                        from.SendMessage(text + " was already not targeted for prison.");
                        return;
                    }
            }
        }
    }

    public class KillByNationPrompt : Prompt
    {
        private NationBoard m_NationBoard;
        private int m_Option;

        public KillByNationPrompt(NationBoard board, int option)
        {
            m_NationBoard = board;
            m_Option = option;
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == "Southern" || text == "Western" ||
                text == "Haluaroc" || text == "Mhordul" ||
                text == "Tirebladd" || text == "Northern") 
            {

                switch (m_Option)
                {
                    case 1:
                        {
                            if (m_NationBoard.KillByNationList.Contains(text))
                            {
                                from.SendMessage(text + " is already targeted for execution.");
                                return;
                            }

                            m_NationBoard.KillByNationList.Add(text);
                            from.SendMessage("You have successfully added " + text + " to the killing list.");
                            return;
                        }

                    case 2:
                        {
                            if (m_NationBoard.KillByNationList.Contains(text))
                            {
                                m_NationBoard.KillByNationList.Remove(text);
                                from.SendMessage("You have successfully removed " + text + " from the killing list.");
                                return;
                            }

                            from.SendMessage(text + " was already not targeted for execution.");
                            return;
                        }
                }
            }

            else
                from.SendMessage(text + " is not a valid nationality. Try Southern, Western, Haluaroc, Mhordul, Tirebladd or Northern.");
        }
    }

    public class ArrestByNationPrompt : Prompt
    {
        private NationBoard m_NationBoard;
        private int m_Option;

        public ArrestByNationPrompt(NationBoard board, int option)
        {
            m_NationBoard = board;
            m_Option = option;
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == "Southern" || text == "Western" ||
                text == "Haluaroc" || text == "Mhordul" ||
                text == "Tirebladd" || text == "Northern")
            {

                switch (m_Option)
                {
                    case 1:
                        {
                            if (m_NationBoard.ArrestByNationList.Contains(text))
                            {
                                from.SendMessage(text + " is already targeted for prison.");
                                return;
                            }

                            m_NationBoard.ArrestByNationList.Add(text);
                            from.SendMessage("You have successfully added " + text + " to the arresting list.");
                            return;
                        }

                    case 2:
                        {
                            if (m_NationBoard.ArrestByNationList.Contains(text))
                            {
                                m_NationBoard.ArrestByNationList.Remove(text);
                                from.SendMessage("You have successfully removed " + text + " from the arresting list.");
                                return;
                            }

                            from.SendMessage(text + " was already not targeted for prison.");
                            return;
                        }
                }
            }

            else
                from.SendMessage(text + " is not a valid nationality. Try Southern, Western, Haluaroc, Mhordul, Tirebladd or Northern.");
        }
    }

    public class KillByNationExceptionPrompt : Prompt
    {
        private NationBoard m_NationBoard;
        private int m_Option;

        public KillByNationExceptionPrompt(NationBoard board, int option)
        {
            m_NationBoard = board;
            m_Option = option;
        }

        public override void OnResponse(Mobile from, string text)
        {
            switch (m_Option)
            {
                case 1:
                    {
                        if (m_NationBoard.KillByNationExceptionList.Contains(text))
                        {
                            from.SendMessage(text + " is already an exception to execution.");
                            return;
                        }

                        m_NationBoard.KillByNationExceptionList.Add(text);
                        from.SendMessage("You have successfully added " + text + " to the killing exception list.");
                        return;
                    }

                case 2:
                    {
                        if (m_NationBoard.KillByNationExceptionList.Contains(text))
                        {
                            m_NationBoard.KillByNationExceptionList.Remove(text);
                            from.SendMessage("You have successfully removed " + text + " from the killing exception list.");
                            return;
                        }

                        from.SendMessage(text + " was already not an exception to execution.");
                        return;
                    }
            }
        }
    }

    public class ArrestByNationExceptionPrompt : Prompt
    {
        private NationBoard m_NationBoard;
        private int m_Option;

        public ArrestByNationExceptionPrompt(NationBoard board, int option)
        {
            m_NationBoard = board;
            m_Option = option;
        }

        public override void OnResponse(Mobile from, string text)
        {
            switch (m_Option)
            {
                case 1:
                    {
                        if (m_NationBoard.ArrestByNationExceptionList.Contains(text))
                        {
                            from.SendMessage(text + " is already an exception to prison.");
                            return;
                        }

                        m_NationBoard.ArrestByNationExceptionList.Add(text);
                        from.SendMessage("You have successfully added " + text + " to the jailing exception list.");
                        return;
                    }

                case 2:
                    {
                        if (m_NationBoard.ArrestByNationExceptionList.Contains(text))
                        {
                            m_NationBoard.ArrestByNationExceptionList.Remove(text);
                            from.SendMessage("You have successfully removed " + text + " from the jailing exception list.");
                            return;
                        }

                        from.SendMessage(text + " was already not an exception to prison.");
                        return;
                    }
            }
        }
    }

    public class NationBoardOptionsPrompt : Prompt
    {
        private NationBoard m_NationBoard;

        public NationBoardOptionsPrompt(NationBoard board)
        {
            m_NationBoard = board;
        }

        public override void OnResponse(Mobile from, string text)
        {
            if (text == "01" || text == "02" || text == "03")
                m_NationBoard.KillByNameMethod(from, text);

            else if (text == "11" || text == "12" || text == "13")
                m_NationBoard.ArrestByNameMethod(from, text);

            else if (text == "21" || text == "22" || text == "23")
                m_NationBoard.KillByNationMethod(from, text);

            else if (text == "41" || text == "42" || text == "43")
                m_NationBoard.ArrestByNationMethod(from, text);

            else if (text == "31" || text == "32" || text == "33")
                m_NationBoard.KillByNationExceptionMethod(from, text);

            else if (text == "51" || text == "52" || text == "53")
                m_NationBoard.ArrestByNationExceptionMethod(from, text);

            else
                from.SendMessage("You chose an invalid option.");
        }
    }

    #endregion
}