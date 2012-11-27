using System;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using System.Collections;
using System.Collections.Generic;
using Server.Commands;
using Server.Targeting;
using Server.Prompts;

namespace Server.Misc.ImprovedAI
{
	public class Commands
	{
		public static void Initialize()
		{
			CommandSystem.Register( "SetStance", AccessLevel.Player, new CommandEventHandler( SetStance_OnCommand ) );
			CommandSystem.Register( "SetManeuver", AccessLevel.Player, new CommandEventHandler( SetManeuver_OnCommand ) );
		}
		
		public static List<KeyValuePair<string, BaseStance>> ValidStances
		{
			get
			{
				List<KeyValuePair<string, BaseStance>> list = new List<KeyValuePair<string, BaseStance>>();
				list.Add( new KeyValuePair<string, BaseStance>("Flurry of Blows", new FlurryOfBlows()) );
				list.Add( new KeyValuePair<string, BaseStance>("Focused Attack", new FocusedAttack()) );
				list.Add( new KeyValuePair<string, BaseStance>("Defensive Stance", new DefensiveStance()) );
				list.Add( new KeyValuePair<string, BaseStance>("Swift Shot", new SwiftShot()) );
				list.Add( new KeyValuePair<string, BaseStance>("Focused Shot", new FocusedShot()) );
				list.Add( new KeyValuePair<string, BaseStance>("None", new BaseStance()) );
				return list;
			}
		}
		
		public static List<KeyValuePair<string, BaseCombatManeuver>> ValidManeuvers
		{
			get
			{
				List<KeyValuePair<string, BaseCombatManeuver>> list = new List<KeyValuePair<string, BaseCombatManeuver>>();
				list.Add( new KeyValuePair<string, BaseCombatManeuver>("Critical Strike", new CriticalStrike()) );
				list.Add( new KeyValuePair<string, BaseCombatManeuver>("Crippling Blow", new CripplingBlow()) );
				list.Add( new KeyValuePair<string, BaseCombatManeuver>("Savage Strike", new SavageStrike()) );
				list.Add( new KeyValuePair<string, BaseCombatManeuver>("Flashy Attack", new FlashyAttack()) );
				list.Add( new KeyValuePair<string, BaseCombatManeuver>("Critical Shot", new CriticalShot()) );
				list.Add( new KeyValuePair<string, BaseCombatManeuver>("Crippling Shot", new CripplingShot()) );
				list.Add( new KeyValuePair<string, BaseCombatManeuver>("None", new BaseCombatManeuver()) );
				return list;
			}
		}
		
		[Usage( "SetStance" )]
        [Description( "Allows you to change the preferred stance of one of your followers." )]
        private static void SetStance_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;

        	BaseStance stance = null;
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	string text = e.ArgString.ToLower().Replace( " ", "" );
        	
        	foreach( KeyValuePair<string, BaseStance> kvp in ValidStances )
        	{
        		if( kvp.Key.ToLower().Replace( " ", "" ) == text )
        			stance = kvp.Value;
        	}
        	
        	if( stance != null)
        	{
        		if( text == "none" )
		    		text = null;
        		
		    	m.SendMessage( "Change the preferred stance of which of your followers?" );
		    	m.Target = new SetStanceTarget( stance, text );
        	}
        	
        	else
        	{
        		m.SendMessage( "Invalid option. The following uses of this command are valid:" );
        		
        		foreach( KeyValuePair<string, BaseStance> kvp in ValidStances )
	        	{
	        		m.SendMessage( ".SetStance " + kvp.Key );
	        	}
        	}
        }
        
        private class SetStanceTarget : Target
		{
        	private BaseStance stance;
        	private string text;
        	
			public SetStanceTarget( BaseStance newStance, string Text ) : base( -1, false, TargetFlags.None )
			{
				stance = newStance;
				text = Text;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is BaseCreature && ((BaseCreature)targeted).ControlMaster == from && stance.CanUseThisStance((BaseCreature)targeted) )
				{
					((BaseCreature)targeted).SetStance = stance;
					((BaseCreature)targeted).VerifiedFavouriteStance( text );
					from.SendMessage( "You have successfully changed your follower's preferred stance." );
				}
			}
        }
        
        [Usage( "SetManeuver" )]
        [Description( "Allows you to change the preferred maneuver of one of your followers." )]
        private static void SetManeuver_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;

        	BaseCombatManeuver maneuver = null;
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	string text = e.ArgString.ToLower().Replace( " ", "" );
        	
        	foreach( KeyValuePair<string, BaseCombatManeuver> kvp in ValidManeuvers )
        	{
        		if( kvp.Key.ToLower().Replace( " ", "" ) == text )
        			maneuver = kvp.Value;
        	}
        	
        	if( maneuver != null)
        	{
        		if( text == "none" )
		    		text = null;
        		
		    	m.SendMessage( "Change the preferred maneuver of which of your followers?" );
		    	m.Target = new SetManeuverTarget( maneuver, text );
        	}
        	
        	else
        	{
        		m.SendMessage( "Invalid option. The following uses of this command are valid:" );
        		
        		foreach( KeyValuePair<string, BaseCombatManeuver> kvp in ValidManeuvers )
	        	{
	        		m.SendMessage( ".SetManeuver " + kvp.Key );
	        	}
        	}
        }
        
        private class SetManeuverTarget : Target
		{
        	private BaseCombatManeuver maneuver;
        	private string text;
        	
			public SetManeuverTarget( BaseCombatManeuver newManeuver, string Text ) : base( -1, false, TargetFlags.None )
			{
				maneuver = newManeuver;
				Text = text;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is BaseCreature && ((BaseCreature)targeted).ControlMaster == from && maneuver.CanUseThisManeuver((BaseCreature)targeted) )
				{
					((BaseCreature)targeted).SetManeuver = maneuver;
					((BaseCreature)targeted).VerifiedFavouriteManeuver( text );
					from.SendMessage( "You have successfully changed your follower's preferred maneuver." );
				}
			}
        }
	}
}
