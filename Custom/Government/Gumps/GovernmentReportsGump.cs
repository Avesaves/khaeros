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
    public enum Button
    {
        Close,
        Delete,
        LastReport,
        NextReport,
        ScrollUp,
        ScrollDown
    }

    public class GovernmentReportsGump : Gump
    {
        private PlayerMobile m_Viewer;
        private GovernmentEntity m_Government;
        private int m_CurrentReport;
        private int m_Scroll;
        private int m_Start;
        private const int m_LinesPerReport = 11;
        private List<string> m_Text = new List<string>();

        public GovernmentReportsGump(PlayerMobile from, GovernmentEntity g, int currentReport) : this(from, g, currentReport, 0) { }

        public GovernmentReportsGump(PlayerMobile from, GovernmentEntity g, int currentReport, int currentScroll)
            : base(0, 0)
        {
            InitialSetup(from, g, currentReport, currentScroll);
            AddReport();
        }

        public void InitialSetup(PlayerMobile from, GovernmentEntity g, int currentReport, int currentScroll)
        {            
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            m_Viewer = from;
            m_Government = g;
            m_CurrentReport = currentReport;
            m_Scroll = currentScroll;

            m_Viewer.CloseGump(typeof(GovernmentReportsGump));

            AddPage(0);

            AddBackground(245, 100, 325, 400, 9270);
            AddBackground(266, 122, 283, 74, 9350);
            AddBackground(266, 208, 283, 244, 9350);

            AddLabel(407 - (int)(m_Government.Name.Length * 3.5), 133, 0, m_Government.Name.ToString());
            AddLabel(373, 162, 0, "Report #" + (m_CurrentReport + 1).ToString());

            if (m_CurrentReport > 0)
            {
                AddButton(269, 465, 9766, 9767, (int)Button.LastReport, GumpButtonType.Reply, 0);
                AddLabel(291, 462, 1000, "Last Report");
            }

            if (m_Government.Reports.Count - 1 > m_CurrentReport)
            {
                AddButton(529, 465, 9762, 9763, (int)Button.NextReport, GumpButtonType.Reply, 0);
                AddLabel(448, 462, 1000, "Next Report");
            }
        }

        public void AddReport()
        {
            if (m_Government.Reports.Count == 0)
            {
                AddLabel(284, 225, 0, "No reports are available.");
            }
            else
            {
                AddButton(377, 459, 5531, 5532, (int)Button.Delete, GumpButtonType.Reply, 0);

                #region Adding Lines to m_Text Dictionary

                string[] reportString;
                string currentLine = "";

                //Formatting the date of the report.
                if (m_Government.Reports[m_CurrentReport].TimeOfReport != null)
                {
                    string removeString = m_Government.Reports[m_CurrentReport].TimeOfReport.ToString().Replace("The moon is new.", "");
                    removeString = removeString.Remove(0, 5);
                    reportString = ("Date of Report: " + removeString).Split(' ');

                    foreach (string word in reportString)
                    {
                        currentLine += word + " ";
                        if (currentLine.Length >= 30 || word == reportString[reportString.Length - 1])
                        {
                            m_Text.Add(currentLine);
                            currentLine = "";
                        }
                    }
                }

                if (m_Government.Reports[m_CurrentReport].Information != null)
                {
                    m_Text.Add("");

                    reportString = m_Government.Reports[m_CurrentReport].Information.Split(' ');
                    currentLine = "";
                    foreach (string word in reportString)
                    {
                        if (word.Equals("<br>"))
                        {
                            m_Text.Add(currentLine);
                            currentLine = "";
                            m_Text.Add("");
                        }
                        else
                        {
                            currentLine += word + " ";
                            if (currentLine.Length >= 30 || word == reportString[reportString.Length - 1])
                            {
                                m_Text.Add(currentLine);
                                currentLine = "";
                            }
                        }
                    }
                }

                if (m_Government.Reports[m_CurrentReport].ReporterName != null)
                {
                    m_Text.Add("");
                    m_Text.Add("Reported By: " + m_Government.Reports[m_CurrentReport].ReporterName.ToString());
                }
                #endregion

                m_Start = m_LinesPerReport * m_Scroll;

                if (m_Text.Count > m_Start || m_Scroll > 0)
                {
                    AddImageTiled(528, 214, 15, 232, 2712); //scroll bar
                    AddButton(528, 212, 251, 250, (int)Button.ScrollUp, GumpButtonType.Reply, 0); // scroll up
                    AddButton(528, 426, 253, 252, (int)Button.ScrollDown, GumpButtonType.Reply, 0); // scroll down
                }

                if (m_Text != null)
                {
                    int m_Y = 225;
                    for (int i = m_Start; i < m_Start + m_LinesPerReport && i < m_Text.Count; i++)
                    {
                        AddLabel(280, m_Y, 0, m_Text[i]);
                        m_Y += 20;
                    }
                }
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Viewer == null || m_Viewer != sender.Mobile)
                return;

            if (info.ButtonID == (int)Button.Close)
                return;

            if (info.ButtonID == (int)Button.ScrollUp)
            {
                m_Scroll--;
                if (m_Scroll < 0)
                    m_Scroll = 0;

                m_Viewer.SendGump(new GovernmentReportsGump(m_Viewer, m_Government, m_CurrentReport, m_Scroll));
            }

            if (info.ButtonID == (int)Button.ScrollDown)
            {
                if(m_Text.Count / m_LinesPerReport > m_Scroll)
                    m_Scroll++;
                m_Viewer.SendGump(new GovernmentReportsGump(m_Viewer, m_Government, m_CurrentReport, m_Scroll));
            }

            if (info.ButtonID == (int)Button.Delete)
            {
                if (m_CurrentReport > m_Government.Reports.Count)
                {
                    m_Government.Reports.Remove(m_Government.Reports[m_CurrentReport]);
                    m_Viewer.SendGump(new GovernmentReportsGump(m_Viewer, m_Government, m_CurrentReport - 1));
                }
                else
                {
                    m_Government.Reports.Remove(m_Government.Reports[m_CurrentReport]);
                    m_Viewer.SendGump(new GovernmentReportsGump(m_Viewer, m_Government, 0));
                }
            }
            else if (info.ButtonID == (int)Button.LastReport)
                m_Viewer.SendGump(new GovernmentReportsGump(m_Viewer, m_Government, m_CurrentReport - 1));
            else if (info.ButtonID == (int)Button.NextReport)
            {
                if (m_Government.Reports.Count >= m_CurrentReport + 1)
                    m_Viewer.SendGump(new GovernmentReportsGump(m_Viewer, m_Government, m_CurrentReport + 1));
                else
                    m_Viewer.SendGump(new GovernmentReportsGump(m_Viewer, m_Government, m_CurrentReport));
            }
            else
                return;
        }
    }
}