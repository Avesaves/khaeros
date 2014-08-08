using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Server;
using Server.Accounting;
using Server.Items;
using Server.Prompts;
using Server.Gumps;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Spells;
using Server.Misc;
using Server.Targeting;
using CPA = Server.CommandPropertyAttribute;
using Server.Commands;
using Server.Commands.Generic;
using Server.TimeSystem;
using Server.Engines.XmlSpawner2;
using Server.Regions;
using System.Text;
using System.Xml;
using Server.Misc.ImprovedAI;
using Server.Misc.BreedingSystem;

namespace Server.Commands
{	
	public class LevelSystemCommands
	{
		public static void Initialize() 
		{
			CommandSystem.Register( "Reforge", AccessLevel.Player, new CommandEventHandler( Reforge_OnCommand ) );
			CommandSystem.Register( "Bandage", AccessLevel.Player, new CommandEventHandler( Bandage_OnCommand ) );
			CommandSystem.Register( "BandageSelf", AccessLevel.Player, new CommandEventHandler( BandageSelf_OnCommand ) );
			CommandSystem.Register( "LogMsgs", AccessLevel.Counselor, new CommandEventHandler( LogMsgs_OnCommand ) );
			CommandSystem.Register( "PureDodge", AccessLevel.Player, new CommandEventHandler( PureDodge_OnCommand ) );
			CommandSystem.Register( "ToggleVendor", AccessLevel.Player, new CommandEventHandler( ToggleVendor_OnCommand ) );
			CommandSystem.Register( "IncBody", AccessLevel.Counselor, new CommandEventHandler( IncBody_OnCommand ) );
			CommandSystem.Register( "DecBody", AccessLevel.Counselor, new CommandEventHandler( DecBody_OnCommand ) );
			CommandSystem.Register( "SetBody", AccessLevel.Counselor, new CommandEventHandler( SetBody_OnCommand ) );
			CommandSystem.Register( "ChangeName", AccessLevel.Player, new CommandEventHandler( ChangeName_OnCommand ) );
			CommandSystem.Register( "CreateAccount", AccessLevel.GameMaster, new CommandEventHandler( CreateAccount_OnCommand ) );
			CommandSystem.Register( "AcceptCharacter", AccessLevel.GameMaster, new CommandEventHandler( AcceptCharacter_OnCommand ) );
			CommandSystem.Register( "Technique", AccessLevel.Player, new CommandEventHandler( Technique_OnCommand ) );
			CommandSystem.Register( "MassSmelt", AccessLevel.Player, new CommandEventHandler( MassSmelt_OnCommand ) );
			CommandSystem.Register( "CraftContainer", AccessLevel.Player, new CommandEventHandler( CraftContainer_OnCommand ) );
			CommandSystem.Register( "GemHarvesting", AccessLevel.Player, new CommandEventHandler( GemHarvesting_OnCommand ) );
			CommandSystem.Register( "Visit", AccessLevel.GameMaster, new CommandEventHandler( Visit_OnCommand ) );
			CommandSystem.Register( "Buypack", AccessLevel.GameMaster, new CommandEventHandler( Buypack_OnCommand ) );
			CommandSystem.Register( "InspireHeroics", AccessLevel.Player, new CommandEventHandler( InspireHeroics_OnCommand ) );
			CommandSystem.Register( "InspireResilience", AccessLevel.Player, new CommandEventHandler( InspireResilience_OnCommand ) );
			CommandSystem.Register( "SongOfMockery", AccessLevel.Player, new CommandEventHandler( SongOfMockery_OnCommand ) );
			CommandSystem.Register( "SongOfEnthrallment", AccessLevel.Player, new CommandEventHandler( SongOfEnthrallment_OnCommand ) );
			CommandSystem.Register( "InspireFortitude", AccessLevel.Player, new CommandEventHandler( InspireFortitude_OnCommand ) );
			CommandSystem.Register( "CancelCommand", AccessLevel.Player, new CommandEventHandler( CancelCommand_OnCommand ) );
			CommandSystem.Register( "ExpeditiousRetreat", AccessLevel.Player, new CommandEventHandler( ExpeditiousRetreat_OnCommand ) );
			CommandSystem.Register( "SpeedHack", AccessLevel.Counselor, new CommandEventHandler( SpeedHack_OnCommand ) );
			CommandSystem.Register( "DrumsOfWar", AccessLevel.Player, new CommandEventHandler( DrumsOfWar_OnCommand ) );
			CommandSystem.Register( "AutoPicking", AccessLevel.Player, new CommandEventHandler( AutoPicking_OnCommand ) );
			CommandSystem.Register( "BreakLock", AccessLevel.Player, new CommandEventHandler( BreakLock_OnCommand ) );
			CommandSystem.Register( "PetStealing", AccessLevel.Player, new CommandEventHandler( PetStealing_OnCommand ) );
			CommandSystem.Register( "Cutpurse", AccessLevel.Player, new CommandEventHandler( Cutpurse_OnCommand ) );
			CommandSystem.Register( "Stash", AccessLevel.Player, new CommandEventHandler( Stash_OnCommand ) );
			CommandSystem.Register( "JudgeWealth", AccessLevel.Player, new CommandEventHandler( JudgeWealth_OnCommand ) );
			CommandSystem.Register( "Split", AccessLevel.Player, new CommandEventHandler( Split_OnCommand ) );
			CommandSystem.Register( "ClearStudentList", AccessLevel.Player, new CommandEventHandler( ClearStudentList_OnCommand ) );
			CommandSystem.Register( "Teach", AccessLevel.Player, new CommandEventHandler( Teach_OnCommand ) );
			CommandSystem.Register( "Student", AccessLevel.Player, new CommandEventHandler( Student_OnCommand ) );
			CommandSystem.Register( "NameEquip", AccessLevel.Player, new CommandEventHandler( NameEquip_OnCommand ) );
			CommandSystem.Register( "ClearAllyList", AccessLevel.Player, new CommandEventHandler( ClearAllyList_OnCommand ) );
			CommandSystem.Register( "Password", AccessLevel.Player, new CommandEventHandler( Password_OnCommand ) );
			CommandSystem.Register( "Spar", AccessLevel.Player, new CommandEventHandler( Spar_OnCommand ) );
			CommandSystem.Register( "StaffAccount", AccessLevel.Administrator, new CommandEventHandler( StaffAccount_OnCommand ) );
			CommandSystem.Register( "HideStatus", AccessLevel.Counselor, new CommandEventHandler( HideStatus_OnCommand ) );
			CommandSystem.Register( "FollowerSay", AccessLevel.Player, new CommandEventHandler( MercSay_OnCommand ) );
			CommandSystem.Register( "FollowerEmote", AccessLevel.Player, new CommandEventHandler( MercEmote_OnCommand ) );
			CommandSystem.Register( "FollowerMount", AccessLevel.Player, new CommandEventHandler( MercMount_OnCommand ) );
			CommandSystem.Register( "FollowerDismount", AccessLevel.Player, new CommandEventHandler( MercDismount_OnCommand ) );
			CommandSystem.Register( "FollowerName", AccessLevel.Player, new CommandEventHandler( MercName_OnCommand ) );
			CommandSystem.Register( "FollowerTitle", AccessLevel.Player, new CommandEventHandler( MercTitle_OnCommand ) );
			CommandSystem.Register( "Birthday", AccessLevel.Player, new CommandEventHandler( Birthday_OnCommand ) );
			CommandSystem.Register( "GrantSlot", AccessLevel.Administrator, new CommandEventHandler( GrantSlot_OnCommand ) );
			CommandSystem.Register( "HearParty", AccessLevel.GameMaster, new CommandEventHandler( HearParty_OnCommand ) );
			CommandSystem.Register( "HearAll", AccessLevel.GameMaster, new CommandEventHandler( HearAll_OnCommand ) );
			CommandSystem.Register( "HairStyling", AccessLevel.Player, new CommandEventHandler( HairStyling_OnCommand ) );
			CommandSystem.Register( "CharInfo", AccessLevel.Player, new CommandEventHandler( CharInfo_OnCommand ) );
			CommandSystem.Register( "Say", AccessLevel.Player, new CommandEventHandler( Say_OnCommand ) );
            CommandSystem.Register("CreateEcho", AccessLevel.Player, new CommandEventHandler(CreateEcho_OnCommand));
            CommandSystem.Register("Telepathy", AccessLevel.Player, new CommandEventHandler(Telepathy_OnCommand));
			CommandSystem.Register( "Emote", AccessLevel.Counselor, new CommandEventHandler( Emote_OnCommand ) );
			CommandSystem.Register( "LastSay", AccessLevel.Counselor, new CommandEventHandler( LastSay_OnCommand ) );
			CommandSystem.Register( "LastEmote", AccessLevel.Counselor, new CommandEventHandler( LastEmote_OnCommand ) );
			CommandSystem.Register( "CustomNPC", AccessLevel.GameMaster, new CommandEventHandler( CustomNPC_OnCommand ) );
			CommandSystem.Register( "Grab", AccessLevel.Player, new CommandEventHandler( Grab_OnCommand ) );
			CommandSystem.Register( "AddAlly", AccessLevel.Player, new CommandEventHandler( AddAlly_OnCommand ) );
			CommandSystem.Register( "TurnInto", AccessLevel.GameMaster, new CommandEventHandler( TurnInto_OnCommand ) );
			CommandSystem.Register( "RemoveAlly", AccessLevel.Player, new CommandEventHandler( RemoveAlly_OnCommand ) );
			CommandSystem.Register( "XP", AccessLevel.Player, new CommandEventHandler( XP_OnCommand ) );
			CommandSystem.Register( "Experience", AccessLevel.Player, new CommandEventHandler( XP_OnCommand ) );
			CommandSystem.Register( "Level", AccessLevel.Player, new CommandEventHandler( XP_OnCommand ) );
			CommandSystem.Register( "CheckExp", AccessLevel.Player, new CommandEventHandler( CheckExp_OnCommand ) );
			CommandSystem.Register( "CheckXP", AccessLevel.Player, new CommandEventHandler( CheckExp_OnCommand ) );
			CommandSystem.Register( "CheckExperience", AccessLevel.Player, new CommandEventHandler( CheckExp_OnCommand ) );
			CommandSystem.Register( "CheckLevel", AccessLevel.Player, new CommandEventHandler( CheckExp_OnCommand ) );
			CommandSystem.Register( "WoodStaining", AccessLevel.Player, new CommandEventHandler( WoodStaining_OnCommand ) );
			CommandSystem.Register( "AdvancedDying", AccessLevel.Player, new CommandEventHandler( AdvancedDying_OnCommand ) );
			CommandSystem.Register( "Look", AccessLevel.Player, new CommandEventHandler( Look_OnCommand ) );
			CommandSystem.Register( "MasterworkEquip", AccessLevel.Player, new CommandEventHandler( MasterworkEquip_OnCommand ) );
			CommandSystem.Register( "Enamel", AccessLevel.Player, new CommandEventHandler( Enamel_OnCommand ) );
			CommandSystem.Register( "Resists", AccessLevel.Player, new CommandEventHandler( Resists_OnCommand ) );
			CommandSystem.Register( "SpeakLanguage", AccessLevel.Player, new CommandEventHandler( SpeakLanguage_OnCommand ) );
			CommandSystem.Register( "StatPoints", AccessLevel.Player, new CommandEventHandler( StatBonus_OnCommand ) );
            CommandSystem.Register( "FightingStyle", AccessLevel.Player, new CommandEventHandler( FightingStyle_OnCommand ) );
            CommandSystem.Register( "AddBackpack", AccessLevel.Player, new CommandEventHandler( AddBackpack_OnCommand ) );
            CommandSystem.Register( "UpdateStats", AccessLevel.Player, new CommandEventHandler( UpdateStats_OnCommand ) );
            CommandSystem.Register( "RopeTrick", AccessLevel.Player, new CommandEventHandler( RopeTrick_OnCommand ) );
            CommandSystem.Register( "EscortPrisoner", AccessLevel.Player, new CommandEventHandler( EscortPrisoner_OnCommand ) );
            CommandSystem.Register( "Carry", AccessLevel.Player, new CommandEventHandler( Carry_OnCommand ) );
            CommandSystem.Register( "Intimidate", AccessLevel.Player, new CommandEventHandler( Intimidate_OnCommand ) );
            //CommandSystem.Register( "Throw", AccessLevel.Player, new CommandEventHandler( Throw_OnCommand ) );
            CommandSystem.Register( "Trample", AccessLevel.Player, new CommandEventHandler( Trample_OnCommand ) );
            CommandSystem.Register( "Rage", AccessLevel.Player, new CommandEventHandler( Rage_OnCommand ) );
            CommandSystem.Register( "WeaponSpecialization", AccessLevel.Player, new CommandEventHandler( WeaponSpecialization_OnCommand ) );
            CommandSystem.Register( "TravelingShot", AccessLevel.Player, new CommandEventHandler( TravelingShot_OnCommand ) );
            CommandSystem.Register( "EngravePottery", AccessLevel.Player, new CommandEventHandler( EngravePottery_OnCommand ) );
            CommandSystem.Register( "EngraveMasonry", AccessLevel.Player, new CommandEventHandler( EngraveMasonry_OnCommand ) );
            CommandSystem.Register( "Forge", AccessLevel.Player, new CommandEventHandler( Forge_OnCommand ) );
            CommandSystem.Register( "BiteCoin", AccessLevel.Player, new CommandEventHandler( BiteCoin_OnCommand ) );
            CommandSystem.Register( "DefensiveFury", AccessLevel.Player, new CommandEventHandler( DefensiveFury_OnCommand ) );
            CommandSystem.Register( "CraftingSpecialization", AccessLevel.Player, new CommandEventHandler( CraftingSpecialization_OnCommand ) );
            CommandSystem.Register("SecondWind", AccessLevel.Player, new CommandEventHandler(SecondWind_OnCommand));
            CommandSystem.Register("GoToGreenAcres", AccessLevel.GameMaster, new CommandEventHandler(GoToGreenAcres_OnCommand));
            CommandSystem.Register("Burn", AccessLevel.GameMaster, new CommandEventHandler(Burn_OnCommand));
            CommandSystem.Register("Dazzle", AccessLevel.Player, new CommandEventHandler(Dazzle_OnCommand)); 
        }
		
		
		public static string AddSpacesToString( string st )
		{
			char[] stchars = st.ToCharArray();
			string formst = st;
			int offset = 0;
        				
			for( int i = 1; i < st.Length; ++i )
			{
				char ch = stchars[i];
				
				if( char.IsUpper( ch ) )
				{
					formst = formst.Insert( i + offset, " " );
					offset++;
				}
			}
			
			return formst;
		}
		
		public static string GetRacialPowerName( PlayerMobile caster, FeatList featname )
		{
			switch( caster.Nation )
			{
				case Nation.Tirebladd:
				{
					if( featname == FeatList.SummonProtector )
						return "Summon Servant of Ohlm";
					
					return "Consecrate Weapon";
				}
					
				case Nation.Northern:
				{
					if( featname == FeatList.SummonProtector )
						return "Summon Divine Protector";
					
					return "Holy Water";
				}
			}
			
			return "a spell";
		}
        
        [Usage("GoToGreenAcres")]
        [Description("Takes you to the GM areas.")]
        private static void GoToGreenAcres_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Location = new Point3D(5398, 3652, 0);
        }

