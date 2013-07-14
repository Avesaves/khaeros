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
using Server.FeatInfo;
using Server.Targeting;
using Server.Commands;

namespace Server.Misc
{
    [PropertyObject]
    public class Feats
    {
        public override string ToString()
		{
			return "...";
        }
        
        public static void Initialize()
		{
			CommandSystem.Register( "SetFeat", AccessLevel.GameMaster, new CommandEventHandler( SetFeat_OnCommand ) );
			CommandSystem.Register( "GetFeat", AccessLevel.GameMaster, new CommandEventHandler( GetFeat_OnCommand ) );
			CommandSystem.Register( "GetFeatDictionarySize", AccessLevel.GameMaster, new CommandEventHandler( GetFeatDictionarySize_OnCommand ) );
			
			using ( StreamWriter op = new StreamWriter( Misc.StatusPage.WebFolder + BaseFeat.WikiFolder + "skills.txt" ) )
			{
				op.WriteLine( "====== Skills ======" );
				
				foreach( BaseFeat feat in AllValidFeats )
				{
					op.WriteLine( @"[[guides:skills:" + feat.Name.Replace(" ", "_").ToLower() + "|" + feat.Name + "]]" );
					op.WriteLine( @"\\ " );
				}
			}
		}
        
        [Usage( "GetFeat" )]
        [Description( "Gets the level of a given feat." )]
        private static void GetFeat_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( e.Length < 1 || e.Arguments[0].Trim().Length < 1 )
        	{
        		m.SendMessage( "Please add one argument to the command." );
        		m.SendMessage( "Example: \".GetFeat ShieldBash\"." );
        		return;
        	}
        	
        	try
        	{
        		FeatList feat = (FeatList)Enum.Parse( typeof(FeatList), e.Arguments[0].Trim(), true );
        		m.Target = new FeatTarget( feat, 0, false );
        	}
        	
        	catch
        	{
        		m.SendMessage( "Invalid feat name." );
        	}
        }
        
        [Usage( "SetFeat" )]
        [Description( "Sets a given feat to a given level." )]
        private static void SetFeat_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	
        	if( e.Length < 2 || e.Arguments[0].Trim().Length < 1 || e.Arguments[1].Trim().Length < 1 )
        	{
        		m.SendMessage( "Please add two arguments to the command." );
        		m.SendMessage( "Example: \".SetFeat ShieldBash 3\"." );
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
        		FeatList feat = (FeatList)Enum.Parse( typeof(FeatList), e.Arguments[0].Trim(), true );
        		m.Target = new FeatTarget( feat, level, true );
        	}
        	
