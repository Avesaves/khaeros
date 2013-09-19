using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
    public class MilitaryPolicyGump : Gump
    {
        private enum Buttons
        {
            KillOnSight, JailOnSight, KillNation, JailNation, Exceptions,
            Close, Apply, Cancel, MainScrollUp, MainScrollDown, 
            MB1, MB2, MB3, MB4, MB5, MB6, MB7, MB8, MB9, MB10
        }

        private string[] m_NavigationItems
        {
            get { return new string[]
    	            { 
    		            "Kill on Sight",
                        "Jail on Sight",
                        "Kill Nation",
                        "Jail Nation",
                        "Exceptions"
    	            };
                }
        }

        private PlayerMobile m_Viewer;
        private GovernmentEntity m_Government;
        private int m_CurrentNav;
        private int m_MainScroll;
        private int m_Y;
        private int m_X;

        private const int m_ItemsPerPage = 9;

        public MilitaryPolicyGump(PlayerMobile viewer, GovernmentEntity g) : this(viewer, g, 0, 0) { }
        public MilitaryPolicyGump(PlayerMobile viewer, GovernmentEntity g, int currentNav, int mainScroll) : base(0, 0)
        {
            InitialSetup(viewer, g, currentNav, mainScroll);
            AddNavigation();
            AddMain();
        }

        public void InitialSetup(PlayerMobile viewer, GovernmentEntity g, int currentNav, int mainScroll)
        {
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            m_Viewer = viewer;
            m_Government = g;
            m_CurrentNav = currentNav;
            m_MainScroll = mainScroll;

            AddBackground(203, 50, 427, 504, 9270);
            AddBackground(217, 65, 399, 75, 3500);
            AddBackground(218, 145, 165, 394, 3500);
            AddBackground(393, 147, 224, 394, 3500);

            AddLabel(420 - (int)(m_Government.Name.ToString().Length * 3.5), 78, 0, m_Government.Name.ToString());
            AddLabel(367, 101, 0, "Military Policies");
            AddButton(595, 70, 22150, 22152, (int)Buttons.Close, GumpButtonType.Reply, 0);
        }

        public void AddNavigation()
        {
            m_Y = 172;
            m_X = 237;

            for(int i = 0; i < m_NavigationItems.Length; i++)
            {
                if(m_CurrentNav == (int)(Buttons.KillOnSight + i))
                    AddButton(m_X, m_Y, 2511, 2510, (int)(Buttons.KillOnSight + i), GumpButtonType.Reply, 0);
                else
                    AddButton(m_X, m_Y, 2510, 2511, (int)(Buttons.KillOnSight + i), GumpButtonType.Reply, 0);
                AddLabel(m_X + 25, m_Y, 2511, m_NavigationItems[i]);
                m_Y += 30;
            }
        }

        public void AddMain()
        {
            switch (m_CurrentNav)
            {
                case (int)Buttons.KillOnSight: AddKillOnSight(); break;
                case (int)Buttons.JailOnSight: AddJailOnSight(); break;
                case (int)Buttons.KillNation: AddKillNation(); break;
                case (int)Buttons.JailNation: AddJailNation(); break;
                case (int)Buttons.Exceptions: AddExceptions(); break;
            }
        }

        #region Main Gumps
        public void AddKillOnSight()
        {
            AddLabel(415, 174, 0, "Add Name:");
            AddBackground(412, 195, 190, 23, 9300);
            AddTextEntry(417, 196, 179, 20, 0, 1, "");

            AddLabel(415, 229, 0, "Remove Name:");
            AddBackground(412, 250, 190, 23, 9300);
            AddTextEntry(417, 250, 179, 20, 0, 2, "");

            AddButton(431, 284, 2074, 2075, (int)Buttons.Apply, GumpButtonType.Reply, 0);
            AddButton(519, 284, 2071, 2072, (int)Buttons.Cancel, GumpButtonType.Reply, 0);

            AddScrollList(m_Government.MilitaryPolicies.KillIndividualOnSight);
        }

        public void AddJailOnSight()
        {
            AddLabel(415, 174, 0, "Add Name:");
            AddBackground(412, 195, 190, 23, 9300);
            AddTextEntry(417, 196, 179, 20, 0, 1, "");

            AddLabel(415, 229, 0, "Remove Name:");
            AddBackground(412, 250, 190, 23, 9300);
            AddTextEntry(417, 250, 179, 20, 0, 2, "");

            AddButton(431, 284, 2074, 2075, (int)Buttons.Apply, GumpButtonType.Reply, 0);
            AddButton(519, 284, 2071, 2072, (int)Buttons.Cancel, GumpButtonType.Reply, 0);

            AddScrollList(m_Government.MilitaryPolicies.JailIndividualOnSight);
        }

        public void AddKillNation()
        {
            m_X = 419;
            m_Y = 196;

            for (int i = 1; i < 7; i++)
            {
                if (m_Government.MilitaryPolicies.KillNationOnSight.Contains((Nation)i))
                {
                    AddButton(m_X, m_Y, 211, 210, (int)Buttons.MB1 + i, GumpButtonType.Reply, 0);
                    AddLabel(m_X + 35, m_Y, 37, ((Nation)i).ToString());
                }
                else
                {
                    AddButton(m_X, m_Y, 210, 211, (int)Buttons.MB1 + i, GumpButtonType.Reply, 0);
                    AddLabel(m_X + 35, m_Y, 0, ((Nation)i).ToString());
                }

                m_Y += 30;
            }
        }

        public void AddJailNation()
        {
            m_X = 419;
            m_Y = 196;

            for (int i = 1; i < 7; i++)
            {
                if (m_Government.MilitaryPolicies.JailNationOnSight.Contains((Nation)i))
                {
                    AddButton(m_X, m_Y, 211, 210, (int)Buttons.MB1 + i, GumpButtonType.Reply, 0);
                    AddLabel(m_X + 35, m_Y, 37, ((Nation)i).ToString());
                }
                else
                {
                    AddButton(m_X, m_Y, 210, 211, (int)Buttons.MB1 + i, GumpButtonType.Reply, 0);
                    AddLabel(m_X + 35, m_Y, 0, ((Nation)i).ToString());
                }

                m_Y += 30;
            }
        }

        public void AddExceptions()
        {
            AddLabel(415, 174, 0, "Add Name:");
            AddBackground(412, 195, 190, 23, 9300);
            AddTextEntry(417, 196, 179, 20, 0, 1, "");

            AddLabel(415, 229, 0, "Remove Name:");
            AddBackground(412, 250, 190, 23, 9300);
            AddTextEntry(417, 250, 179, 20, 0, 2, "");

            AddButton(431, 284, 2074, 2075, (int)Buttons.Apply, GumpButtonType.Reply, 0);
            AddButton(519, 284, 2071, 2072, (int)Buttons.Cancel, GumpButtonType.Reply, 0);

            AddScrollList(m_Government.MilitaryPolicies.Exceptions);
        }
        #endregion

        public void AddScrollList(List<string> scrollList)
        {
            m_Y = 325;
            m_X = 411;

            while (scrollList.Count < (m_MainScroll * m_ItemsPerPage))
                m_MainScroll--;

            AddBackground(408, 317, 193, 192, 9300);

            if (scrollList.Count > (m_MainScroll * m_ItemsPerPage) || m_MainScroll > 0)
            {
                AddImageTiled(589, 317, 13, 191, 2712);
                AddButton(588, 316, 251, 250, (int)Buttons.MainScrollUp, GumpButtonType.Reply, 0);
                AddButton(588, 489, 253, 252, (int)Buttons.MainScrollDown, GumpButtonType.Reply, 0);
            }            
            
            for (int i = (m_MainScroll * m_ItemsPerPage); i < ( (m_MainScroll * m_ItemsPerPage) + m_ItemsPerPage) && i < scrollList.Count; i++)
            {
                AddLabel(m_X, m_Y, 0, scrollList[i].ToString());
                m_Y += 20;
            }
        }

        #region Gump Response Handling
        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Viewer == null || m_Viewer != sender.Mobile)
                return;
            else if (info.ButtonID == (int)Buttons.Close)
                m_Viewer.CloseGump(typeof(MilitaryPolicyGump));
            else if (info.ButtonID == (int)Buttons.MainScrollUp)
            {
                m_MainScroll--;
                if (m_MainScroll < 0)
                    m_MainScroll = 0;
                m_Viewer.SendGump(new MilitaryPolicyGump(m_Viewer, m_Government, m_CurrentNav, m_MainScroll));
                return;
            }
            else if (info.ButtonID == (int)Buttons.MainScrollDown)
            {
                m_MainScroll++;
                m_Viewer.SendGump(new MilitaryPolicyGump(m_Viewer, m_Government, m_CurrentNav, m_MainScroll));
                return;
            }
            else if (info.ButtonID >= (int)Buttons.KillOnSight && info.ButtonID <= (int)Buttons.Exceptions)
            {
                m_Viewer.SendGump(new MilitaryPolicyGump(m_Viewer, m_Government, (int)info.ButtonID, m_MainScroll));
                return;
            }
            else if (info.ButtonID >= (int)Buttons.Apply)
            {
                HandleMainPageUse(info);
                return;
            }
            else
                return;
        }

        public void HandleMainPageUse(RelayInfo info)
        {
            switch (m_CurrentNav)
            {
                case (int)Buttons.KillOnSight: KillOnSightOnResponse(info.ButtonID, info.GetTextEntry(1).Text, info.GetTextEntry(2).Text); break;
                case (int)Buttons.JailOnSight: JailOnSightOnResponse(info.ButtonID, info.GetTextEntry(1).Text, info.GetTextEntry(2).Text); break;
                case (int)Buttons.KillNation: KillNationOnResponse(info.ButtonID); break;
                case (int)Buttons.JailNation: JailNationOnResponse(info.ButtonID); break;
                case (int)Buttons.Exceptions: ExceptionsOnResponse(info.ButtonID, info.GetTextEntry(1).Text, info.GetTextEntry(2).Text); break;
            }
        }
        #endregion

        #region OnResponse methods
        public void KillOnSightOnResponse(int id, string add, string remove)
        {
            if (id == (int)Buttons.Apply)
            {
                if (add != "" && !m_Government.MilitaryPolicies.KillIndividualOnSight.Contains(add))
                {
                    m_Government.MilitaryPolicies.KillIndividualOnSight.Add(add);
                    m_Viewer.SendMessage(663, add.ToString() + " has been successfully added to the Kill on Sight list.");
                }
                else if (add != "" && m_Government.MilitaryPolicies.KillIndividualOnSight.Contains(add))
                    m_Viewer.SendMessage(37, add.ToString() + " is already on the Kill On Sight list.");

                if (remove != "" && m_Government.MilitaryPolicies.KillIndividualOnSight.Contains(remove))
                {
                    m_Government.MilitaryPolicies.KillIndividualOnSight.Remove(remove);
                    m_Viewer.SendMessage(663, remove.ToString() + " has been successfully removed from the Kill on Sight list.");
                }
                else if (remove != "" && !m_Government.MilitaryPolicies.KillIndividualOnSight.Contains(remove))
                    m_Viewer.SendMessage(37, remove.ToString() + " is not on the Kill On Sight list.");

                m_Viewer.SendGump(new MilitaryPolicyGump(m_Viewer, m_Government, m_CurrentNav, 0));
            }
        }

        public void JailOnSightOnResponse(int id, string add, string remove)
        {
            if (id == (int)Buttons.Apply)
            {
                if (add != "" && !m_Government.MilitaryPolicies.JailIndividualOnSight.Contains(add))
                {
                    m_Government.MilitaryPolicies.JailIndividualOnSight.Add(add);
                    m_Viewer.SendMessage(663, add.ToString() + " has been successfully added to the Jail on Sight list.");
                }
                else if (add != "" && m_Government.MilitaryPolicies.JailIndividualOnSight.Contains(add))
                    m_Viewer.SendMessage(37, add.ToString() + " is already on the Jail On Sight list.");

                if (remove != "" && m_Government.MilitaryPolicies.JailIndividualOnSight.Contains(remove))
                {
                    m_Government.MilitaryPolicies.JailIndividualOnSight.Remove(remove);
                    m_Viewer.SendMessage(663, remove.ToString() + " has been successfully removed from the Jail on Sight list.");
                }
                else if (remove != "" && !m_Government.MilitaryPolicies.JailIndividualOnSight.Contains(remove))
                    m_Viewer.SendMessage(37, remove.ToString() + " is not on the Jail On Sight list.");

                m_Viewer.SendGump(new MilitaryPolicyGump(m_Viewer, m_Government, m_CurrentNav, 0));
            }
        }

        public void KillNationOnResponse(int id)
        {
            id = id - (int)Buttons.MB1;

            if (m_Government.MilitaryPolicies.KillNationOnSight.Contains((Nation)id))
            {
                m_Government.MilitaryPolicies.KillNationOnSight.Remove((Nation)id);
                m_Viewer.SendMessage(663, "The " + ((Nation)id).ToString() + " have been successfully removed from the Kill Nation on Sight list.");
            }
            else if (!m_Government.MilitaryPolicies.KillNationOnSight.Contains((Nation)id))
            {
                m_Government.MilitaryPolicies.KillNationOnSight.Add((Nation)id);
                m_Viewer.SendMessage(663, "The " + ((Nation)id).ToString() + " have been successfully added to the Kill Nation on Sight list.");
            }

            m_Viewer.SendGump(new MilitaryPolicyGump(m_Viewer, m_Government, m_CurrentNav, m_MainScroll));
        }

        public void JailNationOnResponse(int id)
        {
            id = id - (int)Buttons.MB1;

            if (m_Government.MilitaryPolicies.JailNationOnSight.Contains((Nation)id))
            {
                m_Government.MilitaryPolicies.JailNationOnSight.Remove((Nation)id);
                m_Viewer.SendMessage(663, "The " + ((Nation)id).ToString() + " have been successfully removed from the Jail Nation on Sight list.");
            }
            else if (!m_Government.MilitaryPolicies.JailNationOnSight.Contains((Nation)id))
            {
                m_Government.MilitaryPolicies.JailNationOnSight.Add((Nation)id);
                m_Viewer.SendMessage(663, "The " + ((Nation)id).ToString() + " have been successfully added to the Jail Nation on Sight list.");
            }

            m_Viewer.SendGump(new MilitaryPolicyGump(m_Viewer, m_Government, m_CurrentNav, m_MainScroll));
        }

        public void ExceptionsOnResponse(int id, string add, string remove)
        {
            if (id == (int)Buttons.Apply)
            {
                if (add != "" && !m_Government.MilitaryPolicies.Exceptions.Contains(add))
                {
                    m_Government.MilitaryPolicies.Exceptions.Add(add);
                    m_Viewer.SendMessage(663, add.ToString() + " has been successfully added to the Exceptions list.");
                }
                else if (add != "" && m_Government.MilitaryPolicies.Exceptions.Contains(add))
                    m_Viewer.SendMessage(37, add.ToString() + " is already on the Exceptions list.");

                if (remove != "" && m_Government.MilitaryPolicies.Exceptions.Contains(remove))
                {
                    m_Government.MilitaryPolicies.Exceptions.Remove(remove);
                    m_Viewer.SendMessage(663, remove.ToString() + " has been successfully removed from the Exceptions list.");
                }
                else if (remove != "" && !m_Government.MilitaryPolicies.Exceptions.Contains(remove))
                    m_Viewer.SendMessage(37, remove.ToString() + " is not on the Exceptions list.");
            }

            m_Viewer.SendGump(new MilitaryPolicyGump(m_Viewer, m_Government, m_CurrentNav, 0));
        }
        #endregion

    }
}
