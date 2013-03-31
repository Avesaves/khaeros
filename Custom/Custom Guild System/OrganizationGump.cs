using System;
using System.Collections.Generic;
using Server.Network;
using Server.Commands;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
	public enum Navigation
	{
        //Common
		MyOrganizations,
		Ranks,
		Roster,
		Applicants,
		Hues,
		Politics,
        //Government Only
        Resources,
        Spawners,
        WayPoints,
	}
	
    public class OrganizationGump : Gump
    {
        public PlayerMobile m_Viewer = null;
        public CustomGuildStone m_Guild = null;
        public GuildRankInfo m_Rank = null;
        public PlayerMobile m_Member = null;
        public int m_CurrentNav;
        public int m_NavScroll;
        public int m_MainScroll;
        public int m_CurrentMain;
        public int m_ItemsPerPage;
        public int m_Start;
        public int m_Y;
        public int m_Current;
        public int m_LineHeight;
    	public virtual string[] m_NavItems { get{ return new string[]
    	{ 
    		"My Organizations", 
    		"Ranks", 
    		"Roster", 
    		"Applicants", 
    		"Hues", 
    		"Politics"
    	}; } }
    	
        public static void Initialize()
        {
            CommandSystem.Register( "Organizations", AccessLevel.Player, new CommandEventHandler(Organizations_OnCommand) );
        }

        [Usage( "Organizations" )]
        [Description( "Opens the gump with your organizations." )]
        public static void Organizations_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || e.Mobile.Deleted || !(e.Mobile is PlayerMobile) )
            	return;

            PlayerMobile from = e.Mobile as PlayerMobile;   

            if (e.ArgString.Trim() != null && e.ArgString.Trim() != "")
            {
                foreach (KeyValuePair<CustomGuildStone,CustomGuildInfo> kvp in from.CustomGuilds)
                {
                    if ((kvp.Key as CustomGuildStone).Name.ToLower() == e.ArgString.ToLower())
                    {
                        if(kvp.Key is GovernmentEntity)
                            from.SendGump(new GovernmentGump(from, kvp.Key));
                        else
                            from.SendGump(new OrganizationGump(from, kvp.Key));

                        continue;
                    }
                }
            }
            else
                from.SendGump( new OrganizationGump(from) );
        }

        public OrganizationGump( PlayerMobile from ) : this( from, null, 0, 0, 0, 0 ) {}
        
        public OrganizationGump( PlayerMobile from, CustomGuildStone guild ) : this( from, guild, 0, 0, 0, 0 ) {}
        
        public OrganizationGump( PlayerMobile from, CustomGuildStone guild, int currentNav, int navScroll, int currentMain, int mainScroll ) 
        	: this( from, guild, currentNav, navScroll, currentMain, mainScroll, null, null ) {}
        	
        public OrganizationGump( PlayerMobile from, CustomGuildStone guild, int currentNav, int navScroll, int currentMain, int mainScroll, 
                                GuildRankInfo rank, PlayerMobile member ) : base( 0, 0 )
        {
        	InitialSetup( from, guild, currentNav, navScroll, currentMain, mainScroll, rank, member );
        	AddNavigation();
        	AddMain();
        }
        
        public virtual void InitialSetup( PlayerMobile from, CustomGuildStone guild, int currentNav, int navScroll, int currentMain, int mainScroll, 
                                GuildRankInfo rank, PlayerMobile member )
        {
        	Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
			m_Guild = guild;
			m_CurrentNav = currentNav;
			m_NavScroll = navScroll;
			m_CurrentMain = currentMain;
			m_MainScroll = mainScroll;
			m_Rank = rank;
			m_Member = member;
			m_ItemsPerPage = 9;
			m_Start = m_ItemsPerPage * m_NavScroll;
			m_Y = 228;
    		m_LineHeight = 28;
			m_Viewer = from;
			m_Viewer.CloseGump( typeof(OrganizationGump) );
			AddPage( 0 );
			AddBackground( 64, 114, 671, 55, 9270 );
			AddLabel( 215, 132, 2010, @"Khaeros'   Government   and   Private   Organization   System" );
			AddBackground( 64, 169, 203, 338, 9270 );
			AddBackground( 267, 169, 468, 338, 9270 );
			AddBackground( 283, 185, 435, 305, 3500 );
			AddBackground( 80, 185, 170, 305, 3500 );
			AddBackground( 64, 507, 671, 55, 9270 );
			AddImage( 703, 61, 10441 );
			AddImage( 16, 61, 10440 );
			
			if( m_Guild != null && !String.IsNullOrEmpty(m_Guild.Name) )
				AddLabel( 82, 525, 2010, "Organization:   " + m_Guild.Name.Replace(" ", "   ") );
        }
        
        public virtual void AddNavigation()
        {
        	AddLabel( 127, 198, 2010, @"Navigation" );
        	
        	for( m_Current = 0; m_Current < m_ItemsPerPage && (m_Start + m_Current) < m_NavItems.Length; m_Current++ )
        	{
        		AddLabel( 117, (m_Y + (m_Current * m_LineHeight)), 0, m_NavItems[m_Start + m_Current] );
        		AddButton( 97, (m_Y + 2 + (m_Current * m_LineHeight)), (m_Start + m_Current) == m_CurrentNav ? 2511 : 2510, 2511, 
        		          ((int)Buttons.NB1 + m_Current), GumpButtonType.Reply, 0 );
        	}
        	
        	if( m_NavItems.Length > (m_Start + m_Current + 1) )
        		AddButton( 222, 462, 2648, 2649, (int)Buttons.NavDown, GumpButtonType.Reply, 0 );
        	
        	if( m_NavScroll > 0 )
				AddButton( 222, 193, 2650, 2651, (int)Buttons.NavUp, GumpButtonType.Reply, 0 );
        }
        
        public virtual void AddMain()
        {
        	switch( m_CurrentNav )
        	{
        		case (int)Navigation.MyOrganizations: AddMyOrganizations(); break;
        		case (int)Navigation.Ranks: AddRanks(); break;
        		case (int)Navigation.Roster: AddRoster(); break;
        		case (int)Navigation.Applicants: AddApplicants(); break;
        		case (int)Navigation.Hues: AddHues(); break;
        		case (int)Navigation.Politics: AddPolitics(); break;
        	}
        }
        
        public void AddMainScroll( int total, int next )
        {
        	if( total > next )
        		AddButton( 690, 462, 2648, 2649, (int)Buttons.MainDown, GumpButtonType.Reply, 0 );
        	
        	if( m_MainScroll > 0 )
				AddButton( 690, 193, 2650, 2651, (int)Buttons.MainUp, GumpButtonType.Reply, 0 );
        }
        
        public void AddMainLabels( string title, string one, string two, string three )
        {
        	if( !String.IsNullOrEmpty(title) )
        		AddLabel( 315, 228, 0, title );
        	
        	if( !String.IsNullOrEmpty(one) )
        		AddLabel( 515, 228, 0, one );
			
        	if( !String.IsNullOrEmpty(two) )
        		AddLabel( 580, 228, 0, two );
			
        	if( !String.IsNullOrEmpty(three) )
        		AddLabel( 645, 228, 0, three );
        }
        
        public void AddMainApplyCancelButtons()
        {
        	AddButton( 436, 453, 239, 240, (int)Buttons.Apply, GumpButtonType.Reply, 0 );
        	AddButton( 505, 453, 243, 241, (int)Buttons.Cancel, GumpButtonType.Reply, 0 );
        }
        
        public void AddMyOrganizations()
        {
        	AddLabel( 315, 198, 2010, "My Organizations" );
        	m_ItemsPerPage = 8;
        	m_Start = m_ItemsPerPage * m_MainScroll;
        	m_Y = 253;
        	AddMainLabels( "Organization", "Apply", "Member", "View" );
        	
			for( m_Current = 0; m_Current < m_ItemsPerPage && (m_Start + m_Current) < CustomGuildStone.Guilds.Count; m_Current++ )
        	{
       			AddLabel( 315, (m_Y + (m_Current * m_LineHeight)), 0, CustomGuildStone.Guilds[m_Current + m_Start].Name );
        		
       			AddButton( 515, (m_Y + 2 + (m_Current * m_LineHeight)), CustomGuildStone.Guilds[m_Current + m_Start].Applicants.Contains(m_Viewer) 
       			          == true ? 10742 : 10740, 10742, (int)Buttons.MB1 + m_Current, GumpButtonType.Reply, 0 );


				AddButton( 580, (m_Y + 2 + (m_Current * m_LineHeight)), CustomGuildStone.Guilds[m_Current + m_Start].Members.Contains(m_Viewer) 
       			          == true ? 10742 : 10740, 10742, (int)Buttons.MB9 + m_Current, GumpButtonType.Reply, 0 );
       			AddButton( 645, (m_Y + 2 + (m_Current * m_LineHeight)), m_Guild != null && 
       			          CustomGuildStone.Guilds[m_Start + m_Current] == m_Guild ? 10742 : 10740, 10742,
       			          (int)Buttons.MB17 + m_Current, GumpButtonType.Reply, 0 );
        	}
			
			AddMainScroll( CustomGuildStone.Guilds.Count, (m_Start + m_Current) );
        }
        
        public void AddRanks()
        {
        	if( m_Rank != null )
        	{
        		AddEditRank();
        		return;
        	}
        	
        	AddLabel( 315, 198, 2010, "Ranks" );
        	m_ItemsPerPage = 8;
        	m_Start = m_ItemsPerPage * m_MainScroll;
        	m_Y = 253;
        	AddMainLabels( "Rank", "Add", "Remove", "Edit" );
        	
        	if( m_Guild == null )
        		return;
        	
			for( m_Current = 0; m_Current < m_ItemsPerPage && (m_Start + m_Current) < m_Guild.Ranks.Count; m_Current++ )
        	{
				int id = m_Current + m_Start + 1;
				AddLabel( 315, (m_Y + (m_Current * m_LineHeight)), 0, id.ToString() + " - " + m_Guild.Ranks[id].Name );
       			AddButton( 515, (m_Y + 2 + (m_Current * m_LineHeight)), 10740, 10742, (int)Buttons.MB1 + m_Current, GumpButtonType.Reply, 0 );
				AddButton( 580, (m_Y + 2 + (m_Current * m_LineHeight)), 10740, 10742, (int)Buttons.MB9 + m_Current, GumpButtonType.Reply, 0 );
       			AddButton( 645, (m_Y + 2 + (m_Current * m_LineHeight)), 10740, 10742, (int)Buttons.MB17 + m_Current, GumpButtonType.Reply, 0 );
        	}
			
			AddMainScroll( m_Guild.Ranks.Count, (m_Start + m_Current) );
        }
        
        public void AddEditRank()
        {
        	AddLabel( 315, 198, 2010, "Rank " + m_Rank.Rank.ToString() );
        	m_Y = 228;
        	int x = 315;
			
        	if( m_Guild == null )
        		return;
        	
        	AddLabel( x, m_Y, 0, "Name:" ); AddTextEntry( x + 40, m_Y, 200, 20, 0, 1, m_Rank.Name ); m_Y += m_LineHeight;
        	AddLabel( x, m_Y, 0, "Title:" ); AddTextEntry( x + 36, m_Y, 200, 20, 0, 2, String.IsNullOrEmpty(m_Rank.Title) ? 
        	                                               "None" : m_Rank.Title ); m_Y += m_LineHeight;
        	AddLabel( x, m_Y, 0, "Prefix:" ); AddTextEntry( x + 46, m_Y, 200, 20, 0, 3, String.IsNullOrEmpty(m_Rank.Prefix) ? 
        	                                               "None" : m_Rank.Prefix ); m_Y += m_LineHeight;
			AddLabel( x, m_Y, 0, "Pay:" ); AddTextEntry( x + 31, m_Y, 200, 20, 0, 4, m_Rank.Pay.ToString() ); m_Y += m_LineHeight;
			AddLabel( x, m_Y, 0, "Fee:" ); AddTextEntry( x + 30, m_Y, 200, 20, 0, 5, m_Rank.Fee.ToString() ); m_Y += m_LineHeight;
			AddLabel( x, m_Y, 0, "Is Officer?" ); AddTextEntry( x + 73, m_Y, 20, 20, 0, 6, m_Rank.IsOfficer ? "Yes" : "No" ); m_Y += m_LineHeight;

            if (m_Guild is GovernmentEntity)
            { AddLabel(x, m_Y, 0, "Is Military?"); AddTextEntry(x + 73, m_Y, 20, 20, 0, 7, m_Rank.IsMilitary ? "Yes" : "No"); m_Y += m_LineHeight; } 

            if (m_Guild is GovernmentEntity)
            { AddLabel(x, m_Y, 0, "Is Economic?"); AddTextEntry(x + 78, m_Y, 20, 20, 0, 8, m_Rank.IsEconomic ? "Yes" : "No"); m_Y += m_LineHeight; }

			AddMainApplyCancelButtons();
        }
        
        public void AddRoster()
        {
        	if( m_Member != null && m_Guild != null && m_Member.CustomGuilds.ContainsKey(m_Guild) )
        	{
        		AddEditMember();
        		return;
        	}
        	
        	AddLabel( 315, 198, 2010, "Roster" );
        	m_ItemsPerPage = 8;
        	m_Start = m_ItemsPerPage * m_MainScroll;
        	m_Y = 253;
        	AddMainLabels( "Name", "Rank", "Remove", "Edit" );
        	
        	if( m_Guild == null )
        		return;
        	
			for( m_Current = 0; m_Current < m_ItemsPerPage && (m_Start + m_Current) < m_Guild.Members.Count; m_Current++ )
        	{
				PlayerMobile m = m_Guild.Members[m_Current + m_Start] as PlayerMobile;
				
				if( !m.CustomGuilds.ContainsKey(m_Guild) )
					continue;
				
				AddLabel( 315, (m_Y + (m_Current * m_LineHeight)), 0, m.CustomGuilds[m_Guild].RegistrationName );
				AddLabel( 515, (m_Y + (m_Current * m_LineHeight)), 0, m.CustomGuilds[m_Guild].RankID.ToString() );
				AddButton( 580, (m_Y + 2 + (m_Current * m_LineHeight)), 10740, 10742, (int)Buttons.MB9 + m_Current, GumpButtonType.Reply, 0 );
       			AddButton( 645, (m_Y + 2 + (m_Current * m_LineHeight)), 10740, 10742, (int)Buttons.MB17 + m_Current, GumpButtonType.Reply, 0 );
        	}
			
			AddMainScroll( m_Guild.Members.Count, (m_Start + m_Current) );
        }
        
        public void AddEditMember()
        {
			AddLabel( 315, 198, 2010, "Member: " + m_Member.CustomGuilds[m_Guild].RegistrationName );
        	m_Y = 253;
        	int x = 315;
			
        	if( m_Guild == null )
        		return;
        	
        	AddLabel( x, m_Y, 0, "Rank:" ); AddTextEntry( x + 37, m_Y, 200, 20, 0, 1, m_Member.CustomGuilds[m_Guild].RankID.ToString() ); m_Y += m_LineHeight;
			AddLabel( x, m_Y, 0, "Active Title?" ); AddTextEntry( x + 83, m_Y, 20, 20, 0, 2, m_Member.CustomGuilds[m_Guild].ActiveTitle ? "Yes" : "No" ); m_Y += m_LineHeight;
			AddLabel( x, m_Y, 0, "Balance: " + m_Member.CustomGuilds[m_Guild].Balance.ToString() ); m_Y += m_LineHeight;
			AddLabel( x, m_Y, 0, "Withdraw Funds?" ); AddTextEntry( x + 107, m_Y, 20, 20, 0, 3, "No" ); m_Y += m_LineHeight;
			AddLabel( x, m_Y, 0, "Deposit Funds?" ); AddTextEntry( x + 95, m_Y, 20, 20, 0, 4, "No" ); m_Y += m_LineHeight;
			AddMainApplyCancelButtons();
        }
        
        public void AddApplicants()
        {
        	AddLabel( 315, 198, 2010, "Applicants" );
        	m_ItemsPerPage = 8;
        	m_Start = m_ItemsPerPage * m_MainScroll;
        	m_Y = 253;
        	AddMainLabels( "Name", null, "Accept", "Deny" );
        	
        	if( m_Guild == null )
        		return;
        	
			for( m_Current = 0; m_Current < m_ItemsPerPage && (m_Start + m_Current) < m_Guild.Applicants.Count; m_Current++ )
        	{
				AddLabel( 315, (m_Y + (m_Current * m_LineHeight)), 0, ((PlayerMobile)m_Guild.Applicants[m_Current + m_Start]).CustomGuilds[m_Guild].RegistrationName );

				AddButton( 580, (m_Y + 2 + (m_Current * m_LineHeight)), 10740, 10742, (int)Buttons.MB9 + m_Current, GumpButtonType.Reply, 0 );
       			AddButton( 645, (m_Y + 2 + (m_Current * m_LineHeight)), 10740, 10742, (int)Buttons.MB17 + m_Current, GumpButtonType.Reply, 0 );
        	}
			
			AddMainScroll( m_Guild.Applicants.Count, (m_Start + m_Current) );
        }
        
        public void AddHues()
        {
        	AddLabel( 315, 198, 2010, "Hues" );
        	m_Y = 253;
        	int x = 315;
			
        	if( m_Guild == null )
        		return;
        	
        	AddLabel( x, m_Y, 0, "Dye Clothing" );
        	AddButton( 515, (m_Y + 2), 10740, 10742, (int)Buttons.MB1, GumpButtonType.Reply, 0 );
        	m_Y += m_LineHeight;
			AddLabel( x, m_Y, 0, "Dye Armour" );
			AddButton( 515, (m_Y + 2), 10740, 10742, (int)Buttons.MB2, GumpButtonType.Reply, 0 );
        }
        
        public void AddPolitics()
        {
        	AddLabel( 315, 198, 2010, "Politics" );
        	m_ItemsPerPage = 8;
        	m_Start = m_ItemsPerPage * m_MainScroll;
        	m_Y = 253;
        	AddMainLabels( "Organization", "Ally", "Neutral", "Enemy" );
        	
        	if( m_Guild == null )
        		return;
        	
			for( m_Current = 0; m_Current < m_ItemsPerPage && (m_Start + m_Current) < CustomGuildStone.Guilds.Count; m_Current++ )
        	{
       			AddLabel( 315, (m_Y + (m_Current * m_LineHeight)), 0, CustomGuildStone.Guilds[m_Current + m_Start].Name );
        		
       			if( CustomGuildStone.Guilds[m_Current + m_Start] == m_Guild )
       				continue;
       			
       			bool ally = m_Guild.AlliedGuilds.Contains(CustomGuildStone.Guilds[m_Current + m_Start]);
       			bool enemy = m_Guild.EnemyGuilds.Contains(CustomGuildStone.Guilds[m_Current + m_Start]);
       			bool neutral = !ally && !enemy;
       			
       			AddButton( 515, (m_Y + 2 + (m_Current * m_LineHeight)), ally ? 10742 : 10740, 10742, 
       			          (int)Buttons.MB1 + m_Current, GumpButtonType.Reply, 0 );
				AddButton( 580, (m_Y + 2 + (m_Current * m_LineHeight)), neutral ? 10742 : 10740, 10742, 
       			          (int)Buttons.MB9 + m_Current, GumpButtonType.Reply, 0 );
       			AddButton( 645, (m_Y + 2 + (m_Current * m_LineHeight)), enemy ? 10742 : 10740, 10742, 
       			          (int)Buttons.MB17 + m_Current, GumpButtonType.Reply, 0 );
        	}
			
			AddMainScroll( CustomGuildStone.Guilds.Count, (m_Start + m_Current + 1) );
        }

        public enum Buttons
		{
        	Close, NavUp, NavDown, MainUp, MainDown, Apply, Cancel,
			MB1, MB2, MB3, MB4, MB5, MB6, MB7, MB8,
			MB9, M10, MB11, MB12, MB13, MB14, MB15, MB16,
			MB17, MB18, MB19, MB20, MB21, MB22, MB23, MB24,
			NB1, NB2, NB3, NB4, NB5, NB6, NB7, NB8, NB9
		}

        public override void OnResponse( NetState sender, RelayInfo info )
        {
        	if( m_Viewer == null || m_Viewer != sender.Mobile )
        		return;

            if( info.ButtonID >= (int)Buttons.NB1 && info.ButtonID <= (int)Buttons.NB9 )
            {
            	m_CurrentNav = (m_NavScroll * m_ItemsPerPage) + (info.ButtonID - (int)Buttons.NB1); 
            	m_MainScroll = 0;
            	m_CurrentMain = 0;
            }
            
            if( info.ButtonID == (int)Buttons.NavUp )
            	m_NavScroll--;
            
            if( info.ButtonID == (int)Buttons.NavDown )
            	m_NavScroll++;
            
            if( info.ButtonID == (int)Buttons.MainUp )
            	m_MainScroll--;
            
            if( info.ButtonID == (int)Buttons.MainDown )
            	m_MainScroll++;

            if( info.ButtonID >= (int)Buttons.MB1 && info.ButtonID <= (int)Buttons.MB24 )
                if( HandleMainPageUse( info ) )
                    return;

            if (info.ButtonID == (int)Buttons.Apply && m_Rank != null && m_Guild != null)
            {
                if (m_Guild is GovernmentEntity)
                {
                    m_Guild.TryToEditRank(m_Viewer, m_Rank, info.GetTextEntry(1).Text, info.GetTextEntry(2).Text, info.GetTextEntry(3).Text,
                                          info.GetTextEntry(4).Text, info.GetTextEntry(5).Text, info.GetTextEntry(6).Text, info.GetTextEntry(7).Text,
                                          info.GetTextEntry(8).Text);
                }
                else
                {
                    m_Guild.TryToEditRank(m_Viewer, m_Rank, info.GetTextEntry(1).Text, info.GetTextEntry(2).Text, info.GetTextEntry(3).Text,
                                          info.GetTextEntry(4).Text, info.GetTextEntry(5).Text, info.GetTextEntry(6).Text);
                }
            }
            
            if( info.ButtonID == (int)Buttons.Apply && m_Member != null && m_Guild != null )
            	m_Guild.TryToEditMember( m_Viewer, m_Member, info.GetTextEntry(1).Text, info.GetTextEntry(2).Text, info.GetTextEntry(3).Text,
            	                      info.GetTextEntry(4).Text );

            if( info.ButtonID > (int)Buttons.Close )
                SendNewGump();
        }

        public virtual bool HandleMainPageUse( RelayInfo info )
        {
            switch( m_CurrentNav )
            {
                case (int)Navigation.MyOrganizations: MyOrganizationsOnResponse( info.ButtonID ); break;
                case (int)Navigation.Ranks: RanksOnResponse( info.ButtonID ); return true;
                case (int)Navigation.Roster: RosterOnResponse( info.ButtonID ); return true;
                case (int)Navigation.Applicants: ApplicantsOnResponse( info.ButtonID ); break;
                case (int)Navigation.Hues: HuesOnResponse( info.ButtonID ); break;
                case (int)Navigation.Politics: PoliticsOnResponse( info.ButtonID ); break;
            }

            return false;
        }

        public virtual void SendNewGump()
        {
            if(m_Guild is GovernmentEntity)
                m_Viewer.SendGump( new GovernmentGump(m_Viewer, (GovernmentEntity)m_Guild, m_CurrentNav, m_NavScroll, m_CurrentMain, m_MainScroll) );
            else
                m_Viewer.SendGump( new OrganizationGump( m_Viewer, m_Guild, m_CurrentNav, m_NavScroll, m_CurrentMain, m_MainScroll ) );
        }
        
        public void MyOrganizationsOnResponse( int id )
        {
        	if( id >= (int)Buttons.MB1 && id <= (int)Buttons.MB8 )
        	{
        		int index = id - (int)Buttons.MB1 + m_Start;
        		
        		if( CustomGuildStone.Guilds.Count > index )
        			CustomGuildStone.Guilds[index].TryToApply( m_Viewer );
        	}
        	
        	if( id >= (int)Buttons.MB9 && id <= (int)Buttons.MB16 )
        	{
        		int index = id - (int)Buttons.MB9 + m_Start;
        		
        		if( CustomGuildStone.Guilds.Count > index )
        			CustomGuildStone.Guilds[index].TryToResign( m_Viewer );
        	}
        	
        	if( id >= (int)Buttons.MB17 && id <= (int)Buttons.MB24 )
        	{
        		int index = id - (int)Buttons.MB17 + m_Start;

                if( CustomGuildStone.Guilds.Count > index && CustomGuildStone.HasViewingRights( m_Viewer, CustomGuildStone.Guilds[index], true ) )
        			m_Guild = CustomGuildStone.Guilds[index];
        	}
        }
        
        public void RanksOnResponse( int id )
        {
        	if( m_Guild == null )
        		return;
        	
        	if( id >= (int)Buttons.MB1 && id <= (int)Buttons.MB8 )
        	{
        		int index = id - (int)Buttons.MB1 + m_Start + 1;
        		
        		if( m_Guild.Ranks.ContainsKey(index) )
        			m_Guild.TryToAddRankAfter( m_Viewer, index );
        		
        		else
        			m_Viewer.SendMessage( "Rank " + id.ToString() + " not found." );

                SendNewGump();
        	}
        	
        	if( id >= (int)Buttons.MB9 && id <= (int)Buttons.MB16 )
        	{
        		int index = id - (int)Buttons.MB9 + m_Start + 1;
        		
        		if( m_Guild.Ranks.ContainsKey(index) )
        			m_Guild.TryToRemoveRank( m_Viewer, index );
        		
        		else
        			m_Viewer.SendMessage( "Rank " + id.ToString() + " not found." );

                SendNewGump();
        	}
        	
        	if( id >= (int)Buttons.MB17 && id <= (int)Buttons.MB24 )
        	{
        		int index = id - (int)Buttons.MB17 + m_Start + 1;
        		
        		if( m_Guild.Ranks.ContainsKey(index) )      
                {
                    if(m_Guild is GovernmentEntity)
                        m_Viewer.SendGump( new GovernmentGump(m_Viewer, (GovernmentEntity)m_Guild, m_CurrentNav, m_NavScroll, m_CurrentMain, m_MainScroll, m_Guild.Ranks[index], null) );
                    else
                        m_Viewer.SendGump( new OrganizationGump( m_Viewer, m_Guild, m_CurrentNav, m_NavScroll, m_CurrentMain, m_MainScroll, m_Guild.Ranks[index], null ) );
                }
        		
        		else
        			m_Viewer.SendMessage( "Rank " + id.ToString() + " not found." );
        	}
        }
        
        public void RosterOnResponse( int id )
        {
        	if( m_Guild == null )
        		return;
        	
        	if( id >= (int)Buttons.MB9 && id <= (int)Buttons.MB16 )
        	{
        		int index = id - (int)Buttons.MB9 + m_Start;
        		
        		if( m_Guild != null && m_Guild.Members.Count > index )
        			m_Guild.TryToRemoveMember( m_Viewer, (PlayerMobile)m_Guild.Members[index] );

                SendNewGump();
        	}
        	
        	if( id >= (int)Buttons.MB17 && id <= (int)Buttons.MB24 )
        	{
        		int index = id - (int)Buttons.MB17 + m_Start;

                if (m_Guild != null && m_Guild.Members.Count > index)
                {
                    if (m_Guild is GovernmentEntity)
                        m_Viewer.SendGump(new GovernmentGump(m_Viewer, (GovernmentEntity)m_Guild, m_CurrentNav, m_NavScroll, m_CurrentMain, m_MainScroll,
                            null, (PlayerMobile)m_Guild.Members[index]));
                    else        			
                        m_Viewer.SendGump( new OrganizationGump(m_Viewer, m_Guild, m_CurrentNav, m_NavScroll, m_CurrentMain, m_MainScroll, 
        			                                        null, (PlayerMobile)m_Guild.Members[index]) );
                }
        	}
        }
        
        public void ApplicantsOnResponse( int id )
        {
        	if( m_Guild == null )
        		return;
        	
        	if( id >= (int)Buttons.MB9 && id <= (int)Buttons.MB16 )
        	{
        		int index = id - (int)Buttons.MB9 + m_Start;
        		
        		if( m_Guild != null && m_Guild.Applicants.Count > index )
        			m_Guild.TryToAccept( m_Viewer, (PlayerMobile)m_Guild.Applicants[index] );
        	}
        	
        	if( id >= (int)Buttons.MB17 && id <= (int)Buttons.MB24 )
        	{
        		int index = id - (int)Buttons.MB17 + m_Start;
        		
        		if( m_Guild != null && m_Guild.Applicants.Count > index )
        			m_Guild.TryToDeny( m_Viewer, (PlayerMobile)m_Guild.Applicants[index] );
        	}
        }
        
        public void HuesOnResponse( int id )
        {
        	if( m_Guild == null )
        		return;
        	
        	if( !m_Guild.OfficialGuild )
        	{
        		m_Viewer.SendMessage( "This feature is only available for governmental organizations." );
        		return;
        	}
        	
        	if( id == (int)Buttons.MB1 )
        	{
        		if( CustomGuildStone.IsGuildMember(m_Viewer, m_Guild, true) )
        			m_Viewer.Target = new GuildDyeTarget( m_Viewer, m_Guild.ClothingHue, true, m_Guild );
        	}
        	
        	else if( id == (int)Buttons.MB2 )
        	{
        		if( CustomGuildStone.IsGuildMember(m_Viewer, m_Guild, true) )
        			m_Viewer.Target = new GuildDyeTarget( m_Viewer, m_Guild.ArmourHue, false, m_Guild );
        	}
        }
        
        public void PoliticsOnResponse( int id )
        {
        	if( m_Guild == null )
        		return;
        	
        	if( id >= (int)Buttons.MB1 && id <= (int)Buttons.MB8 )
        	{
        		int index = id - (int)Buttons.MB1 + m_Start;
        		
        		if( CustomGuildStone.Guilds.Count > index )
        			m_Guild.TryToAddAlly( m_Viewer, CustomGuildStone.Guilds[index] );
        	}
        	
        	if( id >= (int)Buttons.MB9 && id <= (int)Buttons.MB16 )
        	{
        		int index = id - (int)Buttons.MB9 + m_Start;
        		
        		if( CustomGuildStone.Guilds.Count > index )
        			m_Guild.TryToAddNeutral( m_Viewer, CustomGuildStone.Guilds[index] );
        	}
        	
        	if( id >= (int)Buttons.MB17 && id <= (int)Buttons.MB24 )
        	{
        		int index = id - (int)Buttons.MB17 + m_Start;
        		
        		if( CustomGuildStone.Guilds.Count > index )
        			m_Guild.TryToAddEnemy( m_Viewer, CustomGuildStone.Guilds[index] );
        	}
        }
    }
}
