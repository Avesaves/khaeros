using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Misc;
using Server.Mobiles;
using Server.BackgroundInfo;
using Server.Targeting;

namespace Server.Gumps
{
    public class BackgroundsGump : Gump
    {
    	private static int m_BackgroundsPerPage = 6;
    	private static int m_SelectedCheckBox = 2511;
    	private static int m_UnselectedCheckBox = 2510;
    	private static int m_UnavailableBackground = 2360;
    	private static int m_AvailableBackground = 2362;
    	private static int m_AcquiredBackground = 2361;
    	private static int m_UnselectedBackground = 10810;
    	private static int m_SelectedBackground = 10830;
    	private static int m_UnSelectableBackground = 10850;
        private PlayerMobile m_Viewer = null;
    	private PlayerMobile m_Viewed = null;
    	private BackgroundList m_Viewing = BackgroundList.None;
    	private int m_Scroll = 0;
    	private bool m_AllowPurchase = false;
    	private static BaseBackground[] OurBackgrounds = Backgrounds.AllBackgrounds;
    	
        public static void Initialize()
        {
            CommandSystem.Register( "Backgrounds", AccessLevel.Player, new CommandEventHandler(Backgrounds_OnCommand) );
            CommandSystem.Register( "ViewBackgrounds", AccessLevel.GameMaster, new CommandEventHandler( ViewBackgrounds_OnCommand ) );
        }

        [Usage( "Backgrounds" )]
		[Description( "Sends the Backgrounds Gump." )]
		private static void Backgrounds_OnCommand( CommandEventArgs e )
		{
			PlayerMobile m = e.Mobile as PlayerMobile;
			m.SendGump( new BackgroundsGump(m) );
		}

        [Usage( "ViewBackgrounds" )]
        [Description( "Allows you to see someone's Backgrounds." )]
        private static void ViewBackgrounds_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
            m.Target = new ViewBackgroundsTarget();
        }

        private class ViewBackgroundsTarget : Target
        {
            public ViewBackgroundsTarget()
                : base( 15, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile from, object targeted )
			{
                if( targeted != null && targeted is PlayerMobile && from != null && from is PlayerMobile )
                    from.SendGump( new BackgroundsGump( (PlayerMobile)from, (PlayerMobile)targeted ) );
            }
        }
		
		public BackgroundsGump( PlayerMobile from ) : this( from, from, BackgroundList.None, 0, false ) {}

        public BackgroundsGump( PlayerMobile viewer, PlayerMobile viewed ) : this( viewer, viewed, BackgroundList.None, 0, true ) { }

        public BackgroundsGump( PlayerMobile from, BackgroundList viewing, int scroll, bool allowPurchase ) : this( from, from, viewing, scroll, allowPurchase ) { }

        public BackgroundsGump( PlayerMobile viewer, PlayerMobile viewed, BackgroundList viewing, int scroll, bool allowPurchase ) : base( 0, 0 )
        {
        	if( viewer == null || viewer.Deleted || viewed == null || viewed.Deleted )
        		return;
        	
        	InitialSetup( viewer, viewed, viewing, scroll, allowPurchase );
        	AddBackgroundAndImages();
        	AddMainMenu();
        	AddPurchaseMenu();
        }
        
        public void InitialSetup( PlayerMobile viewer, PlayerMobile viewed, BackgroundList viewing, int scroll, bool allowPurchase )
        {
            m_Viewed = viewed;
            m_Viewer = viewer;
        	m_Viewing = viewing;
        	m_AllowPurchase = allowPurchase;
        	m_Scroll = Math.Max( 0, scroll );
        	Closable = true;
			Disposable = true;
			Dragable = true;
			Resizable = false;
        }
        
        public void AddBackgroundAndImages()
        {
        	AddPage( 0 );
        	
			AddBackground( 207, 54, 372, 213, 9270 );
			AddBackground( 222, 69, 341, 182, 3500 );
			AddBackground( 295, 32, 194, 52, 9270 );
			AddBackground( 242, 296, 306, 272, 9270 );
			AddBackground( 257, 311, 275, 241, 3500 );
			
			AddImage( 156, 10, 10400 );
			AddImage( 548, 10, 10410 );
			AddImage( 156, 180, 10402 );
			AddImage( 548, 180, 10412 );
			AddImage( 266, 257, 10450 );
			AddImage( 485, 257, 10450 );
			AddImage( 305, 257, 10452 );
        }
        
        public int GetCheckBox( BackgroundList background )
        {
        	return (background == m_Viewing ? m_SelectedCheckBox : m_UnselectedCheckBox);
        }
        
