using System;
using Server;
using Server.Items;
using Server.TimeSystem;
using System.Collections.Generic;
using Server.Gumps;
using Server.Targets;
using Server.Targeting;

namespace Server.Mobiles
{
    [PropertyObject]
    public class OrderInfo
    {
        private string m_Speech;
        private int m_Proximity;
        private int m_Interval;
        private DateTime m_LastActivity;
        private KeyInfo m_Keys;

        private bool m_Aggressive;

        private string m_Issuant;
        private string m_DateOfIssue;

        private DateTime m_Created = DateTime.Now;
        private const int m_DecayDays = 3;

        public string Speech { get { return m_Speech; } set { m_Speech = value; } }
        public int Proximity { get { return m_Proximity; } set { m_Proximity = value; } }
        public int Interval { get { return m_Interval; } set { m_Interval = value; } }
        public DateTime LastActivity { get { return m_LastActivity; } set { m_LastActivity = value; } }
        public KeyInfo Keys { get { return m_Keys; } set { m_Keys = value; } }

        public bool Aggressive { get { return m_Aggressive; } set { m_Aggressive = value; } }

        public string Issuant { get { return m_Issuant; } set { m_Issuant = value; } }
        public string DateOfIssue { get { return m_DateOfIssue; } set { m_DateOfIssue = value; } }

        public DateTime Created { get { return m_Created; } set { m_Created = value; } }
        public int DecayDays { get { return m_DecayDays; } }

        public OrderInfo(string speech, int proximity, int interval, Mobile issuant)
        {
            m_Speech = speech;
            m_Proximity = proximity;
            m_Interval = interval;
            m_LastActivity = DateTime.Now;
            m_Created = DateTime.Now;
            m_Keys = new KeyInfo();
            m_Aggressive = false;
            m_Issuant = issuant.Name;
        }

        public static void OnSpeech(object o, Mobile trigger, SpeechEventArgs e)
        {
            if (o is BaseCreature && ((o as BaseCreature).ControlOrder == OrderType.Stop || (o as BaseCreature).ControlOrder == OrderType.Follow || (o as BaseCreature).Combatant != null))
                return;

            if (trigger.AccessLevel > AccessLevel.Player)
                return;

            if (o == trigger)
                return;

            if (o is BaseCreature && (o as BaseCreature).Orders != null && (o as BaseCreature).Orders.Count > 0)
            {
                BaseCreature creat = o as BaseCreature;

                foreach (OrderInfo order in creat.Orders)
                {
                    if (KeyInfo.ValidKey(trigger, order.Keys) && order.Keys.Keywords.Count > 0 && KeyInfo.ValidSpeech(e, order.Keys) && Soldier.VisionCheck(trigger, creat, order.Proximity))
                    {
                        if (order.LastActivity != null && order.LastActivity + TimeSpan.FromSeconds(order.Interval) < DateTime.Now)
                        {
                            if (order.Speech != null && order.Speech != "")
                            {
                                if (trigger.InRange(creat.Location, order.Proximity))
                                {
                                    int[] keywords = { -1 };
                                    creat.DoSpeech(order.Speech, keywords, Network.MessageType.Regular, creat.SpeechHue);
                                    //creat.Say(order.Speech);
                                    order.LastActivity = DateTime.Now;
                                }
                            }
                        }

                        if (order.Aggressive)
                        {
                            if (trigger.InRange(creat.Location, order.Proximity))
                            {
                                order.OrderToAttack(creat, trigger);
                            }
                        }
                    }
                }
            }
        }

