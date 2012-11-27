using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Government;
using Server.Targeting;
using Server.Commands;

namespace Server.Mobiles
{
    [PropertyObject]
    public class GroupInfo
    {
        public static void Initialize()
        {
            CommandSystem.Register("Group", AccessLevel.Player, new CommandEventHandler(Group_OnCommand));
        }

        [Usage("Group")]
        [Description("Activates the 'group' system for this player.")]
        public static void Group_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile == null || e.Mobile.Deleted || !e.Mobile.Alive)
                return;

            if (!(e.Mobile is PlayerMobile))
                return;

            if ((e.Mobile as PlayerMobile).Group == null)
                (e.Mobile as PlayerMobile).Group = new GroupInfo(e.Mobile as PlayerMobile);

            e.Mobile.SendGump(new GroupGump(e.Mobile as PlayerMobile, (e.Mobile as PlayerMobile).Group));
        }

        public static List<GroupInfo> Groups
        {
            get
            {
                List<GroupInfo> m_Groups = new List<GroupInfo>();
                foreach (Mobile m in World.Mobiles.Values)
                {
                    if (m is PlayerMobile)
                        m_Groups.Add((m as PlayerMobile).Group);
                }
                return m_Groups;
            }
        }

        private string m_Name;
        private PlayerMobile m_Leader;
        private List<BaseCreature> m_Members = new List<BaseCreature>();
        private int m_ResetTime;
        private DateTime m_TimeOfCreation;

        public string Name { get { return m_Name; } set { m_Name = value; } }
        public PlayerMobile Leader { get { return m_Leader; } set { m_Leader = value; } }
        public List<BaseCreature> Members 
        { 
            get 
            {
                return m_Members; 
            } 
            set 
            {
                m_Members = value;
            }
        }
        public int ResetTime { get { return m_ResetTime; } set { m_ResetTime = value; } }
        public DateTime TimeOfCreation { get { return m_TimeOfCreation; } set { m_TimeOfCreation = value; } }

        public GroupInfo(PlayerMobile leader) 
            : this(leader, "An Unnamed Group", 60, DateTime.Now, new List<BaseCreature>())
        {
            m_Name = "An Unnamed Group";
            m_Leader = leader;
            m_Members = new List<BaseCreature>();
            m_ResetTime = 60;
            m_TimeOfCreation = DateTime.Now;
        }

        public GroupInfo(PlayerMobile leader, string name, int reset, DateTime creation, List<BaseCreature> members)
        {
            m_Leader = leader;
            m_Name = name;
            m_ResetTime = reset;
            m_TimeOfCreation = creation;
            m_Members = members;
            leader.Group = this;
        }

        public static void SetGroupName(string name, PlayerMobile pm)
        {
            pm.Group.Name = name;
        }

        public static void SetResetTime(int newResetMinutes, PlayerMobile pm)
        {
            pm.Group.TimeOfCreation = DateTime.Now;
            pm.Group.ResetTime = newResetMinutes;
        }

        public static void AddToGroup(Mobile m, PlayerMobile pm)
        {
            if (m is BaseCreature)
            {
                BaseCreature bc = m as BaseCreature;

                if (!HasGroup(bc))
                {
                    if (pm.Group.Members.Contains(bc))
                    {
                        pm.SendMessage(bc.Name + " is already a member of " + pm.Group.Name + ".");
                        pm.SendGump(new GroupGump(pm, pm.Group));
                        return;
                    }
                    else
                    {
                        bc.ControlTarget = null;
                        bc.ControlOrder = OrderType.Stop;
                        pm.Group.Members.Add(bc);
                        pm.SendMessage(bc.Name + " is now a member of " + pm.Group.Name + ".");
                        pm.SendGump(new GroupGump(pm, pm.Group));
                        return;
                    }
                }
                else if (HasGroup(bc))
                {
                    if (GetGroup(bc).Leader == pm)
                    {
                        if (!pm.Group.Members.Contains(bc))
                        {
                            bc.ControlTarget = null;
                            bc.ControlOrder = OrderType.Stop;
                            pm.Group.Members.Add(bc);
                            pm.SendGump(new GroupGump(pm, pm.Group));
                        }
                        return;
                    }
                    else
                    {
                        pm.SendMessage(bc.Name + " is already in a group.");
                        pm.SendGump(new GroupGump(pm, pm.Group));
                        return;
                    }
                }
            }
            else
            {
                pm.SendMessage("That is not a valid target for your group.");
                pm.SendGump(new GroupGump(pm, pm.Group));
                return;
            }
        }

        public static void RemoveFromGroup(Mobile m, PlayerMobile pm)
        {
            if (m is BaseCreature)
            {
                BaseCreature bc = m as BaseCreature;
                
                if (IsGroupMember(bc, pm))
                {
                    bc.ControlTarget = null;
                    bc.ControlOrder = OrderType.Stop;
                    pm.Group.Members.Remove(bc);
                    pm.SendMessage(bc.Name + " has been removed from " + pm.Group.Name + ".");
                    pm.SendGump(new GroupGump(pm, pm.Group));
                    return;
                }
                else
                {
                    pm.SendMessage(bc.Name + " is not a member of " + pm.Group.Name + ".");
                    pm.SendGump(new GroupGump(pm, pm.Group));
                    return;
                }
            }
            else
            {
                pm.SendMessage("That is not a valid selection.");
                pm.SendGump(new GroupGump(pm, pm.Group));
                return;
            }
        }

        public static void ClearGroup(PlayerMobile pm)
        {
            foreach (BaseCreature bc in pm.Group.Members)
            {
                if (bc != null && !bc.Deleted)
                {
                    bc.ControlTarget = null;
                    bc.ControlOrder = OrderType.Stop;
                }
            }

            pm.Group.Members.Clear();
        }

        public static void CheckReset(PlayerMobile pm)
        {
            if (pm.Group.TimeOfCreation + TimeSpan.FromMinutes(pm.Group.ResetTime) < DateTime.Now)
                ClearGroup(pm);

            if (pm.Map == Map.Internal)
                ClearGroup(pm);
        }

        public static bool IsGroupMember(BaseCreature bc, PlayerMobile pm)
        {
            return pm.Group.Members.Contains(bc);
        }

        public static bool IsGroupLeader(BaseCreature bc, PlayerMobile pm)
        {
            if (!HasGroup(bc))
                return false;

            if (pm.AccessLevel > AccessLevel.Player)
                return true;

            return (IsGroupMember(bc, pm));
        }

        public static bool HasGroup(BaseCreature bc)
        {
            foreach (GroupInfo group in GroupInfo.Groups)
            {
                if (IsGroupMember(bc, group.Leader))
                {
                    return true;
                }
            }

            return false;
        }

        public static GroupInfo GetGroup(BaseCreature bc)
        {
            foreach (GroupInfo group in GroupInfo.Groups)
            {
                if (IsGroupMember(bc, group.Leader))
                {
                    return group;
                }
            }
            return null;
        }

        public static void TryGiveGroup(PlayerMobile currentLeader, PlayerMobile newLeader)
        {
            if (newLeader == currentLeader)
            {
                newLeader.SendMessage("You are already the leader of " + currentLeader.Group.Name + ".");
                return;
            }

            ClearGroup(newLeader);
            new GroupInfo(newLeader, currentLeader.Group.Name, currentLeader.Group.ResetTime, currentLeader.Group.TimeOfCreation, currentLeader.Group.Members);
            GroupInfo.ClearGroup(currentLeader);
            newLeader.SendGump(new GroupGump(newLeader, newLeader.Group));
        }

        public static bool TryGetGroupGump(BaseCreature bc, SpeechEventArgs e)
        {
            bc.DebugSay("Checking to see if someone wants to see my group...");

            bool wasNamed = false;

            string[] speech = e.Speech.ToLower().Split(' ');
            string[] listenerName = bc.Name.ToLower().Split(' ');

            foreach (string word in speech)
            {
                foreach (string name in listenerName)
                {
                    if (word == name)
                    {
                        wasNamed = true; 
                        continue;
                    }
                }
                if (wasNamed)
                    continue;  
            }

            if (e.Mobile is PlayerMobile && wasNamed && (e.Speech.ToLower().Contains("group") && e.Speech.ToLower().Contains("your")))
            {
                PlayerMobile pm = e.Mobile as PlayerMobile;

                if (HasGroup(bc))
                {
                    if (IsGroupLeader(bc, pm))
                        return true;
                    else if (bc is Soldier && (bc as Soldier).Government != null && !(bc as Soldier).Government.Deleted)
                    {
                        if (CustomGuildStone.IsGuildOfficer(pm, (bc as Soldier).Government))
                            return true;
                    }
                    else if (bc.Controlled && bc.ControlMaster != null && bc.ControlMaster == pm)
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    if (bc.Controlled && bc.ControlMaster != null && bc.ControlMaster == pm)
                    {
                        pm.SendMessage(bc.Name + " has no group.");
                    }
                    if (bc is Soldier && (bc as Soldier).Government != null && !(bc as Soldier).Government.Deleted)
                    {
                        if (CustomGuildStone.IsGuildOfficer(pm, (bc as Soldier).Government))
                            pm.SendMessage(bc.Name + " has no group.");
                    }
                    return false;
                }
            }
            else
                return false;
        }

        public static void Serialize(GenericWriter writer, GroupInfo info)
        {
            writer.Write((int)0); //version

            writer.Write((string)info.Name);
            writer.Write((PlayerMobile)info.Leader);
            writer.Write((int)info.ResetTime);
            writer.Write((DateTime)info.TimeOfCreation);
            writer.Write((int)info.Members.Count);
            foreach (BaseCreature bc in info.Members)
            {
                writer.Write((BaseCreature)bc);
            }
        }

        public static void Deserialize(GenericReader reader, GroupInfo info)
        {
            int version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        info.Name = reader.ReadString();
                        info.Leader = (PlayerMobile)reader.ReadMobile();
                        info.ResetTime = reader.ReadInt();
                        info.TimeOfCreation = reader.ReadDateTime();
                        info.Members = new List<BaseCreature>();
                        int count = reader.ReadInt();
                        for (int i = 0; i < count; i++)
                        {
                            BaseCreature member = (BaseCreature)reader.ReadMobile();
                            info.Members.Add(member);
                        }
                        break;
                    }
            }
        }

    }

    public class AddToGroupTarget : Target
    {
        private GroupInfo m_Group;

        public AddToGroupTarget(GroupInfo g)
            : base(20, true, TargetFlags.None)
        {
            m_Group = g;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is BaseCreature)
            {
                BaseCreature bc = targeted as BaseCreature;

                GroupInfo thisGroup = GroupInfo.GetGroup(bc);

                if (thisGroup != null)
                {
                    if (bc is Soldier && (bc as Soldier).Government != null && !(bc as Soldier).Government.Deleted)
                    {
                        if (CustomGuildStone.Outranks(from as PlayerMobile, m_Group.Leader, (bc as Soldier).Government))
                        {
                            GroupInfo.RemoveFromGroup(bc, thisGroup.Leader);
                            GroupInfo.AddToGroup(bc, m_Group.Leader);
                        }
                        else if (CustomGuildStone.IsGuildOfficer(from as PlayerMobile, (bc as Soldier).Government))
                        {
                            from.SendMessage(bc.Name + " is already part of " + thisGroup.Name + ".");
                        }
                        from.SendGump(new GroupGump(from as PlayerMobile, (from as PlayerMobile).Group));
                        return;
                    }
                    else if (bc.Controlled)
                    {
                        if (bc.ControlMaster != null && !bc.ControlMaster.Deleted && bc.ControlMaster == from)
                        {
                            GroupInfo.RemoveFromGroup(bc, thisGroup.Leader);
                            GroupInfo.AddToGroup(bc, m_Group.Leader);
                        }
                        else if (bc.ControlMaster != null && !bc.ControlMaster.Deleted && bc.IsFriend(from))
                        {
                            from.SendMessage(bc.Name + " is already part of " + thisGroup.Name + ".");
                        }
                        from.SendGump(new GroupGump(from as PlayerMobile, (from as PlayerMobile).Group));
                        return;
                    }

                    from.SendMessage("You cannot do that.");
                    from.SendGump(new GroupGump(from as PlayerMobile, (from as PlayerMobile).Group));
                    return;
                }
                else
                {
                    if (bc is Soldier && (bc as Soldier).Government != null && !(bc as Soldier).Government.Deleted)
                    {
                        if(CustomGuildStone.IsGuildOfficer(from as PlayerMobile, (bc as Soldier).Government))
                            GroupInfo.AddToGroup(bc, m_Group.Leader);
                        from.SendGump(new GroupGump(from as PlayerMobile, (from as PlayerMobile).Group));
                        return;
                    }
                    else if (bc.Controlled)
                    {
                        if (bc.ControlMaster != null && !bc.ControlMaster.Deleted && bc.ControlMaster == from)
                        {
                            GroupInfo.AddToGroup(bc, m_Group.Leader);
                        }
                        from.SendGump(new GroupGump(from as PlayerMobile, (from as PlayerMobile).Group));
                        return;
                    }

                    from.SendMessage("You cannot do that.");
                    from.SendGump(new GroupGump(from as PlayerMobile, (from as PlayerMobile).Group));
                    return;
                }
            }
            else
            {
                from.SendMessage("Invalid target.");
                from.SendGump(new GroupGump(from as PlayerMobile, (from as PlayerMobile).Group));
                return;
            }
        }
    }

    public class ChangeGroupLeaderTarget : Target
    {
        private GroupInfo m_Group;

        public ChangeGroupLeaderTarget(GroupInfo group)
            : base(20, true, TargetFlags.None)
        {
            m_Group = group;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is PlayerMobile)
            {
                (targeted as PlayerMobile).SendGump(new ChangeGroupLeaderGump(targeted as PlayerMobile, m_Group.Leader));
            }
            else
            {
                from.SendMessage("Invalid target.");
                from.SendGump(new GroupGump(from as PlayerMobile, m_Group));
            }

            base.OnTarget(from, targeted);
        }
    }

    public class ChangeGroupLeaderGump : Gump
    {
        private enum LeaderButton
        {
            Accept = 1,
            Refuse = 2
        }

        private PlayerMobile m_CurrentLeader;
        private PlayerMobile m_Viewer;

        public ChangeGroupLeaderGump(PlayerMobile viewer, PlayerMobile currentLeader)
            : base(0, 0)
        {
            m_Viewer = viewer;
            m_CurrentLeader = currentLeader;
            InitialSetup();
        }

        private void InitialSetup()
        {
            m_Viewer.CloseGump(typeof(GroupGump));
            m_Viewer.CloseGump(typeof(ChangeGroupLeaderGump));
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            AddPage(0);

            AddBackground(250, 250, 297, 100, 9270);
            AddLabel(401 - (int)(m_CurrentLeader.Group.Name.Length * 2.5), 264, 247, m_CurrentLeader.Group.Name);
            AddLabel(265, 284, 247, "Do you wish to take command of this group?");
            AddButton(270, 315, 12000, 12001, (int)LeaderButton.Accept, GumpButtonType.Reply, 0);
            AddButton(450, 315, 12018, 12020, (int)LeaderButton.Refuse, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case (int)LeaderButton.Accept:
                    {
                        GroupInfo.TryGiveGroup(m_CurrentLeader, m_Viewer);
                        return;
                    }
                case (int)LeaderButton.Refuse:
                    {
                        m_CurrentLeader.SendMessage(m_CurrentLeader.Name + " has refused to lead " + m_CurrentLeader.Group.Name + ".");
                        m_CurrentLeader.SendGump(new GroupGump(m_CurrentLeader, m_CurrentLeader.Group));
                        return;
                    }
                default: goto case (int)LeaderButton.Refuse;
            }

            base.OnResponse(sender, info);
        }
    }

    public class GroupGump : Gump
    {
        private enum GroupButton
        {
            Cancel,
            Okay,
            Clear,
            ChangeLeader,
            AddMember,
            ScrollUp,
            ScrollDown,
            Remove
        }

        private enum GroupText
        {
            Name = 1,
            ResetTime = 2
        }

        private PlayerMobile m_Viewer;
        private GroupInfo m_Group;
        private int m_Current;

        private const int m_MaxMembers = 7;

        public GroupGump(PlayerMobile viewer, GroupInfo group) : this(viewer, group, 0)
        {

        }

        public GroupGump(PlayerMobile viewer, GroupInfo group, int current) :base(0,0)
        {
            m_Viewer = viewer;
            m_Group = group;
            m_Current = current;

            InitialSetup();
        }

        private void InitialSetup()
        {
            m_Viewer.CloseGump(typeof(GroupGump));
            m_Viewer.CloseGump(typeof(ChangeGroupLeaderGump));
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;
            AddPage(0);

            AddBackground(165, 200, 472, 203, 9270);

            #region Details

            AddBackground(180, 215, 205, 30, 9350);
            AddLabel(185, 220, 247, "Leader:");
            AddLabelCropped(243, 220, 133, 16, 0, m_Group.Leader.Name);

            AddBackground(180, 250, 205, 30, 9350);
            AddLabel(185, 255, 247, "Name:");
            AddTextEntry(230, 255, 149, 20, 0, (int)GroupText.Name, m_Group.Name);

            AddBackground(180, 285, 205, 30, 9350);
            AddLabel(185, 290, 247, "Minutes to Reset:");
            AddTextEntry(302, 289, 76, 20, 0, (int)GroupText.ResetTime, m_Group.ResetTime.ToString());

            #endregion

            #region Main Buttons

            AddButton(180, 318, 249, 248, (int)GroupButton.Okay, GumpButtonType.Reply, 0);
            AddButton(319, 318, 243, 241, (int)GroupButton.Cancel, GumpButtonType.Reply, 0);
            AddButton(266, 318, 4020, 4022, (int)GroupButton.Clear, GumpButtonType.Reply, 0);
            AddButton(185, 359, 4002, 4004, (int)GroupButton.ChangeLeader, GumpButtonType.Reply, 0);
            AddLabel(220, 361, 247, "Change This Group's Leader");

            #endregion

            #region Member List

            AddLabel(462, 213, 247, "Group Members");
            AddButton(570, 215, 2460, 2461, (int)GroupButton.AddMember, GumpButtonType.Reply, 0);
            AddBackground(403, 235, 217, 151, 9350);

            int m_X = 410;
            int m_Y = 240;

            for(int i = m_Current * m_MaxMembers; i < (m_Current * m_MaxMembers) + m_MaxMembers && i < m_Group.Members.Count; i++)
            {
                AddLabelCropped(m_X, m_Y, 120, 20, 0, m_Group.Members[i].Name);
                AddButton(m_X + 140, m_Y + 4, 2181, 2181, i + (int)GroupButton.Remove, GumpButtonType.Reply, 0);
                m_Y += 20;
            }

            AddImageTiled(611, 247, 9, 120, 9203);
            AddButton(609, 235, 250, 251, (int)GroupButton.ScrollUp, GumpButtonType.Reply, 0);
            AddButton(609, 365, 252, 253, (int)GroupButton.ScrollDown, GumpButtonType.Reply, 0);

            #endregion
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            if (info.ButtonID < (int)GroupButton.Remove)
            {
                switch (info.ButtonID)
                {
                    case (int)GroupButton.Cancel:
                        {
                            return;
                        }
                    case (int)GroupButton.Okay:
                        {
                            int val = 0;
                            if (ValidateInt(info.GetTextEntry((int)GroupText.ResetTime).Text, ref val))
                                GroupInfo.SetResetTime(val, m_Group.Leader);

                            GroupInfo.SetGroupName(info.GetTextEntry((int)GroupText.Name).Text, m_Group.Leader);

                            if (!GroupInfo.Groups.Contains(m_Group))
                                GroupInfo.Groups.Add(m_Group);

                            return;
                        }
                    case (int)GroupButton.Clear:
                        {
                            m_Viewer.SendMessage("Clearing your group...");
                            GroupInfo.ClearGroup(m_Group.Leader);
                            m_Viewer.SendMessage("Group cleared!");
                            m_Viewer.SendGump(new GroupGump(m_Viewer, m_Group, m_Current));
                            return;
                        }
                    case (int)GroupButton.ChangeLeader:
                        {
                            m_Viewer.Target = new ChangeGroupLeaderTarget(m_Group);
                            m_Viewer.SendMessage("Target a player to make them the new leader of this group.");
                            return;
                        }
                    case (int)GroupButton.AddMember:
                        {
                            int val = 0;
                            if (ValidateInt(info.GetTextEntry((int)GroupText.ResetTime).Text, ref val))
                                GroupInfo.SetResetTime(val, m_Group.Leader);

                            GroupInfo.SetGroupName(info.GetTextEntry((int)GroupText.Name).Text, m_Group.Leader);

                            if (!GroupInfo.Groups.Contains(m_Group))
                                GroupInfo.Groups.Add(m_Group);

                            m_Viewer.Target = new AddToGroupTarget(m_Group);
                            m_Viewer.SendMessage("Target an NPC to add them to your group.");
                            return;
                        }
                    case (int)GroupButton.ScrollUp:
                        {
                            int val = 0;
                            if (ValidateInt(info.GetTextEntry((int)GroupText.ResetTime).Text, ref val))
                                GroupInfo.SetResetTime(val, m_Group.Leader);

                            GroupInfo.SetGroupName(info.GetTextEntry((int)GroupText.Name).Text, m_Group.Leader);

                            if (!GroupInfo.Groups.Contains(m_Group))
                                GroupInfo.Groups.Add(m_Group);

                            if (m_Current > 0)
                                m_Current--;
                            m_Viewer.SendGump(new GroupGump(m_Viewer, m_Group, m_Current));
                            return;
                        }
                    case (int)GroupButton.ScrollDown:
                        {
                            int val = 0;
                            if (ValidateInt(info.GetTextEntry((int)GroupText.ResetTime).Text, ref val))
                                GroupInfo.SetResetTime(val, m_Group.Leader);

                            GroupInfo.SetGroupName(info.GetTextEntry((int)GroupText.Name).Text, m_Group.Leader);

                            if (!GroupInfo.Groups.Contains(m_Group))
                                GroupInfo.Groups.Add(m_Group);

                            if (m_Current * m_MaxMembers < m_Group.Members.Count)
                                m_Current++;

                            m_Viewer.SendGump(new GroupGump(m_Viewer, m_Group, m_Current));
                            return;
                        }
                }
            }
            else
            {
                GroupInfo.RemoveFromGroup(m_Group.Members[info.ButtonID - (int)GroupButton.Remove], m_Group.Leader);
                m_Viewer.SendGump(new GroupGump(m_Viewer, m_Group, m_Current));
                return;
            }

            base.OnResponse(sender, info);
        }

        private bool ValidateInt(string st, ref int parsed)
        {
            if (!int.TryParse(st, out parsed))
                return false;

            return true;
        }
    }
}