        public int GetInfoColour( BaseBackground background )
        {
        	if( background.HasThisBackground(m_Viewed) )
        		return m_AcquiredBackground;
        	
        	return (background.MeetsOurRequirements(m_Viewed) == true ? m_AvailableBackground : m_UnavailableBackground); 
        }
        
        public void AddMainMenu()
        {
        	int height = 345;
        	
        	for( int i = 0; (i + (m_Scroll * m_BackgroundsPerPage)) < OurBackgrounds.Length && i < m_BackgroundsPerPage; i++ )
    		{
        		BaseBackground background = m_Viewed.Backgrounds.BackgroundDictionary[OurBackgrounds[i + (m_Scroll * m_BackgroundsPerPage)].ListName];
    			AddLabel( 309, height, 0, background.Name );
    			AddButton( 272, (height + 2), GetCheckBox( background.ListName ), m_UnselectedCheckBox, (i + (int)Buttons.Background1), GumpButtonType.Reply, 0 );
				AddImage( 291, (height + 5), GetInfoColour(background) );
				height += 30;
    		}
    		
    		if( m_Scroll > 0 )
    			AddButton( 489, 328, 250, 251, (int)Buttons.ScrollUp, GumpButtonType.Reply, 0 );
    		
    		if( OurBackgrounds.Length > ((m_Scroll * m_BackgroundsPerPage) + m_BackgroundsPerPage) )
				AddButton( 489, 515, 252, 253, (int)Buttons.ScrollDown, GumpButtonType.Reply, 0 );
        }
        
        public void AddPurchaseMenu()
        {
        	BaseBackground background = GetActiveBackground();
        	AddPurchaseInfo( background );
        }
        
        public void AddPurchaseInfo( BaseBackground background )
        {
        	string description = "Click on the golden check box in front of a Background to see its description.";
        	string name = "Backgrounds";
        	
        	if( background != null )
        	{
	    		name = background.Name;
	    		description = background.FullDescription;
	    		
	    		if( background.MeetsOurRequirements(m_Viewed) && m_AllowPurchase )
	   				AddButton( 514, 147, (background.Level < 1 ? m_UnselectedBackground : m_SelectedBackground), m_SelectedBackground, (int)Buttons.Purchase, GumpButtonType.Reply, 0 );
        		
	    		else if( m_AllowPurchase )
	    			AddImage( 514, 147, m_UnSelectableBackground );
        	}
        	
        	int labelOffset = Math.Max( 0, (60 - (name.Length * 3)) );
        	int htmlOffset = 0;
        	
        	if( background == null || !m_AllowPurchase )
        		htmlOffset = 33;
        	
    		AddHtml( 247, 98, 258 + htmlOffset, 125, description, (bool)true, (bool)true );
			AddLabel( 330 + labelOffset, 48, 2010, name );
        }
        
        public BaseBackground GetActiveBackground()
        {
        	BaseBackground background = null;
        	
        	if( m_Viewing != BackgroundList.None )
        		background = m_Viewed.Backgrounds.BackgroundDictionary[m_Viewing];

        	return background;
        }

        public enum Buttons
		{
        	Exit = 0,
			Purchase = 1,
			Background1 = 2,
			Background2 = 3,
			Background3 = 4,
			Background4 = 5,
			Background5 = 6,
			Background6 = 7,
			ScrollUp = 8,
			ScrollDown = 9
		}
        
        public BackgroundList GetIndex( int offset )
        {
        	int index = (m_Scroll * m_BackgroundsPerPage) + offset;
        	return OurBackgrounds[index].ListName;
        }

        public override void OnResponse( NetState sender, RelayInfo info )
        {
            if( sender.Mobile == null || m_Viewed == null )
            	return;
            
            BaseBackground background = GetActiveBackground();
            
            switch( info.ButtonID )
            {
            	case (int)Buttons.ScrollUp: m_Scroll -= 1; break;
            	case (int)Buttons.ScrollDown: m_Scroll += 1; break;
            	case (int)Buttons.Purchase: background.AttemptPurchase( m_Viewed ); break;
            	case (int)Buttons.Background1: m_Viewing = GetIndex(0); break;
            	case (int)Buttons.Background2: m_Viewing = GetIndex(1); break;
            	case (int)Buttons.Background3: m_Viewing = GetIndex(2); break;
            	case (int)Buttons.Background4: m_Viewing = GetIndex(3); break;
            	case (int)Buttons.Background5: m_Viewing = GetIndex(4); break;
            	case (int)Buttons.Background6: m_Viewing = GetIndex(5); break;
            	default: break;
            }
            
            if( info.ButtonID > 0 )
            	m_Viewer.SendGump( new BackgroundsGump(m_Viewer, m_Viewed, m_Viewing, m_Scroll, m_AllowPurchase) );
        }
    }
}
