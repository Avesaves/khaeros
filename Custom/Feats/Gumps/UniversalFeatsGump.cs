using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Misc;
using Server.Mobiles;
using Server.FeatInfo;
using Server.Targeting;

namespace Server.Gumps
{
    public class UniversalFeatsGump : Gump
    {
    	private static int m_FeatsPerPage = 5;
    	private static int m_SelectedCheckBox = 2511;
    	private static int m_UnselectedCheckBox = 2510;
    	private static int m_UnavailableFeat = 2360;
    	private static int m_AvailableFeat = 2362;
    	private static int m_AcquiredFeat = 2361;
    	private static int m_UnselectedSkill1 = 1238;
    	private static int m_SelectedSkill1 = 1239;
    	private static int m_UnselectedSkill2 = 1241;
    	private static int m_SelectedSkill2 = 1242;
    	private static int m_UnselectedSkill3 = 1244;
    	private static int m_SelectedSkill3 = 1245;
        private PlayerMobile m_Viewer = null;
    	private PlayerMobile m_Viewed = null;
    	private List<FeatList> m_Previous = new List<FeatList>();
    	private FeatList m_Current = FeatList.None;
    	private FeatList m_Viewing = FeatList.None;
    	private int m_Scroll = 0;
    	
        public static void Initialize()
        {
            CommandSystem.Register( "MySkills", AccessLevel.Player, new CommandEventHandler(MySkills_OnCommand) );
            CommandSystem.Register( "ViewSkills", AccessLevel.GameMaster, new CommandEventHandler( ViewSkills_OnCommand ) );
        }

        [Usage( "MySkills" )]
		[Description( "Sends the Skills Gump." )]
		private static void MySkills_OnCommand( CommandEventArgs e )
		{
			PlayerMobile m = e.Mobile as PlayerMobile;
			m.SendGump( new UniversalFeatsGump(m) );
		}

        [Usage( "ViewSkills" )]
        [Description( "Allows you to see someone's Skills." )]
        private static void ViewSkills_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
            m.Target = new ViewSkillsTarget();
        }

        private class ViewSkillsTarget : Target
        {
            public ViewSkillsTarget()
                : base( 15, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile from, object targeted )
            {
                if( targeted != null && targeted is PlayerMobile && from != null && from is PlayerMobile )
                    from.SendGump( new UniversalFeatsGump( (PlayerMobile)from, (PlayerMobile)targeted ) );
            }
        }
		
		public UniversalFeatsGump( PlayerMobile from ) : this( from, from, new List<FeatList>(), FeatList.None, FeatList.None, 0 ) {}

        public UniversalFeatsGump( PlayerMobile viewer, PlayerMobile viewed ) : this( viewer, viewed, new List<FeatList>(), FeatList.None, FeatList.None, 0 ) { }

        public UniversalFeatsGump( PlayerMobile viewer, PlayerMobile viewed, List<FeatList> previous, FeatList current, FeatList viewing, int scroll ) : base( 0, 0 )
        {
            if( viewer == null || viewer.Deleted || viewed == null || viewed.Deleted )
        		return;
        	
        	InitialSetup( viewer, viewed, previous, current, viewing, scroll );
        	AddBackgroundAndImages();
        	AddMainMenu();
        	AddPurchaseMenu();
        }
        
