using Server.Commands;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{


    public class ResourceNodeGump : Gump
    {
        public int m_CurrentNav;
        public int m_NavScroll;
        public int m_MainScroll;
        public int m_CurrentMain;
        public int m_ItemsPerPage;
        public int m_Start;
        public int m_Y;
        public int m_Current;
        public int m_LineHeight;

        private PlayerMobile m_Viewer;
        private ResourceNode m_Node;
        private GovernmentEntity m_ChosenGovernment;

        public enum Buttons
        {
            Close, 
            ScrollUp, 
            ScrollDown, 
            Release, 
            Claim
        }

        public ResourceNodeGump(PlayerMobile from, ResourceNode n) : this(from, n, 0, 0) { }

        public ResourceNodeGump(PlayerMobile from, ResourceNode n, int currentMain, int mainScroll)
            : base(0, 0)
        {
            InitialSetup(from, n, currentMain, mainScroll);
        }

        public void InitialSetup(PlayerMobile from, ResourceNode n, int currentMain, int mainScroll)
        {
            m_Viewer = from;
            m_Node = n;
            
            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;
            m_CurrentMain = currentMain;
            m_MainScroll = mainScroll;
            m_ItemsPerPage = 9;
            m_Start = m_ItemsPerPage * m_NavScroll;

            AddBackground(183, 92, 450, 406, 9270);
            AddBackground(201, 112, 412, 368, 3500);

            m_Y = 140;
            m_LineHeight = 28;

            AddLabel(235, m_Y, 2010, "Resource:");
            AddLabel(330, m_Y, 0, m_Node.Resource.ToString());
            m_Y += m_LineHeight;

            AddLabel(235, m_Y, 2010, "Production:");
            AddLabel(330, m_Y, 0, m_Node.ProductionRate.ToString() + " units per season");
            m_Y += m_LineHeight;

            AddLabel(235, m_Y, 2010, "Next Shipment:");
            AddLabel(330, m_Y, 0, m_Node.ProductionDate.ToString());
            m_Y += m_LineHeight;

            AddLabel(235, m_Y, 2010, "Owners:");
            if((m_Node.Government == null) || (m_Node.Government.Deleted))
                AddLabel(330, m_Y, 0, "Unclaimed");
            else
                AddLabel(330, m_Y, 0, m_Node.Government.Name.ToString());
            m_Y += m_LineHeight;

            if (m_Node.Government != null && m_Node.Owned && GovernmentEntity.IsGuildOfficer(m_Viewer, m_Node.Government))
            {
                AddButton(235, m_Y, 10740, 10742, (int)Buttons.Release, GumpButtonType.Reply, 0);
                AddLabel(255, m_Y, 0, "Release this resource from " + m_Node.Government.Name.ToString() + "'s control.");
                m_Y += m_LineHeight;
            }
            else
                AddMyGovernments();
        }

        public void AddMyGovernments()
        {
            m_ItemsPerPage = 8;
            m_Start = m_ItemsPerPage * m_MainScroll;

            for (m_Current = 0; m_Current < m_ItemsPerPage && (m_Start + m_Current) < GovernmentEntity.Governments.Count; m_Current++)
            {
                if (GovernmentEntity.Governments[m_Current + m_Start].Members.Contains(m_Viewer))
                {
                    AddButton(235, (m_Y + 2 + (m_Current * m_LineHeight)), 10740, 10742, (int)Buttons.Claim + m_Current, GumpButtonType.Reply, 0);
                    string claimString = "Claim this for " + GovernmentEntity.Governments[m_Current + m_Start].Name.ToString() + ".";
                    AddLabel(255, (m_Y + (m_Current * m_LineHeight)), 0, claimString);
                }
            }

            AddScroll(CustomGuildStone.Guilds.Count, (m_Start + m_Current));
        }

        public void AddScroll(int total, int next)
        {
            if (total > next)
                AddButton(565, 435, 2648, 2649, (int)Buttons.ScrollDown, GumpButtonType.Reply, 0);

            if (m_MainScroll > 0)
                AddButton(565, 140, 2650, 2651, (int)Buttons.ScrollUp, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Viewer == null || m_Viewer != sender.Mobile)
                return;
            if (info.ButtonID == (int)Buttons.ScrollUp)
                m_MainScroll--;
            if (info.ButtonID == (int)Buttons.ScrollDown)
                m_MainScroll++;
            if (info.ButtonID == (int)Buttons.Release)
                m_Node.Release(m_Viewer, m_Node.Government);
            if (info.ButtonID >= (int)Buttons.Claim)
            {
                int index = info.ButtonID - (int)Buttons.Claim + m_Start;

                if (GovernmentEntity.Governments.Count > index)
                    m_Node.Claim(m_Viewer, GovernmentEntity.Governments[index]);
            }
            if ((info.ButtonID > (int)Buttons.Close) && !(info.ButtonID >= (int)Buttons.Release))
                SendNewGump();
        }

        public void SendNewGump()
        {
            m_Viewer.SendGump(new ResourceNodeGump(m_Viewer, m_Node, m_CurrentMain, m_MainScroll));
        }
    }
}