        [Usage( "ClearAllyList" )]
        [Description( "Allows you to remove all mobiles from your ally list." )]
        private static void ClearAllyList_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m != null && m is PlayerMobile )
        	{
        		m.AllyList.Clear();
        		m.SendMessage( 60, "You have successfully cleared your ally list." );
        	}
        }
        
        [Usage( "Spar" )]
        [Description( "Allows you to fight without risk of mauling your partner beyond recognition." )]
        private static void Spar_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m != null && m is PlayerMobile )
        	{
        		if( m.Spar )
        		{
        			m.Spar = false;
        			m.SendMessage( "Spar Mode Off." );
        		}
        		
        		else if( !m.Spar )
        		{
        			m.Spar = true;
        			m.SendMessage( "Spar Mode On." );
        		}
        	}
        }
        
        [Usage( "GemHarvesting" )]
        [Description( "Allows you to turn your Gem Harvesting on and off." )]
        private static void GemHarvesting_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m != null && m is PlayerMobile && m.Feats.GetFeatLevel(FeatList.GemHarvesting) > 0 )
        	{
        		if( m.GemHarvesting )
        		{
        			m.GemHarvesting = false;
        			m.SendMessage( "Gem Harvesting Off." );
        		}
        		
        		else if( !m.GemHarvesting )
        		{
        			m.GemHarvesting = true;
        			m.SendMessage( "Gem Harvesting On." );
        		}
        	}
        }
        
        [Usage( "BreakLock" )]
        [Description( "Allows you pick a lock and leave it unlocked." )]
        private static void BreakLock_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m != null && m is PlayerMobile && m.Feats.GetFeatLevel(FeatList.Locksmith) > 1 )
        	{
        		if( m.BreakLock )
        		{
        			m.BreakLock = false;
        			m.SendMessage( "Break Lock Off." );
        		}
        		
        		else if( !m.BreakLock )
        		{
        			m.BreakLock = true;
        			m.SendMessage( "Break Lock On." );
        		}
        	}
        }
        
        [Usage( "AutoPicking" )]
        [Description( "Allows you pick a lock without a lockpick." )]
        private static void AutoPicking_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m != null && m is PlayerMobile && m.Feats.GetFeatLevel(FeatList.Locksmith) > 2 )
        	{
        		if( m.AutoPicking )
        		{
        			m.AutoPicking = false;
        			m.SendMessage( "Auto Picking Off." );
        		}
        		
        		else if( !m.AutoPicking )
        		{
        			m.AutoPicking = true;
        			m.SendMessage( "Auto Picking On." );
        		}
        	}
        }
		
		[Usage( "HideStatus" )]
        [Description( "Allows you to check the date of your birth." )]
        private static void HideStatus_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m != null && m is PlayerMobile )
        	{
        		if( m.HideStatus )
        		{
        			m.HideStatus = false;
        			m.SendMessage( "Hide Status Off." );
        		}
        		
        		else if( !m.HideStatus )
        		{
        			m.HideStatus = true;
        			m.SendMessage( "Hide Status On." );
        		}
        	}
        }
        
        [Usage( "CancelCommand" )]
        [Description( "Allows you to stop singing your current command." )]
        private static void CancelCommand_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m != null && m is PlayerMobile )
        	{
    			m.CurrentCommand = SongList.None;
    			m.SendMessage( "You halt your commands." );
        	}
        }
        
        [Usage( "SpeedHack" )]
        [Description( "Allows you to run on horseback speed." )]
        private static void SpeedHack_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m != null && m is PlayerMobile )
        	{
        		if( m.SpeedHack )
        		{
        			m.SpeedHack = false;
        			m.SendMessage( "Speed Hack Off." );
        		}
        		
        		else if( !m.SpeedHack )
        		{
        			m.SpeedHack = true;
        			m.SendMessage( "Speed Hack On." );
        		}
        	}
        }
        
        public static void FormatDayAndMonth( ref string formattedday, ref string formattedmonth )
        {
        	if( formattedday.EndsWith( "1" ) )
        	   formattedday = formattedday + "st";
        	else if( formattedday.EndsWith( "2" ) )
        	   formattedday = formattedday + "nd";
        	else if( formattedday.EndsWith( "3" ) )
        	   formattedday = formattedday + "rd";
        	else
        	   formattedday = formattedday + "th";
        	
        	if( formattedmonth == "1" )
        		formattedmonth = "Elysius";
        	else if( formattedmonth == "2" )
        		formattedmonth = "Ohlmus";
        	else if( formattedmonth == "3" )
        		formattedmonth = "Solius";
        	else if( formattedmonth == "4" )
        		formattedmonth = "Arianthus";
        	else if( formattedmonth == "5" )
        		formattedmonth = "Maiseus";
        	else if( formattedmonth == "6" )
        		formattedmonth = "Kal'ius";
        	else if( formattedmonth == "7" )
        		formattedmonth = "Othevus";
        	else if( formattedmonth == "8" )
        		formattedmonth = "Kyrus";
        }
		
		[Usage( "Birthday" )]
        [Description( "Allows you to check the date of your birth." )]
        private static void Birthday_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m.DayOfBirth == null )
        	{
        		m.SendMessage( "You still have not gone through the creation process and, thus, do not have a date of birth yet." );
        		return;
        	}
        	
        	string formattedday = m.DayOfBirth;
        	string formattedmonth = m.MonthOfBirth;
        	
        	FormatDayAndMonth( ref formattedday, ref formattedmonth );
        	
        	m.SendMessage( "You were born on the " + formattedday + " of " + formattedmonth + ", " + m.YearOfBirth + "." );
        }
        
        [Usage( "MercMount" )]
        [Description( "Allows you to force one of your followers to mount one of your mounts." )]
        private static void MercMount_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.SendMessage( "Which of your followers do you wish to order to mount?" );
        	m.Target = new MercMountTarget();
        }
        
        private class MercMountTarget : Target
        {
            public MercMountTarget()
                : base( 15, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null )
            		return;
            	
            	if( obj is Mercenary && ( (Mercenary)obj ).Owner == m )
            	{
            		Mercenary merc = obj as Mercenary;
            		
            		if( !merc.Alive || merc.Deleted || merc.Map != m.Map || m.Frozen || m.Paralyzed || m.CantWalk )
            			m.SendMessage( "Invalid target." );
            		
            		else
            		{
            			m.SendMessage( "Which of your mounts do you wish to order your follower to mount on?" );
            			m.Target = new MountForMercTarget( merc );
            		}
            	}
            	
            	else
            		m.SendMessage( "Invalid target." );
            }
        }
        
        private class MountForMercTarget : Target
        {
        	private Mercenary merc;
        	
            public MountForMercTarget( Mercenary mercenary )
                : base( 15, false, TargetFlags.None )
            {
            	merc = mercenary;
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null || merc == null )
            		return;
            	
            	if( obj is BaseMount && ( (BaseMount)obj ).ControlMaster == m )
            	{
            		BaseMount mount = obj as BaseMount;
            		
            		if( mount.Rider != null || !mount.Alive || mount.Deleted || mount.Map != merc.Map || 
            		   mount.Frozen || mount.Paralyzed || mount.CantWalk || !merc.Alive || merc.Deleted || 
            		   merc.Map != m.Map || m.Frozen || m.Paralyzed || m.CantWalk )
            			m.SendMessage( "Invalid target." );
            		
            		else if( merc.InLOS( mount ) && merc.InRange( mount, 2 ) )
            			mount.Rider = merc;
            		
            		else
            			m.SendMessage( "The follower is either too far away from the mount or unable to mount it." );
            	}
            	
            	else
            		m.SendMessage( "Invalid target." );
            }
        }
        
        [Usage( "MercDismount" )]
        [Description( "Allows you to force one of your followers to mount one of your mounts." )]
        private static void MercDismount_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.SendMessage( "Which of your followers do you wish to order to dismount?" );
        	m.Target = new MercDismountTarget();
        }
        
        private class MercDismountTarget : Target
        {
            public MercDismountTarget()
                : base( 15, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null )
            		return;
            	
            	if( obj is Mercenary && ( (Mercenary)obj ).Owner == m && ( (Mercenary)obj ).Mounted )
            	{
            		Mercenary merc = obj as Mercenary;
            		
            		if( !merc.Alive || merc.Deleted || merc.Map != m.Map || m.Frozen || m.Paralyzed || m.CantWalk )
            			m.SendMessage( "Invalid target." );
            		
            		else
            		{
            			BaseMount mount = merc.Mount as BaseMount;
            			mount.Rider = null;
            		}
            	}
            	
            	else
            		m.SendMessage( "Invalid target." );
            }
        }
        
        [Usage( "MercName" )]
        [Description( "Allows you to rename a follower you have hired." )]
        private static void MercName_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.SendMessage( "Which of your followers do you wish to rename?" );
        	m.Target = new MercNameTarget();
        }
        
        private class MercNameTarget : Target
        {
            public MercNameTarget()
                : base( 8, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null )
            		return;
            	
            	if( obj is Mercenary && ( (Mercenary)obj ).Owner == m )
            	{
            		Mercenary merc = obj as Mercenary;
            		
            		m.SendMessage( "Type in a new name for your follower and press 'Enter'." );
            		m.Prompt = new MercNamePrompt( merc );
            	}
            	
            	else
            		m.SendMessage( "Invalid target." );
            }
        }
        
        private class MercNamePrompt : Prompt
		{
			private Mercenary merc;
	
			public MercNamePrompt( Mercenary mercenary )
			{
				merc = mercenary;
			}
	
			public override void OnResponse( Mobile from, string text )
			{
				if( text != null && merc != null && from != null && merc.Owner == from )
					merc.Name = text;
			}
        }
        
        [Usage( "MercTitle" )]
        [Description( "Allows you to change the title of a follower you have hired." )]
        private static void MercTitle_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.SendMessage( "Which of your followers will have their title changed?" );
        	m.Target = new MercTitleTarget();
        }
        
        private class MercTitleTarget : Target
        {
            public MercTitleTarget()
                : base( 8, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null )
            		return;
            	
            	if( obj is Mercenary && ( (Mercenary)obj ).Owner == m )
            	{
            		Mercenary merc = obj as Mercenary;
            		
            		m.SendMessage( "Type in a new title for your follower and press 'Enter'." );
            		m.Prompt = new MercTitlePrompt( merc );
            	}
            	
            	else
            		m.SendMessage( "Invalid target." );
            }
        }
        
        private class MercTitlePrompt : Prompt
		{
			private Mercenary merc;
	
			public MercTitlePrompt( Mercenary mercenary )
			{
				merc = mercenary;
			}
	
			public override void OnResponse( Mobile from, string text )
			{
				if( text != null && merc != null && from != null && merc.Owner == from )
					merc.Title = text;
			}
        }
        
        [Usage( "StaffAccount" )]
        [Description( "Allows you to open four slots to a player account." )]
        private static void StaffAccount_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.Target = new StaffAccountTarget( m );
        }
        
        private class StaffAccountTarget : Target
        {
        	private PlayerMobile admin;
        	
            public StaffAccountTarget( PlayerMobile pm )
                : base( 8, false, TargetFlags.None )
            {
            	admin = pm;
                pm.SendMessage( "Whom do you wish to grant four character slots to?" );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null )
            		return;
            	
            	if( obj is PlayerMobile )
            	{
            		PlayerMobile player = obj as PlayerMobile;
            		
            		if( player.Account.DonationSlot == 4 )
            			admin.SendMessage( "That account already has six character slots." );
            		
            		else
            		{
            			admin.SendMessage( "You have granted four additional character slots to the account '" + player.Account.Username + "'." );
            			player.SendMessage( "You have been granted four additional character slots." );
            			player.Account.DonationSlot = 4;
            		}
            	}
            	
            	else
            		admin.SendMessage( "Invalid target." );
            }
        }
			
		[Usage( "GrantSlot" )]
        [Description( "Allows you to open an additional character slot to an account." )]
        private static void GrantSlot_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.Target = new GrantSlotTarget( m );
        }
        
        private class GrantSlotTarget : Target
        {
        	private PlayerMobile admin;
        	
            public GrantSlotTarget( PlayerMobile pm )
                : base( 8, false, TargetFlags.None )
            {
            	admin = pm;
                pm.SendMessage( "Whom do you wish to grant an additional character slot to?" );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null )
            		return;
            	
            	if( obj is PlayerMobile )
            	{
            		PlayerMobile player = obj as PlayerMobile;
            		
            		if( player.Account.DonationSlot > 1 )
            			admin.SendMessage( "That account already has four character slots." );
            		
            		else
            		{
            			admin.SendMessage( "You have granted an additional character slot to the account '" + player.Account.Username + "'." );
            			player.SendMessage( "You have been granted an additional character slot." );
            			player.Account.DonationSlot += 1;
            		}
            	}
            	
            	else
            		admin.SendMessage( "Invalid target." );
            }
        }
		
		[Usage( "HearAll" )]
        [Description( "Allows you to hear everything that is being said or emoted on the shard." )]
        private static void HearAll_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m.AccessLevel >= AccessLevel.GameMaster )
        	{
        		if( m.HearAll == 0 || m.HearAll == 2 )
        		{
        			m.HearAll++;
        			m.SendMessage( "Hear All On." );
        			return;
        		}
        		
        		if( m.HearAll == 1 || m.HearAll == 3 )
        		{
        			m.HearAll--;
        			m.SendMessage( "Hear All Off." );
        			return;
        		}
        	}
        }
                
        [Usage( "HearParty" )]
        [Description( "Allows you to hear everything that is being said in party on the shard." )]
        private static void HearParty_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m.AccessLevel > AccessLevel.GameMaster )
        	{
        		if( m.HearAll == 0 || m.HearAll == 1 )
        		{
        			m.HearAll += 2 ;
        			m.SendMessage( "Hear Party On." );
        			return;
        		}
        		
        		if( m.HearAll == 2 || m.HearAll == 3 )
        		{
        			m.HearAll -= 2;
        			m.SendMessage( "Hear Party Off." );
        			return;
        		}
        	}
        }
		
		[Usage( "CharInfo" )]
        [Description( "Displays your character's stats." )]
        private static void CharInfo_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.SendGump( new CharInfoGump( m ) );
        }
        
        [Usage( "Password" )]
        [Description( "Allows you to change your account's password." )]
        private static void Password_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	string password = e.ArgString;
        	string newpassword = password.Trim();

    		if( newpassword.Length > 0 )
    		{
    			m.Account.SetPassword( newpassword );
    			m.SendMessage( "Password changed to:" );
    			m.SendMessage( newpassword );
    		}
    		
    		else
    			m.SendMessage( "You must enter a password. Example: .Password 123" );
        }

		[Usage( "SpeakLanguage" )]
        [Description( "Change the language you are currently speaking." )]
        private static void SpeakLanguage_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
            
            string language = e.ArgString.Trim();

            switch( language.ToLower() )
            {
            	case "southern":
            	{
            		SettingLanguage( m, KnownLanguage.Southern, m.Feats.GetFeatLevel(FeatList.SouthernLanguage), 3, "Southern" );
            		break;
            	}
            	
            	case "western":
            	{
            		SettingLanguage( m, KnownLanguage.Western, m.Feats.GetFeatLevel(FeatList.WesternLanguage), 3, "Western" );
            		break;
            	}
            		
            	case "common":
            	{
            		SettingLanguage( m, KnownLanguage.Common, 3, 3, "Common" );
            		break;
            	}
            		
            	case "ancient":
            	{
            		SettingLanguage( m, KnownLanguage.Haluaroc, m.Feats.GetFeatLevel(FeatList.HaluarocLanguage), 3, "Ancient" );
            		break;
            	}
            		
            	case "mhordul":
            	{
            		SettingLanguage( m, KnownLanguage.Mhordul, m.Feats.GetFeatLevel(FeatList.MhordulLanguage), 3, "Mhordul" );
            		break;
            	}
            		
            	case "tirebladd":
            	{
            		SettingLanguage( m, KnownLanguage.Tirebladd, m.Feats.GetFeatLevel(FeatList.TirebladdLanguage), 3, "Tirebladd" );
            		break;
            	}
            		
            	case "northern":
            	{
            		SettingLanguage( m, KnownLanguage.Northern, m.Feats.GetFeatLevel(FeatList.NorthernLanguage), 3, "Northern" );
            		break;
            	}
            		
            	case "shorthand":
            	{
            		SettingLanguage( m, KnownLanguage.Shorthand, m.Feats.GetFeatLevel(FeatList.Shorthand), 1, "Shorthand" );
            		break;
            	}
            }
        }
        
        [Usage( "CheckExp" )]
        [Description( "Check the current experience points and level of one of your followers." )]
        private static void CheckExp_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.Target = new CheckExpTarget( m );
        }
        
        private class CheckExpTarget : Target
        {
        	private PlayerMobile m_player;
        	
            public CheckExpTarget( PlayerMobile pm )
                : base( 8, false, TargetFlags.None )
            {
            	m_player = pm;
                pm.SendMessage( "Choose one of your followers to check their experience and level." );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( obj is BaseCreature && ( (BaseCreature)obj ).ControlMaster != null && ( (BaseCreature)obj ).ControlMaster.Serial == m_player.Serial )
            	{
            		BaseCreature bc = obj as BaseCreature;
            		string pron = "his";
            		string pron2 = "He";
            		
            		if( bc.Female )
            		{
            			pron = "her";
            			pron2 = "She";
            		}
            		
            		m_player.SendMessage( 60, "Your follower is level " + bc.Level + " and has " + bc.XP + " experience points." );
            	
            		if( obj is Mercenary )
            		{
            			Mercenary merc = obj as Mercenary;
            			m_player.SendMessage( 60, "This follower's fighting skills are at " + bc.Skills[SkillName.Tactics].Fixed / 10 + "% and " + pron + " stats are: Str {0}, Dex {1}, Int {2}, Hits {3}, Stam {4}, Mana {5}. " + pron2 + " charges you " + ( (Mercenary)bc ).ChargePerDay + " copper per Khaeros day and is currently carrying " + ( (Mercenary)bc ).HoldCopper + " copper pieces.", bc.RawStr, bc.RawDex, bc.RawInt, bc.RawHits, bc.RawStam, bc.RawMana );
            			m_player.SendMessage( 65, "This follower has the following feats:" );
            			
            			for( int i = 0; i < 21; i++ )
            			{
            				if( MercTraining.GetFeat(merc, i) > 0 )
            					m_player.SendMessage( 70, MercTraining.GetMercFeatName(i) + " - 3" );
            			}
            		}
            		
            		if( obj is BaseBreedableCreature )
            		{
            			BaseBreedableCreature bbc = obj as BaseBreedableCreature;
            			m_player.SendMessage( 65, "This pet has the following feats:" );
            			
            			for( int i = 0; i < 21; i++ )
            			{
            				if( bbc.GetFeat(i) > 0 )
            					m_player.SendMessage( 70, Utilities.GetBBCFeatName(i) + " - " + bbc.GetFeat(i).ToString() );
            			}
            		}
            		
            		if( obj is BaseCreature )
            		{ 
            			m_player.SendMessage( 75, "Your follower has " + bc.Lives.ToString() + " {0} left and the following scales:", bc.Lives == 1 ? "life" : "lives" );
            			m_player.SendMessage( 80, "XP Scale: " + bc.XPScale.ToString() );
            			m_player.SendMessage( 80, "Stat Scale: " + bc.StatScale.ToString() );
            			m_player.SendMessage( 80, "Skill Scale: " + bc.SkillScale.ToString() );
            			
            			if( obj is BaseBreedableCreature )
            			{
            				m_player.SendMessage( 85, "This pet is a {0} and has the following stats: Str {1}, Dex {2}, Int {3}, Hits {4}, Stam {5}, Mana {6}.", 
            			                     	bc.Female == true ? "female" : "male", bc.RawStr, bc.RawDex, bc.RawInt, bc.RawHits, bc.RawStam, bc.RawMana );
            			
            				if( ((BaseBreedableCreature)obj).Pregnant )
            				{
            					if( DateTime.Compare( DateTime.Now, (((BaseBreedableCreature)obj).Conception + TimeSpan.FromDays(5)) ) > 0 )
            						m_player.SendMessage( 90, "This pet is pregnant and is ready to go into labour." );
            					
            					else
            					{
            						TimeSpan time = (((BaseBreedableCreature)obj).Conception + TimeSpan.FromDays(5)) - DateTime.Now;
            						
            						if( time.Days > 0 || time.Hours > 1 )
            							m_player.SendMessage( 90, "This pet is pregnant and will be ready to go into labour in " + time.Days + " days and " + time.Hours + " hours." );
            						
            						else
            							m_player.SendMessage( 90, "This pet is pregnant and will be ready to go into labour in less than an hour." );
            					}
            				}
            			}
            		}
            	}
            	
            	else
            		m_player.SendMessage( "Invalid target." );
            }
        }
        
        public static void SettingLanguage( PlayerMobile m, KnownLanguage currentlang, int knowledge, int requirement, string language )
        {
        	if( m.SpokenLanguage == currentlang )
        		m.SendMessage( 60, "You are already speaking " + language );
        	
        	else if( knowledge >= requirement )
        	{
        		m.SendMessage( 60, "You will be speaking " + language + " from now on." );
        		m.SpokenLanguage = currentlang;
        	}
        	
        	else
        		m.SendMessage( 60, "You are still not proficient enough in " + language + " to speak it." );
        }
        
        [Usage( "Forge" )]
        [Description( "Allows you to forge gold, silver and copper coins." )]
        private static void Forge_OnCommand( CommandEventArgs e )
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;
            
            string coin = e.ArgString.Trim();

            if( pm.Feats.GetFeatLevel(FeatList.Counterfeiting) > 0 )
            {
            	if( coin.ToLower() == "copper" )
                {
                    Tin tin = pm.Backpack.FindItemByType( typeof( Tin ) ) as Tin;
                    CopperIngot copperingot = pm.Backpack.FindItemByType( typeof( CopperIngot ) ) as CopperIngot;

                    if ( tin != null && copperingot != null )
                    {
                        if ( tin.Amount > 99 )
                        {
                            copperingot.Consume();
                            tin.Consume( 100 );

                            ForgedCopper forgedcopper = new ForgedCopper( 100 );

                            pm.Backpack.DropItem( forgedcopper );

                            pm.SendMessage( "You create some forged copper coins." );
                            return;
                        }
                    }

                    pm.SendMessage( "You lack the necessary ingredients for that." );
                    return;
                }

                if( pm.Feats.GetFeatLevel(FeatList.Counterfeiting) > 1 )
                {
                    if( coin.ToLower() == "silver" )
                    {
                        Tin tin = pm.Backpack.FindItemByType( typeof( Tin ) ) as Tin;
                        SilverIngot silveringot = pm.Backpack.FindItemByType( typeof( SilverIngot ) ) as SilverIngot;

                        if( tin != null && silveringot != null )
                        {
                            if( tin.Amount > 99 )
                            {
                                silveringot.Consume();
                                tin.Consume( 100 );

                                ForgedSilver forgedsilver = new ForgedSilver( 100 );

                                pm.Backpack.DropItem( forgedsilver );

                                pm.SendMessage( "You create some forged silver coins." );
                                return;
                            }
                        }

                        pm.SendMessage( "You lack the necessary ingredients for that." );
                        return;
                    }

                    if( pm.Feats.GetFeatLevel(FeatList.Counterfeiting) > 2 )
                    {
                        if( coin.ToLower() == "gold" )
                        {
                            Tin tin = pm.Backpack.FindItemByType( typeof( Tin ) ) as Tin;
                            GoldIngot goldingot = pm.Backpack.FindItemByType( typeof( GoldIngot ) ) as GoldIngot;

                            if( tin != null && goldingot != null )
                            {
                                if( tin.Amount > 99 )
                                {
                                    goldingot.Consume();
                                    tin.Consume( 100 );

                                    ForgedGold forgedgold = new ForgedGold( 100 );

                                    pm.Backpack.DropItem( forgedgold );

                                    pm.SendMessage( "You create some forged gold coins." );
                                    return;
                                }
                            }

                            pm.SendMessage( "You lack the necessary ingredients for that." );
                            return;
                        }
                    }
                }
            }

            if( pm.Feats.GetFeatLevel(FeatList.Counterfeiting) > 0 )
            {
                pm.SendMessage( 60, "Command usage:" );
                pm.SendMessage( ".forge copper" );
                pm.SendMessage( ".forge silver" );
                pm.SendMessage( ".forge gold" );
                return;
            }

            else
                pm.SendMessage( 60, "You need to take the Counterfeiting feat to do that." );
        }

        [Usage( "BiteCoin" )]
        [Description( "Allows you to bite a coin to test if it is authentic." )]
        private static void BiteCoin_OnCommand( CommandEventArgs e )
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;

            if( pm.Feats.GetFeatLevel(FeatList.VerifyCurrency) > 0 )
            	pm.Target = new BiteCoinTarget( pm );

            else
                pm.SendMessage( 60, "You need to take the Bite Coin feat to do that." );
        }

        private class BiteCoinTarget : Target
        {
            public BiteCoinTarget( PlayerMobile pm )
                : base( 8, false, TargetFlags.None )
            {
                pm.SendMessage( 60, "Choose the currency you wish to examine." );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                PlayerMobile pm = m as PlayerMobile;

                if( obj is Copper || obj is Silver || obj is Gold )
                {
                    pm.Emote ( "*bites a coin to test if it is authentic*" );
                    pm.SendMessage( 60, "After a brief examination, you are sure that they are authentic." );
                    return;
                }

                if ( obj is ForgedCopper || obj is ForgedSilver || obj is ForgedGold )
                {
                    pm.Emote ( "*bites a coin to test if it is authentic*" );
                    
                    int chance = 0;

                    switch( pm.Feats.GetFeatLevel(FeatList.VerifyCurrency) )
                    {
                        case 0: break;
                        case 1: chance = 5; break;
                        case 2: chance = 10; break;
                        case 3: chance = 100; break;
                    }

                    int roll = Utility.Random( 1, 100 );

                    if ( chance >= roll )
                        pm.SendMessage( 60, "After a brief examination, you discover that they are forged coins." );

                    else
                        pm.SendMessage( 60, "After a brief examination, you are sure that they are authentic." );
                }

                else
                    m.SendMessage( 60, "That is not a coin." );
            }
        }

        [Usage( "EngravePottery" )]
        [Description( "Allows you to engrave a piece of pottery with symbols, drawings or words." )]
        private static void EngravePottery_OnCommand( CommandEventArgs e )
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;

            if( pm.Feats.GetFeatLevel(FeatList.Potter) > 2 )
            {
                if ( e.ArgString == "" )
                {
                    pm.SendMessage( 60, "Example of a correct usage for this command:"  );
                    pm.SendMessage( ".EngravePottery a statue of an armoured man" );
                    return;
                }

                pm.Target = new EngravePotteryTarget( pm, e.ArgString );
            }

            else
                pm.SendMessage( 60, "You need to take the third level of Potter to do that." );
        }

        private class EngravePotteryTarget : Target
        {
            public string m_newname;

            public EngravePotteryTarget( PlayerMobile pm, string newname )
                : base( 8, false, TargetFlags.None )
            {
                pm.SendMessage( 60, "Choose a piece of pottery to engrave." );
                m_newname = newname;
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                if( obj is CeramicMug || obj is Basin || obj is Vase || obj is LargeVase || obj is FancyVase ||
                    obj is LargeFancyVase || obj is TallVase || obj is OrnamentedVase || obj is ClayStatueSouth ||
                    obj is ClayStatueSouth2 || obj is ClayStatueNorth || obj is ClayStatueWest || obj is ClayStatueEast ||
                   	obj is ClayStatueEast2 || obj is ClayStatueSouthEast || obj is ClayBustSouth || obj is ClayBustEast ||
                   	obj is ClayStatuePegasus || obj is ClayStatuePegasus2 || obj is SmallClayTowerSculpture )
                {
                    Item item = obj as Item;

                    if( item.Movable )
                        item.Name = m_newname;

                    else
                        m.SendMessage( 60, "That is an engraved item already." );
                }

                else
                    m.SendMessage( 60, "That is not a piece of pottery." );
            }
        }

        [Usage( "EngraveMasonry" )]
        [Description( "Allows you to engrave a piece of pottery with symbols, drawings or words." )]
        private static void EngraveMasonry_OnCommand( CommandEventArgs e )
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;

            if( pm.Feats.GetFeatLevel(FeatList.Sculptor) > 2 )
            {
                if( e.ArgString == "" )
                {
                    pm.SendMessage( 60, "Example of a correct usage for this command:" );
                    pm.SendMessage( ".EngravePottery a statue of an armoured man" );
                    return;
                }

                pm.Target = new EngraveMasonryTarget( pm, e.ArgString );
            }

            else
                pm.SendMessage( 60, "You need to take the third level of Sculptor to do that." );
        }

        private class EngraveMasonryTarget : Target
        {
            public string m_newname;

            public EngraveMasonryTarget( PlayerMobile pm, string newname )
                : base( 8, false, TargetFlags.None )
            {
                pm.SendMessage( 60, "Choose a piece of masonry to engrave." );
                m_newname = newname;
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                if( obj is Pedestal || obj is LargePedestal || obj is GuardStatueEast || obj is GuardStatueSouth ||
                    obj is StatueSouth2 || obj is StatueNorth || obj is StatueWest || obj is StatueEast ||
                    obj is StatueEast2 || obj is StatueSouthEast || obj is BustSouth || obj is BustEast ||
                    obj is StatuePegasus || obj is StatuePegasus2 || obj is StatueSouth )
                {
                    Item item = obj as Item;

                    if( item.Movable )
                        item.Name = m_newname;

                    else
                        m.SendMessage( 60, "That is an engraved item already." );
                }

                else
                    m.SendMessage( 60, "That is not a piece of masonry." );
            }
        }
        
        [Usage( "FightingStyle" )]
        [Description( "Allows you to choose a weapon to specialize in." )]
        private static void FightingStyle_OnCommand( CommandEventArgs e )
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;
            
            if( pm.Feats.GetFeatLevel(FeatList.FightingStyle) > 0 )
            {
            	if( pm.CombatStyles.Axemanship + pm.CombatStyles.ExoticWeaponry + pm.CombatStyles.Fencing +
            	    pm.CombatStyles.MaceFighting + pm.CombatStyles.Polearms + pm.CombatStyles.Swordsmanship == 0 )
            		pm.Target = new FightingStyleTarget( pm );
            	
            	else
            		pm.SendMessage( 60, "You have already chosen your fighting style." );
            }
            
            else
            	pm.SendMessage( 60, "You do not know this feat." );
        }

        private class FightingStyleTarget : Target
        {
            public FightingStyleTarget( PlayerMobile pm )
                : base( 8, false, TargetFlags.None )
            {
                pm.SendMessage( 60, "Choose a weapon of the type you wish to specialize in." );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                PlayerMobile pm = m as PlayerMobile;

                if( obj is BaseWeapon )
                {
                	BaseWeapon weapon = obj as BaseWeapon;
                	
                	if( weapon is BaseRanged )
                	{
                		pm.SendMessage( 60, "You cannot choose Archery as your fighting style." );
                		return;
                	}
                	
                	switch( weapon.Skill )
                	{                			
                		case SkillName.Axemanship: 
            			{
            				pm.CombatStyles.Axemanship++; 
            				pm.SendMessage( 60, "You have chosen Axemanship as your fighting style." );
            				break;
            			}
                			
                		case SkillName.Swords: 
            			{
            				pm.CombatStyles.Swordsmanship++; 
            				pm.SendMessage( 60, "You have chosen Swordsmanship as your fighting style." );
            				break;
            			}
                			
                		case SkillName.Fencing: 
            			{
            				pm.CombatStyles.Fencing++; 
            				pm.SendMessage( 60, "You have chosen Fencing as your fighting style." );
            				break;
            			}
                			
                		case SkillName.Macing: 
            			{
            				pm.CombatStyles.MaceFighting++; 
            				pm.SendMessage( 60, "You have chosen Macing as your fighting style." );
            				break;
            			}
                			
                		case SkillName.ExoticWeaponry: 
            			{
            				pm.CombatStyles.ExoticWeaponry++; 
            				pm.SendMessage( 60, "You have chosen ExoticWeaponry as your fighting style." );
            				break;
            			}
                			
                		case SkillName.Polearms: 
            			{
            				pm.CombatStyles.Polearms++; 
            				pm.SendMessage( 60, "You have chosen Polearms as your fighting style." );
            				break;
            			}
                	}
                }
            }
        }

        [Usage( "WeaponSpecialization" )]
        [Description( "Allows you to choose a weapon to specialize in." )]
        private static void WeaponSpecialization_OnCommand( CommandEventArgs e )
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;

            if( pm.Feats.GetFeatLevel(FeatList.WeaponSpecialization) > 0 )
            {
            	if( pm.WeaponSpecialization == null )
            	{
            		pm.Target = new WeaponSpecializationTarget( pm, true );
            		return;
            	}
            	
            	if( pm.Feats.GetFeatLevel(FeatList.SecondSpecialization) > 0 )
            	{
	            	if( pm.SecondSpecialization == null )
	            	{
	            		pm.Target = new WeaponSpecializationTarget( pm, false );
	            		return;
	            	}
            	}
            		
            	pm.SendMessage( 60, "You have already chosen a weapon to specialize in." );
            	return;
            }

            pm.SendMessage( 60, "You do not know this feat." );
        }

        private class WeaponSpecializationTarget : Target
        {
        	bool m_first = false;
        	
            public WeaponSpecializationTarget( PlayerMobile pm, bool first )
                : base( 8, false, TargetFlags.None )
            {
            	m_first = first;
                pm.SendMessage( 60, "Choose a weapon to specialize in." );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                PlayerMobile pm = m as PlayerMobile;

                if( obj is BaseWeapon )
                {
                	if( ( (BaseWeapon)obj ) is BaseRanged )
                		pm.SendMessage( 60, "You cannot choose a ranged weapon to specialize in." );
                	
                	else if( m_first )
                	{
                		pm.WeaponSpecialization =  ( (BaseWeapon)obj ).NameType;
                		pm.SendMessage( 60, "You have chosen to specialize in " + ( (BaseWeapon)obj ).NameType + "s." );
                	}
                	
                	else if( pm.WeaponSpecialization != ( (BaseWeapon)obj ).NameType )
                	{
                		pm.SecondSpecialization =  ( (BaseWeapon)obj ).NameType;
                		pm.SendMessage( 60, "You have chosen to specialize in " + ( (BaseWeapon)obj ).NameType + "s." );
                	}
                	
                	else
                		pm.SendMessage( 60, "You have already chosen that weapon for your first specialization." );
                }
            }
        }
        
        [Usage( "WoodStaining" )]
        [Description( "Allows you to stain a piece of wood." )]
        private static void WoodStaining_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.Feats.GetFeatLevel(FeatList.WoodStaining) < 1 )
            {
            	m.SendMessage( 60, "You do not know how to do that." );
            	return;
            }
            
            m.SendGump( new WoodStainGump( m, 0 ) );
        }
        
        [Usage( "AdvancedDying" )]
        [Description( "Allows you to dye a piece of clothing." )]
        private static void AdvancedDying_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.Feats.GetFeatLevel(FeatList.AdvancedDying) < 1 )
            {
            	m.SendMessage( 60, "You do not know how to do that." );
            	return;
            }
            
            m.SendGump( new DyingTubGump( m, 0 ) );
        }
        
        [Usage( "Enamel" )]
        [Description( "Allows you to enamel a piece of armour." )]
        private static void Enamel_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.Feats.GetFeatLevel(FeatList.ArmourEnameling) < 1 )
            {
            	m.SendMessage( 60, "You do not know how to do that." );
            	return;
            }
            
            m.SendGump( new EnamelGump( m, 0 ) );
        }
        
        [Usage( "MasterworkEquip" )]
        [Description( "Allows you to choose your Masterwork Equipment bonuses." )]
        private static void MasterworkEquip_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.Feats.GetFeatLevel(FeatList.RenownedMasterwork) < 1 )
            {
            	m.SendMessage( 60, "You do not know how to do that." );
            	return;
            }
            
            m.SendGump( new MasterworkEquipGump( m ) );
        }

        /*[Usage( "Throw" )]
        [Description( "Allows you to attempt to throw your weapon at a target mobile." )]
        private static void Throw_OnCommand( CommandEventArgs e )
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;

            if( pm.Weapon is Fists )
                pm.SendMessage( 60, "You need to be equipping a weapon in order to perform that attack." );

            else
            {
                BaseWeapon weapon = pm.Weapon as BaseWeapon;

                if( weapon.Throwable || pm.Feats.GetFeatLevel(FeatList.ThrowingMastery) > 0 )
                    pm.Target = new ThrowTarget( pm );

                else
                    pm.SendMessage( 60, "You are not able to throw that weapon." );
            }
        }

        private class ThrowTarget : Target
        {
            public ThrowTarget( PlayerMobile pm )
                : base( 8, false, TargetFlags.None )
            {
                pm.SendMessage( 60, "Choose the character you wish to throw your weapon at." );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                PlayerMobile pm = m as PlayerMobile;

                if( pm.Weapon is Fists )
                    pm.SendMessage( 60, "You need to be equipping a weapon in order to perform that attack." );

                if( obj == m )
                    return;

                else
                {
                    BaseWeapon weapon = pm.Weapon as BaseWeapon;

                    if( weapon.Throwable || pm.Feats.GetFeatLevel(FeatList.ThrowingMastery) > 0 )
                    {
                        
                        if( obj is Mobile )
                        {
                            Mobile mob = obj as Mobile;

                            if( mob.Alive && mob != null && !mob.Deleted && mob.Map == pm.Map && pm.InRange( mob, 3 + pm.Feats.GetFeatLevel(FeatList.ThrowingMastery) ) )
                            {
                            	if( BaseWeapon.CheckStam( pm, Math.Max( Convert.ToInt32( weapon.Weight ), 2 ), Math.Max( 2, Convert.ToInt32( ( 60 - pm.Level ) * 0.2 ) ) ) )
                    			{
                                    bool snatched = false;
                                    bool deflected = false;

                                    if( mob is PlayerMobile )
                                    {
                                        if( ( (PlayerMobile)mob ).Snatched() )
                                            snatched = true;

                                        if( ( (PlayerMobile)mob ).DeflectedProjectile() )
                                            deflected = true;
                                    }

                                    pm.Emote( "*throws {0} weapon at {1}*", pm.Female == true ? "her" : "his", mob.Name );
									
									double damagebonus = 1.0;

									if( weapon.Throwable )
									{
										switch( pm.Feats.GetFeatLevel(FeatList.ThrowingMastery) )
										{
											case 0: break;
											case 1: damagebonus = 1.1; break;
											case 2: damagebonus = 1.2; break;
											case 3: damagebonus = 1.3; break;
										}
									}
									
									if ( ((IKhaerosMobile)m).Feats.GetFeatLevel(FeatList.Finesse) > 0 && Utility.RandomDouble() <= (0.6*((IKhaerosMobile)m).Feats.GetFeatLevel(FeatList.Finesse))/(weapon.Weight*weapon.Weight) )
										damagebonus += 1.0;
									Direction to = m.GetDirectionTo( mob );
									m.Direction = to;
									pm.OffensiveFeat = FeatList.ThrowingMastery;
									Point3D loc = mob.Location;
									Map map = mob.Map;
									if( snatched )
									{
										weapon.PlaySwingAnimation( m );
										weapon.GetMissAttackSound( m, mob );
										mob.Emote( "*snatched a projectile shot at {0} by {1}*", mob.Female == true ? "her" : "him", m.Name );
									}

									else if( deflected )
									{
										weapon.PlaySwingAnimation( m );
										weapon.GetMissAttackSound( m, mob );
										mob.Emote( "*uses {0} shield to deflect a projectile shot at {1} by {2}*", mob.Female == true ? "her" : "him", mob.Female == true ? "her" : "him", m.Name );
									}

									else
										weapon.OnSwing( pm, mob, damagebonus, true );
										
									if ( !snatched )
										weapon.MoveToWorld( loc, map );
									else
										mob.AddToBackpack( weapon ); // Nigga stole my ______!
                                }
                            }

                            else
                                pm.SendMessage( 60, "That is out of range." );
                        }
                        

                        else
                            pm.SendMessage( 60, "That is not a valid target." );

                    }

                    else
                        pm.SendMessage( 60, "You are not able to throw that weapon." );
                }
            }
        }*/

        [Usage("PolymorphSpell")]
        [Description("Turns your body into a sheep.")]
        private static void PolymorphSpell_OnCommand(CommandEventArgs e)
        {
            PlayerMobile caster = e.Mobile as PlayerMobile;

            if (caster.BodyValue == 400 || caster.BodyValue == 401)
            {
                caster.BodyValue = 207;
                caster.InvalidateProperties();
            }
            else
            {
                if (caster.Female)
                    caster.BodyValue = 401;

                else
                    caster.BodyValue = 400;
                caster.InvalidateProperties();
            }
        }

        [Usage("DecoySpell")]
        [Description("Creates a decoy out of you.")]
        private static void DecoySpell_OnCommand(CommandEventArgs e)
        {
            PlayerMobile caster = e.Mobile as PlayerMobile;
            new Clone(caster).MoveToWorld(caster.Location, caster.Map);
            caster.FixedParticles(0x376A, 1, 14, 0x13B5, EffectLayer.Waist);
            caster.FixedParticles(0x376A, 1, 14, 0x13B5, EffectLayer.Head);
            caster.PlaySound(534);
        }

        [Usage("HideSpell")]
        [Description("Hides you from sight.")]
        private static void HideSpell_OnCommand(CommandEventArgs e)
        {
            PlayerMobile caster = e.Mobile as PlayerMobile;
            caster.FixedParticles(0x3709, 1, 30, 9904, 1108, 6, EffectLayer.RightFoot);
            caster.PlaySound(0x22F);
            if (caster.Hidden)
            {
                caster.Hidden = false;
                caster.FixedParticles(0x3709, 1, 30, 9904, 1108, 6, EffectLayer.RightFoot);
            }
            else
            {
                caster.Hidden = true;
                caster.FixedParticles(0x3709, 1, 30, 9904, 1108, 6, EffectLayer.RightFoot);
                caster.PlaySound(0x22F);
            }
            
        }

       

        [Usage ("SecondWind")]
        [Description("Gives you Second Wind, revitalizing you for a moment.")]
        private static void SecondWind_OnCommand(CommandEventArgs e)
        {
            PlayerMobile player = e.Mobile as PlayerMobile;
            SecondWind secondWind = new SecondWind();
            secondWind.ExecuteOn(player);
        }

        [Usage( "Intimidate" )]
        [Description( "Allows you to attempt to intimidate a target mobile." )]
        private static void Intimidate_OnCommand( CommandEventArgs e )
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;
			BaseWeapon wep = pm.Weapon as BaseWeapon;
            if( pm.Feats.GetFeatLevel(FeatList.Intimidate) < 1 )
            {
                pm.SendMessage( 60, "You do not have this feat." );
                return;
            }

			CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( pm );
			if ( pm.Combatant == null || wep == null )
			{
				pm.SendMessage( 60, "You must be fighting someone in order to use this ability." );
                return;
			}
			else if ( !pm.Combatant.Alive )
				return;
			else if ( !pm.Combatant.InRange( pm, 10 ) )
			{
				pm.SendMessage( "That is too far away." );
				return;
			}
			else if( !BaseWeapon.CheckStam( pm, pm.Feats.GetFeatLevel(FeatList.Intimidate), false, false ) )
				return;
			
			pm.PlaySound ( (pm.Female ? 824 : 1098) );
			bool success = Utility.RandomDouble() < 0.25*pm.Feats.GetFeatLevel(FeatList.Intimidate);
			if ( success )
			{
				CombatSystemAttachment opponentCSA = CombatSystemAttachment.GetCSA( pm.Combatant );
				if ( opponentCSA.AttackTimer != null || opponentCSA.DefenseTimer != null )
				{
					if ( !pm.Mounted )
						pm.Animate( 30, 2, 1, false, false, 1 ); // this will interrupt our attack if we have any, as it is an external animation
					else
						pm.Animate( 28, 2, 1, false, false, 0 );
					opponentCSA.Interrupted( true );
				}
			}
			/*else
				pm*/
            //pm.Target = new IntimidateTarget( pm );
        }
