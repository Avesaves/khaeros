using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Text;
using Server;
using Server.Misc;
using Server.ContextMenus;
using Server.Network;
using Server.Mobiles;
using Server.BackgroundInfo;
using Server.Targeting;
using Server.Commands;

namespace Server.Misc
{
    [PropertyObject]
    public class Backgrounds
    {
        public override string ToString()
		{
			return "...";
        }
        
        public static void Initialize()
		{
			CommandSystem.Register( "SetBackground", AccessLevel.GameMaster, new CommandEventHandler( SetBackground_OnCommand ) );
			CommandSystem.Register( "GetBackground", AccessLevel.GameMaster, new CommandEventHandler( GetBackground_OnCommand ) );
			
			using ( StreamWriter op = new StreamWriter( Misc.StatusPage.WebFolder + BaseBackground.WikiFolder + "backgrounds.txt" ) )
			{
				op.WriteLine( "====== Backgrounds ======" );
				
				foreach( KeyValuePair<BackgroundList, BaseBackground> kvp in Catalogue )
				{
					op.WriteLine( @"[[guides:backgrounds:" + kvp.Value.Name.Replace(" ", "_").ToLower() + "|" + kvp.Value.Name + "]]" );
					op.WriteLine( @"\\ " );
				}
			}
		}
        
        [Usage( "GetBackground" )]
        [Description( "Gets the level of a given background." )]
        private static void GetBackground_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( e.Length < 1 || e.Arguments[0].Trim().Length < 1 )
        	{
        		m.SendMessage( "Please add one argument to the command." );
        		m.SendMessage( "Example: \".GetBackground Strong\"." );
        		return;
        	}
        	
        	try
        	{
        		BackgroundList background = (BackgroundList)Enum.Parse( typeof(BackgroundList), e.Arguments[0].Trim(), true );
        		m.Target = new BackgroundTarget( background, 0, false );
        	}
        	
        	catch
        	{
        		m.SendMessage( "Invalid background name." );
        	}
        }
        
        [Usage( "SetBackground" )]
        [Description( "Sets a given background to a given level." )]
        private static void SetBackground_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( e.Length < 2 || e.Arguments[0].Trim().Length < 1 || e.Arguments[1].Trim().Length < 1 )
        	{
        		m.SendMessage( "Please add two arguments to the command." );
        		m.SendMessage( "Example: \".SetBackground Strong 1\"." );
        		return;
        	}
        	
        	int level = 0;
        	
        	if( !int.TryParse(e.Arguments[1].Trim(), out level) )
        	{
        		m.SendMessage( "The second argument must be a valid number." );
        		return;
        	}
        	
        	try
        	{
        		BackgroundList background = (BackgroundList)Enum.Parse( typeof(BackgroundList), e.Arguments[0].Trim(), true );
        		m.Target = new BackgroundTarget( background, level, true );
        	}
        	
