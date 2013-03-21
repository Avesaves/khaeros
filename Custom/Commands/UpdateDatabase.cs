using System;
using Server.Mobiles;
using System.Collections.Generic;
using System.IO;
using FTPLib;
using System.Threading;

namespace Server.Commands
{
    public class UpdateDatabase
    {
        public static void Initialize()
        {
            CommandSystem.Register( "UpdateDataBase", AccessLevel.GameMaster, new CommandEventHandler( UpdateDataBase_OnCommand ) );
        }

        [Usage( "UpdateDataBase" )]
        [Description( "Loads database info." )]
        private static void UpdateDataBase_OnCommand( CommandEventArgs e )
        {
            if( e.Mobile == null || !( e.Mobile is PlayerMobile ) || e.Mobile.Deleted )
                return;

            PlayerMobile m = e.Mobile as PlayerMobile;

            if (m != null)
                WikiDataBase.LoadCreatureFiles(m);
        }
    }
	
	public class WikiDataBase : object
	{
        public static Thread trd = null;
        public static Mobile mob = null;
		public static string WikiFolder = Server.Misc.StatusPage.LiveServer == true ? @"\Wiki\data\pages\database\creatures\" : @"\creatures\";
		public static string FilePath = Server.Misc.StatusPage.WebFolder + WikiFolder;
		
		private static Dictionary<string, Dictionary<string, string>> m_CreatureEntries = new Dictionary<string, Dictionary<string, string>>();
		public static Dictionary<string, Dictionary<string, string>> CreatureEntries{ get{ return m_CreatureEntries; } }
		
		public static void Initialize()
		{
            if( Misc.StatusPage.LiveServer )
                StartFTPThread( null );
		}

        public static void StartFTPThread( Mobile m )
        {
            mob = m;
            trd = new Thread( new ThreadStart( DownloadFilesFromFTP ) );
            trd.IsBackground = true;
            trd.Start();

            if( m != null )
                m.SendMessage( "Updating the database. Please wait." );
        }

        public static void DownloadFilesFromFTP()
        {
            if( Directory.Exists( FilePath ) )
                Directory.Delete( FilePath, true );

            Directory.CreateDirectory( FilePath );

            FTP ftplib = new FTP();

            try
            {
                Console.WriteLine( "Connecting to \"ftp.khaeros.net\"..." );
                ftplib.Connect( "ftp.khaeros.net", "khaeros", "mVJJQLvpR5" );
                Console.WriteLine( "Retrieving file log from the database..." );
                ftplib.ChangeDir( "public_html/data/pages/database/creatures/" );
            }

            catch( Exception ex )
            {
                Console.WriteLine( ex.Message );

                if( mob != null )
                {
                    mob.SendMessage( ex.Message );
                    mob = null;
                }

                trd = null;
                return;
            }

            try
            {
                int perc =  0;
                List<string> lines = new List<string>();
                List<string> files = new List<string>();

                Console.WriteLine( "Received the following log:" );

                foreach( string line in ftplib.List() )
                {
                    Console.WriteLine( line );
                    lines.Add( line );
                }

                for( int i = 0; i < lines.Count; i++ )
                {
                    if( !lines[i].Contains( ".txt" ) )
                    {
                        Console.WriteLine( "Skipping line " + i.ToString() + "." );
                        continue;
                    }

                    Console.WriteLine( "Parsing line " + i.ToString() + "." );
                    string[] strings = lines[i].Split( new string[] { " " }, StringSplitOptions.RemoveEmptyEntries );

                    for( int a = 0; a < strings.Length; a++ )
                    {
                        if( !String.IsNullOrEmpty( strings[a] ) && strings[a].Contains( ".txt" ) )
                            files.Add( strings[a] );
                    }
                }

                foreach( string file in files )
                {
                    Console.WriteLine( "Attempting to download file: \"" + file + "\"." );
                    ftplib.OpenDownload( file, FilePath + file, true );

                    while( ftplib.DoDownload() > 0 )
                    {
                        perc = (int)( ( ftplib.BytesTotal * 100 )  / ftplib.FileSize );
                        Console.Write( "\rDownloading: {0}/{1} {2}%",
                          ftplib.BytesTotal, ftplib.FileSize, perc );
                        Console.Out.Flush();
                    }

                    if( Misc.CrashGuard.Crashed )
                    {
                        trd = null;
                        return;
                    }

                    Console.WriteLine( "" );
                }
            }

            catch( Exception ex )
            {
                Console.WriteLine( "" );
                Console.WriteLine( ex.Message );

                if( mob != null )
                {
                    mob.SendMessage( ex.Message );
                    mob = null;
                }

                trd = null;
                return;
            }

            if( Misc.CrashGuard.Crashed )
            {
                trd = null;
                return;
            }

            LoadCreatureFiles( mob );

            if( mob != null )
            {
                mob.SendMessage( "The database was successfully updated." );
                mob = null;
            }

            Console.WriteLine( "The database was successfully updated." );
            
            trd = null;
        }
		
