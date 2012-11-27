using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Prompts;

namespace Server.Commands
{
	public class LookForRP
	{
		public static void Initialize()
		{
			CommandSystem.Register( "FreeForRP", AccessLevel.Player, new CommandEventHandler( FreeForRP_OnCommand ) );
			CommandSystem.Register( "LookForRP", AccessLevel.Player, new CommandEventHandler( LookForRP_OnCommand ) );
		}
		
		[Usage( "FreeForRP" )]
        [Description( "Adds/Removes yourself from the list of characters looking for RP." )]
        private static void FreeForRP_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;

    		if( m.FreeForRP )
    		{
    			m.SendMessage( "Free For RP Off." );
    			m.FreeForRP = false;
    		}
    		
    		else
    		{
    			m.SendMessage( "Free For RP On." );
    			m.FreeForRP = true;
    		}
        }
        
        [Usage( "LookForRP" )]
        [Description( "Shows a list of characters available for RP." )]
        private static void LookForRP_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	List<string> list = new List<string>();

        	foreach( NetState state in NetState.Instances )
			{
				PlayerMobile player = state.Mobile as PlayerMobile;

				if( state.Mobile != null && state.Mobile != m && state.Mobile is PlayerMobile && player.FreeForRP && player.Name != null && player.Name.Length > 0 )
					list.Add( player.Name );
			}
        	
        	if( list.Count > 0 )
        	{
	        	m.SendMessage( "List of online characters looking for RP:" );

	        	foreach( string st in list )
	        		m.SendMessage( st );
	        	
	        	m.SendMessage( "Please type the name of one of the characters mentioned above." );
	        	m.Prompt = new LookForRPPrompt();
        	}
        	
        	else
        		m.SendMessage( "There are currently no players online looking for RP." );
        }        

		private class LookForRPPrompt : Prompt
		{
			public LookForRPPrompt()
			{
			}
	
			public override void OnResponse( Mobile from, string text )
			{
				PlayerMobile found = null;
				
				foreach( NetState state in NetState.Instances )
				{
					PlayerMobile player = state.Mobile as PlayerMobile;
	
					if( state.Mobile != null && state.Mobile is PlayerMobile && player.FreeForRP && player.Name == text )
						found = player;
				}
				
				if( found != null && found.Alive )
				{
					int distance = (int)from.GetDistanceToSqrt( found.Location );
					from.SendMessage( found.Name + " is currently " + distance.ToString() + " squares away in this direction." );
					from.QuestArrow = new FindRPArrow( (PlayerMobile)from, found );
				}
				
				else
					from.SendMessage( "No character with that name was found." );
			}
		}
		
		private class FindRPArrow : QuestArrow
		{
			private PlayerMobile m_From;
			private Timer m_Timer;
	
			public FindRPArrow( PlayerMobile from, PlayerMobile target ) : base( from )
			{
				m_From = from;
				m_Timer = new TrackTimer( from, target, this );
				m_Timer.Start();
			}
	
			public override void OnClick( bool rightClick )
			{
				if ( rightClick )
				{
					m_From = null;
					Stop();
				}
			}
	
			public override void OnStop()
			{
				m_Timer.Stop();
	
				if ( m_From != null )
					m_From.SendLocalizedMessage( 503177 ); // You have lost your quarry.
			}
		}
	
		private class TrackTimer : Timer
		{
			private PlayerMobile m_From, m_Target;
			private int m_LastX, m_LastY;
			private QuestArrow m_Arrow;
	
			public TrackTimer( PlayerMobile from, PlayerMobile target, QuestArrow arrow ) : base( TimeSpan.FromSeconds( 0.25 ), TimeSpan.FromSeconds( 2.5 ) )
			{
				m_From = from;
				m_Target = target;
				m_Arrow = arrow;
			}
	
			protected override void OnTick()
			{
				if ( !m_Arrow.Running )
				{
					Stop();
					return;
				}
				else if ( m_From.NetState == null || m_From.Deleted || m_Target.Deleted || m_From.Map != m_Target.Map || !m_Target.FreeForRP )
				{
					m_From.Send( new CancelArrow() );
					m_From.SendLocalizedMessage( 503177 ); // You have lost your quarry.
					Stop();
					return;
				}
	
				if ( m_LastX != m_Target.X || m_LastY != m_Target.Y )
				{
					m_LastX = m_Target.X;
					m_LastY = m_Target.Y;
	
					m_Arrow.Update( m_LastX, m_LastY );
				}
			}
		}
	}
}