        public void InitialSetup( PlayerMobile viewer, PlayerMobile viewed, List<FeatList> previous, FeatList current, FeatList viewing, int scroll )
        {
            m_Viewer = viewer;
            m_Viewed = viewed;
        	m_Previous = previous;
        	m_Current = current;
        	m_Viewing = viewing;
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
        
        public int GetCheckBox( FeatList skill )
        {
        	if( m_Viewing == FeatList.None )
        		return (skill == m_Current ? m_SelectedCheckBox : m_UnselectedCheckBox);
        	
        	return (skill == m_Viewing ? m_SelectedCheckBox : m_UnselectedCheckBox);
        }
        
        public int GetInfoColour( BaseFeat skill )
        {
        	if( skill.HasThisFeat(m_Viewed) )
        		return m_AcquiredFeat;
        	
        	return (skill.MeetsOurRequirements(m_Viewed) == true ? m_AvailableFeat : m_UnavailableFeat); 
        }
        
        public void AddMainMenu()
        {
        	if( m_Current == FeatList.None )
        		AddSlaveInfo( Feats.FeatsWithoutRequirements.ToArray() );
        	
        	else
        	{
                string name = "";
        		BaseFeat skill = Feats.ConvertFeatListToFeatObject( m_Current );

                if( m_Viewed != null && !skill.ShouldDisplayTo(m_Viewer) )
                    name = "???";

                else
                    name = skill.Name;

        		AddButton( 281, 512, 2466, 2467, (int)Buttons.Previous, GumpButtonType.Reply, 0 );
        		AddLabel( 309, 330, 0, name );
				AddButton( 272, 332, GetCheckBox( skill.ListName ), m_UnselectedCheckBox, (int)Buttons.PickMaster, GumpButtonType.Reply, 0 );
				AddImage( 291, 335, GetInfoColour(skill) );
				
				List<BaseFeat> list = new List<BaseFeat>();
				
				for( int i = 0; i < skill.Allows.Length; i++ )
					list.Add( Feats.ConvertFeatListToFeatObject(skill.Allows[i]) );
				
				AddSlaveInfo( list.ToArray() );
        	}
        }
        
        public void AddSlaveInfo( BaseFeat[] list )
        {
        	int height = 365;
            string name = "";
        	
        	for( int i = 0; (i + (m_Scroll * m_FeatsPerPage)) < list.Length && i < m_FeatsPerPage; i++ )
    		{
    			BaseFeat skill = list[i + (m_Scroll * m_FeatsPerPage)];

                if( m_Viewed != null && !skill.ShouldDisplayTo(m_Viewer) )
                    name = "???";

                else
                    name = skill.Name;

    			AddLabel( 309, height, 0, name );
    			AddButton( 272, (height + 2), GetCheckBox( skill.ListName ), m_UnselectedCheckBox, (i + (int)Buttons.PickSlave1), GumpButtonType.Reply, 0 );
				AddImage( 291, (height + 5), GetInfoColour(skill) );
				
				if( skill.Allows.Length > 0 )
					AddButton( 455, (height + 2), 2469, 2470, (i + (int)Buttons.Next1), GumpButtonType.Reply, 0 );
				
				height += 30;
    		}
    		
    		if( m_Scroll > 0 )
    			AddButton( 489, 328, 250, 251, (int)Buttons.ScrollUp, GumpButtonType.Reply, 0 );
    		
    		if( list.Length > ((m_Scroll * m_FeatsPerPage) + m_FeatsPerPage) )
				AddButton( 489, 515, 252, 253, (int)Buttons.ScrollDown, GumpButtonType.Reply, 0 );
        }
        
        public void AddPurchaseMenu()
        {
        	BaseFeat skill = GetActiveFeat();

        	AddPurchaseInfo( skill );
        }
        
        public void AddPurchaseInfo( BaseFeat skill )
        {
        	string description = "Click on the golden check box in front of a skill to see its description or click on Next to see what skills it gives access to.";
        	string name = "Skills";
        	
        	if( skill != null )
        	{
	    		name = skill.Name;
	    		description = skill.FullDescription;

                if( m_Viewed != null && !skill.ShouldDisplayTo( m_Viewer ) )
                {
                    name = "???";
                    description = "???";
                }

                else if( m_Viewed != null && skill.ShouldDisplayTo( m_Viewer ) && skill is VampireAbilities )
                    description = "Use .vp for more info.";
	    		
	    		if( skill.CostLevel > FeatCost.None )
	    		{
	    			AddButton( 512, 104, (skill.Level < 1 ? m_UnselectedSkill1 : m_SelectedSkill1), m_SelectedSkill1, (int)Buttons.Skill1, GumpButtonType.Reply, 0 );
					AddButton( 512, 144, (skill.Level < 2 ? m_UnselectedSkill2 : m_SelectedSkill2), m_SelectedSkill2, (int)Buttons.Skill2, GumpButtonType.Reply, 0 );
					AddButton( 512, 182, (skill.Level < 3 ? m_UnselectedSkill3 : m_SelectedSkill3), m_SelectedSkill3, (int)Buttons.Skill3, GumpButtonType.Reply, 0 );
	    		}
        	}
        	
        	int labelOffset = Math.Max( 0, (60 - (name.Length * 3)) );
        	int htmlOffset = (skill == null ? 33 : 0);
        	
    		AddHtml( 247, 98, 258 + htmlOffset, 125, description, (bool)true, (bool)true );
			AddLabel( 330 + labelOffset, 48, 2010, name );
        }
        
        public BaseFeat GetActiveFeat()
        {
        	BaseFeat skill = null;
        	
        	if( m_Viewing != FeatList.None )
        		skill = m_Viewed.Feats.FeatDictionary[m_Viewing];
        	
        	else if( m_Current != FeatList.None )
	        	skill = m_Viewed.Feats.FeatDictionary[m_Current];
        	
        	return skill;
        }

        public enum Buttons
		{
        	Exit,
			Skill1,
			Skill2,
			Skill3,
			Previous,
			Next1,
			Next2,
			Next3,
			Next4,
			Next5,
			PickMaster,
			PickSlave1,
			PickSlave2,
			PickSlave3,
			PickSlave4,
			PickSlave5,
			ScrollUp,
			ScrollDown
		}
        
        public FeatList GetIndex( int offset )
        {
        	int index = (m_Scroll * m_FeatsPerPage) + offset;
        	
        	if( m_Current == FeatList.None )
        		return Feats.FeatsWithoutRequirements[index].ListName;
        	
        	return Feats.ConvertFeatListToFeatObject( m_Current ).Allows[index];
        }

        public override void OnResponse( NetState sender, RelayInfo info )
        {
            if( sender.Mobile == null || m_Viewed == null )
            	return;
            
            BaseFeat skill = GetActiveFeat();
            bool freeRemoval = (m_Viewer.AccessLevel > AccessLevel.Player || (m_Viewed.Forging || m_Viewed.Reforging));
            
            switch( info.ButtonID )
            {
            	case (int)Buttons.ScrollUp: m_Scroll -= 1; break;
            	case (int)Buttons.ScrollDown: m_Scroll += 1; break;
            	case (int)Buttons.Skill1: skill.AttemptPurchase( m_Viewed, 1, freeRemoval ); break;
            	case (int)Buttons.Skill2: skill.AttemptPurchase( m_Viewed, 2, freeRemoval ); break;
            	case (int)Buttons.Skill3: skill.AttemptPurchase( m_Viewed, 3, freeRemoval ); break;
            	case (int)Buttons.Previous: m_Current = m_Previous[m_Previous.Count - 1]; m_Previous.Remove( m_Current ); m_Scroll = 0; break;
            	case (int)Buttons.PickSlave1: m_Viewing = GetIndex(0); break;
            	case (int)Buttons.PickSlave2: m_Viewing = GetIndex(1); break;
            	case (int)Buttons.PickSlave3: m_Viewing = GetIndex(2); break;
            	case (int)Buttons.PickSlave4: m_Viewing = GetIndex(3); break;
            	case (int)Buttons.PickSlave5: m_Viewing = GetIndex(4); break;
            	case (int)Buttons.PickMaster: m_Viewing = m_Current; break;
            	case (int)Buttons.Next1: m_Previous.Add( m_Current ); m_Current = GetIndex(0); m_Viewing = m_Current; m_Scroll = 0; break;
            	case (int)Buttons.Next2: m_Previous.Add( m_Current ); m_Current = GetIndex(1); m_Viewing = m_Current; m_Scroll = 0; break;
            	case (int)Buttons.Next3: m_Previous.Add( m_Current ); m_Current = GetIndex(2); m_Viewing = m_Current; m_Scroll = 0; break;
            	case (int)Buttons.Next4: m_Previous.Add( m_Current ); m_Current = GetIndex(3); m_Viewing = m_Current; m_Scroll = 0; break;
            	case (int)Buttons.Next5: m_Previous.Add( m_Current ); m_Current = GetIndex(4); m_Viewing = m_Current; m_Scroll = 0; break;
            	default: break;
            }
            
            if( info.ButtonID > 0 )
            	m_Viewer.SendGump( new UniversalFeatsGump(m_Viewer, m_Viewed, m_Previous, m_Current, m_Viewing, m_Scroll) );
        }
    }
}