        public static bool WasOrdered(SpeechEventArgs e, Mobile listener)
        {
            if (listener == null || listener.Deleted || !listener.Alive || listener.IsDeadBondedPet)
                return false;
            if (e.Mobile == null || e.Mobile.Deleted || !e.Mobile.Alive)
                return false;

            if (!(e.Mobile is PlayerMobile))
                return false;

            string thisSpeech = e.Speech.ToLower().Replace(".", "").Replace(",", "").Replace("!", "").Replace("?", "").Replace(";", "").Replace("-", "").Replace(":", "");
            string[] speech = thisSpeech.Split(' ');

            bool isOrder = false;
            foreach (string word in speech)
            {
                if (word == "order" || word == "orders")
                {
                    isOrder = true;
                    continue;
                }
            }

            if (isOrder)
            {
                string[] listenerName = listener.Name.ToLower().Split(' ');

                if (speech[0] == "all")
                    return true;
                else
                {
                    foreach (string word in speech)
                    {
                        foreach (string name in listenerName)
                        {
                            if (word == name)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public static void ExecuteOrders(object o, Mobile trigger)
        {
            if (o is BaseCreature && ((o as BaseCreature).ControlOrder == OrderType.Stop || (o as BaseCreature).ControlOrder == OrderType.Follow || (o as BaseCreature).Combatant != null))
                return;

            if (trigger.AccessLevel > AccessLevel.Player)
                return;

            if (o == trigger)
                return;

            if (o is BaseCreature && (o as BaseCreature).Orders != null && (o as BaseCreature).Orders.Count > 0)
            {
                BaseCreature creat = o as BaseCreature;

                foreach (OrderInfo order in creat.Orders)
                {
                    if (KeyInfo.ValidKey(trigger, order.Keys) && order.Keys.Keywords.Count == 0 && Soldier.VisionCheck(trigger, creat, order.Proximity ))
                    {
                            if (order.LastActivity != null && order.LastActivity + TimeSpan.FromSeconds(order.Interval) < DateTime.Now)
                            {
                                if (order.Speech != null && order.Speech != "")
                                {
                                    if (trigger.InRange(creat.Location, order.Proximity))
                                    {
                                        int[] keywords = { -1 };
                                        creat.DoSpeech(order.Speech, keywords, Network.MessageType.Regular, creat.SpeechHue);
                                        //creat.Say(order.Speech);
                                        order.LastActivity = DateTime.Now;
                                    }
                                }
                            }

                            if (order.Aggressive)
                            {
                                if (trigger.InRange(creat.Location, order.Proximity))
                                    order.OrderToAttack(creat, trigger);
                            }
                    }
                }
            }
        }

        public void OrderToAttack(Mobile attacker, Mobile target)
        {
            bool validTarget = true;

            if (target is PlayerMobile)
            {
                PlayerMobile targ = target as PlayerMobile;

                if (attacker is Soldier && (attacker as Soldier).Government != null && !(attacker as Soldier).Government.Deleted)
                {
                    Soldier sol = attacker as Soldier;

                    if (CustomGuildStone.IsGuildOfficer(targ, sol.Government))
                        validTarget = false;
                    else if (sol.Government.MilitaryPolicies.Exceptions.Contains(targ.Name))
                        validTarget = false;
                }
                else if (attacker is BaseCreature && (attacker as BaseCreature).Controlled && (attacker as BaseCreature).ControlMaster != null && !(attacker as BaseCreature).ControlMaster.Deleted)
                {
                    BaseCreature creat = attacker as BaseCreature;

                    if (creat.ControlMaster == target)
                        validTarget = false;
                    if (creat.Friends != null && creat.Friends.Count > 0 && creat.Friends.Contains(target))
                        validTarget = false;
                }
            }
            else if (target is BaseCreature)
            {
                BaseCreature targ = target as BaseCreature;

                if (attacker is Soldier && (attacker as Soldier).Government != null && !(attacker as Soldier).Government.Deleted)
                {
                    Soldier sol = attacker as Soldier;

                    if (targ is Soldier && (targ as Soldier).Government != null && !(targ as Soldier).Government.Deleted && targ.Government == sol.Government)
                        validTarget = false;

                    if (targ.Controlled && targ.ControlMaster != null && !targ.ControlMaster.Deleted)
                    {
                        if (targ.ControlMaster is PlayerMobile)
                        {
                            PlayerMobile master = targ.ControlMaster as PlayerMobile;

                            if (CustomGuildStone.IsGuildOfficer(master, sol.Government))
                                validTarget = false;
                        }
                    }
                }
                else if (attacker is BaseCreature && (attacker as BaseCreature).Controlled && (attacker as BaseCreature).ControlMaster != null && !(attacker as BaseCreature).ControlMaster.Deleted)
                {
                    BaseCreature creat = attacker as BaseCreature;

                    if (targ.Controlled && targ.ControlMaster != null && !targ.ControlMaster.Deleted)
                    {
                        if (targ.ControlMaster == creat.ControlMaster)
                            validTarget = false;
                        if (creat.Friends != null && creat.Friends.Count > 0 && creat.Friends.Contains(targ))
                            validTarget = false;
                    }
                }
            }

            if (validTarget && attacker is BaseCreature)
                (attacker as BaseCreature).AddAggressor = target;
        }

        public static void Serialize(GenericWriter writer, OrderInfo info)
        {
            writer.Write((int)1); //version

            #region Version 1
            writer.Write((DateTime)info.Created);
            #endregion

            #region Version 0
            writer.Write((string)info.Speech);
            writer.Write((int)info.Proximity);
            writer.Write((int)info.Interval);
            writer.Write((DateTime)info.LastActivity);
            KeyInfo.Serialize(writer, info.Keys);
            writer.Write((bool)info.Aggressive);
            writer.Write((string)info.Issuant);
            writer.Write((string)info.DateOfIssue);
            #endregion
        }

        public static void Deserialize(GenericReader reader, OrderInfo info)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        info.Created = reader.ReadDateTime();
                        goto case 0;
                    }
                case 0:
                    {
                        info.Speech = reader.ReadString();
                        info.Proximity = reader.ReadInt();
                        info.Interval = reader.ReadInt();
                        info.LastActivity = reader.ReadDateTime();
                        KeyInfo.Deserialize(reader, info.Keys);
                        info.Aggressive = reader.ReadBool();
                        info.Issuant = reader.ReadString();
                        info.DateOfIssue = reader.ReadString();
                        break;
                    }
            }
        }
    }

    [PropertyObject]
    public class KeyInfo
    {
        #region Private Variables & Get/Setters

        private List<string> m_Keywords = new List<string>();
        private List<string> m_KeyNames = new List<string>();
        private int m_KeyGender; // 0 = doesn't matter, 1 = male, 2 = female
        private int m_KeyTime; // 0 = none, 1 = day, 2 = night
        private List<Nation> m_KeyNations = new List<Nation>();
        private List<string> m_KeyGuilds = new List<string>();
        private int m_KeyRank;
        private string m_RankSign;

        //version 1
        private bool m_PlayerOnly;
        private bool m_NPCOnly;

        //version 2
        private List<string> m_ExceptionNames = new List<string>();

        public List<string> Keywords { get { return m_Keywords; } set { m_Keywords = value; } }
        public List<string> KeyNames { get { return m_KeyNames; } set { m_KeyNames = value; } }
        public int KeyGender { get { return m_KeyGender; } set { m_KeyGender = value; } }
        public int KeyTime { get { return m_KeyTime; } set { m_KeyTime = value; } }
        public List<Nation> KeyNations { get { return m_KeyNations; } set { m_KeyNations = value; } }
        public List<string> KeyGuilds { get { return m_KeyGuilds; } set { m_KeyGuilds = value; } }
        public int KeyRank { get { return m_KeyRank; } set { m_KeyRank = value; } }
        public string RankSign { get { return m_RankSign; } set { m_RankSign = value; } }

        //version 1
        public bool PlayerOnly
        {
            get { return m_PlayerOnly; }
            set
            {
                m_PlayerOnly = value;
                if (m_PlayerOnly == true)
                {
                    m_NPCOnly = false;
                }
            }
        }
        public bool NPCOnly
        {
            get { return m_NPCOnly; }
            set
            {
                m_NPCOnly = value;
                if (m_NPCOnly == true)
                {
                    m_PlayerOnly = false;
                }
            }
        }

        //version 2
        public List<string> ExceptionNames 
        { 
            get 
            {
                if (m_ExceptionNames == null)
                    m_ExceptionNames = new List<string>();
                return m_ExceptionNames; 
            } 
            set 
            { m_ExceptionNames = value; } 
        }

        #endregion

        public KeyInfo()
        {
            m_KeyGender = 0;
            m_KeyTime = 0;
            m_KeyRank = 0;
            m_RankSign = "=";
            m_PlayerOnly = false;
            m_NPCOnly = false;
        }

        #region Key Validation

        public bool CheckNames(Mobile m)
        {
            if (KeyNames.Count > 0)
            {
                foreach (string name in KeyNames)
                {
                    if (m.Name == name)
                        return true;
                }
                return false;
            }
            else
                return true;
        }

        public bool CheckGender(Mobile m)
        {
            if (KeyGender > 0)
            {
                if (m.BodyValue == 400 || m.BodyValue == 401)
                {
                    switch (KeyGender)
                    {
                        case 1:
                            {
                                if (!m.Female)
                                    return true;
                                break;
                            }
                        case 2:
                            {
                                if (m.Female)
                                    return true;
                                break;
                            }
                        default: return true; break;
                    }
                    return false;
                }
                return false;
            }
            else
                return true;
        }

        public bool CheckTime(Mobile m)
        {
            if (KeyTime > 0)
            {
                switch (KeyTime)
                {
                    case 1:
                        {
                            if (!TimeSystem.TimeEngine.IsNightTime(m))
                                return true;
                            break;
                        }
                    case 2:
                        {
                            if (TimeSystem.TimeEngine.IsNightTime(m))
                                return true;
                            break;
                        }
                    default: break;
                }
                return false;
            }

            return true;
        }

        public bool CheckNation(Mobile m)
        {
            if (KeyNations.Count > 0)
            {
                foreach (Nation nat in KeyNations)
                {
                    if (m is PlayerMobile)
                    {
                        if ((m as PlayerMobile).GetDisguisedNation() == nat)
                            return true;
                    }
                    else if (m is BaseCreature && (m.BodyValue == 400 || m.BodyValue == 401))
                    {
                        if ((m as BaseCreature).Nation == nat)
                            return true;
                    }
                    else
                        continue;
                }
                return false;
            }
            return true;
        }
 
        public bool CheckGuild(Mobile m)
        {
            if (KeyGuilds.Count > 0)
            {
                foreach (string guildName in KeyGuilds)
                {
                    CustomGuildStone guild = null;
                    foreach (CustomGuildStone g in CustomGuildStone.Guilds)
                    {
                        if (g.Name == guildName)
                        {
                            guild = g;
                            continue;
                        }
                    }

                    if (guild == null)
                    {
                        continue;
                    }

                    if (m is PlayerMobile)
                    {
                        PlayerMobile pm = m as PlayerMobile;

                        if (m_RankSign == "!=")
                        {
                            if (KeyRank < 1)
                                return true;
                            else
                            {
                                if (CustomGuildStone.IsGuildMember(pm, guild) && pm.CustomGuilds[guild].RankID != KeyRank)
                                    return true;
                                else if (!CustomGuildStone.IsGuildMember(pm, guild))
                                    return true;
                            }

                        }
                        else
                        {
                            if (CustomGuildStone.IsGuildMember(pm, guild) && pm.CustomGuilds[guild].ActiveTitle)
                            {
                                if (KeyRank < 1)
                                {
                                    return true;
                                }
                                else
                                {
                                    switch (m_RankSign)
                                    {
                                        case ">": // Greater Than This Rank
                                            {
                                                if (pm.CustomGuilds[guild].RankID > KeyRank)
                                                    return true;
                                                break;
                                            }
                                        case "<": // Less Than This Rank
                                            {
                                                if (pm.CustomGuilds[guild].RankID < KeyRank)
                                                    return true;
                                                break;
                                            }
                                        case "=": // Equal To This Rank
                                            {
                                                if (pm.CustomGuilds[guild].RankID == KeyRank)
                                                    return true;
                                                break;
                                            }
                                        case "!=": // Not Equal To This Rank
                                            {
                                                if (pm.CustomGuilds[guild].RankID != KeyRank)
                                                    return true;
                                                break;
                                            }
                                        default: break;
                                    }
                                }
                            }
                        }
                    }
                    else if (m is Soldier && (m as Soldier).Government != null && !(m as Soldier).Government.Deleted && (m as Soldier).Government.Name == guildName)
                    {
                        switch (m_RankSign)
                        {
                            case ">": // Greater Than This Rank
                                {
                                    if (KeyRank > 1)
                                        return false;
                                    else
                                        return true;
                                }
                            case "<": // Less Than This Rank
                                {
                                    return true;
                                }
                            case "=": // Equal To This Rank
                                {
                                    if (KeyRank == 1)
                                        return true;
                                    else
                                        return false;
                                }
                            case "!=": // Not Equal To This Rank
                                {
                                    if (KeyRank == 1)
                                        return true;
                                    else
                                        return false;
                                }
                            default: break;
                        }
                    }
                    else
                        continue;
                }
            }
            else
                return true;

            return false;
        }

        public bool IsException(Mobile m)
        {
            if (m_ExceptionNames == null)
                return false;

            if (m_ExceptionNames.Count < 1)
                return false;

            if (m_ExceptionNames.Contains(m.Name))
                return true;

            return false;
        }

        public static bool ValidKey(Mobile m, KeyInfo info)
        {
            if (m.AccessLevel > AccessLevel.Player)
                return false;

            if (info.PlayerOnly && !(m is PlayerMobile))
                return false;

            if (info.NPCOnly && !(m is BaseCreature))
                return false;

            return (!info.IsException(m) && info.CheckNames(m) && info.CheckGender(m) && info.CheckTime(m) && info.CheckNation(m) && info.CheckGuild(m));
        }

        public static bool ValidSpeech(SpeechEventArgs e, KeyInfo info)
        {
            if (e == null)
                return false;

            if (e.Mobile.AccessLevel > AccessLevel.Player)
                return false;

            string sp = e.Speech.ToLower().Replace(".", "").Replace(",", "").Replace("!", "").Replace("?", "").Replace(";", "").Replace("-", "").Replace(":", "");
            string[] speech = sp.Split(' ');

            foreach (string spokenWord in speech)
            {
                foreach (string keyword in info.Keywords)
                {
                    if (keyword.ToLower() == spokenWord.ToLower())
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion

        #region Serialization/Deserialization & Utilities
        public static void Serialize(GenericWriter writer, KeyInfo info)
        {
            writer.Write((int)2); //version

            //version 2
            writer.Write((int)info.ExceptionNames.Count);
            for (int i = 0; i < info.ExceptionNames.Count; i++)
                writer.Write((string)info.ExceptionNames[i]);

            //version 1
            writer.Write((bool)info.PlayerOnly);
            writer.Write((bool)info.NPCOnly);

            //version 0
            writer.Write((int)info.Keywords.Count);
            for (int i = 0; i < info.Keywords.Count; i++)
                writer.Write((string)info.Keywords[i]);

            writer.Write((int)info.KeyNames.Count);
            for (int i = 0; i < info.KeyNames.Count; i++)
                writer.Write((string)info.KeyNames[i]);

            writer.Write((int)info.KeyGender);

            writer.Write((int)info.KeyTime);

            writer.Write((int)info.KeyNations.Count);
            for (int i = 0; i < info.KeyNations.Count; i++)
                writer.Write((int)info.KeyNations[i]);

            writer.Write((int)info.KeyGuilds.Count);
            for (int i = 0; i < info.KeyGuilds.Count; i++)
                writer.Write((string)info.KeyGuilds[i]);

            writer.Write((int)info.KeyRank);

            writer.Write((string)info.RankSign);
        }

        public static void Deserialize(GenericReader reader, KeyInfo info)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 2:
                    {
                        info.ExceptionNames = new List<string>();
                        int count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                        {
                            string name = reader.ReadString();
                            info.ExceptionNames.Add(name);
                        }
                        goto case 1;
                    }
                case 1:
                    {
                        info.PlayerOnly = reader.ReadBool();
                        info.NPCOnly = reader.ReadBool();
                        goto case 0;
                    }
                case 0:
                    {
                        int count; // used to store the .Count for the various Lists.

                        info.Keywords = new List<string>();
                        count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                            info.Keywords.Add(reader.ReadString());

                        info.KeyNames = new List<string>();
                        count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                            info.KeyNames.Add(reader.ReadString());

                        info.KeyGender = reader.ReadInt();

                        info.KeyTime = reader.ReadInt();

                        info.KeyNations = new List<Nation>();
                        count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                            info.KeyNations.Add((Nation)reader.ReadInt());

                        info.KeyGuilds = new List<string>();
                        count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                            info.KeyGuilds.Add(reader.ReadString());

                        info.KeyRank = reader.ReadInt();

                        info.RankSign = reader.ReadString();

                        break;

                    }
            }
        }
        #endregion
    }

    public class AddOrderGump : Gump
    {
        private enum AddOrderGumpButton
        {
            Cancel,
            Okay,
            Options,
            Aggressive
        }

        private PlayerMobile m_Viewer;
        private BaseCreature m_Ordered;
        private OrderInfo m_ThisOrder;

        public AddOrderGump(PlayerMobile viewer, BaseCreature ordered) : this(viewer, ordered, new OrderInfo("", 3, 180, viewer))
        {
            
        }

        public AddOrderGump(PlayerMobile viewer, BaseCreature ordered, OrderInfo order) : base(0, 0)
        {
            m_Viewer = viewer;
            m_Ordered = ordered;
            m_ThisOrder = order;

            InitialSetup();
        }

        public void InitialSetup()
        {
            m_Viewer.CloseGump(typeof(AddOrderGump));
            m_Viewer.CloseGump(typeof(SetKeyInfoGump));
            m_Viewer.CloseGump(typeof(ViewOrdersGump));
            m_Viewer.CloseGump(typeof(KeyExceptionsGump));

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddBackground(194, 152, 436, 287, 9270); // Large blue background.
            AddBackground(204, 171, 408, 223, 9390); // Smaller scroll background.
            AddLabel(232, 176, 0, m_Ordered.Name.ToString());

            AddLabel(233, 207, 0, "Proximity Activation:");
            AddLabel(233, 231, 0, "Activation Interval (sec.)");
            AddLabel(233, 251, 0, "Aggressive?:");
            AddLabel(233, 271, 0, "Text:");

            AddTextEntry(376, 207, 200, 20, 0, 1, m_ThisOrder.Proximity.ToString());
            AddTextEntry(404, 232, 171, 20, 0, 2, m_ThisOrder.Interval.ToString());
            if(m_ThisOrder.Aggressive)
                AddButton(322, 255, 10742, 10740, (int)AddOrderGumpButton.Aggressive, GumpButtonType.Reply, 0);
            else
                AddButton(322, 255, 10740, 10742, (int)AddOrderGumpButton.Aggressive, GumpButtonType.Reply, 0);
            AddTextEntry(235, 293, 345, 66, 0, 3, m_ThisOrder.Speech.ToString());

            if(m_ThisOrder.Issuant != null && m_ThisOrder.Issuant != "")
                AddLabel(225, 372, 0, "Issued By: " + m_ThisOrder.Issuant);

            AddButton(523, 371, 2006, 2007, (int)AddOrderGumpButton.Options, GumpButtonType.Reply, 0);
            AddButton(340, 402, 243, 241, (int)AddOrderGumpButton.Cancel, GumpButtonType.Reply, 0);
            AddButton(425, 402, 249, 248, (int)AddOrderGumpButton.Okay, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            if (m_Viewer == null || m_Viewer != sender.Mobile)
                return;

            if (m_Ordered == null || m_Ordered.Deleted || m_Ordered.IsDeadBondedPet)
                return;

            if (info.ButtonID != (int)AddOrderGumpButton.Cancel)
            {
                int num = 0;

                if (ValidateInt(m_Viewer, info.GetTextEntry(1).Text, "Proximity Activation", ref num))
                    m_ThisOrder.Proximity = num;

                if (ValidateInt(m_Viewer, info.GetTextEntry(2).Text, "Activation Interval", ref num))
                    m_ThisOrder.Interval = num;

                if (ValidateString(m_Viewer, info.GetTextEntry(3).Text, "Text"))
                    m_ThisOrder.Speech = info.GetTextEntry(3).Text;

                if (info.ButtonID == (int)AddOrderGumpButton.Aggressive)
                {
                    if (m_Ordered is Soldier || m_Ordered.Controlled)
                    {
                        if (m_ThisOrder.Aggressive)
                            m_ThisOrder.Aggressive = false;
                        else
                            m_ThisOrder.Aggressive = true;
                    }
                    else
                    {
                        m_ThisOrder.Aggressive = false;
                        m_ThisOrder.Created = DateTime.Now;
                    }
                        
                    m_Viewer.SendGump(new AddOrderGump(m_Viewer, m_Ordered, m_ThisOrder));
                    return;
                }

                if (info.ButtonID == (int)AddOrderGumpButton.Options)
                {
                    m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_ThisOrder));
                    return;
                }
                else
                {
                    if (!m_Ordered.Orders.Contains(m_ThisOrder))
                    {
                        m_ThisOrder.Issuant = m_Viewer.Name;
                        m_Ordered.Orders.Add(m_ThisOrder);
                    }
                    int sendOrder = 0;
                    for (int i = 0; i < m_Ordered.Orders.Count; i++)
                    {
                        if (m_Ordered.Orders[i] == m_ThisOrder)
                        {
                            sendOrder = i;
                            continue;
                        }
                    }
                    m_Viewer.SendGump(new ViewOrdersGump(m_Viewer, m_Ordered, sendOrder));
                    return;
                }
            }
            else
            {
                m_Viewer.SendGump(new ViewOrdersGump(m_Viewer, m_Ordered));
                return;
            }

            base.OnResponse(sender, info);
        }

        public bool ValidateString(PlayerMobile m, string st, string name)
        {
            if (String.IsNullOrEmpty(st))
            {
                m.SendMessage("Field \"" + name + "\" cannot be empty.");
                return false;
            }

            return true;
        }

        public bool ValidateInt(PlayerMobile m, string st, string name, ref int parsed)
        {
            if (!int.TryParse(st, out parsed))
            {
                m.SendMessage("Field \"" + name + "\" needs to be a valid number.");
                return false;
            }

            return true;
        }
    }

    public class SetKeyInfoGump : Gump
    {
        private enum KeyInfoButton
        {
            Cancel,
            Okay,
            Exceptions,
            SexNone,
            SexMale,
            SexFemale,
            TimeNone,
            TimeDay,
            TimeNight,
            RankGreater,
            RankLess,
            RankDoesNotEqual,
            RankEquals,
            PlayerOnly,
            NPCOnly,
            AddRemWord,
            WordScrollUp,
            WordScrollDown,
            AddRemName,
            NameScrollUp,
            NameScrollDown,
            AddRemGuild,
            GuildScrollUp,
            GuildScrollDown,
            AddRemNation
        }

        private enum KeyInfoText
        {
            Rank,
            AddRemoveWord,
            AddRemoveName,
            AddRemoveGuild
        }

        #region Variables
        private PlayerMobile m_Viewer;
        private BaseCreature m_Ordered;
        private OrderInfo m_Order;
        private KeyInfo m_Keys;

        private int m_WordStart;
        private int m_NameStart;
        private int m_GuildStart;
        private const int m_MaxEntries = 7;
        #endregion

        public SetKeyInfoGump(PlayerMobile viewer, BaseCreature ordered, OrderInfo order)
            : this(viewer, ordered, order, order.Keys != null ? order.Keys : new KeyInfo())
        {

        }

        public SetKeyInfoGump(PlayerMobile viewer, BaseCreature ordered, OrderInfo order, KeyInfo keys)
            : this(viewer, ordered, order, keys, 0, 0, 0)
        {
            
        }

        public SetKeyInfoGump(PlayerMobile viewer, BaseCreature ordered, OrderInfo order, KeyInfo keys, int wordStart, int nameStart, int guildStart)
            : base(0, 0)
        {
            m_Viewer = viewer;
            m_Ordered = ordered;
            m_Order = order;
            m_Keys = keys;

            m_WordStart = wordStart;
            m_NameStart = nameStart;
            m_GuildStart = guildStart;

            InitialSetup();
        }

        public void InitialSetup()
        {
            m_Viewer.CloseGump(typeof(AddOrderGump));
            m_Viewer.CloseGump(typeof(SetKeyInfoGump));
            m_Viewer.CloseGump(typeof(ViewOrdersGump));
            m_Viewer.CloseGump(typeof(KeyExceptionsGump));

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            // pane 1
            AddBackground(262, 23, 287, 303, 9270);
            AddBackground(281, 39, 249, 40, 9350);
            AddLabel(401 - (m_Ordered.Name.Length * 4), 49, 0, m_Ordered.Name);
            AddButton(281, 89, 243, 241, (int)KeyInfoButton.Cancel, GumpButtonType.Reply, 0);
            AddButton(466, 89, 249, 248, (int)KeyInfoButton.Okay, GumpButtonType.Reply, 0);
            AddButton(350, 89, 2445, 2445, (int)KeyInfoButton.Exceptions, GumpButtonType.Reply, 0);
            AddLabel(373, 89, 0, "Exceptions");

            #region Sex Options
            AddBackground(281, 121, 251, 40, 9350);
            AddLabel(288, 131, 0, "Sex:");
            AddButton(322, 131, m_Keys.KeyGender == 0 ? 211 : 210, m_Keys.KeyGender == 0 ? 210 : 211, (int)KeyInfoButton.SexNone, GumpButtonType.Reply, 0);
            AddLabel(348, 131, m_Keys.KeyGender == 0 ? 133 : 0, "None");
            AddButton(384, 131, m_Keys.KeyGender == 1 ? 211 : 210, m_Keys.KeyGender == 1 ? 210 : 211, (int)KeyInfoButton.SexMale, GumpButtonType.Reply, 0);
            AddLabel(408, 131, m_Keys.KeyGender == 1 ? 133 : 0, "Male");
            AddButton(444, 131, m_Keys.KeyGender == 2 ? 211 : 210, m_Keys.KeyGender == 2 ? 210 : 211, (int)KeyInfoButton.SexFemale, GumpButtonType.Reply, 0);
            AddLabel(469, 131, m_Keys.KeyGender == 2 ? 133 : 0, "Female");
            #endregion

            #region Time Options
            AddBackground(281, 171, 251, 40, 9350);
            AddLabel(288, 181, 0, "Time:");
            AddButton(325, 181, m_Keys.KeyTime == 0 ? 211 : 210, m_Keys.KeyTime == 0 ? 210 : 211, (int)KeyInfoButton.TimeNone, GumpButtonType.Reply, 0);
            AddLabel(348, 181, m_Keys.KeyTime == 0 ? 133 : 0, "None");
            AddButton(386, 181, m_Keys.KeyTime == 1 ? 211 : 210, m_Keys.KeyTime == 1 ? 210 : 211, (int)KeyInfoButton.TimeDay, GumpButtonType.Reply, 0);
            AddLabel(410, 181, m_Keys.KeyTime == 1 ? 133 : 0, "Day");
            AddButton(444, 181, m_Keys.KeyTime == 2 ? 211 : 210, m_Keys.KeyTime == 2 ? 210 : 211, (int)KeyInfoButton.TimeNight, GumpButtonType.Reply, 0);
            AddLabel(470, 181, m_Keys.KeyTime == 2 ? 133 : 0, "Night");
            #endregion

            #region Rank Options

            AddBackground(281, 221, 251, 40, 9350);
            AddLabel(290, 232, 0, "Rank:");
            AddButton(337, 231, 210, 210, (int)KeyInfoButton.RankGreater, GumpButtonType.Reply, 0);
            AddLabel(342, 231, m_Keys.RankSign == ">" ? 133 : 0, ">");

            AddButton(372, 231, 210, 210, (int)KeyInfoButton.RankLess, GumpButtonType.Reply, 0);
            AddLabel(377, 231, m_Keys.RankSign == "<" ? 133 : 0, "<");

            AddButton(407, 231, 210, 210, (int)KeyInfoButton.RankDoesNotEqual, GumpButtonType.Reply, 0);
            AddLabel(412, 231, m_Keys.RankSign == "!=" ? 133 : 0, "!=");

            AddButton(442, 231, 210, 210, (int)KeyInfoButton.RankEquals, GumpButtonType.Reply, 0);
            AddLabel(447, 231, m_Keys.RankSign == "=" ? 133 : 0, "=");

            AddTextEntry(483, 231, 30, 20, 0, (int)KeyInfoText.Rank, m_Keys.KeyRank.ToString());

            #endregion

            #region Player/NPC Activation Control
            AddBackground(281, 271, 251, 40, 9350);

            AddButton(292, 282, m_Keys.PlayerOnly ? 211 : 210, m_Keys.PlayerOnly ? 210 : 211, (int)KeyInfoButton.PlayerOnly, GumpButtonType.Reply, 0);
            AddLabel(322, 282, m_Keys.PlayerOnly ? 133 : 0, "Players Only");

            AddButton(422, 282, m_Keys.NPCOnly ? 211 : 210, m_Keys.NPCOnly ? 210 : 211, (int)KeyInfoButton.NPCOnly, GumpButtonType.Reply, 0);
            AddLabel(455, 282, m_Keys.NPCOnly ? 133 : 0, "NPCs Only");
            #endregion

            // pane 2

            int m_Y = 0;
            int m_X = 0;

            AddBackground(98, 318, 609, 266, 9270);

            #region Keywords

            AddBackground(118, 333, 134, 25, 9200);
            AddLabel(123, 336, 0, "Add/Rem. Word");
            AddButton(229, 339, 2117, 2118, (int)KeyInfoButton.AddRemWord, GumpButtonType.Reply, 0);

            AddBackground(117, 362, 138, 36, 9350);
            AddTextEntry(123, 368, 124, 20, 961, (int)KeyInfoText.AddRemoveWord, "");

            AddBackground(117, 401, 137, 162, 9350);
            AddImageTiled(243, 404, 11, 157, 2712);
            AddButton(241, 400, 250, 251, (int)KeyInfoButton.WordScrollUp, GumpButtonType.Reply, 0);
            AddButton(241, 542, 252, 253, (int)KeyInfoButton.WordScrollDown, GumpButtonType.Reply, 0);

            if (m_Keys.Keywords != null && m_Keys.Keywords.Count > 0)
            {
                m_Y = 405;
                for (int i = m_WordStart * m_MaxEntries; i < (m_WordStart * m_MaxEntries) + m_MaxEntries && i < m_Keys.Keywords.Count; i++)
                {
                    AddLabel(124, m_Y, 0, m_Keys.Keywords[i]);
                    m_Y += 20;
                }
            }

            #endregion

            #region Names

            AddBackground(266, 333, 134, 25, 9200);
            AddLabel(270, 336, 0, "Add/Rem. Name");
            AddButton(379, 339, 2117, 2118, (int)KeyInfoButton.AddRemName, GumpButtonType.Reply, 0);

            AddBackground(265, 361, 136, 36, 9350);
            AddTextEntry(272, 368, 119, 20, 961, (int)KeyInfoText.AddRemoveName, "");

            AddBackground(264, 401, 137, 164, 9350);
            AddImageTiled(391, 404, 11, 157, 2712);
            AddButton(389, 400, 250, 251, (int)KeyInfoButton.NameScrollUp, GumpButtonType.Reply, 0);
            AddButton(389, 542, 252, 253, (int)KeyInfoButton.NameScrollDown, GumpButtonType.Reply, 0);

            if (m_Keys.KeyNames != null && m_Keys.KeyNames.Count > 0)
            {
                m_Y = 405;
                for (int i = m_NameStart * m_MaxEntries; i < (m_NameStart * m_MaxEntries) + m_MaxEntries && i < m_Keys.KeyNames.Count; i++)
                {
                    AddLabel(271, m_Y, 0, m_Keys.KeyNames[i]);
                    m_Y += 20;
                }
            }

            #endregion

            #region Guilds

            AddBackground(409, 333, 134, 25, 9200);
            AddLabel(414, 336, 0, "Add/Rem. Guild");
            AddButton(520, 339, 2117, 2118, (int)KeyInfoButton.AddRemGuild, GumpButtonType.Reply, 0);

            AddBackground(408, 361, 136, 36, 9350);
            AddTextEntry(415, 368, 119, 20, 961, (int)KeyInfoText.AddRemoveGuild, "");

            AddBackground(409, 401, 137, 164, 9350);
            AddImageTiled(536, 404, 11, 157, 2712);
            AddButton(534, 400, 250, 251, (int)KeyInfoButton.GuildScrollUp, GumpButtonType.Reply, 0);
            AddButton(534, 542, 252, 253, (int)KeyInfoButton.GuildScrollDown, GumpButtonType.Reply, 0);

            if (m_Keys.KeyGuilds != null && m_Keys.KeyGuilds.Count > 0)
            {
                m_Y = 405;
                for (int i = m_GuildStart * m_MaxEntries; i < (m_GuildStart * m_MaxEntries) + m_MaxEntries && i < m_Keys.KeyGuilds.Count; i++)
                {
                    AddLabel(414, m_Y, 0, m_Keys.KeyGuilds[i]);
                    m_Y += 20;
                }
            }
            #endregion

            #region Nations

            AddBackground(553, 337, 137, 231, 9350);
            m_Y = 343;
            m_X = 561;
            for (int i = 1; i < 10; i++)
            {
                AddButton(m_X, m_Y, m_Keys.KeyNations.Contains((Nation)i) ? 211 : 210, m_Keys.KeyNations.Contains((Nation)i) ? 210 : 211, (int)KeyInfoButton.AddRemNation + i, GumpButtonType.Reply, 0);
                AddLabel(m_X + 27, m_Y, m_Keys.KeyNations.Contains((Nation)i) ? 133 : 0, ((Nation)i).ToString());
                m_Y += 25;
            }

            #endregion
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            if (m_Viewer == null || m_Viewer != sender.Mobile)
                return;

            if (m_Ordered == null || m_Ordered.Deleted || m_Ordered.IsDeadBondedPet)
                return;

            if (m_Keys == null)
                return;

            switch (info.ButtonID)
            {

                case (int)KeyInfoButton.Okay:
                    {
                        int val = 0;
                        if (ValidateInt(m_Viewer, info.GetTextEntry((int)KeyInfoText.Rank).Text, "Rank", ref val))
                            m_Keys.KeyRank = val;
                        m_Order.Keys = m_Keys;
                        m_Viewer.SendGump(new AddOrderGump(m_Viewer, m_Ordered, m_Order));
                        return;
                    }
                case (int)KeyInfoButton.Exceptions:
                    {
                        m_Viewer.SendGump(new KeyExceptionsGump(m_Viewer, m_Ordered, m_Order, m_Keys));
                        return;
                    }
                case (int)KeyInfoButton.SexNone:
                    {
                        m_Keys.KeyGender = 0;
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.SexMale:
                    {
                        if (m_Keys.KeyGender == 1)
                        {
                            m_Keys.KeyGender = 0;
                        }
                        else
                        {
                            m_Keys.KeyGender = 1;
                        }

                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.SexFemale:
                    {
                        if (m_Keys.KeyGender == 2)
                        {
                            m_Keys.KeyGender = 0;
                        }
                        else
                        {
                            m_Keys.KeyGender = 2;
                        }

                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.TimeNone:
                    {
                        m_Keys.KeyTime = 0;
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.TimeDay:
                    {
                        if (m_Keys.KeyTime == 1)
                        {
                            m_Keys.KeyTime = 0;
                        }
                        else
                        {
                            m_Keys.KeyTime = 1;
                        }

                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.TimeNight:
                    {
                        if (m_Keys.KeyTime == 2)
                        {
                            m_Keys.KeyTime = 0;
                        }
                        else
                        {
                            m_Keys.KeyTime = 2;
                        }

                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.RankGreater:
                    {
                        int val = 0;
                        if (ValidateInt(m_Viewer, info.GetTextEntry((int)KeyInfoText.Rank).Text, "Rank", ref val))
                            m_Keys.KeyRank = val;

                        m_Keys.RankSign = ">";
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.RankLess:
                    {
                        int val = 0;
                        if (ValidateInt(m_Viewer, info.GetTextEntry((int)KeyInfoText.Rank).Text, "Rank", ref val))
                            m_Keys.KeyRank = val;

                        m_Keys.RankSign = "<";
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.RankDoesNotEqual:
                    {
                        int val = 0;
                        if (ValidateInt(m_Viewer, info.GetTextEntry((int)KeyInfoText.Rank).Text, "Rank", ref val))
                            m_Keys.KeyRank = val;

                        m_Keys.RankSign = "!=";
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.RankEquals:
                    {
                        int val = 0;
                        if (ValidateInt(m_Viewer, info.GetTextEntry((int)KeyInfoText.Rank).Text, "Rank", ref val))
                            m_Keys.KeyRank = val;

                        m_Keys.RankSign = "=";
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.PlayerOnly:
                    {
                        if (m_Keys.PlayerOnly)
                            m_Keys.PlayerOnly = false;
                        else
                            m_Keys.PlayerOnly = true;
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.NPCOnly:
                    {
                        if (m_Keys.NPCOnly)
                            m_Keys.NPCOnly = false;
                        else
                            m_Keys.NPCOnly = true;
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.AddRemWord:
                    {
                        string keyword = info.GetTextEntry((int)KeyInfoText.AddRemoveWord).Text;
                        if (keyword != null && keyword != "")
                        {
                            if (m_Keys.Keywords.Contains(keyword))
                            {
                                m_Keys.Keywords.Remove(keyword);
                                m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                                return;
                            }
                            else
                            {
                                if(keyword.Contains(" "))
                                {
                                    m_Viewer.SendMessage("INVALID KEYWORD: Keywords cannot contain a space.");
                                    m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                                }
                                else
                                {
                                    m_Keys.Keywords.Add(keyword);
                                    m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                                    return;
                                }
                            }
                        }
                        else
                        {
                            m_Viewer.SendMessage("Error: Invalid text entry in Keyword Entry.");
                            m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        }

                        return;
                    }
                case (int)KeyInfoButton.AddRemName:
                    {
                        string keyname = info.GetTextEntry((int)KeyInfoText.AddRemoveName).Text;
                        if (keyname != null && keyname != "")
                        {
                            if (m_Keys.KeyNames.Contains(keyname))
                            {
                                m_Keys.KeyNames.Remove(keyname);
                                m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                                return;
                            }
                            else
                            {
                                m_Keys.KeyNames.Add(keyname);
                                m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                                return;
                            }
                        }
                        else
                        {
                            m_Viewer.SendMessage("Error: Invalid text entry in Key Name Entry.");
                            m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        }

                        return;
                    }
                case (int)KeyInfoButton.AddRemGuild:
                    {
                        string keyguild = info.GetTextEntry((int)KeyInfoText.AddRemoveGuild).Text;
                        bool isGuild = false;
                        if (keyguild != null && keyguild != "")
                        {
                            foreach (CustomGuildStone g in CustomGuildStone.Guilds)
                            {
                                if (g.Name != null)
                                {
                                    if (g.Name == keyguild)
                                    {
                                        isGuild = true;
                                        continue;
                                    }
                                }
                            }
                        }

                        if (isGuild)
                        {
                            if (m_Keys.KeyGuilds.Contains(keyguild))
                            {
                                m_Keys.KeyGuilds.Remove(keyguild);
                                m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                                return;
                            }
                            else
                            {
                                m_Keys.KeyGuilds.Add(keyguild);
                                m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                                return;
                            }
                        }
                        else
                        {
                            if (m_Keys.KeyGuilds.Contains(keyguild))
                            {
                                m_Keys.KeyGuilds.Remove(keyguild);
                                m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                                return;
                            }
                            else
                            {
                                m_Viewer.SendMessage("ERROR: Invalid organization name for Key Guild Entry.");
                                m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                            }
                        }
                        return;
                    }
                case (int)KeyInfoButton.WordScrollUp:
                    {
                        if (m_WordStart > 0)
                            m_WordStart--;
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.WordScrollDown:
                    {
                        if (m_WordStart * m_MaxEntries < m_Keys.Keywords.Count)
                            m_WordStart++;
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.NameScrollUp:
                    {
                        if (m_NameStart > 0)
                            m_NameStart--;
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.NameScrollDown:
                    {
                        if (m_NameStart * m_MaxEntries < m_Keys.KeyNames.Count)
                            m_NameStart++;
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.GuildScrollUp:
                    {
                        if (m_GuildStart > 0)
                            m_GuildStart--;
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.GuildScrollDown:
                    {
                        if (m_GuildStart * m_MaxEntries < m_Keys.KeyGuilds.Count)
                            m_GuildStart++;
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                        return;
                    }
                case (int)KeyInfoButton.Cancel:
                    {
                        m_Viewer.SendGump(new AddOrderGump(m_Viewer, m_Ordered, m_Order));
                        return;
                    }
            }

            if (info.ButtonID > (int)KeyInfoButton.AddRemNation)
            {
                Nation keyNation = (Nation)(info.ButtonID - (int)KeyInfoButton.AddRemNation);
                if (m_Keys.KeyNations.Contains(keyNation))
                {
                    m_Keys.KeyNations.Remove(keyNation);
                    m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                    return;
                }
                else
                {
                    m_Keys.KeyNations.Add(keyNation);
                    m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Keys, m_WordStart, m_NameStart, m_GuildStart));
                    return;
                }
            }
            base.OnResponse(sender, info);
        }

        public bool ValidateInt(PlayerMobile m, string st, string name, ref int parsed)
        {
            if (!int.TryParse(st, out parsed))
            {
                m.SendMessage("Field \"" + name + "\" needs to be a valid number.");
                return false;
            }

            return true;
        }
    }

    public class KeyExceptionsGump : Gump
    {
        private enum ExceptionButton
        {
            Cancel,
            Okay,
            AddRemove,
            ScrollUp,
            ScrollDown
        }

        private PlayerMobile m_Viewer;
        private BaseCreature m_Ordered;
        private OrderInfo m_Order;
        private KeyInfo m_Key;

        private const int m_MaxNames = 10;

        private int m_Current;

        public KeyExceptionsGump(PlayerMobile viewer, BaseCreature ordered, OrderInfo order, KeyInfo key) : this(viewer, ordered, order, key, 0) { }

        public KeyExceptionsGump(PlayerMobile viewer, BaseCreature ordered, OrderInfo order, KeyInfo key, int currentPage)
            : base(0, 0)
        {
            m_Viewer = viewer;
            m_Ordered = ordered;
            m_Order = order;
            m_Key = key;

            m_Current = currentPage;

            InitialSetup();
        }

        public void InitialSetup()
        {
            m_Viewer.CloseGump(typeof(AddOrderGump));
            m_Viewer.CloseGump(typeof(SetKeyInfoGump));
            m_Viewer.CloseGump(typeof(ViewOrdersGump));
            m_Viewer.CloseGump(typeof(KeyExceptionsGump));

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);

            AddBackground(296, 127, 218, 382, 9270);
            AddBackground(316, 142, 182, 36, 9350);
            AddLabel(425 - (m_Ordered.Name.Length * 4), 150, 0, m_Ordered.Name);
            AddBackground(315, 184, 182, 36, 9350);
            AddTextEntry(320, 192, 169, 20, 0, 0, "");
            AddBackground(315, 224, 180, 25, 9200);
            AddLabel(329, 227, 0, "Add/Remove Name");
            AddButton(455, 226, 4005, 4007, (int)ExceptionButton.AddRemove, GumpButtonType.Reply, 0);
            AddBackground(314, 257, 184, 208, 9350);
            AddImageTiled(486, 263, 11, 195, 2712);
            AddButton(484, 257, 250, 251, (int)ExceptionButton.ScrollUp, GumpButtonType.Reply, 0);
            AddButton(484, 445, 252, 253, (int)ExceptionButton.ScrollDown, GumpButtonType.Reply, 0);
            AddButton(373, 471, 249, 248, (int)ExceptionButton.Okay, GumpButtonType.Reply, 0);

            if (m_Key.ExceptionNames == null)
                m_Key.ExceptionNames = new List<string>();

            int m_X = 320;
            int m_Y = 260;
            int m_AddY = 20;

            for (int i = m_Current * m_MaxNames; i < (m_Current * m_MaxNames) + m_MaxNames && i < m_Key.ExceptionNames.Count; i++)
            {
                AddLabel(m_X, m_Y, 0, m_Key.ExceptionNames[i]);
                m_Y += m_AddY;
            }
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            if (m_Viewer == null || m_Viewer != sender.Mobile)
                return;

            if (m_Ordered == null || m_Ordered.Deleted || m_Ordered.IsDeadBondedPet)
                return;

            if (m_Order == null)
                return;

            if (m_Key == null)
                return;

            switch (info.ButtonID)
            {
                case (int)ExceptionButton.Cancel:
                    {
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order, m_Key));
                        return;
                    }
                case (int)ExceptionButton.Okay:
                    {
                        m_Order.Keys = m_Key;
                        m_Viewer.SendGump(new SetKeyInfoGump(m_Viewer, m_Ordered, m_Order));
                        return;
                    }
                case (int)ExceptionButton.AddRemove:
                    {
                        string Text = info.GetTextEntry(0).Text;
                        if (Text != null && Text != "")
                        {
                            if (m_Key.ExceptionNames.Contains(Text))
                            {
                                m_Key.ExceptionNames.Remove(Text);
                            }
                            else
                            {
                                m_Key.ExceptionNames.Add(Text);
                            }
                        }
                        m_Viewer.SendGump(new KeyExceptionsGump(m_Viewer, m_Ordered, m_Order, m_Key, m_Current));
                        return;
                    }
                case (int)ExceptionButton.ScrollUp:
                    {
                        if (m_Current > 0)
                        {
                            m_Current--;
                        }
                        m_Viewer.SendGump(new KeyExceptionsGump(m_Viewer, m_Ordered, m_Order, m_Key, m_Current));
                        return;
                    }
                case (int)ExceptionButton.ScrollDown:
                    {
                        if (m_Current * m_MaxNames < m_Key.ExceptionNames.Count)
                            m_Current++;
                        m_Viewer.SendGump(new KeyExceptionsGump(m_Viewer, m_Ordered, m_Order, m_Key, m_Current));
                        return;
                    }
                default: return;
            }

            base.OnResponse(sender, info);
        }
    }

    public class ViewOrdersGump : Gump
    {
        private enum OrderButton
        {
            Exit,
            DeleteOrder,
            NextOrder,
            PreviousOrder,
            AddOrder,
            EditOrder,
            CopyOrder
        }

        private PlayerMobile m_Viewer;
        private BaseCreature m_Ordered;
        private int m_CurrentOrder;

        private List<string> m_Text;

        public ViewOrdersGump(PlayerMobile viewer, BaseCreature ordered)
            : this(viewer, ordered, 0)
        {

        }

        public ViewOrdersGump(PlayerMobile viewer, BaseCreature ordered, int currentOrder)
            : base(0, 0)
        {
            m_Viewer = viewer;
            m_Ordered = ordered;
            m_CurrentOrder = currentOrder;

            InitialSetup();
        }

        public void InitialSetup()
        {
            m_Viewer.CloseGump(typeof(AddOrderGump));
            m_Viewer.CloseGump(typeof(SetKeyInfoGump));
            m_Viewer.CloseGump(typeof(ViewOrdersGump));
            m_Viewer.CloseGump(typeof(KeyExceptionsGump));

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            if (m_Ordered == null || m_Ordered.Deleted || m_Ordered.IsDeadBondedPet || m_Ordered.IsDeadPet)
                return;

            if (m_CurrentOrder > m_Ordered.Orders.Count - 1 || m_CurrentOrder < 0)
            {
                if (m_Ordered.Orders.Count >= 1)
                    m_CurrentOrder = 0;
            }

            AddPage(0);

            AddBackground(213, 65, 369, 260, 9270);

            #region Overhead Info
            AddBackground(230, 80, 332, 49, 3500);
            AddButton(252, 97, 9766, 9767, (int)OrderButton.PreviousOrder, GumpButtonType.Reply, 0);
            AddButton(534, 97, 9762, 9763, (int)OrderButton.NextOrder, GumpButtonType.Reply, 0);
            AddLabel(277, 96, 0, m_Ordered.Name.ToString());
            AddLabel(487, 96, 0, "#" + (m_CurrentOrder + 1).ToString());
            #endregion

            if (m_Ordered.Orders.Count > 0)
            {
                #region Writing Basic Order Information
                AddBackground(229, 133, 339, 155, 3500);
                AddLabel(246, 146, 0, "Proximity: " + m_Ordered.Orders[m_CurrentOrder].Proximity.ToString());
                AddLabel(359, 145, 0, "Interval: " + m_Ordered.Orders[m_CurrentOrder].Interval.ToString() + "s.");
                if (m_Ordered.Orders[m_CurrentOrder].Aggressive)
                    AddLabel(479, 146, 133, "Aggressive");

                string[] orderSpeech = m_Ordered.Orders[m_CurrentOrder].Speech.Split(' ');
                string currentLine = "";
                m_Text = new List<string>();
                foreach (string word in orderSpeech)
                {
                    currentLine += word + " ";
                    if (currentLine.Length >= 50 || word == orderSpeech[orderSpeech.Length - 1])
                    {
                        m_Text.Add(currentLine);
                        currentLine = "";
                    }
                }

                int m_X = 245;
                int m_Y = 170;
                foreach (string line in m_Text)
                {
                    AddLabel(m_X, m_Y, 247, line); m_Y += 20;
                }
                #endregion
                
                AddButton(422, 291, 2008, 2007, (int)OrderButton.EditOrder, GumpButtonType.Reply, 0);
                AddButton(505, 289, 5531, 5532, (int)OrderButton.DeleteOrder, GumpButtonType.Reply, 0);
                AddButton(306, 291, 2113, 2112, (int)OrderButton.CopyOrder, GumpButtonType.Reply, 0);
            }

            AddButton(226, 289, 5534, 5535, (int)OrderButton.AddOrder, GumpButtonType.Reply, 0);
            AddButton(385, 291, 2708, 2709, (int)OrderButton.Exit, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            if (m_Viewer == null || m_Viewer != sender.Mobile)
                return;

            if (m_Ordered == null || m_Ordered.Deleted || m_Ordered.IsDeadBondedPet)
                return;

            switch (info.ButtonID)
            {
                case (int)OrderButton.PreviousOrder:
                    {
                        if (m_CurrentOrder > 0)
                            m_CurrentOrder--;
                        m_Viewer.SendGump(new ViewOrdersGump(m_Viewer, m_Ordered, m_CurrentOrder));
                        return;
                    }
                case (int)OrderButton.NextOrder:
                    {
                        if (m_CurrentOrder < m_Ordered.Orders.Count - 1)
                            m_CurrentOrder++;
                        m_Viewer.SendGump(new ViewOrdersGump(m_Viewer, m_Ordered, m_CurrentOrder));
                        return;
                    }
                case (int)OrderButton.AddOrder:
                    {
                        m_Viewer.SendGump(new AddOrderGump(m_Viewer, m_Ordered));
                        return;
                    }
                case (int)OrderButton.DeleteOrder:
                    {
                        m_Ordered.Orders.Remove(m_Ordered.Orders[m_CurrentOrder]);
                        if (m_CurrentOrder <= 0)
                            m_Viewer.SendGump(new ViewOrdersGump(m_Viewer, m_Ordered));
                        else
                        {
                            while (m_CurrentOrder + 1 >= m_Ordered.Orders.Count)
                            {
                                m_CurrentOrder--;
                            }
                            m_Viewer.SendGump(new ViewOrdersGump(m_Viewer, m_Ordered, m_CurrentOrder));
                        }
                        return;
                    }
                case (int)OrderButton.CopyOrder:
                    {
                        if (m_CurrentOrder < m_Ordered.Orders.Count)
                            m_Viewer.Target = new CopyOrderTarget(m_Viewer, m_Ordered.Orders[m_CurrentOrder]);
                        else
                            m_Viewer.SendGump(new ViewOrdersGump(m_Viewer, m_Ordered));
                        return;
                    }
                case (int)OrderButton.EditOrder:
                    {
                        if (m_CurrentOrder < m_Ordered.Orders.Count)
                            m_Viewer.SendGump(new AddOrderGump(m_Viewer, m_Ordered, m_Ordered.Orders[m_CurrentOrder]));
                        else
                            m_Viewer.SendGump(new ViewOrdersGump(m_Viewer, m_Ordered));
                        return;
                    }
                case (int)OrderButton.Exit:
                    {
                        return;
                    }
            }
            base.OnResponse(sender, info);
        }
    }

    public class CopyOrderTarget : Target
    {
        OrderInfo m_Order;

        public CopyOrderTarget(Mobile from, OrderInfo ord)
            : base(20, true, TargetFlags.None)
        {
            m_Order = ord;
            from.SendMessage("Target a mobile within your control to pass the same orders to them.");
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if(m_Order == null)
                return;

            if(from == null || from.Deleted)
                return;

            if (from is PlayerMobile)
            {
                PlayerMobile pm = from as PlayerMobile;

                if (targeted is Soldier && (targeted as Soldier).Government != null && !(targeted as Soldier).Government.Deleted)
                {
                    Soldier sol = targeted as Soldier;

                    if (CustomGuildStone.IsGuildMilitary(pm, sol.Government))
                    {
                        sol.Orders.Add(m_Order);
                        pm.SendMessage("Orders were successfully passed on to " + sol.Name + ".");
                        pm.Target = new CopyOrderTarget(pm, m_Order);
                    }
                    else
                        pm.SendMessage("Only military personnel of " + sol.Government.Name.ToString() + " may do that.");
                }
                else if (targeted is BaseCreature)
                {
                    BaseCreature creat = targeted as BaseCreature;

                    if (creat.Controlled)
                    {
                        if (creat.ControlMaster != null && !creat.ControlMaster.Deleted && creat.ControlMaster == pm)
                        {
                            creat.Orders.Add(m_Order);
                            pm.SendMessage(creat.Name + " will now obey those orders.");
                            pm.Target = new CopyOrderTarget(pm, m_Order);
                        }
                        else
                            pm.SendMessage("You canot pass orders to that.");
                    }
                    else
                        pm.SendMessage("You canot pass orders to that.");
                }
                else
                    pm.SendMessage("You cannot pass orders to that.");
            }

            base.OnTarget(from, targeted);
        }
    }
}