        	catch
        	{
        		m.SendMessage( "Invalid feat name." );
        	}
        }
        
        [Usage( "GetFeatDictionarySize" )]
        [Description( "Gets the amount of feats in a mobile's feat dictionary." )]
        private static void GetFeatDictionarySize_OnCommand( CommandEventArgs e )
        {
        	if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
        		return;
        	
        	PlayerMobile m = e.Mobile as PlayerMobile;
        	m.Target = new FeatDictionarySizeTarget();
        }
        
        private class FeatTarget : Target
		{
        	FeatList m_Feat;
        	int m_Level;
        	bool m_Setting;
        	
			public FeatTarget( FeatList feat, int level, bool setting ) : base( 30, false, TargetFlags.None )
			{
				m_Feat = feat;
				m_Level = level;
				m_Setting = setting;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if( from == null || targeted == null || !(from is PlayerMobile) || !(targeted is IKhaerosMobile) )
					return;
				
				IKhaerosMobile target = targeted as IKhaerosMobile;
				
				if( m_Setting )
				{
					target.Feats.SetFeatLevel( m_Feat, m_Level );
					from.SendMessage( "Property has been set." );
				}
				
				else
					from.SendMessage( m_Feat.ToString() + " = " + target.Feats.GetFeatLevel(m_Feat).ToString() );
			}
        }
        
        private class FeatDictionarySizeTarget : Target
		{
			public FeatDictionarySizeTarget() : base( 30, false, TargetFlags.None )
			{
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if( from == null || targeted == null || !(from is PlayerMobile) || !(targeted is IKhaerosMobile) )
					return;
				
				IKhaerosMobile target = targeted as IKhaerosMobile;
				from.SendMessage( "This mobile has " + target.Feats.FeatDictionary.Count.ToString() + " feats in their feat dictionary." );
			}
        }
        
        //Method to set a feat to a value. It will add the feat to the dictionary if it doesn't exist in it yet.
        public void SetFeatLevel( FeatList feat, int level )
        {
        	SetFeatLevel( feat, level, null );
        }
        
        public void SetFeatLevel( FeatList feat, int level, PlayerMobile owner )
        {
        	if( level < 1 )
        	{
        		if( FeatDictionary.ContainsKey(feat) )
        		{
        			FeatDictionary[feat].Level = level;
        			
        			if( owner != null )
        				FeatDictionary[feat].OnLevelChanged( owner );
        		}
        	}
        	
        	else
        	{
        		if( !FeatDictionary.ContainsKey(feat) )
        		{
        			Type type = Catalogue[feat].GetType();
        			BaseFeat featClass = (BaseFeat)Activator.CreateInstance( type );
        			FeatDictionary.Add( feat, featClass );
        		}
        		
        		FeatDictionary[feat].Level = level;
        		
        		if( owner != null )
        			FeatDictionary[feat].OnLevelChanged( owner );
        	}
        }
        
        //Method to get a feat value. It will return 0 if the feat doesn't exist in the dictionary.
        public int GetFeatLevel( FeatList feat )
        {
        	if( !FeatDictionary.ContainsKey(feat) )
        		return 0;
        	
        	return FeatDictionary[feat].Level;
        }

        //List of all feat classes. Created just once when compiled.
        public static List<Type> Types = GetAllClasses( "Server.FeatInfo" );
        
        //Method to get all the feat classes within our feats namespace.
		public static List<Type> GetAllClasses( string nameSpace )
		{
			Assembly asm = Assembly.GetExecutingAssembly();
		    List<Type> list = new List<Type>();

		    foreach( Type type in asm.GetTypes() )
		    {
		    	if( type.Namespace == nameSpace && type.IsClass && type.BaseType == typeof(BaseFeat) && !type.IsAbstract )
		    		list.Add( type );
		    }

		    return list;
		}
        
		//Producing a new feat list with all our feats.
        public static Dictionary<FeatList, BaseFeat> NewFeatList()
        {
        	Dictionary<FeatList, BaseFeat> dictionary = new Dictionary<FeatList, BaseFeat>();
        	
        	foreach( Type type in Types )
        	{
        		BaseFeat feat = (BaseFeat)Activator.CreateInstance( type );
        		
        		if( !dictionary.ContainsKey(feat.ListName) )
        			dictionary.Add( feat.ListName, feat );
        		
        		else
        			Console.WriteLine( "Duplicated key " + feat.ListName + " on " + feat.Name + "." );
        	}
        	
        	return dictionary;
        }
        
        public static BaseFeat ConvertFeatListToFeatObject( FeatList list )
        {
        	if( Catalogue.ContainsKey(list) )
        		return Catalogue[list];
        	
        	return null;
        }
        
        //Static dictionary for ease of getting info.
        public static Dictionary<FeatList, BaseFeat> Catalogue = NewFeatList();

        //The dictionary is populated when it is created, to prevent null references.
        private Dictionary<FeatList, BaseFeat> m_FeatDictionary = new Dictionary<FeatList, BaseFeat>();
        public Dictionary<FeatList, BaseFeat> FeatDictionary{ get{ return m_FeatDictionary; } set{ m_FeatDictionary = value; } }
        
        //Static list of feats that can populate the first menu of the feats gump.
        public static List<BaseFeat> FeatsWithoutRequirements = GetValidFeatsWithoutRequirements();
        
        public static List<BaseFeat> GetValidFeatsWithoutRequirements()
        {
        	List<BaseFeat> list = new List<BaseFeat>();
        	
        	foreach( KeyValuePair<FeatList, BaseFeat> kvp in Catalogue )
        	{
        		if( kvp.Value.Requires.Length == 0 && (kvp.Value.CostLevel > FeatCost.None || kvp.Value is VampireAbilities) )
        			list.Add( kvp.Value );
        	}
        	
        	return list;
        }
        
        //Static list of feats that can be acquired.
        public static List<BaseFeat> AllValidFeats = GetAllValidFeats();
        
        public static List<BaseFeat> GetAllValidFeats()
        {
        	List<BaseFeat> list = new List<BaseFeat>();
        	
        	foreach( KeyValuePair<FeatList, BaseFeat> kvp in Catalogue )
        	{
        		if( FeatsWithoutRequirements.Contains(kvp.Value) || kvp.Value.Requires.Length > 0 )
        			list.Add( kvp.Value );
        	}
        	
        	return list;
        }
        
        public Feats( bool creature )
        {
        	if( !creature )
        		FeatDictionary = NewFeatList();
        }

        public Feats() : this( false )
		{
		}

        public Feats( GenericReader reader ) : this( reader, false )
        {
        }
        
		public Feats( GenericReader reader, bool creature )
		{
			if( !creature )
        		FeatDictionary = NewFeatList();
			
			int version = reader.ReadInt();

        	int count = reader.ReadInt();
        	
        	for( int i = 0; i < count; i++ )
        	{
        		FeatList feat = (FeatList)reader.ReadInt();
        		int level = reader.ReadInt();
        		
        		if(creature && BaseBreedableCreature.GetAllowedPetFeats().Contains(feat))
        			SetFeatLevel( feat, level );
                else if (!creature || level > 1)
                    SetFeatLevel(feat, level);
        	}
		}

        public static void Serialize( GenericWriter writer, Feats info )
		{
			writer.Write( (int) 9 ); // version
			writer.Write( (int) info.FeatDictionary.Count );
			
			foreach( KeyValuePair<FeatList, BaseFeat> kvp in info.FeatDictionary )
			{
				writer.Write( (int) kvp.Key );
				writer.Write( (int) kvp.Value.Level );
			}
		}
    }
}