/*
        private class IntimidateTarget : Target
        {
            public IntimidateTarget( PlayerMobile pm )
                : base( 8, false, TargetFlags.None )
            {
                pm.SendMessage( 60, "Choose the character you wish to intimidate." );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                PlayerMobile pm = m as PlayerMobile;
                int featlevel = pm.Feats.GetFeatLevel(FeatList.Intimidate) * 20;

                if( obj is PlayerMobile || obj is BaseCreature )
                {
                    Mobile mob = obj as Mobile;

                    if( !mob.Alive || mob.Deleted || mob.Map != m.Map || !m.Alive || !m.InRange( mob, pm.Feats.GetFeatLevel(FeatList.Intimidate) * 2 ) )
                        return;

                    if( obj is BaseCreature )
                    {
                        BaseCreature basecreature = obj as BaseCreature;

                        if( basecreature.Intimidated != 0 )
                        {
                            pm.SendMessage( 60, "{0} is already quite intimidated.", basecreature.Name );
                            return;
                        }
                    }

                    if( obj is PlayerMobile )
                    {
                        PlayerMobile playermobile = obj as PlayerMobile;

                        if( playermobile == pm )
                            return;

                        if( playermobile.Intimidated != 0 )
                        {
                            pm.SendMessage( 60, "{0} is already quite intimidated.", playermobile.Name );
                            return;
                        }
                    }

                    if ( BaseWeapon.CheckStam( pm, ( pm.Feats.GetFeatLevel(FeatList.Intimidate) * 2 ) + 4 ) )
                    {
                        int chancetohit = 0;
                        int level = 0;
                        
                        if( mob is BaseCreature )
                        	level = ( (BaseCreature)mob ).Level;
                        
                        if( mob is PlayerMobile )
                        	level = ( (PlayerMobile)mob ).Level;

                        chancetohit = Math.Max( ( pm.Feats.GetFeatLevel(FeatList.Intimidate) * 10 ), ( ( pm.Level * 2 ) - level ) );

                        int attackroll = Utility.Random( 100 );

                        if( chancetohit >= attackroll )
                        {
                            mob.Emote( "*shudders before {0}*", pm.Name );
                            new IntimidateTimer( mob, pm.Feats.GetFeatLevel(FeatList.Intimidate) ).Start();
                        }

                        else
                            pm.Emote( "*fails to intimidate {0}*", mob.Name );
                    }
                }

                else
                    pm.SendMessage( 60, "That is not a valid target." );
            }
        }

        public class IntimidateTimer : Timer
        {
            private Mobile m_from;

            public IntimidateTimer( Mobile from, int featlevel )
                : base( TimeSpan.FromSeconds( featlevel * 5 ) )
            {
                m_from = from;
                ((IKhaerosMobile)m_from).Intimidated = featlevel * 20;
            }

            protected override void OnTick()
            {
                if( m_from != null )
                {
                    ((IKhaerosMobile)m_from).Intimidated = 0;
                    m_from.Emote( "*is no longer intimidated*" );
                }
            }
        }*/

        [Usage( "Carry" )]
        [Description( "Allows you to carry a knocked out player mobile." )]
        private static void Carry_OnCommand( CommandEventArgs e )
        {
            PlayerMobile pm = e.Mobile as PlayerMobile;
            Item weapon = pm.FindItemOnLayer( Layer.FirstValid ) as Item;
            Item shield = pm.FindItemOnLayer( Layer.TwoHanded ) as Item;

            Corpse corpse = pm.Backpack.FindItemByType( typeof( Corpse ) ) as Corpse;

            if( weapon == null && shield == null && corpse == null )
                pm.Target = new CarryTarget( pm );

            else if( corpse != null )
            {
                corpse.Movable = true;
                corpse.DropToWorld( pm, pm.Location );
                corpse.Visible = true;
                corpse.Movable = false;
            }

            else
                pm.SendMessage( 60, "You need to empty both your hands before trying this." );
        }

        private class CarryTarget : Target
        {
            public CarryTarget( PlayerMobile pm )
                : base( 8, false, TargetFlags.None )
            {
                pm.SendMessage( 60, "Choose a knocked out player character to carry." );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                PlayerMobile pm = m as PlayerMobile;
                Item weapon = pm.FindItemOnLayer( Layer.FirstValid ) as Item;
                Item shield = pm.FindItemOnLayer( Layer.TwoHanded ) as Item;

                if( weapon == null && shield == null )
                {
                    if( obj is Corpse )
                    {
                        Corpse corpse = obj as Corpse;

                        if( corpse.Owner != null && corpse.Owner is PlayerMobile )
                        {
                            PlayerMobile owner = corpse.Owner as PlayerMobile;

                            if( !pm.Frozen && !pm.Deleted )
                            {
                                pm.Emote( "*takes {0} in {1} arms and carries {2}*", owner.Name, pm.Female == true ? "her" : "his", owner.Female == true ? "her" : "him" );
                                corpse.Movable = true;
                                corpse.Weight = 0;
                                Container pack = pm.Backpack;
                                pack.DropItem( corpse );
                                corpse.Visible = false;
                            }
                        }

                        else
                            pm.SendMessage( 60, "You can only carry knocked out player characters." );
                    }

                    else
                        pm.SendMessage( 60, "You can only carry knocked out player characters." );
                }

                else
                    pm.SendMessage( 60, "You need to empty both your hands before trying this." );
            }
        }

        [Usage( "EscortPrisoner" )]
        [Description( "Allows you to drag a leashed mobile." )]
        private static void EscortPrisoner_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( e.Mobile is PlayerMobile )
            {
                if( m.Feats.GetFeatLevel(FeatList.EscortPrisoner) > 0 )
                {
                    if( m.EscortPrisoner == null )
                    {
                        if( !m.Deleted && m.Alive && !m.Frozen )
                            m.Target = new EscortPrisonerTarget( m );
                    }

                    else
                    {
                        Mobile mob = World.FindMobile( m.EscortPrisoner.Serial );
                        
                        if( mob.Alive && !mob.Deleted )
                            mob.Emote( "*is not longer being dragged by {0}*", m.Name );

                        m.EscortPrisoner = null;
                    }
                }

                else
                    m.SendMessage( 60, "You do not know how to perform this." );
            }
        }

        private class EscortPrisonerTarget : Target
        {
            public EscortPrisonerTarget( PlayerMobile pm )
                : base( 8, false, TargetFlags.None )
            {
                pm.SendMessage( 60, "Choose a paralyzed target to escort." );
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                PlayerMobile pm = m as PlayerMobile;
                Mobile mob = obj as Mobile;

                if ( obj is Mobile )
                {
                    if ( mob.Paralyzed )
                    {
                        if ( mob.Alive && !mob.Deleted )
                        {
                            if ( pm.Mounted && pm.Feats.GetFeatLevel(FeatList.EscortPrisoner) < 3 )
                                pm.SendMessage( 60, "You need to reach the third level of Escort Prisoner before trying to drag someone while mounted." );

                            else
                            {
                                pm.SendMessage( 60, "You start dragging {0}.", mob.Name );
                                mob.Emote( "*starts being dragged by {0}*", pm.Name );
                                pm.EscortPrisoner = mob;
                            }
                        }
                    }

                    else
                        pm.SendMessage( 60, "You can only drag paralyzed mobiles." );
                }

                else
                    pm.SendMessage( 60, "That is not a valid target." );
            }
        }

        [Usage( "AddBackpack" )]
        [Description( "Adds a new Backpack to your PlayerMobile." )]
        private static void AddBackpack_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            Backpack backpack = m.FindItemOnLayer( Layer.Backpack ) as Backpack;

            if ( backpack == null )
            {
                Container pack = m.Backpack;

                if( pack == null )
                {
                    pack = new ArmourBackpack();
                    pack.Movable = false;

                    m.AddItem( pack );
                }
            }
        }

        [Usage( "UpdateStats" )]
        [Description( "Updates your stats." )]
        private static void UpdateStats_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            ArmourBackpack backpack = m.FindItemOnLayer( Layer.Backpack ) as ArmourBackpack;

            if( backpack is ArmourBackpack && backpack != null )
                backpack.OnEquip( e.Mobile );
        }

		[Usage( "XP" )]
		[Description( "Shows XP info." )]
		private static void XP_OnCommand( CommandEventArgs e ) 
		{
			PlayerMobile m = e.Mobile as PlayerMobile;
			LevelSystem.CheckLevel( m );
			m.SendMessage( 60, "You are level " + m.Level + ". You have " + m.XP + " experience point{0} and " + m.CP + " character points.", m.XP == 1 ? "" : "s", m.CP == 1 ? "" : "s" );
		}
		
		[Usage( "Resists" )]
		[Description( "Shows resists info." )]
		private static void Resists_OnCommand( CommandEventArgs e ) 
		{
			PlayerMobile m = e.Mobile as PlayerMobile;
			m.SendMessage( "Blunt Resistance: " + m.BluntResistance );
			m.SendMessage( "Slashing Resistance: " + m.SlashingResistance );
			m.SendMessage( "Piercing Resistance: " + m.PiercingResistance );
		}

        [Usage( "StatBonus" )]
        [Description( "Opens Bonus Stats Gump." )]
        private static void StatBonus_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.Forging || m.Reforging )
            	m.SendGump( new InitialStatsGump(m) );
            
            else if( m.StatPoints > 0 )
                m.SendGump( new StatPointsGump(m) );

            else
                m.SendMessage( "You have already spent all your Stat Bonus Points." );
        }

        [Usage( "Trample" )]
        [Description( "Allows you to trample your foes while mounted." )]
        private static void Trample_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if ( m.Feats.GetFeatLevel(FeatList.Trample) < 1 )
            {
                m.SendMessage( 60, "You do not know how to perform this attack." );
                return;
            }

            if( !m.Mounted )
            {
                m.SendMessage( "You must be mounted in order to activate this attack." );
                return;
            }

            if( !m.Trample && m.Mounted )
            {
                m.Trample = true;
                m.SendMessage( 60, "You rush with your mount, ready to trample your foes." );
            }

            else if( m.Trample && m.Mounted )
            {
                m.Trample = false;
                m.SendMessage( 60, "You are no longer attempting to trample your foes." );
            }
        }

        [Usage( "TravelingShot" )]
        [Description( "Allows the user to attempt a Traveling Shot." )]
        private static void TravelingShot_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
            BaseWeapon weapon = m.Weapon as BaseWeapon;

            if( m.Feats.GetFeatLevel(FeatList.TravelingShot) > 0 )
            {
                if( m.Combatant != null && !m.Combatant.Deleted && m.Combatant.Alive && !m.Combatant.IsDeadBondedPet )
                {
                    if( m.Combatant.Map != m.Map || !m.InRange( m.Combatant, weapon.MaxRange ) )
                    {
                        m.SendMessage( 60, "Your enemy is too far away." );
                        return;
                    }

					if ( m.InRange( m.Combatant, 1 ) )
					{
                        m.SendMessage( 60, "Your enemy is too close." );
                        return;
                    }
                    if( m.Deleted || !m.Alive || m.Frozen || m.Paralyzed )
                        return;
                    
					CombatSystemAttachment csa = CombatSystemAttachment.GetCSA( m );
                    if( !csa.CanBeginAttack() )
                    {
                    	m.SendMessage( 60, csa.ErrorMessage );
                    	return;
                    }
					
                    if( ((BaseWeapon)m.Weapon).Skill == SkillName.Archery && BaseWeapon.CheckStam( m, ( m.Feats.GetFeatLevel(FeatList.TravelingShot) ), false, true ) )
                    {
                        double speedPenalty = 1.6 - (m.Feats.GetFeatLevel(FeatList.TravelingShot) * 0.1); // 50/40/30 penalty
						if( m.Feats.GetFeatLevel(FeatList.QuickTravelingShot) > 0 && !((BaseWeapon)m.Weapon).NameType.Contains("Crossbow") )
                        	speedPenalty -= (m.Feats.GetFeatLevel(FeatList.QuickTravelingShot) * 0.1); // 20/10/0 penalty
						csa.NextAttackAction = DateTime.Now + TimeSpan.FromSeconds( ( csa.ComputeNextSwingTime().TotalSeconds * speedPenalty ) );
                        m.OffensiveFeat = FeatList.TravelingShot;
                        weapon.OnSwing( m, m.Combatant, 1.0, true );
						m.Emote( "*skillfully shoots at {0} while moving*", m.Combatant.Name );
                    }
                    else
					{
                        m.SendMessage( 60, "You need to have a bow or crossbow equipped to perform that attack." );
						return;
					}
                }

                else
				{
                    m.SendMessage( 60, "You're not fighting anybody." );
					return;
				}
            }

            else
			{
                m.SendMessage( 60, "You do not know how to perform this attack." );
				return;
			}
        }
        
        [Usage( "DefensiveFury" )]
        [Description( "Allows the user to go on a defensive fury." )]
        private static void DefensiveFury_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.Feats.GetFeatLevel(FeatList.DefensiveFury) > 0 )
            {
            	if( DateTime.Compare( m.NextRage, DateTime.Now ) > 0 )
            	{
            		TimeSpan waitingtime = m.NextRage - DateTime.Now;
            		m.SendMessage( 60, "You need wait another " + ((waitingtime.Minutes * 60) + waitingtime.Seconds).ToString() + " seconds before using this ability again." );
            		return;
            	}
            	
                if( m.RageTimer == null )
                {
                    m.RageTimer = new RageTimer( m, m.Feats.GetFeatLevel(FeatList.DefensiveFury) );
                    m.SendMessage( 60, "You go on an unstoppable fury." );
                    m.RageFeatLevel = m.Feats.GetFeatLevel(FeatList.DefensiveFury) + m.Feats.GetFeatLevel(FeatList.Rage);
                    m.RageTimer.Start();
                }

                else
                    m.SendMessage( 60, "You are already enraged." );
            }

            else
                m.SendMessage( 60, "You do not know how to do this." );
        }
        
        [Usage( "Rage" )]
        [Description( "Allows the user to go on a bloodthirsty rage." )]
        private static void Rage_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.Feats.GetFeatLevel(FeatList.Rage) > 0 )
            {
            	if( DateTime.Compare( m.NextRage, DateTime.Now ) > 0 )
            	{
            		TimeSpan waitingtime = m.NextRage - DateTime.Now;
            		m.SendMessage( 60, "You need wait another " + ((waitingtime.Minutes * 60) + waitingtime.Seconds).ToString() + " seconds before using this ability again." );
            		return;
            	}
            	
                if( m.RageTimer == null )
                {
                    m.RageTimer = new RageTimer( m, m.Feats.GetFeatLevel(FeatList.Rage) );
                    m.SendMessage( 60, "You go on a bloodthirsty rage." );
                    m.RageFeatLevel = m.Feats.GetFeatLevel(FeatList.Rage);
                    m.RageTimer.Start();
                }

                else
                    m.SendMessage( 60, "You are already enraged." );
            }

            else
                m.SendMessage( 60, "You do not know how to do this." );
        }

        public class RageTimer : Timer
        {
            private Mobile m;
            private int m_Level;

            public RageTimer( Mobile from, int featlevel )
                : base( TimeSpan.FromMinutes(featlevel) )
            {
                m = from;
                m_Level = featlevel;
                SpellHelper.AddStatBonus( m, m, StatType.HitsMax, (featlevel * 10), TimeSpan.FromMinutes(featlevel) );
                m.Hits += featlevel * 10;
                m.Emote( "*breaks the stillness of the air with an enraged roar*" );
            }

            protected override void OnTick()
            {
            	if( m == null || m.Deleted )
            		return;
            	
            	m.Stam -= ( m.RawStam / 6 ) * ( m_Level - ((IKhaerosMobile)m).Feats.GetFeatLevel(FeatList.TirelessRage) );
            	m.Emote( "*grows weary as " + ((IKhaerosMobile)m).GetPossessivePronoun() + " rage subsides*" );
            	((IKhaerosMobile)m).RageTimer = null;
            	((IKhaerosMobile)m).NextRage = DateTime.Now + TimeSpan.FromMinutes(5);
            }
        }

        [Usage( "RopeTrick" )]
        [Description( "Allows you to try to leash a mobile." )]
        private static void RopeTrick_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if ( m.Feats.GetFeatLevel(FeatList.RopeTrick) > 0 )
            {
                Item item = m.Backpack.FindItemByType( typeof( Rope ) );

                if( item != null )
                {
                    Rope rope = item as Rope;
                    rope.OnDoubleClick( e.Mobile );
                }

                else
                    m.SendMessage( 60, "You must a have a rope in your backpack in order to attempt this attack." );
            }

            else
                m.SendMessage( 60, "You do not know how to perform this attack." );
        }

        
        [Usage( "Look" )]
        [Description( "Allows you to check out an object's description." )]
        private static void Look_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.Alive && !m.Deleted && m != null )
                m.Target = new LookTarget( m );

        }

        private class LookTarget : Target
        {
            public LookTarget( PlayerMobile m )
                : base( 8, false, TargetFlags.None )
            {
            	this.AllowNonlocal = true;
            	this.CheckLOS = false;
            	m.SendMessage( 60, "What do you wish to look at?" );
            }
			
			protected override void OnTargetNotAccessible( Mobile from, object targeted )
			{
				OnTarget( from, targeted );
			}
            
            protected override void OnTarget( Mobile m, object obj )
            {
                PlayerMobile pm = m as PlayerMobile;
                BaseCreature bc = null;
                PlayerMobile self = null;
                Item item = null;
                BaseArmor armor = obj as BaseArmor;
                BaseWeapon weapon = obj as BaseWeapon;
                string article = "a";
                
                if( obj is Item )
                	item = obj as Item;
                
                if( obj is BaseCreature )
                	bc = obj as BaseCreature;
                
                if( obj is PlayerMobile )
                	self = obj as PlayerMobile;
                		
                if( obj is BaseWeapon )
                {
                	if( weapon.Engraved1 != null )
                	{       	
                		if( weapon.Engraved1 == "amber" || weapon.Engraved1 == "amethyst" )
                			article = "an";
                		
                		m.SendMessage( 60, "This weapon has been engraved with " + article + " " + weapon.Engraved1 + "." );
                	}
                	
                	if( weapon.Engraved2 != null )
                	{     
                		article = "a";
                		
                		if( weapon.Engraved2 == "amber" || weapon.Engraved2 == "amethyst" )
                			article = "an";
                		
                		m.SendMessage( 60, "This weapon has been engraved with " + article + " " + weapon.Engraved2 + "." );
                	}
                	
                	if( weapon.Engraved3 != null )
                	{       
                		article = "a";
                		
                		if( weapon.Engraved3 == "amber" || weapon.Engraved3 == "amethyst" )
                			article = "an";
                		
                		m.SendMessage( 60, "This weapon has been engraved with " + article + " " + weapon.Engraved3 + "." );
                	}
                }
                
                if( obj is BaseArmor )
                {
                	if( armor.Engraved1 != null )
                	{    
                		article = "a";
                		
                		if( armor.Engraved1 == "amber" || armor.Engraved1 == "amethyst" )
                			article = "an";
                		
                		m.SendMessage( 60, "This piece of armour has been engraved with " + article + " " + armor.Engraved1 + "." );
                	}
                	
                	if( armor.Engraved2 != null )
                	{      
                		article = "a";
                		
                		if( armor.Engraved2 == "amber" || armor.Engraved2 == "amethyst" )
                			article = "an";
                		
                		m.SendMessage( 60, "This piece of armour has been engraved with " + article + " " + armor.Engraved2 + "." );
                	}
                	
                	if( armor.Engraved3 != null )
                	{       
                		article = "a";
                		
                		if( armor.Engraved3 == "amber" || armor.Engraved3 == "amethyst" )
                			article = "an";
                		
                		m.SendMessage( 60, "This piece of armour been engraved with " + article + " " + armor.Engraved3 + "." );
                	}
                }

                pm.SendGump( new LookGump( pm, bc, self, item, 1 ) );
            }
        }
        
        [Usage( "NameEquip" )]
        [Description( "Allows you to change the name of an equipment you have created." )]
        private static void NameEquip_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
            
            string name = e.ArgString.Trim();
            
            if( name == null || name.Length < 1 )
            {
            	m.SendMessage( 60, "Usage example: .NameEquip a well-crafted bastard sword" );
            	return;
            }

            if( m != null && m.Alive && !m.Deleted && m.Feats.GetFeatLevel(FeatList.RenownedMasterwork) > 1 )
                m.Target = new NameEquipTarget( m, name );
            
            else
            	m.SendMessage( 60, "You lack the appropriate feat." );
        }

        private class NameEquipTarget : Target
        {
        	private string m_name;
            public NameEquipTarget( PlayerMobile m, string name )
                : base( 8, false, TargetFlags.None )
            {
            	m_name = name;
            	m.SendMessage( 60, "What do you wish to rename?" );
            }
            
            protected override void OnTarget( Mobile m, object obj )
            {
                PlayerMobile pm = m as PlayerMobile;

                if( obj is BaseJewel )
                {
                	BaseJewel jewel = obj as BaseJewel;
                	
                	if( jewel.Crafter == pm && jewel.Quality > WeaponQuality.Exceptional )
                		jewel.Name = m_name;
                	
                	else
                		m.SendMessage( 60, "You can only rename jewelry of Extraordinary or Masterwork quality that you have crafted yourself." );
                }
                
                else if( obj is BaseArmor )
                {
                	BaseArmor armor = obj as BaseArmor;

                    if (armor.Crafter == pm && armor.Quality == ArmorQuality.Extraordinary)
                		armor.Name = m_name;

                    else if (armor.Crafter == pm && armor.Quality == ArmorQuality.Masterwork)
                        armor.Name = m_name;
                	else
                		m.SendMessage( 60, "You can only rename masterwork equipment you have crafted yourself." );
                }
                
                else if( obj is BaseWeapon )
                {
                	BaseWeapon weapon = obj as BaseWeapon;
                	
                	if( weapon.Crafter == pm && weapon.Quality == WeaponQuality.Masterwork )
                		weapon.Name = m_name;
                    else if (weapon.Crafter == pm && weapon.Quality == WeaponQuality.Extraordinary)
                        weapon.Name = m_name;
                	
                	else
                		m.SendMessage( 60, "You can only rename masterwork equipment you have crafted yourself." );
                }
                
                else if( obj is BaseClothing )
                {
                	BaseClothing weapon = obj as BaseClothing;
                	
                	if( weapon.Crafter == pm && ( weapon.Quality == ClothingQuality.Masterwork || weapon.Quality == ClothingQuality.Extraordinary ) )
                		weapon.Name = m_name;
                	
                	else
                		m.SendMessage( 60, "You can only rename masterwork equipment you have crafted yourself." );
                }
                
                else
                	m.SendMessage( 60, "That is not a valid target" );
            }
        }
        
        public class EmbedTarget : Target
        {
        	private Item m_gem = null;
        	private string m_gemname = null;
        	
            public EmbedTarget( PlayerMobile m, string gemname, Item gem )
                : base( 8, false, TargetFlags.None )
            {
            	m_gem = gem;
            	m_gemname = gemname;
                
            	m.SendMessage( 60, "Choose a piece of armour or a weapon to enbed this gem on." );
                if (m_gem is Cinnabar)
                    m.SendMessage(60, "Or, target a mortar to refine the cinnabar into mercury.");
            }
            
            protected override void OnTarget( Mobile m, object obj )
            {
                PlayerMobile pm = m as PlayerMobile;

                if(obj is AlchemyTool && (obj as AlchemyTool).IsChildOf(m.Backpack) && m_gemname == "cinnabar")
                {
                    AlchemyTool tool = obj as AlchemyTool;
                    tool.UsesRemaining--;
                    m.PlaySound(0x242);
                    m.SendMessage("You refine the cinnabar into mercury.");
                    m_gem.Consume();
                    RefinedMercury mercury = new RefinedMercury();
                    m.AddToBackpack(mercury);
                    m.PlaySound(0x04E);
                    return;
                }
                if( obj is BaseWeapon )
                {
                	BaseWeapon weapon = obj as BaseWeapon;
                	
                	if( weapon.Crafter == pm )
                	{
                		if( weapon.RootParentEntity != pm )
                		{
                			pm.SendMessage( 60, "That must be in your backpack before you can enbed a gem on it." );
                			return;
                		}
                		
                		if( weapon.Engraved1 == null && pm.Feats.GetFeatLevel(FeatList.GemEmbedding) > 0 )
                		{
                			weapon.Engraved1 = m_gemname;
                			
                			if( m_gem.Amount > 1 )
                				m_gem.Amount--;
                			
                			else
                				m_gem.Delete();
                		}
                		
                		else if( weapon.Engraved2 == null && pm.Feats.GetFeatLevel(FeatList.GemEmbedding) > 1 )
                		{
                			weapon.Engraved2 = m_gemname;
                			
                			if( m_gem.Amount > 1 )
                				m_gem.Amount--;
                			
                			else
                				m_gem.Delete();
                		}
                		
                		else if( weapon.Engraved3 == null && pm.Feats.GetFeatLevel(FeatList.GemEmbedding) > 2 )
                		{
                			weapon.Engraved3 = m_gemname;
                			
                			if( m_gem.Amount > 1 )
                				m_gem.Amount--;
                			
                			else
                				m_gem.Delete();
                		}
                		
                		else
                			return;
                	}
                	
                	else
                	{
                		pm.SendMessage( "You can only engrave exceptional or masterwork weapons that you crafted." );
                		return;
                	}
                }
                
                else if( obj is BaseArmor )
                {
                	BaseArmor armor = obj as BaseArmor;
                	
                	if( armor.Crafter == pm )
                	{
                		if( armor.RootParentEntity != pm )
                		{
                			pm.SendMessage( 60, "That must be in your backpack before you can enbed a gem on it." );
                			return;
                		}
                		
                		if( armor.Engraved1 == null && pm.Feats.GetFeatLevel(FeatList.GemEmbedding) > 0 )
                		{
                			armor.Engraved1 = m_gemname;
                			
                			if( m_gem.Amount > 1 )
                				m_gem.Amount--;
                			
                			else
                				m_gem.Delete();
                		}
                		
                		else if( armor.Engraved2 == null && pm.Feats.GetFeatLevel(FeatList.GemEmbedding) > 1 )
                		{
                			armor.Engraved2 = m_gemname;
                			
                			if( m_gem.Amount > 1 )
                				m_gem.Amount--;
                			
                			else
                				m_gem.Delete();
                		}
                		
                		else if( armor.Engraved3 == null && pm.Feats.GetFeatLevel(FeatList.GemEmbedding) > 2 )
                		{
                			armor.Engraved3 = m_gemname;
                			
                			if( m_gem.Amount > 1 )
                				m_gem.Amount--;
                			
                			else
                				m_gem.Delete();
                		}
                		
                		else
                			return;
                	}
                	
                	else
                	{
                		pm.SendMessage( "You can only engrave exceptional or masterwork armor that you crafted." );
                		return;
                	}
                }
                
                string article = "a";
                
                if( m_gemname == "amber" || m_gemname == "amethyst" )
                	article = "an";

                pm.SendMessage( 60, "You have successfully embedded " + article + " " + m_gemname + " on that item." );
            	pm.PlaySound( 0x2A );
            }
        }
        
        [Usage( "AddAlly" )]
        [Description( "Allows you to add a mobile to your ally list." )]
        private static void AddAlly_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.Alive && !m.Deleted && m != null )
                m.Target = new AddAllyTarget( m );
        }

        private class AddAllyTarget : Target
        {
            public AddAllyTarget( PlayerMobile m )
                : base( 8, false, TargetFlags.None )
            {
            	m.SendMessage( 60, "Whom do you wish to add to your ally list?" );
            }
            
            protected override void OnTarget( Mobile from, object obj )
            {
            	PlayerMobile m = from as PlayerMobile;
            	
            	if( obj is Mobile )
            	{
            		Mobile mob = obj as Mobile;
            		
            		if( m.AllyList == null )
            			m.AllyList = new List<Mobile>();
            		
            		if( m.AllyList.Contains( mob ) )
            		{
            			m.SendMessage( 60, mob.Name + " is already in your ally list." );
            			return;
            		}
            		
            		m.AllyList.Add( mob );
            		m.SendMessage( 60, "You have successfully added " + mob.Name + " to your ally list." );
            		return;
            	}
            	
            	m.SendMessage( 60, "That is not a valid target." );
            }          
        }
        
        [Usage( "RemoveAlly" )]
        [Description( "Allows you to remove a mobile from your ally list." )]
        private static void RemoveAlly_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m.Alive && !m.Deleted && m != null )
                m.Target = new RemoveAllyTarget( m );
        }
        
        private class RemoveAllyTarget : Target
        {
            public RemoveAllyTarget( PlayerMobile m )
                : base( 8, false, TargetFlags.None )
            {
            	m.SendMessage( 60, "Whom do you wish to remove from your ally list?" );
            }
            
            protected override void OnTarget( Mobile from, object obj )
            {
            	PlayerMobile m = from as PlayerMobile;
            	
            	if( obj is Mobile )
            	{
            		Mobile mob = obj as Mobile;
            		
            		if( m.AllyList == null )
            		{
            			m.AllyList = new List<Mobile>();
            			m.SendMessage( 60, "You still do not have anyone in your ally list." );
            			return;
            		}
            		
            		if( m.AllyList.Contains( mob ) )
            		{
            			m.AllyList.Remove( mob );
            			m.SendMessage( 60, "You have successfully removed " + mob.Name + " from your ally list." );
            			return;
            		}
            		
            		m.SendMessage( 60, mob.Name + " is not in your ally list." );
            		return;
            	}
            	
            	m.SendMessage( 60, "That is not a valid target." );
            }          
        }
        
        [Usage( "TurnInto" )]
        [Description( "Allows you to turn a human mobile into a member of the chosen race." )]
        private static void TurnInto_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
            
            string language = e.ArgString.Trim();

            switch( language.ToLower() )
            {
            	case "Southern":
            	{
            		m.Target = new TurnIntoTarget( Nation.Southern, false, false, false, false );
            		break;
            	}
            		
            	case "Southernpoor":
            	{
            		m.Target = new TurnIntoTarget( Nation.Southern, true, false, false, false );
            		break;
            	}
            		
            	case "Southernrich":
            	{
            		m.Target = new TurnIntoTarget( Nation.Southern, false, true, false, false );
            		break;
            	}
            		
            	case "Southerncrafter":
            	{
            		m.Target = new TurnIntoTarget( Nation.Southern, false, false, true, false );
            		break;
            	}
            	
            	case "Southernguard":
            	{
            		m.Target = new TurnIntoTarget( Nation.Southern, false, false, false, true );
            		break;
            	}
            	
            	case "Western":
            	{
            		m.Target = new TurnIntoTarget( Nation.Western, false, false, false, false );
            		break;
            	}
            		
            	case "Westernpoor":
            	{
            		m.Target = new TurnIntoTarget( Nation.Western, true, false, false, false );
            		break;
            	}
            		
            	case "Westernrich":
            	{
            		m.Target = new TurnIntoTarget( Nation.Western, false, true, false, false );
            		break;
            	}
            		
            	case "Westerncrafter":
            	{
            		m.Target = new TurnIntoTarget( Nation.Western, false, false, true, false );
            		break;
            	}
            		
            	case "Westernguard":
            	{
            		m.Target = new TurnIntoTarget( Nation.Western, false, false, false, true );
            		break;
            	}
            		
            	case "Haluaroc":
            	{
            		m.Target = new TurnIntoTarget( Nation.Haluaroc, false, false, false, false );
            		break;
            	}
            		
            	case "Haluarocpoor":
            	{
            		m.Target = new TurnIntoTarget( Nation.Haluaroc, true, false, false, false );
            		break;
            	}
            		
            	case "Haluarocrich":
            	{
            		m.Target = new TurnIntoTarget( Nation.Haluaroc, false, true, false, false );
            		break;
            	}
            		
            	case "Haluaroccrafter":
            	{
            		m.Target = new TurnIntoTarget( Nation.Haluaroc, false, false, true, false );
            		break;
            	}
            		
            	case "Haluarocguard":
            	{
            		m.Target = new TurnIntoTarget( Nation.Haluaroc, false, false, false, true );
            		break;
            	}
            		
            	case "mhordul":
            	{
            		m.Target = new TurnIntoTarget( Nation.Mhordul, false, false, false, false );
            		break;
            	}
            		
            	case "mhordulpoor":
            	{
            		m.Target = new TurnIntoTarget( Nation.Mhordul, true, false, false, false );
            		break;
            	}
            		
            	case "mhordulrich":
            	{
            		m.Target = new TurnIntoTarget( Nation.Mhordul, false, true, false, false );
            		break;
            	}
            		
            	case "mhordulcrafter":
            	{
            		m.Target = new TurnIntoTarget( Nation.Mhordul, false, false, true, false );
            		break;
            	}
            		
            	case "mhordulguard":
            	{
            		m.Target = new TurnIntoTarget( Nation.Mhordul, false, false, false, true );
            		break;
            	}
            		
            	case "Tirebladd":
            	{
            		m.Target = new TurnIntoTarget( Nation.Tirebladd, false, false, false, false );
            		break;
            	}
            		
            	case "Tirebladdpoor":
            	{
            		m.Target = new TurnIntoTarget( Nation.Tirebladd, true, false, false, false );
            		break;
            	}
            		
            	case "Tirebladdrich":
            	{
            		m.Target = new TurnIntoTarget( Nation.Tirebladd, false, true, false, false );
            		break;
            	}
            		
            	case "Tirebladdcrafter":
            	{
            		m.Target = new TurnIntoTarget( Nation.Tirebladd, false, false, true, false );
            		break;
            	}
            		
            	case "Tirebladdguard":
            	{
            		m.Target = new TurnIntoTarget( Nation.Tirebladd, false, false, false, true );
            		break;
            	}
            		
            	case "Northern":
            	{
            		m.Target = new TurnIntoTarget( Nation.Northern, false, false, false, false );
            		break;
            	}
            		
            	case "Northernpoor":
            	{
            		m.Target = new TurnIntoTarget( Nation.Northern, true, false, false, false );
            		break;
            	}
            		
            	case "Northernrich":
            	{
            		m.Target = new TurnIntoTarget( Nation.Northern, false, true, false, false );
            		break;
            	}
            		
            	case "Northerncrafter":
            	{
            		m.Target = new TurnIntoTarget( Nation.Northern, false, false, true, false );
            		break;
            	}
            		
            	case "Northernguard":
            	{
            		m.Target = new TurnIntoTarget( Nation.Northern, false, false, false, true );
            		break;
            	}
            		
            	case "imperialguard":
            	{
            		m.Target = new TurnIntoTarget( Utility.RandomBool() == true ? Nation.Northern : Nation.Tirebladd, false, false, false, true, "imperial" );
            		break;
            	}
            }
        }
        
        private class TurnIntoTarget : Target
        {
        	private Nation m_nation;
        	private bool Poor;
        	private bool Rich;
        	private bool Crafter;
        	private bool Guard;
        	private string m_custom;
        	
        	public TurnIntoTarget( Nation nation, bool IsPoor, bool IsRich, bool IsCrafter, bool IsGuard ) : this( nation, IsPoor, IsRich, IsCrafter, IsGuard, null ) {}
            public TurnIntoTarget( Nation nation, bool IsPoor, bool IsRich, bool IsCrafter, bool IsGuard, string custom )
                : base( 8, false, TargetFlags.None )
            {
            	m_nation = nation;
            	Poor = IsPoor;
            	Rich = IsRich;
            	Crafter = IsCrafter;
            	Guard = IsGuard;
            	m_custom = custom;
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	m.SendMessage( 60, "Whom do you wish to convert?" );
            	
                Mobile mob = obj as Mobile;
                
                if( !( obj is Mobile ) )
                {
                	m.SendMessage( 60, "That is not a valid target." );
                	return;
                }
                
                if( mob.BodyValue != 400 && mob.BodyValue != 401 )
                {
                	m.SendMessage( 60, "You can only convert human mobiles." );
                	return;
                }

                mob.Hue = 0;
                mob.HairItemID = 0;
                mob.FacialHairItemID = 0;
                
                if( mob.BodyValue == 401 )
                	mob.Female = true;
                
                int hairhue = BaseKhaerosMobile.AssignRacialHairHue( m_nation );
                mob.Hue = BaseKhaerosMobile.AssignRacialHue( m_nation );
				mob.HairItemID = BaseKhaerosMobile.AssignRacialHair( m_nation, mob.Female );
				mob.HairHue = hairhue;
				
				if( !mob.Female )
				{
					mob.FacialHairItemID = BaseKhaerosMobile.AssignRacialFacialHair( m_nation );
					mob.FacialHairHue = hairhue;
					mob.Name = BaseKhaerosMobile.RandomName( m_nation, false );
				}
				
				else
					mob.Name = BaseKhaerosMobile.RandomName( m_nation, true );
				
				if( Poor || Rich || Crafter || Guard )
					RemoveEquippedItems( mob );
				
				if( Poor )
					BaseKhaerosMobile.RandomPoorClothes( mob, m_nation );
				
				if( Rich )
					BaseKhaerosMobile.RandomRichClothes( mob, m_nation );
				
				if( Crafter )
					BaseKhaerosMobile.RandomCrafterClothes( mob, m_nation );
				
				if( mob is BaseCreature )
	            {
		            BaseCreature bc = mob as BaseCreature;
		            bc.AI = AIType.AI_Melee;
				}
				
				if( Guard )
				{
					if( m_custom == null )
						BaseKhaerosMobile.RandomGuardEquipment( mob, m_nation, 0 );
					
					if( mob.Str < 100 )
						m.SendMessage( 60, "Warning: target might not be strong enough to equip guard armour and weaponry." );
				}
            }
        }
        
        public static void RemoveEquippedItems( Mobile mob )
        {
        	Item item;
        	
        	if( mob.FindItemOnLayer( Layer.FirstValid ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.FirstValid );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.TwoHanded ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.TwoHanded );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.InnerTorso ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.InnerTorso );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.OuterTorso ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.OuterTorso );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.Arms ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.Arms );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.InnerLegs ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.InnerLegs );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.OuterLegs ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.OuterLegs );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.Waist ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.Waist );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.Helm ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.Helm );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.Gloves ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.Gloves );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.OneHanded ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.OneHanded );
        		item.Delete();
        	}
			
        	if( mob.FindItemOnLayer( Layer.Pants ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.Pants );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.Earrings ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.Earrings );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.Bracelet ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.Bracelet );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.Cloak ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.Cloak );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.Neck ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.Neck );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.MiddleTorso ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.MiddleTorso );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.Talisman ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.Talisman );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.Ring ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.Ring );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.Shirt ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.Shirt );
        		item.Delete();
        	}
        	
        	if( mob.FindItemOnLayer( Layer.Shoes ) != null )
        	{
        		item = mob.FindItemOnLayer( Layer.Shoes );
        		item.Delete();
        	}
        }
        
        [Usage( "Grab" )]
        [Description( "Allows you to grab an unmovable item." )]
        private static void Grab_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
              
            if( m != null && m.Alive && !m.Paralyzed )
        		m.Target = new GrabTarget();
        }
        
        private class GrabTarget : Target
        {
            public GrabTarget()
                : base( 8, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	m.SendMessage( "What do you wish to grab?" );
            	
                Item item = obj as Item;
                
                if( !( obj is Item ) || !item.CanBeGrabbed || 
                    item.RootParentEntity != null || item.Movable )
                {
                	m.SendMessage( "That is not a valid target." );
                	return;
                }
                
               if( !m.InRange( item.Location, 1 ) )
               {
               		m.SendMessage( "That is out of range." );
                	return;
               }
                
                Container pack = m.Backpack;

				if ( pack != null )
				{
					pack.DropItem( item );
					m.SendMessage( "The item has been placed in your backpack." );
					item.Movable = true;
					item.CanBeGrabbed = false;
				}
            }
        }
        
        [Usage( "CustomNPC" )]
        [Description( "Allows you to customize an NPC." )]
        private static void CustomNPC_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
              
            if( m != null )
        	{
            	string nationchoice = e.ArgString.Trim();
            	nationchoice = nationchoice.ToLower();
            	Nation nation = Nation.None;
                	
                if( nationchoice == "Southern" )
                	nation = Nation.Southern;
                	
            	else if( nationchoice == "Western" )
            		nation = Nation.Western;
            	
            	else if( nationchoice == "Haluaroc" )
            		nation = Nation.Haluaroc;
            	
            	else if( nationchoice == "mhordul" )
            		nation = Nation.Mhordul;
            	
            	else if( nationchoice == "Tirebladd" )
            		nation = Nation.Tirebladd;
            	
            	else if( nationchoice == "Northern" )
            		nation = Nation.Northern;
            	
            	else
            	{
            		m.SendMessage( "Usage example: .CustomNPC Southern" );
            		return;
            	}
            	
        		m.Target = new CustomNPCTarget( nation );
        	}
        }
        
        private class CustomNPCTarget : Target
        {
        	private Nation m_nation;
            public CustomNPCTarget( Nation nation )
                : base( 8, false, TargetFlags.None )
            {
            	m_nation = nation;
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                Mobile mob = obj as Mobile;
                
                if( !( obj is Mobile ) )
                {
                	m.SendMessage( "That is not a valid target." );
                	return;
                }
                
                m.SendGump( new NPCCustomGump( mob, m_nation, 0, true ) );
            }
        }
        
        [Usage( "HairStyling" )]
        [Description( "Allows you to customize a mobile's hair." )]
        private static void HairStyling_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
            
            if( !m.Alive || m.Paralyzed )
            	return;
              
            if( m != null )
        	{
            	if( m.Feats.GetFeatLevel(FeatList.Barbery) > 0 )
        			m.Target = new HairStylingTarget();
            	
            	else
            		m.SendMessage( "You do not have the Hair Styling feat." );
        	}
        }
        
        private class HairStylingTarget : Target
        {
            public HairStylingTarget()
                : base( 8, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( !m.Alive || m.Paralyzed )
            		return;
            	
                Mobile mob = obj as Mobile;
                
                if( !( obj is PlayerMobile ) && !( obj is Mercenary ) )
                {
                	m.SendMessage( "That is not a valid target." );
                	return;
                }
                
                Item apron = mob.FindItemOnLayer( Layer.MiddleTorso );
                
                if( apron != null && apron is HairStylingApron && m.CanSee( mob ) && m.InLOS( mob ) && m.InRange( mob, 1 ) )
	                m.SendGump( new NPCCustomGump( mob, ( (PlayerMobile)m ).Nation, 0, false, true ) );
                
                else
                	m.SendMessage( "Your target needs to be wearing a hair styling apron." );
            }
        }
        [Usage("Dazzle")]
        [Description("Allows mages to use effects for RP.")]
        private static void Dazzle_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile == null || !(e.Mobile is PlayerMobile))
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;

            if (m.Feats.GetFeatLevel(FeatList.Magery) < 3)
                return; 
            if (e.Length >= 2)
            {
                string un = e.Arguments[0];
                string pw = e.Arguments[1];

                //Account dispAccount = null;
                string notice;

                if (un == null || un.Length == 0)
                {
                    notice = "You must enter an itemid.";
                }
                else if (pw == null || pw.Length == 0)
                {
                    notice = "You must enter a hue.";
                }
                else
                {
                    notice = " "; 
                    m.Target = new DazzleTarget(un, pw);
                    
                    
                }

                m.SendMessage(notice);
            }

            else
            {
                m.SendMessage("Incorrect usage. Please add two arguments to the command, the first being the ItemID and the second, its hue.");
                m.SendMessage("Example: \".Dazzle 4154 123.");
            }
        }
        private class DazzleTarget : Target
        {
            private string un1;
            private string pw1;
            private int un2;
            private int pw2; 


            public DazzleTarget(string un, string pw)
                : base(15, false, TargetFlags.None)
            {
                un1 = un;
                pw1 = pw; 
            }

            protected override void OnTarget(Mobile m, object obj)
            {
                if (m == null || m.Deleted)
                    return;
                if (m.Mana < 5)
                {
                    m.SendMessage("You do not have the energy for this.");
                    return;
                }
                if (obj == null)
                {
                    m.SendMessage("That no longer exists.");
                    return;
                }
               /* if (!(obj is Mobile))
                {
                    m.SendMessage("You don't have the skill to converse with objects.");
                    return;
                }*/
                try
                {
                    un2 = Convert.ToInt32(un1);
                    pw2 = Convert.ToInt32(pw1);
                }
                catch (OverflowException)
                {
                    m.SendMessage("Are you trying to crash the shard?");
                    return;
                }
                catch (FormatException)
                {
                    m.SendMessage("Are you trying to crash the shard?");
                    return;
                }  
                Mobile targ = obj as Mobile;
                IPoint3D point3d = obj as IPoint3D;
                Point3D location = new Point3D(point3d.X, point3d.Y, point3d.Z);
                m.Mana -= 5;
                if (obj is Mobile)
                {
                    targ.FixedParticles(un2, 244, 50, 9950, pw2, 0, EffectLayer.Waist);
                }
                else
                {

                    Effects.SendLocationEffect( location, m.Map, un2, 50, 244, pw2, 0);
                }

               // targ.SendMessage(2659, m.Name + " " + "(telepathy):" + " " + m_speech);
            }
        }      
        [Usage("Burn")]
        [Description("Allows staff to create instant bombs!")]
        private static void Burn_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
        
           // Map map = Caster.Map;

          //  if (map == null)
          //      return;
            if (e.Mobile == null || !(e.Mobile is PlayerMobile) || e.Length < 1 || e.Arguments[0].Trim().Length < 1)
                return;
            int dist = 15; 
            int body = 0;

            if (e.Length > 0 && int.TryParse(e.Arguments[0], out body))
            {
               
                m.Target = new BurnTarget(body, false, dist);
            }

        }

        private class BurnTarget : Target
        {
            private int m_body;

            public BurnTarget(int body, bool emote, int dist)
                : base(dist, false, TargetFlags.None)
            {
                m_body = body;
            }

            protected override void OnTarget(Mobile m, object obj)
            {
                if (m == null || m.Deleted)
                    return;
                IPoint3D point3d = obj as IPoint3D;
                Point3D location = new Point3D(point3d.X, point3d.Y, point3d.Z);
           //                 Point3D loc = obj.Location;
            Map map = m.Map;

            BombPotion pot = new BombPotion(1);

            pot.InstantExplosion = true;
            pot.ExplosionRange = m_body;
            pot.AddEffect(CustomEffect.Explosion, 100);
            pot.AddEffect(CustomEffect.Fire, 100);
            pot.AddEffect(CustomEffect.Shrapnel, m.RawInt);
            pot.HeldBy = m;
            pot.PotionEffect = PotionEffect.ExplosionLesser;

            pot.Explode(m, false, location, map);

            }
        }          


        [Usage("CreateEcho")]
        [Description("Allows you to add speech to an item.")]
        private static void CreateEcho_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if (m != null)
            {
                int dist = m.Feats.GetFeatLevel(FeatList.EnchantTrinket) * 2;

              //  if (m.AccessLevel >= AccessLevel.Counselor)
                //    dist = 15;
               // XmlData awe = XmlAttach.FindAttachment(m, typeof(XmlData), "CreateEcho") as XmlData;
             /*   if (awe == null)
                {
                    m.SendMessage("You lack this ability.");
                    return;
                }*/
                if (dist == 0)
                {
                    m.SendMessage("You lack the appropriate feat.");
                    return;
                }

                else
                {
                    string speech = e.ArgString;
                   // string nam = m.Name;
                    m.Target = new CreateEchoTarget(speech, false, dist);
                }
            }
        }
        private class CreateEchoTarget : Target
        {
            private string m_speech;

            public CreateEchoTarget(string speech, bool emote, int dist)
                : base(dist, false, TargetFlags.None)
            {
                m_speech = speech;
            }

            protected override void OnTarget(Mobile m, object obj)
            {
                if (m == null || m.Deleted)
                    return;
                if (m.Mana < 25)
                {
                    m.SendMessage("You do not have the energy for this.");
                    return;
                }
                if (obj == null)
                {
                    m.SendMessage("That no longer exists.");
                    return;
                }
                if (obj is Mobile)
                {
                    m.SendMessage("You cannot enchant creatures.");
                    return;
                }
                //Mobile targ = obj as Mobile;
                m.Mana -= 25;
               XmlMessage one = new Engines.XmlSpawner2.XmlMessage(m_speech, 5, "Echo", 5);
               Engines.XmlSpawner2.XmlAttach.AttachTo(obj, one);;
                m.SendMessage(2659, "This item will speak your message when you command it by saying 'Echo', up to five times.");
            }
        }  
        [Usage("Telepathy")]
        [Description("Allows you to speak into other people's minds.")]
        private static void Telepathy_OnCommand(CommandEventArgs e)
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if (m != null)
            {
                int dist = m.Feats.GetFeatLevel(FeatList.MindI) * 2;

                if (m.AccessLevel >= AccessLevel.Counselor)
                    dist = 15;
                XmlData awe = XmlAttach.FindAttachment(m, typeof(XmlData), "Telepathy") as XmlData;
                if (awe == null)
                {
                    m.SendMessage("You lack this ability.");
                    return;
                }
                if (dist == 0)
                {
                    m.SendMessage("You lack the appropriate feat.");
                    return;
                }

                else
                {
                    string speech = e.ArgString;
                    string nam = m.Name;
                    m.Target = new TelepathyTarget(speech, false, dist);
                }
            }
        }
        private class TelepathyTarget : Target
        {
            private string m_speech;

            public TelepathyTarget(string speech, bool emote, int dist)
                : base(dist, false, TargetFlags.None)
            {
                m_speech = speech;
            }

            protected override void OnTarget(Mobile m, object obj)
            {
                if (m == null || m.Deleted)
                    return;
                if (m.Mana < 5)
                {
                    m.SendMessage("You do not have the energy for this.");
                    return;
                }
                if (obj == null)
                {
                    m.SendMessage("That no longer exists.");
                    return;
                }
                if (!(obj is Mobile))
                {
                    m.SendMessage("You don't have the skill to converse with objects.");
                    return;
                }
                Mobile targ = obj as Mobile;
                m.Mana -= 5; 
                targ.SendMessage(2659, m.Name + " " + "(telepathy):" + " " + m_speech);
            }
        }      
        [Usage( "Say" )]
        [Description( "Allows you to force speech on a mobile." )]
        private static void Say_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
              
            if( m != null )
        	{
            	int dist = m.Feats.GetFeatLevel(FeatList.Ventriloquism) * 2;

                int al = m.Feats.GetFeatLevel(FeatList.AnimalLore) * 2;
            	
            	if( m.AccessLevel >= AccessLevel.Counselor )
            		dist = 15;
            	
            	if(( dist == 0 ) && (al == 0))
            	{
            		m.SendMessage( "You lack the appropriate feat." );
            		return;
            	}

                if ((dist == 0) && (al > 0))
                {
                    string speech = e.ArgString;
                    m.Target = new SayTarget(speech, true, al);
                }
                else
                {
                    string speech = e.ArgString;
                    m.Target = new SayTarget(speech, false, dist);
                }
        	}
        }
        
        [Usage( "Emote" )]
        [Description( "Allows you to force an emote on a mobile." )]
        private static void Emote_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
              
            if( m != null )
        	{
            	string speech = e.ArgString;
        		m.Target = new SayTarget( speech, true, 15 );
        	}
        }
       
        private class SayTarget : Target
        {
        	private string m_speech;
        	private bool m_emote;
            public SayTarget( string speech, bool emote, int dist )
                : base( dist, false, TargetFlags.None )
            {
            	m_speech = speech;
            	m_emote = emote;
            }

            protected override void OnTarget( Mobile m, object obj )
            {
                if (m == null || m.Deleted)
                    return;

                if (obj == null)
                {
                    m.SendMessage("That no longer exists.");
                    return;
                }

            	DoSayEmote( m, obj, m_speech, m_emote );
            }
        }
        
        public static void DoSayEmote( Mobile m, object obj, string args, bool emote )
        {
        	if( m == null || m.Deleted || obj == null)
        		return;
 
        	if( !( obj is Mobile ) )
            {
            	if( obj is Item )
            	{
            		Item item = obj as Item;
            		
            		if( !emote )
            			item.PublicOverheadMessage( MessageType.Regular, 0x3B2, false, args );
            		else
            			item.PublicOverheadMessage( MessageType.Regular, 0x3B2, false, "*" + args + "*" );
            		
            		if( m is PlayerMobile )
                	( (PlayerMobile)m ).LastSayEmote = item;
            		
            		return;
            	}
            	
            	m.SendMessage( "That is not a valid target." );
            	return;
            }
            else if ((obj as Mobile) != null && !(obj as Mobile).Deleted)
            {
                Mobile mob = obj as Mobile;

                if ((m as PlayerMobile).Feats.GetFeatLevel(FeatList.Ventriloquism) < 1 && m.AccessLevel < AccessLevel.Counselor)
                {
                    if ( mob != null && !mob.Deleted && mob is BaseCreature )
                    {
                        if ((m as PlayerMobile).Feats.GetFeatLevel(FeatList.AnimalLore) < 3)
                        {
                            m.SendMessage("You do not know enough about animals to do this.");
                            return;
                        }

                        if ((mob as BaseCreature).Controlled)
                        {
                            if ((mob as BaseCreature).ControlMaster == null || (mob as BaseCreature).ControlMaster.Deleted)
                            {
                                m.SendMessage("This animal is not under your control.");
                                return;
                            }

                            if ((mob as BaseCreature).ControlMaster != m)
                            {
                                m.SendMessage("This animal is not under your control.");
                                return;
                            }
                        }
                        else
                        {
                            m.SendMessage("This animal is not tame.");
                            return;
                        }
                    }
                    else if (mob == null || mob.Deleted)
                    {
                        m.SendMessage("That animal no longer exists.");
                        return;
                    }
                    else
                    {
                        m.SendMessage("That animal is not under your control.");
                        return;
                    }
                }

                if (!emote)
                    mob.PublicOverheadMessage(MessageType.Regular, mob.SpeechHue, false, args);
                //mob.Emote( m_speech );

                else
                    mob.PublicOverheadMessage(MessageType.Regular, mob.EmoteHue, false, "*" + args + "*");

                if (m is PlayerMobile)
                    ((PlayerMobile)m).LastSayEmote = mob;
            }
        }
        
        [Usage( "MercSay" )]
        [Description( "Allows you to force speech on a follower you control." )]
        private static void MercSay_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
              
            if( m != null )
        	{
            	string speech = e.ArgString;
        		m.Target = new MercSayTarget( speech, false );
        	}
        }
        
        [Usage( "MercEmote" )]
        [Description( "Allows you to force an emote on a follower you control." )]
        private static void MercEmote_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
              
            if( m != null )
        	{
            	string speech = e.ArgString;
        		m.Target = new MercSayTarget( speech, true );
        	}
        }
        
        private class MercSayTarget : Target
        {
        	private string m_speech;
        	private bool m_emote;
            public MercSayTarget( string speech, bool emote )
                : base( 15, false, TargetFlags.None )
            {
            	m_speech = speech;
            	m_emote = emote;
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	m.SendMessage( "Choose a follower you control." );
            	
                Mobile mob = obj as Mobile;
                
                if( !( obj is Mobile ) || !( obj is Mercenary ) || 
                   ( obj is Mercenary && ( ( (Mercenary)obj ).Owner != m ) || 
                    !( (Mercenary)obj ).Alive || ( (Mercenary)obj ).Deleted || 
                   ( (Mercenary)obj ).Squelched || ( (Mercenary)obj ).Frozen ||
                  ( (Mercenary)obj ).Paralyzed || ( (Mercenary)obj ).Map != m.Map ) )
                {
                	m.SendMessage( "That is not a valid target." );
                	return;
                }
                
                if( !m_emote )
                	mob.PublicOverheadMessage( MessageType.Regular, mob.SpeechHue, false, m_speech );
                	//mob.Emote( m_speech );
                
                else
                	mob.PublicOverheadMessage( MessageType.Regular, mob.EmoteHue, false, "*" + m_speech + "*" );
                
                if( m is PlayerMobile )
                	( (PlayerMobile)m ).LastSayEmote = mob;
            }
        }
        
        [Usage( "LastSay" )]
        [Description( "Allows you to force speech on a mobile." )]
        private static void LastSay_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
              
            if( m.LastSayEmote != null )
            	DoSayEmote( m, m.LastSayEmote, e.ArgString, false );
        }
        
        [Usage( "LastEmote" )]
        [Description( "Allows you to force an emote on a mobile." )]
        private static void LastEmote_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
              
            if( m.LastSayEmote != null )
            	DoSayEmote( m, m.LastSayEmote, e.ArgString, true );
        }    
        
        [Usage( "ClearStudentList" )]
        [Description( "Clears your list of students." )]
        private static void ClearStudentList_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
              
            if( m != null && e.Mobile is PlayerMobile )
        	{
            	if( m.m_Students == null || ( m.m_Students != null && m.m_Students.Count == 0 ) )
            		m.SendMessage( 60, "Your student list is already empty." );
            	
            	else
            	{
            		m.m_Students.Clear();
            		m.SendMessage( 60, "You have successfully cleared your student list." );
            	}
        	}
        }
        
        [Usage( "Teach" )]
        [Description( "Allows you to add a player to your student list and start a teaching session." )]
        private static void Teach_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;

            if( m != null && m.Alive && !m.Deleted && !m.Paralyzed)
            {
            	if( m.Feats.GetFeatLevel(FeatList.Teaching) < 1 )
            	{
            		m.SendMessage( 60, "You lack the appropriate feat." );
            		return;
            	}
            	
                m.Target = new TeachTarget( m );
            }

        }

        private class TeachTarget : Target
        {
            public TeachTarget( PlayerMobile m )
                : base( 8, false, TargetFlags.None )
            {
            	m.SendMessage( 60, "Whom do you wish to add to your student list?" );
            }
            
            protected override void OnTarget( Mobile from, object obj )
            {
            	PlayerMobile m = from as PlayerMobile;
            	           	
            	if( obj is PlayerMobile && obj != from )
            	{
            		PlayerMobile mob = obj as PlayerMobile;
            		
            		if( m.m_Students == null )
            		{
            			m.m_Students = new List<PlayerMobile>();
            		}
            		
            		if( m.m_Students.Contains( mob ) )
            		{
            			m.SendMessage( 60, mob.Name + " is already in your student list." );
            			return;
            		}
            		
            		int level = m.Feats.GetFeatLevel(FeatList.Teaching) + m.Feats.GetFeatLevel(FeatList.Professor);
    
            		if( m.m_Students.Count < level )
            		{
            			if( m.CanTeach(mob) && mob.Level < 50 )
            			{
            				if( !mob.m_WantsTeaching )
            				{
            					m.SendMessage( 60, "That person does not seem to wish to be taught." );
            					return;
            				}
            				
            				if( SharedClass( m, mob ) )
            				{
			            		m.m_Students.Add( mob );
			            		m.SendMessage( 60, "You have successfully added " + mob.Name + " to your student list." );
			            		return;
            				}
            				
            				else
            				{
            					m.SendMessage( 60, "That person's potential abilities are beyond your understanding." );
            					return;
            				}
            			}
            			
            			else
            			{
            				m.SendMessage( 60, "You would not have anything to teach to that person." );
            				return;
            			}
            		}
            		
            		else
            		{
            			m.SendMessage( 60, "You already have as many students as your feat level allows you to have. Use .ClearStudentList if you wish to change your current students." );
            			return;
            		}
            	}
            	
            	m.SendMessage( 60, "That is not a valid target." );
            }          
        }
        
        public static bool SharedClass( PlayerMobile teacher, PlayerMobile student )
        {
        	return true;
        }
        
        [Usage( "Student" )]
        [Description( "Sets your character to be able to receive teaching." )]
        private static void Student_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m != null && m is PlayerMobile )
        	{
        		if( m.m_WantsTeaching )
        		{
        			m.m_WantsTeaching = false;
        			m.SendMessage( "Student Mode Off." );
        		}
        		
        		else if( !m.m_WantsTeaching )
        		{
        			m.m_WantsTeaching = true;
        			m.SendMessage( "Student Mode On." );
        		}
        	}
        }
        
        [Usage( "Split" )]
        [Description( "Split an pile of objects in two." )]
        private static void Split_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.Target = new SplitTarget();
        }
        
        private class SplitTarget : Target
        {
            public SplitTarget()
                : base( 8, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( !( obj is Item ) )
            		return;
            	
            	if( m == null )
            		return;
            	
            	if( m.Backpack == null )
            		return;
            	
            	Item item = obj as Item;
            	Container pack = m.Backpack;
            	
            	if( item.ParentEntity != pack )
            	{
            		m.SendMessage( "It needs to be in your backpack." );
            		return;
            	}
            	
            	if( item.Amount < 2 )
            	{
            		m.SendMessage( "Insufficient amount for splitting." );
            		return;
            	}
            	
            	int i = item.Amount / 2;
            	
                Type t = item.GetType();
                ConstructorInfo c = t.GetConstructor( Type.EmptyTypes );

                if( c != null )
                {
                	try
                	{
	                    object o = c.Invoke( null );
	
	                    if( o != null && o is Item )
	                    {
	                        Item newItem = (Item)o;
	                        item.DropToWorld( m, m.Location );
	                        Dupe.CopyProperties( newItem, item );
	                        item.OnAfterDuped( newItem );
	                        m.SendMessage( "You split the item in two piles." );
	                        item.Consume( i );
	                        newItem.Consume( item.Amount );
	                        pack.DropItem( newItem );
	                        pack.DropItem( item );
	                    }
                	}
                	
                	catch
                	{
                		m.SendMessage( "Error!" );
                	}
                }
            }
        }
        
        public static int CopperInPack( Container pack )
        {
        	int amount = 0;
        	
        	foreach( Item item in pack.Items )
        	{
        		if( item is Copper )
        			amount += item.Amount;
        		
        		else if( item is Silver )
        			amount += item.Amount * 10;
        		
        		else if( item is Gold )
        			amount += item.Amount * 100;
        		
        		else if( item is Container )
        			amount += CopperInPack( ((Container)item) );
        	}
        	
        	return amount;
        }

        [Usage( "JudgeWealth" )]
        [Description( "Allows you to determine how much copper a character is carrying." )]
        private static void JudgeWealth_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m.Feats.GetFeatLevel(FeatList.JudgeWealth) > 0 )
        		m.Target = new JudgeWealthTarget();
        	
        	else
        		m.SendMessage( "You do not have the appropriate feat." );
        }
        
        private class JudgeWealthTarget : Target
        {
            public JudgeWealthTarget() : base( 9, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( obj == null || !( obj is PlayerMobile ) || m == null || !m.Alive || m.Frozen || m.Paralyzed )
            		return;
            	
            	PlayerMobile pm = m as PlayerMobile;
            	PlayerMobile target = obj as PlayerMobile;
            	Container pack = target.Backpack;
            	
            	if( pack == null || !pm.InLOS( target ) || !pm.CanSee( target ) || !target.Alive )
            		return;
            	
            	int amount = CopperInPack( pack );
            	
            	if( pm.Feats.GetFeatLevel(FeatList.JudgeWealth) > 2 )
            		pm.SendMessage( "You estimate that " + target.Name + " is carrying " + amount + " copper coins." );
            	
            	else if( pm.Feats.GetFeatLevel(FeatList.JudgeWealth) > 1 )
            	{
            		if( amount > 999 )
            			pm.SendMessage( "You estimate that " + target.Name + " is carrying at least a thousand copper coins." );
            		
            		else if( amount > 499 )
            			pm.SendMessage( "You estimate that " + target.Name + " is carrying at least five hundred copper coins." );
            		
            		else if( amount > 99 )
            			pm.SendMessage( "You estimate that " + target.Name + " is carrying at least a hundred copper coins." );
            		
            		else if( amount < 100 )
            			pm.SendMessage( "You estimate that " + target.Name + " is carrying less than a hundred copper coins." );
            	}
            	
            	else if( pm.Feats.GetFeatLevel(FeatList.JudgeWealth) > 0 )
            	{
            		if( amount > 99 )
            			pm.SendMessage( "You estimate that " + target.Name + " is carrying at least a hundred copper coins." );
            		
            		else if( amount < 100 )
            			pm.SendMessage( "You estimate that " + target.Name + " is carrying less than a hundred copper coins." );
            	}
            }
        }
        
        [Usage( "Stash" )]
        [Description( "Allows you to add a secret stash." )]
        private static void Stash_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
            
            if( m == null || !m.Alive || m.Paralyzed )
            	return;
 
        	if( m.Feats.GetFeatLevel(FeatList.Stash) > 0 )
        		m.Target = new StashTarget();

        	else
        		m.SendMessage( "You do not have the Stash feat." );
        }
        
        private class StashTarget : Target
        {
            public StashTarget()
                : base( 2, true, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null || !m.Alive || m.Paralyzed )
            		return;
            	
            	if( obj is LandTarget || obj is StaticTarget || obj is Item )
            	{
            		IPoint3D point3d = obj as IPoint3D;
            		Point3D location = new Point3D( point3d.X, point3d.Y, point3d.Z );
            		PlayerMobile pm = m as PlayerMobile;
            		
            		if( m.InLOS( location ) && m.CanSee( obj ) )
            		{
            			if( obj is Stash && ( (Stash)obj ).Owner == m )
            				m.SendGump( new ConfirmDeleteStashGump( m, (Stash)obj ) );
            		
	            		else if( !pm.HasStash )
	            		{
	            			Stash stash = new Stash( pm, location );
	            			stash.MoveToWorld( location );
	            			stash.Map = pm.Map;
	            			stash.Movable = false;
	            			pm.HasStash = true;
	            			m.SendMessage( "You create a new stash." );
	            		}

	            		else
	            			m.SendMessage( "You already have a stash somewhere else. Use this command on your already existing stash to remove it, or wait until it decays." );
            		}
            	}
            	
            	else
            		m.SendMessage( "Invalid target." );
            }
        }
        
        public static void StealCoins( Container pack, int featlevel, ref int copper, ref int silver, ref int gold, ref ArrayList list )
        {
        	foreach( Item item in pack.Items )
        	{
        		if( item is Copper )
        		{
        			copper += item.Amount;
        			list.Add( item );
        		}
        		
        		else if( item is Silver )
        		{
        			silver += item.Amount;
        			list.Add( item );
        		}
        		
        		else if( item is Gold )
        		{
        			gold += item.Amount;
        			list.Add( item );
        		}
        		
        		else if( item is Container )
        			StealCoins( ((Container)item), featlevel, ref copper, ref silver, ref gold, ref list );
        	}
        }
        
        [Usage( "Cutpurse" )]
        [Description( "Allows you to steal coins from a player without having to snoop first." )]
        private static void Cutpurse_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m.Feats.GetFeatLevel(FeatList.Cutpurse) > 0 )
        		m.Target = new CutpurseTarget();
        	
        	else
        		m.SendMessage( "You do not have the appropriate feat." );
        }
        
        private class CutpurseTarget : Target
        {
            public CutpurseTarget() : base( 1, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( obj == null || !( obj is PlayerMobile ) || m == null || !m.Alive || m.Frozen || m.Paralyzed || m == obj )
            		return;
            	
            	PlayerMobile pm = m as PlayerMobile;
            	PlayerMobile target = obj as PlayerMobile;
            	Container pack = target.Backpack;
            	Container topack = pm.Backpack;
            	
            	if( pack == null || topack == null || !pm.InLOS( target ) || !pm.CanSee( target ) || !target.Alive )
            		return;
            	
            	bool caught = ( pm.Skills[SkillName.Stealing].Value < Utility.Random( ( 135 - (pm.Feats.GetFeatLevel(FeatList.Cutpurse) * 10) ) ) );
            	
            	if( !caught )
            	{
            		int copper = 0;
            		int silver = 0;
            		int gold = 0;
            		ArrayList list = new ArrayList();
            		
            		StealCoins( pack, pm.Feats.GetFeatLevel(FeatList.Cutpurse), ref copper, ref silver, ref gold, ref list );
            		
            		for( int i = 0; i < list.Count; ++i )
		        	{
		        		Item todelete = list[i] as Item;
		        		todelete.Delete();
		        	}
            		
            		if( copper > 0 )
            		{
	            		Copper coppercoins = new Copper( copper );
	            		topack.DropItem( coppercoins );
            		}
            		
            		if( silver > 0 )
            		{
	            		Silver silvercoins = new Silver( silver );
	            		topack.DropItem( silvercoins );
            		}
            		
            		if( gold > 0 )
            		{
	            		Gold goldcoins = new Gold( gold );
	            		topack.DropItem( goldcoins );
            		}
 
            		pm.SendMessage( "You took your target's coinpurse and found: " + copper + " copper coins, " + silver + " silver coins and " + gold + " gold coins." );
            	}
            	
            	else
				{
					string message = String.Format( "You notice {0} trying to steal from {1}.", pm.Name, target.Name );

					foreach ( NetState ns in pm.GetClientsInRange( 8 ) )
					{
						if ( ns != pm.NetState )
							ns.Mobile.SendMessage( message );
					}
				}
            }
        }
        
        [Usage( "PetStealing" )]
        [Description( "Allows you to steal a pet that has been stabled for a while." )]
        private static void PetStealing_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m.Feats.GetFeatLevel(FeatList.PetStealing) > 0 )
        		m.Target = new PetStealingTarget();
        	
        	else
        		m.SendMessage( "You do not have the appropriate feat." );
        }
        
        private class PetStealingTarget : Target
        {
            public PetStealingTarget() : base( 2, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null || m.Deleted || !m.Alive || m.Paralyzed || m.Frozen || !( m is PlayerMobile ) || m.Hidden )
            		return;
            	
            	StablePost post = obj as StablePost;
            	PlayerMobile pm = m as PlayerMobile;
            	
            	if( !( obj is StablePost ) || ( obj is StablePost && ( post.Controlled == null || post.Controlled.Deleted ) ) )
            	{
            		m.SendMessage( "Please target a stable post that has an animal attached to it." );
            		return;
            	}
            	
            	if( !pm.CanSee( post ) || !pm.InLOS( post ) )
            	{
            		m.SendMessage( "Target out of reach." );
            		return;
            	}
            	
            	if( DateTime.Compare( DateTime.Now, ( post.StabledDate + TimeSpan.FromDays( 20 - ( pm.Feats.GetFeatLevel(FeatList.PetStealing) * 5 ) ) ) ) < 0 )
            	{
            		m.SendMessage( "That animal has not been stabled long enough for you to steal it." );
            		return;
            	}
            	
            	BaseCreature bc = post.Controlled;
            	
            	if( ( bc.ControlSlots + pm.Followers ) > pm.FollowersMax )
            	{
            		m.SendMessage( "That would exceed your maximum amount of followers." );
            		return;
            	}
            	
            	post.Owner = m;
            	post.DoRelease( bc );
            }
        }
        
        public static string FixedDirection( string direction )
        {
        	if( direction == "up" )
        		return "northwest";
        	else if( direction == "down" )
        		return "southeast";
        	else if( direction == "left" )
        		return "southwest";
        	else if( direction == "right" )
        		return "northeast";
        	
        	return direction;
        }
        
        [Usage( "DrumsOfWar" )]
        [Description( "Play your drums loudly to warn everyone of your location." )]
        private static void DrumsOfWar_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m.HasFeatLevel( m.Feats.GetFeatLevel(FeatList.DrumsOfWar), 1 ) )
        	{
        		Drums drums = m.FindItemOnLayer( Layer.TwoHanded ) as Drums;
        		
        		if( drums != null && drums is Drums && BaseWeapon.CheckStam( m, m.Feats.GetFeatLevel(FeatList.DrumsOfWar) ) )
        		{
        			drums.ConsumeUse( m );
        			m.SendSound( drums.SuccessSound );
        			
	        		foreach( NetState ns in m.GetClientsInRange( 50 * m.Feats.GetFeatLevel(FeatList.DrumsOfWar) ) )
					{
						if ( ns != m.NetState && ns.Mobile != null && !ns.Mobile.Deleted )
						{
							ns.Mobile.SendMessage( "You hear loud drum sounds coming from the " + FixedDirection( ns.Mobile.GetDirectionTo( m ).ToString().ToLower() ) + "." );
							ns.Mobile.SendSound( drums.SuccessSound );
						}
					}
        		}
        		
        		else if( drums == null )
        			m.SendMessage( "You need to equip some drums before attempting this." );
        	}
        }
        
        [Usage( "ExpeditiousRetreat" )]
        [Description( "Play your instrument to inspire your allies to run faster." )]
        private static void ExpeditiousRetreat_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.CurrentCommand = SongList.ExpeditiousRetreat;
        	m.ExpeditiousRetreat();
        }
        
        [Usage( "InspireFortitude" )]
        [Description( "Play your instrument to inspire your allies to recover faster." )]
        private static void InspireFortitude_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.CurrentCommand = SongList.InspireFortitude;
        	m.InspireFortitude();
        }
        
        [Usage( "InspireHeroics" )]
        [Description( "Play your instrument to inspire your allies to fight more fiercely." )]
        private static void InspireHeroics_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.CurrentCommand = SongList.InspireHeroics;
        	m.InspireHeroics();
        }
        
        [Usage( "InspireResilience" )]
        [Description( "Play your instrument to inspire your allies to withstand anything." )]
        private static void InspireResilience_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.CurrentCommand = SongList.InspireResilience;
        	m.InspireResilience();
        }
        
        [Usage( "SongOfMockery" )]
        [Description( "Play your instrument to anger your enemies." )]
        private static void SongOfMockery_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.CurrentCommand = SongList.SongOfMockery;
        	m.SongOfMockery();
        }
        
        [Usage( "SongOfEnthrallment" )]
        [Description( "Play your instrument to soothe your enemies." )]
        private static void SongOfEnthrallment_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.CurrentCommand = SongList.SongOfEnthrallment;
        	m.SongOfEnthrallment();
        }
        
        [Usage( "Buypack" )]
        [Description( "Open a vendor's buypack." )]
        private static void Buypack_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	m.Target = new BuypackTarget();
        }
        
        private class BuypackTarget : Target
		{
			public BuypackTarget() : base( -1, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is BaseVendor )
				{
					BaseVendor m = (BaseVendor)targeted;

					Container box = m.BuyPack;

					if ( box != null )
						box.DisplayTo( from );
				}
			}
		}
        
        [Usage( "ToggleVendor" )]
        [Description( "Makes a vendor sell or not sell goods to the second race in the city." )]
        private static void ToggleVendor_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	m.Target = new ToggleVendorTarget();
        }
        
        private class ToggleVendorTarget : Target
		{
			public ToggleVendorTarget() : base( 15, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is PlayerVendor )
				{
					PlayerVendor m = (PlayerVendor)targeted;

					if( m.Owner == null || m.Owner != from )
						from.SendMessage( "That is not your employee." );
					
					else if( m.SellsToSecondRace )
					{
						m.SellsToSecondRace = false;
						from.SendMessage( "Your employee will not sell goods to your allied race anymore." );
					}
					
					else
					{
						m.SellsToSecondRace = true;
						from.SendMessage( "Your employee will be selling goods to your allied race from now on." );
					}
				}
			}
		}
        
        [Usage( "Visit" )]
        [Description( "Visits every player online for [arg] seconds. If no argument is given, default is 30 seconds." )]
        private static void Visit_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m.VisitPending )
        	{
        		m.SendMessage( "Please wait while the system cancels your last visit." );
        		return;
        	}
        	
        	if( m.Visiting )
        	{
        		m.Visiting = false;
        		m.SendMessage( "Visit cancelled." );
        		m.VisitPending = true;
        		return;
        	}

        	try{ m.VisitDuration = Convert.ToInt32( e.ArgString.Trim() ); }
        	catch{ m.SendMessage( "Argument could not be parsed to an integer. Using default value of 30 seconds." ); m.VisitDuration = 30; }
        	
        	if( m.VisitDuration < 1 )
        	{
        		m.VisitDuration = 1;
        		m.SendMessage( "Argument lower than 1. Using 1 instead." );
        	}
        	
        	m.Visiting = true;
        	m.Visited.Clear();
        	m.Visit();
        }
        
        [Usage( "CraftContainer" )]
        [Description( "Sets up the container to which your crafted items will go." )]
        private static void CraftContainer_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.SendMessage( "Target a container inside your backpack." );
        	m.Target = new CraftContainerTarget();
        }
        
        private class CraftContainerTarget : Target
        {
            public CraftContainerTarget()
                : base( 1, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null || m.Backpack == null || m.Backpack.Deleted )
            		return;
            	
            	if( obj is Container && ((Item)obj).IsChildOf(m.Backpack) )
            	{
            		((PlayerMobile)m).CraftContainer = (Container)obj;
            		m.SendMessage( "You will place your crafts in there for now on, as long as you keep the container in your backpack." );
            	}
            	
            	else
            		m.SendMessage( "Invalid target. Please target a container inside your backpack." );
            }
        }
        
        [Usage( "MassSmelt" )]
        [Description( "Smelts everything inside a container." )]
        private static void MassSmelt_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.SendMessage( "Target a container inside your backpack." );
        	m.Target = new MassSmeltTarget();
        }
        
        private class MassSmeltTarget : Target
        {
            public MassSmeltTarget()
                : base( 1, false, TargetFlags.None )
            {
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null || m.Backpack == null || m.Backpack.Deleted )
            		return;
            	
            	bool anvil;
            	bool forge;
            	
            	Engines.Craft.DefBlacksmithy.CheckAnvilAndForge( m, 2, out anvil, out forge );
            	
            	if( !forge )
            	{
            		m.SendMessage( "You need to be standing near a forge to do that." );
            		return;
            	}
            	
            	if( obj is Container && ((Item)obj).IsChildOf(m.Backpack) )
            	{
            		Container cont = obj as Container;
            		int wcount = 0;
            		int acount = 0;
                    int jcount = 0;
            		ArrayList list = new ArrayList();

            		foreach( Item item in cont.Items )
                    {
                        if (item is BaseArmor)
                        {
                            BaseArmor armor = item as BaseArmor;
                            if (armor.Quality != ArmorQuality.Extraordinary && armor.Quality != ArmorQuality.Masterwork)
                            {
                                list.Add(item);
                            }
                        }
                        else if (item is BaseWeapon)
                        {
                            BaseWeapon weapon = item as BaseWeapon;
                            if (weapon.Quality != WeaponQuality.Extraordinary && weapon.Quality != WeaponQuality.Masterwork)
                            {
                                list.Add(item);
                            }
                        }
                        else if (item is BaseJewel)
                        {
                            BaseJewel jewel = item as BaseJewel;
                            if (jewel.Quality != WeaponQuality.Extraordinary && jewel.Quality != WeaponQuality.Masterwork)
                            {
                                list.Add(item);
                            }
                        }
                    }
     				
            		for( int i = 0; i < list.Count; i++ )
            		{
            			Item targeted = list[i] as Item;

                        if (targeted is BaseArmor && Engines.Craft.Resmelt.PublicResmelt(m, (BaseArmor)targeted, ((BaseArmor)targeted).Resource, Engines.Craft.DefBlacksmithy.CraftSystem))
                            acount++;

                        else if (targeted is BaseWeapon && Engines.Craft.Resmelt.PublicResmelt(m, (BaseWeapon)targeted, ((BaseWeapon)targeted).Resource, Engines.Craft.DefBlacksmithy.CraftSystem))
                            wcount++;

                        else if (targeted is BaseJewel && Engines.Craft.Resmelt.PublicResmelt(m, (BaseJewel)targeted, ((BaseJewel)targeted).Resource, Engines.Craft.DefBlacksmithy.CraftSystem))
                            jcount++; 
            		}
            		
            		if( wcount > 0 )
            			m.SendMessage( "A total of " + wcount.ToString() + " weapon{0} {1} smelted.", wcount == 1 ? "" : "s", wcount == 1 ? "was" : "were" );
            		
            		if( acount > 0 )
            			m.SendMessage( "A total of " + acount.ToString() + " armour piece{0} {1} smelted.", acount == 1 ? "" : "s", acount == 1 ? "was" : "were" );

                    if (jcount > 0)
                        m.SendMessage("A total of " + acount.ToString() + " jewelery piece{0} {1} smelted.", jcount == 1 ? "" : "s", jcount == 1 ? "was" : "were");
            	
            		if( acount == 0 && wcount == 0 )
            			m.SendMessage( "No items were smelted." );
            	}
            	
            	else
            		m.SendMessage( "Invalid target. Please target a container inside your backpack." );
            }
        }
        
        [Usage( "Technique" )]
        [Description( "Changes the kind of damage you deal with unarmed attacks." )]
        private static void Technique_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( e.Length >= 1 && e.Arguments[0].ToLower() == "blunt" )
        	{
				m.Technique = null;
				m.TechniqueLevel = 0;
				m.SendMessage( "You go back to dealing 100% blunt damage with your fists." );
				if ( m is PlayerMobile )
				{
					((PlayerMobile)m).SendStatusIcons();
					((PlayerMobile)m).Send( new MobileStatus( ((PlayerMobile)m), ((PlayerMobile)m) ) );
				}
				return;
			}
        	
        	if ( e.Length >= 2 )
			{
				string type = e.Arguments[0].ToLower();
				string level = e.Arguments[1].ToLower();
				
				if( type != "slashing" && type != "piercing" )
				{
					m.SendMessage( "The first argument must be either \"Piercing\", \"Slashing\" or \"Blunt\"." );
					m.SendMessage( "Example: .Technique Piercing 2." );
					return;
				}
				
				int parsedLevel = 0;
				
				try{ parsedLevel = Convert.ToInt32( level ); }
        		catch{ m.SendMessage( "The second argument must be comprised of a number." ); m.SendMessage( "Example: .Technique Piercing 2." ); return; }

        		if( parsedLevel > m.Feats.GetFeatLevel(FeatList.Technique) || parsedLevel < 1 )
        		{
        			m.SendMessage( "You must use a number between 1 and your feat level in Technique for the second argument of the command, or use \".Technique Blunt\" to turn the ability off." );
        			return;
        		}
				
        		m.Technique = type;
        		int damage = 25 + (25 * parsedLevel);
        		m.TechniqueLevel = damage;
        		int blunt = 100 - damage;
        		string bluntmsg = "";
        		
        		if( blunt > 0 )
        			bluntmsg = "and " + blunt + "% blunt ";
        		
        		m.SendMessage( "From now on, you will be dealing " + damage + "% " + type + " " + bluntmsg + "damage with your fists." );
			}
        	
        	else
        	{
        		m.SendMessage( "Incorrect usage. Please add two arguments to the command, the first being the damage type and the second, the feat level you wish to use." );
        		m.SendMessage( "Example: .Technique Piercing 2." );
        	}
			
			if ( m is PlayerMobile )
			{
				((PlayerMobile)m).SendStatusIcons();
				((PlayerMobile)m).Send( new MobileStatus( ((PlayerMobile)m), ((PlayerMobile)m) ) );
			}
        }
        
        [Usage( "CreateAccount" )]
        [Description( "Quickly creates an account." )]
        private static void CreateAccount_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if ( e.Length >= 2 )
			{
				string un = e.Arguments[0];
				string pw = e.Arguments[1];

				Account dispAccount = null;
				string notice;

				if ( un == null || un.Length == 0 )
				{
					notice = "You must enter a username to add an account.";
				}
				else if ( pw == null || pw.Length == 0 )
				{
					notice = "You must enter a password to add an account.";
				}
				else
				{
					IAccount account = Accounts.GetAccount( un.ToLower() );

					if ( account != null )
					{
						notice = "There is already an account with that username.";
					}
					else
					{
						dispAccount = new Account( un, pw );
						notice = String.Format( "{0} : Account added.", un );
						CommandLogging.WriteLine( m, "{0} {1} adding new account: {2}", m.AccessLevel, CommandLogging.Format( m ), un );
					}
				}
				
				m.SendMessage( notice );
			}
        	
        	else
        	{
        		m.SendMessage( "Incorrect usage. Please add two arguments to the command, the first being the account's name and the second, its password." );
        		m.SendMessage( "Example: \".CreateAccount JohnSmith 123\"." );
        	}
        }
        
        [Usage( "AcceptCharacter" )]
        [Description( "Accepts a character name to a certain account." )]
        private static void AcceptCharacter_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if ( e.Length >= 2 )
			{
				string account = e.Arguments[0];
				string name = e.Arguments[1];

				string notice;

				if ( account == null || account.Length == 0 )
				{
					notice = "Please enter the account name as the first argument of the command.";
				}
				else if ( name == null || name.Length == 0 )
				{
					notice = "Please enter the character's name as the second argument of the command.";
				}
				else
				{
					IAccount acc = Accounts.GetAccount( account.ToLower() );

					if ( acc != null )
					{
						if( ((Account)acc).AcceptedNames.Contains(name) )
							notice = "That name has already being accepted for that account.";
						
						else
						{
							notice = String.Format( "{0} : Character accepted for account {1}.", name, account );
							((Account)acc).AcceptedNames.Add( name );
						}
					}
					else
					{
						notice = "Account not found.";
					}
				}
				
				m.SendMessage( notice );
			}
        	
        	else
        	{
        		m.SendMessage( "Incorrect usage. Please add two arguments to the command, the first being the account's name and the second, the accepted character's name." );
        		m.SendMessage( "Example: \".AcceptCharacter JohnSmith \"John Smith\"\"." );
        	}
        }
        
        [Usage( "ChangeName" )]
        [Description( "Changes a player's name." )]
        private static void ChangeName_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !(e.Mobile is PlayerMobile) || e.Length < 1 || e.Arguments[0].Trim().Length < 1 )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m.OldMapChar || m.Forging )
        	{
        		m.Name = e.ArgString;
        		m.SendMessage( "You have successfully changed your name." );
        	}
        	
        	else if( m.AccessLevel > AccessLevel.Player )
        	{
	        	m.SendMessage( "Choose the player you wish to rename." );
	        	m.Target = new ChangeNameTarget(e.Arguments[0]);
        	}
        }
        
        private class ChangeNameTarget : Target
        {
        	string newName = "";
        	
            public ChangeNameTarget( string name )
                : base( 30, false, TargetFlags.None )
            {
            	newName = name;
            }

            protected override void OnTarget( Mobile m, object obj )
            {
            	if( m == null || obj == null || obj == null || !(obj is PlayerMobile) )
            		return;
            	
            	((PlayerMobile)obj).Name = newName;
            }
        }
        
        [Usage( "IncBody" )]
        [Description( "Increases your BodyValue by 1." )]
        private static void IncBody_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !(e.Mobile is PlayerMobile) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.BodyValue++;
        }
        
        [Usage( "DecBody" )]
        [Description( "Decreases your BodyValue by 1." )]
        private static void DecBody_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !(e.Mobile is PlayerMobile) )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.BodyValue--;
        }
        
        [Usage( "SetBody" )]
        [Description( "Sets your BodyValue to a specified value." )]
        private static void SetBody_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !(e.Mobile is PlayerMobile) || e.Length < 1 || e.Arguments[0].Trim().Length < 1 )
        		return;
        	
        	int body = 0;
        	
        	if( e.Length > 0 && int.TryParse(e.Arguments[0], out body) )
        	{
	        	PlayerMobile m = e.Mobile as PlayerMobile;
	        	m.BodyValue = body;
        	}
        }
        
        [Usage( "PureDodge" )]
        [Description( "Toggles Pure Dodge on or off." )]
        private static void PureDodge_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m.PureDodge )
        	{
    			m.PureDodge = false;
    			m.SendMessage( "Pure Dodge Off." );

                m.Delta( MobileDelta.Armor );
                m.Delta( MobileDelta.Resistances );
                m.UpdateResistances();

                if( m.HasGump( typeof( CharInfoGump ) ) && m.m_CharInfoTimer == null )
                {
                    m.m_CharInfoTimer = new CharInfoGump.CharInfoTimer( m );
                    m.m_CharInfoTimer.Start();
                }
        	}
        	
        	else if( m.Feats.GetFeatLevel(FeatList.PureDodge) > 0 )
        	{
        		m.PureDodge = true;
        		m.SendMessage( "Pure Dodge On." );

                m.Delta( MobileDelta.Armor );
                m.Delta( MobileDelta.Resistances );
                m.UpdateResistances();

                if( m.HasGump( typeof( CharInfoGump ) ) && m.m_CharInfoTimer == null )
                {
                    m.m_CharInfoTimer = new CharInfoGump.CharInfoTimer( m );
                    m.m_CharInfoTimer.Start();
                }
        	}
        	
        	else
        		m.SendMessage( "You lack the appropriate feat." );
        }
        
        [Usage( "LogMsgs" )]
        [Description( "Toggles log in and log out messages on or off." )]
        private static void LogMsgs_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( m.LogMsgs )
        	{
    			m.LogMsgs = false;
    			m.SendMessage( "Log Messages Off." );
        	}
        	
        	else
        	{
        		m.LogMsgs = true;
        		m.SendMessage( "Log Messages On." );
        	}
        }
        
        [Usage( "Bandage" )]
        [Description( "Looks for a bandage in your backpack and uses it." )]
        private static void Bandage_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( e.Mobile == null || e.Mobile.Deleted || !(e.Mobile is PlayerMobile) || m.Backpack == null || m.Backpack.Deleted )
        		return;
        	
        	if( m.HealingTimer != null )
            {
            	m.SendMessage( "You are alreadying trying to heal someone." );
            	return;
            }
        	
        	foreach( Item item in m.Backpack.Items )
        	{
        		if( item is Bandage )
        		{
        			item.OnDoubleClick( m );
        			return;
        		}
        	}
        	
        	m.SendMessage( "No bandage was found in your backpack." );
        }
        
        [Usage( "BandageSelf" )]
        [Description( "Looks for a bandage in your backpack and uses it on yourself." )]
        private static void BandageSelf_OnCommand( CommandEventArgs e )
        {
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( e.Mobile == null || e.Mobile.Deleted || !(e.Mobile is PlayerMobile) || m.Backpack == null || m.Backpack.Deleted )
        		return;
        	

				if(m.Mounted)
				  {
                	m.SendMessage( "You cannot use bandages while mounted." );
                	return;
                }
			
        	if( m.HealingTimer != null )
            {
            	m.SendMessage( "You are alreadying trying to heal someone." );
            	return;
            }
        	
        	if( !m.Alive || m.Paralyzed )
        	{
            	m.SendMessage( "You cannot do that in your current state." );
            	return;
            }
        	
        	if( m.IsVampire )
        	{
            	m.SendMessage( "You cannot heal yourself with bandages." );
            	return;
            }
        	
        	foreach( Item item in m.Backpack.Items )
        	{
        		if( item is Bandage )
        		{
        			m.RevealingAction();
					m.ClearHands();
        			
					if( BandageContext.BeginHeal(m, m) != null )
						((Bandage)item).Consume();
					
					return;
        		}
        	}
        	
        	m.SendMessage( "No bandage was found in your backpack." );
        }
        
        [Usage( "Reforge" )]
        [Description( "Allows you to repick all your abilities." )]
        private static void Reforge_OnCommand( CommandEventArgs e )
        {        	
            if( e.Mobile == null || e.Mobile.Deleted || !(e.Mobile is PlayerMobile) )
        		return;
        	PlayerMobile m = e.Mobile as PlayerMobile;

            if (m.ReforgesLeft > 0)
            {
                if (m.ReforgesLeft == 1)
                    m.SendMessage("You have " + m.ReforgesLeft.ToString() + " reforge remaining. Type 'yes' to reforge now, or 'no' to cancel.");
                else
                    m.SendMessage("You have " + m.ReforgesLeft.ToString() + " reforges remaining. Type 'yes' to reforge now, or 'no' to cancel.");

                m.Prompt = new ReforgePrompt();
            }
            else
                m.SendMessage("You have no reforges.");
        }

        private class ReforgePrompt : Prompt
        {
            public ReforgePrompt()
            {
            }
            
            public override void OnResponse(Mobile from, string text)
            {
                if (text.ToLower().Contains("yes"))
                {
                    if (from == null || from.Deleted || !(from is PlayerMobile))
                        return;

                    PlayerMobile m = from as PlayerMobile;

                    if (m.Reforging)
                        m.SendMessage("You are already reforging.");

                    else if (m.ReforgesLeft < 1)
                        m.SendMessage("You have no reforges left.");

                    else
                    {
                        m.ReforgesLeft--;
                        m.Reforge();
                    }

                    base.OnResponse(from, text);
                }
                else if (text.ToLower().Contains("no"))
                    return;
                else
                {
                    from.SendMessage("Type 'yes' after entering .reforge to agree to reforging.");
                    return;
                }
            }
        }
        
        [Usage( "CraftingSpecialization" )]
        [Description( "Allows you to specialize in one field of crafting." )]
        private static void CraftingSpecialization_OnCommand( CommandEventArgs e )
        {
            PlayerMobile m = e.Mobile as PlayerMobile;
            
            if( m == null )
            	return;
 
        	if( m.Feats.GetFeatLevel(FeatList.CraftingSpecialization) > 0 )
        	{
        		if( !String.IsNullOrEmpty(m.CraftingSpecialization) )
        		{
        			m.SendMessage( "You have already chosen to specialize in " + m.CraftingSpecialization + "." );
        			return;
        		}
        		
        		string spec = e.ArgString.Trim().ToLower();
                	
                if( spec == "blacksmithing" )
                {
                	m.SendMessage( "You have chosen to specialize in Blacksmithing." );
                	m.CraftingSpecialization = "Blacksmithing";
                }
                	
            	else if( spec == "fletching" )
            	{
                	m.SendMessage( "You have chosen to specialize in Fletching." );
                	m.CraftingSpecialization = "Fletching";
                }
            	
            	else if( spec == "carpentry" )
            	{
                	m.SendMessage( "You have chosen to specialize in Carpentry." );
                	m.CraftingSpecialization = "Carpentry";
                }
            	
            	else if( spec == "tailoring" )
            	{
                	m.SendMessage( "You have chosen to specialize in Tailoring." );
                	m.CraftingSpecialization = "Tailoring";
                }
            	
            	else if( spec == "tinkering" )
            	{
                	m.SendMessage( "You have chosen to specialize in Tinkering." );
                	m.CraftingSpecialization = "Tinkering";
                }
            	
            	else
            	{
            		m.SendMessage( "Invalid argument. Possible options: Blacksmithing, Fletching, Carpentry, Tailoring and Tinkering." );
            		m.SendMessage( "Usage example: \".CraftingSpecialization Blacksmithing\"." );
            		return;
            	}
        	}
        	
        	else
        		m.SendMessage( "You lack the appropriate skill." );
        }
    }
}