		//loading all creature files on the wiki
		public static void LoadCreatureFiles( Mobile mob )
		{
            try
            {
                //DownloadFilesFromFTP();
                string[] creatureFilePaths = Directory.GetFiles( @"C:\Mobs\" );

                for( int i = 0; i < creatureFilePaths.Length; i++ )
                    ReadCreatureFile( creatureFilePaths[i] );
            }

            catch( Exception e )
            {
                Console.WriteLine( e.Message );
                mob.SendMessage( "There was an error while updating the database. Make sure no one is editing the wiki and try again later." );
                trd = null;
            }
		}
		
		//generic method to read files
		public static string ReadFile( string address )
		{
			try 
	        {
	            using( StreamReader sr = new StreamReader(address) ) 
	            {
	            	string st = sr.ReadToEnd();
	            	return st;
	            }
			}
			
	        catch( Exception e ) 
	        {
	            Console.WriteLine( "The file could not be read:" );
	            Console.WriteLine( e.Message );
	        }
	        
	        return null;
		}
		
		public static bool ReadCreatureFile( string address )
		{
			//getting everything on the file
			string st = ReadFile( address );
			
			if( st == null )
				return false;
			
			//removing everything but the table
			string table = GetCreatureTable( st );
			
			if( table == null )
				return false;

			//getting the file name from the path
			string fileName = GetFileName( address );
			
			if( !String.IsNullOrEmpty(fileName) )
				AddCreatureEntry( table, fileName );
			
			return true;
		}
		
		//removes the path from the string and returns just the file name
		public static string GetFileName( string st )
		{
			int start = st.LastIndexOf( @"\" );
			int end = st.LastIndexOf( '.' );
			
			if( start > -1 && end > -1 && end > start )
			{
				string edited = st.Substring( start + 1, end - start - 1 );
				return edited;
			}
			
			return null;
		}
		
		//adds an entry with the creature's name and a list of all its properties and their values
		public static void AddCreatureEntry( string table, string name )
		{
			Dictionary<string, string> entries = new Dictionary<string, string>();
			
			//removing all wiki syntax and using it to split the table elements
			string[] lines = table.Split( new char[]{'|'}, StringSplitOptions.RemoveEmptyEntries );
			List<string> validEntries = new List<string>();
			
			//adding valid table members and removing empty ones
			for( int i = 0; i < lines.Length; i++ )
			{
				string line = lines[i].Trim();
				
				if( !String.IsNullOrEmpty(line) )
					validEntries.Add( line );
			}
			
			int index = 0;
			
			//pairing properties and values
			while( (index + 1) < validEntries.Count )
			{
				//adding/replacing the property/value list member
				if( entries.ContainsKey(validEntries[index]) )
					entries.Remove( validEntries[index] );
				
				entries.Add( validEntries[index], validEntries[index + 1] );
				index += 2;
			}
			
			//adding/replacing the entry
			if( CreatureEntries.ContainsKey(name) )
				CreatureEntries.Remove( name );
			
			CreatureEntries.Add( name, entries );
		}
		
		//returns a string with just the table from the wiki, ignoring everything else
		public static string GetCreatureTable( string st )
		{
			int start = st.IndexOf( '|' );
			int end = st.LastIndexOf( '|' );
			
			if( start > -1 && end > -1 && end > start )
			{
				string edited = st.Substring( start, 1 + end - start );
				return edited;
			}
			
			return null;
		}
	}
}