        	catch
        	{
        		m.SendMessage( "Invalid background name." );
        	}
        }
        
        private class BackgroundTarget : Target
		{
        	BackgroundList m_Background;
        	int m_Level;
        	bool m_Setting;
        	
			public BackgroundTarget( BackgroundList background, int level, bool setting ) : base( 30, false, TargetFlags.None )
			{
				m_Background = background;
				m_Level = level;
				m_Setting = setting;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if( from == null || targeted == null || !(from is PlayerMobile) || !(targeted is PlayerMobile) )
					return;
				
				PlayerMobile target = targeted as PlayerMobile;
				
				if( m_Setting )
				{
					target.Backgrounds.BackgroundDictionary[m_Background].Level = m_Level;
					from.SendMessage( "Property has been set." );
				}
				
				else
					from.SendMessage( m_Background.ToString() + " = " + target.Backgrounds.BackgroundDictionary[m_Background].Level.ToString() );
			}
        }
        
        
        //List of all background classes. Created just once when compiled.
        public static List<Type> Types = GetAllClasses( "Server.BackgroundInfo" );
        
        //Method to get all the background classes within our backgrounds namespace.
		public static List<Type> GetAllClasses( string nameSpace )
		{
			Assembly asm = Assembly.GetExecutingAssembly();
		    List<Type> list = new List<Type>();

		    foreach( Type type in asm.GetTypes() )
		    {
		    	if( type.Namespace == nameSpace && type.IsClass && type.BaseType == typeof(BaseBackground) && !type.IsAbstract )
		    		list.Add( type );
		    }

		    return list;
		}
        
		//Producing a new background list with all our backgrounds.
        public static Dictionary<BackgroundList, BaseBackground> NewBackgroundList()
        {
        	Dictionary<BackgroundList, BaseBackground> dictionary = new Dictionary<BackgroundList, BaseBackground>();
        	
        	foreach( Type type in Types )
        	{
        		BaseBackground background = (BaseBackground)Activator.CreateInstance( type );
        		
        		if( !dictionary.ContainsKey(background.ListName) )
        			dictionary.Add( background.ListName, background );
        		
        		else
        			Console.WriteLine( "Duplicated key " + background.ListName + " on " + background.Name + "." );
        	}
        	
        	return dictionary;
        }
        
        public static BaseBackground ConvertBackgroundListToBackgroundObject( BackgroundList list )
        {
        	if( Catalogue.ContainsKey(list) )
        		return Catalogue[list];
        	
        	return null;
        }
        
        //Static dictionary for ease of getting info.
        public static Dictionary<BackgroundList, BaseBackground> Catalogue = NewBackgroundList();
        
        public static BaseBackground[] AllBackgrounds = GetAllBackgrounds();
        
        public static BaseBackground[] GetAllBackgrounds()
        {
        	List<BaseBackground> list = new List<BaseBackground>();
        	
        	foreach( KeyValuePair<BackgroundList, BaseBackground> kvp in Catalogue )
        		list.Add( kvp.Value );
        		
        	return list.ToArray();
        }

        //The dictionary is populated when it is declared, to prevent null references.
        private Dictionary<BackgroundList, BaseBackground> m_BackgroundDictionary = NewBackgroundList();
        public Dictionary<BackgroundList, BaseBackground> BackgroundDictionary{ get{ return m_BackgroundDictionary; } set{ m_BackgroundDictionary = value; } }

        public Backgrounds()
		{
		}

		public Backgrounds( GenericReader reader )
		{
			int version = reader.ReadInt();

			if( version > 1 )
			{
				int count = reader.ReadInt();
        	
	        	for( int i = 0; i < count; i++ )
	        	{
	        		BackgroundList background = (BackgroundList)reader.ReadInt();
	        		int level = reader.ReadInt();
	        		BackgroundDictionary[background].Level = level;
	        	}
			}
			
			else
			{
				int test = 0;
	            test = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Strong].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Quick].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Smart].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Tough].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Fit].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.IronWilled].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.QuickHealer].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Resilient].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.FocusedMind].Level = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Lucky].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Attractive].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.GoodLooking].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Gorgeous].Level = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Weak].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Clumsy].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Feebleminded].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Frail].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Unenergetic].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.WeakWilled].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.SlowHealer].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.OutOfShape].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.UnfocusedMind].Level = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Unlucky].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Homely].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Ugly].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Hideous].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Mute].Level = reader.ReadInt();
	            BackgroundDictionary[BackgroundList.Deaf].Level = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
	            test = reader.ReadInt();
			}
		}

        public static void Serialize( GenericWriter writer, Backgrounds info )
		{
			writer.Write( (int) 2 ); // version
			writer.Write( (int) info.BackgroundDictionary.Count );
			
			foreach( KeyValuePair<BackgroundList, BaseBackground> kvp in info.BackgroundDictionary )
			{
				writer.Write( (int) kvp.Key );
				writer.Write( (int) kvp.Value.Level );
			}
		}
    }
}
