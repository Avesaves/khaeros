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
    public class GovernmentGump : OrganizationGump
    {
        public static void Initialize()
        {
            CommandSystem.Register("Governments", AccessLevel.Player, new CommandEventHandler(Governments_OnCommand));
        }

        [Usage("Governments")]
        [Description("Opens the gump with your organizations.")]
        public static void Governments_OnCommand(CommandEventArgs e)
        {
            PlayerMobile from = e.Mobile as PlayerMobile;

            if (from == null || from.Deleted || !(from is PlayerMobile))
                return;

            from.SendGump(new GovernmentGump(from));
        }

        public override string[] m_NavItems { get{ return new string[]
    	{ 
    		"My Organizations", 
    		"Ranks", 
    		"Roster", 
    		"Applicants", 
    		"Hues", 
    		"Politics",
            "Resources",
            "Spawners",
            "Way Points"

    	}; } }

        public GovernmentGump( PlayerMobile from ) : this( from, null, 0, 0, 0, 0 ) { }

        public GovernmentGump( PlayerMobile from, CustomGuildStone gov ) : this( from, gov, 0, 0, 0, 0 ) { }

        public GovernmentGump( PlayerMobile from, CustomGuildStone gov, int currentNav, int navScroll, int currentMain, int mainScroll )
            : this( from, gov, currentNav, navScroll, currentMain, mainScroll, null, null ) { }

        public GovernmentGump( PlayerMobile from, CustomGuildStone gov, int currentNav, int navScroll, int currentMain, int mainScroll,
                                GuildRankInfo rank, PlayerMobile member )
            : base( from, gov, currentNav, navScroll, currentMain, mainScroll, rank, member )
        {
        }

        public override void AddMain()
        {
            switch( m_CurrentNav )
            {
                case (int)Navigation.MyOrganizations: AddMyOrganizations(); break;
                case (int)Navigation.Ranks: AddRanks(); break;
                case (int)Navigation.Roster: AddRoster(); break;
                case (int)Navigation.Applicants: AddApplicants(); break;
                case (int)Navigation.Hues: AddHues(); break;
                case (int)Navigation.Politics: AddPolitics(); break;
                case (int)Navigation.Resources: AddResources(); break;
                case (int)Navigation.Spawners: AddSpawners(); break;
                case (int)Navigation.WayPoints: AddWayPoints(); break;
            }
        }

        public override bool HandleMainPageUse( RelayInfo info )
        {
            switch( m_CurrentNav )
            {
                case (int)Navigation.MyOrganizations: MyOrganizationsOnResponse( info.ButtonID ); break;
                case (int)Navigation.Ranks: RanksOnResponse( info.ButtonID ); return true;
                case (int)Navigation.Roster: RosterOnResponse( info.ButtonID ); return true;
                case (int)Navigation.Applicants: ApplicantsOnResponse( info.ButtonID ); break;
                case (int)Navigation.Hues: HuesOnResponse( info.ButtonID ); break;
                case (int)Navigation.Politics: PoliticsOnResponse( info.ButtonID ); break;
                case (int)Navigation.Resources: ResourcesOnResponse(info.ButtonID); break;
                case (int)Navigation.Spawners: SpawnersOnResponse( info.ButtonID ); break;
                case (int)Navigation.WayPoints: WayPointsOnResponse( info.ButtonID ); break;
            }

            return false;
        }

        public void AddPerformButton( int id, string label )
        {
            AddButton( 515, ( m_Y + 2 ), 10740, 10742, id, GumpButtonType.Reply, 0 );
            AddLabel( 325, m_Y, 0, label );
            m_Y += m_LineHeight;
        }

        public void AddResources()
        {
            AddLabel(315, 198, 2010, "Resources");
            m_Y = 253;

            if (m_Guild == null || !(m_Guild is GovernmentEntity))
                return;

            AddLabel(325, m_Y, 0, "Metals:");
            AddLabel(515, m_Y, 0, (m_Guild as GovernmentEntity).Resources[ResourceType.Metals].ToString()); m_Y += m_LineHeight;

            AddLabel(325, m_Y, 0, "Food:");
            AddLabel(515, m_Y, 0, (m_Guild as GovernmentEntity).Resources[ResourceType.Food].ToString()); m_Y += m_LineHeight;

            AddLabel(325, m_Y, 0, "Water:");
            AddLabel(515, m_Y, 0, (m_Guild as GovernmentEntity).Resources[ResourceType.Water].ToString()); m_Y += m_LineHeight;

            AddLabel(325, m_Y, 0, "Cloth:");
            AddLabel(515, m_Y, 0, (m_Guild as GovernmentEntity).Resources[ResourceType.Cloth].ToString()); m_Y += m_LineHeight;

            AddLabel(325, m_Y, 0, "Wood:");
            AddLabel(515, m_Y, 0, (m_Guild as GovernmentEntity).Resources[ResourceType.Wood].ToString()); m_Y += m_LineHeight;

            AddLabel(325, m_Y, 0, "Influence:");
            AddLabel(515, m_Y, 0, (m_Guild as GovernmentEntity).Resources[ResourceType.Influence].ToString()); m_Y += m_LineHeight;

            AddPerformButton((int)Buttons.MB1, "Station trade advisor.");
            AddPerformButton((int)Buttons.MB2, "Remove trade advisor.");
        }

        public void AddSpawners()
        {
            AddLabel( 315, 198, 2010, "Spawners" );
            m_Y = 253;

            if( m_Guild == null || !(m_Guild is GovernmentEntity) )
                return;

            AddPerformButton( (int)Buttons.MB1, "Add a military spawner" );
            AddPerformButton( (int)Buttons.MB2, "Remove a military spawner" );            
            AddPerformButton( (int)Buttons.MB3, "Station a soldier at a spawner");
            AddPerformButton( (int)Buttons.MB4, "Station a military advisor");
            AddPerformButton( (int)Buttons.MB5, "Remove a military advisor");
        }

        public void AddWayPoints()
        {
            AddLabel( 315, 198, 2010, "Way Points" );
            m_Y = 253;

            if( m_Guild == null || !( m_Guild is GovernmentEntity ) )
                return;

            AddPerformButton( (int)Buttons.MB1, "Add a military way point" );
            AddPerformButton( (int)Buttons.MB2, "Remove a military way point" );
        }

        public bool BadNavigation()
        {
            if( m_Guild == null || !( m_Guild is GovernmentEntity ) || m_Viewer == null )
                return true;

            return false;
        }

        public bool AllowAccess( bool msg )
        {
            if (m_Viewer.AccessLevel > AccessLevel.Player)
                return true;
            else
            {
                m_Viewer.SendMessage("Players do not have access to this function.");
                return false;
            }

        }

        public void ResourcesOnResponse(int id)
        {
            if (id == (int)Buttons.MB1)
                TryAddAdvisor();

            if (id == (int)Buttons.MB2)
                TryRemoveAdvisor();
        }

        public void TryAddAdvisor()
        {
            if (BadNavigation() || !AllowAccess(true))
                return;

            if (m_Guild == null || !(m_Guild is GovernmentEntity))
                return;

            if (GovernmentEntity.IsGuildLeader(m_Viewer, m_Guild))
                m_Viewer.Target = new AddAdvisorTarget(m_Guild as GovernmentEntity);
            else
                m_Viewer.SendMessage("You must be the leader of " + m_Guild.Name.ToString() + " to do that.");
        }

        public void TryRemoveAdvisor()
        {
            if (BadNavigation() || !AllowAccess(true))
                return;

            if (m_Guild == null || !(m_Guild is GovernmentEntity))
                return;

            if (GovernmentEntity.IsGuildLeader(m_Viewer, m_Guild))
                m_Viewer.Target = new RemoveAdvisorTarget(m_Guild as GovernmentEntity);
            else
                m_Viewer.SendMessage("You must be the leader of " + m_Guild.Name.ToString() + " to do that.");
        }

        public void SpawnersOnResponse( int id )
        {
            if( id == (int)Buttons.MB1 )
                TryAddSpawner();

            if( id == (int)Buttons.MB2 )
                TryRemoveSpawner();

            if (id == (int)Buttons.MB3)
                TryAddSoldier();

            if (id == (int)Buttons.MB4)
                TryAddMilitaryAdvisor();

            if (id == (int)Buttons.MB5)
                TryRemoveMilitaryAdvisor();
        }

        public void TryAddSpawner()
        {
            if (BadNavigation() || !AllowAccess(true))
                return;

            if (m_Guild == null || !(m_Guild is GovernmentEntity))
                return;

            if (GovernmentEntity.IsGuildMilitary(m_Viewer, m_Guild))
            {
                foreach (Item item in m_Viewer.GetItemsInRange(0))
                {
                    if (item is MilitarySpawner && ((MilitarySpawner)item).Government == m_Guild)
                    {
                        m_Viewer.SendMessage("Your organization already has a military spawner at your current location.");
                        return;
                    }
                }

                MilitarySpawner spawner = new MilitarySpawner((GovernmentEntity)m_Guild);

                spawner.MoveToWorld(m_Viewer.Location, m_Viewer.Map);

                m_Viewer.SendMessage("You have successfully added a military spawner at your current location. Station a soldier there to begin using it.");
            }
            else
                m_Viewer.SendMessage("You must be military personnel of " + m_Guild.Name.ToString() + " to do that.");
        }

        public void TryRemoveSpawner()
        {
            if (BadNavigation() || !AllowAccess(true))
                return;

            if (m_Guild == null || !(m_Guild is GovernmentEntity))
                return;

            if (GovernmentEntity.IsGuildMilitary(m_Viewer, m_Guild))
            {
                foreach (Item item in m_Viewer.GetItemsInRange(0))
                {
                    if (item is MilitarySpawner && ((MilitarySpawner)item).Government == m_Guild)
                    {
                        item.Delete();
                        m_Viewer.SendMessage("You have successfully removed a military spawner from your current location.");
                        return;
                    }
                }

                m_Viewer.SendMessage("No military spawner belonging to your organization was found at your current location.");
            }
            else
                m_Viewer.SendMessage("You must be military personnel of " + m_Guild.Name.ToString() + " to do that.");
        }

        public void TryAddSoldier()
        {
            if (BadNavigation() || !AllowAccess(true))
                return;

            if (m_Guild == null || !(m_Guild is GovernmentEntity))
                return;

            if (GovernmentEntity.IsGuildMilitary(m_Viewer, m_Guild))
            {
                m_Viewer.Target = new SoldierTarget((GovernmentEntity)m_Guild);
                m_Viewer.CloseGump(typeof(GovernmentGump));
                m_Viewer.SendMessage("Target a spawner belonging to " + m_Guild.Name + " to station a soldier at.");
            }
            else
                m_Viewer.SendMessage("You must be military personnel " + m_Guild.Name.ToString() + " to do that.");
        }

        public void TryAddMilitaryAdvisor()
        { 
            if (BadNavigation() || !AllowAccess(true))
                return;

            if (m_Guild == null || !(m_Guild is GovernmentEntity))
                return;

            if (GovernmentEntity.IsGuildLeader(m_Viewer, m_Guild))
            {
                m_Viewer.Target = new AddMilitaryAdvisorTarget((GovernmentEntity)m_Guild);
                m_Viewer.CloseGump(typeof(GovernmentGump));
                m_Viewer.SendMessage("Target a location to station " + m_Guild.Name + "'s military advisor.");
            }
            else
                m_Viewer.SendMessage("You must be the leader of " + m_Guild.Name + " to do that.");
        }

        public void TryRemoveMilitaryAdvisor()
        {
            if (BadNavigation() || !AllowAccess(true))
                return;

            if (m_Guild == null || !(m_Guild is GovernmentEntity))
                return;

            if (GovernmentEntity.IsGuildLeader(m_Viewer, m_Guild))
                m_Viewer.Target = new RemoveMilitaryAdvisorTarget(m_Guild as GovernmentEntity);
            else
                m_Viewer.SendMessage("You must be the leader of " + m_Guild.Name.ToString() + " to do that.");
        }

        public void WayPointsOnResponse( int id )
        {
            if( id == (int)Buttons.MB1 )
                TryAddWayPoint();

            if( id == (int)Buttons.MB2 )
                TryRemoveWayPoint();
        }

        public void TryAddWayPoint()
        {
            if (BadNavigation() || !AllowAccess(true))
                return;

            if (m_Guild == null || !(m_Guild is GovernmentEntity))
                return;

            if (GovernmentEntity.IsGuildMilitary(m_Viewer, m_Guild))
            {
                foreach (Item item in m_Viewer.GetItemsInRange(0))
                {
                    if (item is MilitaryWayPoint && ((MilitaryWayPoint)item).Government == m_Guild)
                    {
                        m_Viewer.SendMessage("Your organization already has a military way point at your current location.");
                        return;
                    }
                }

                MilitaryWayPoint wayPoint = new MilitaryWayPoint((GovernmentEntity)m_Guild);
                wayPoint.MoveToWorld(m_Viewer.Location, m_Viewer.Map);
                m_Viewer.SendMessage("You have successfully added a military way point at your current location. Double-click it to link it to another guard way point.");
            }
            else
                m_Viewer.SendMessage("You must be military personnel of " + m_Guild.Name.ToString() + " to do that.");

        }

        public void TryRemoveWayPoint()
        {
            if (BadNavigation() || !AllowAccess(true))
                return;

            if (m_Guild == null || !(m_Guild is GovernmentEntity))
                return;

            if (GovernmentEntity.IsGuildMilitary(m_Viewer, m_Guild))
            {
                foreach (Item item in m_Viewer.GetItemsInRange(0))
                {
                    if (item is MilitaryWayPoint && ((MilitaryWayPoint)item).Government == m_Guild)
                    {
                        item.Delete();
                        m_Viewer.SendMessage("You have successfully removed a military way point from your current location.");
                        return;
                    }
                }

                m_Viewer.SendMessage("No military way point belonging to your organization was found at your current location.");
            }
            else
                m_Viewer.SendMessage("You must be military personnel of " + m_Guild.Name.ToString() + " to do that.");

        }

        public override void SendNewGump()
        {
            if(m_Guild is GovernmentEntity)
                m_Viewer.SendGump( new GovernmentGump( m_Viewer, (GovernmentEntity)m_Guild, m_CurrentNav, m_NavScroll, m_CurrentMain, m_MainScroll ) );
            else
                m_Viewer.SendGump(new OrganizationGump(m_Viewer, m_Guild, m_CurrentNav, m_NavScroll, m_CurrentMain, m_MainScroll));
        }
    }
}
