using System;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Server.XmlSerialization
{
	public class Engine
	{
		public static void Serialize( object obj, string path )
		{
			try
			{
				XmlSerializer serializer = new XmlSerializer( obj.GetType() );
				Stream stream = new FileStream( path, FileMode.Create, FileAccess.Write, FileShare.None );
				serializer.Serialize( stream, obj );
				stream.Close();
			}
			
			catch( Exception e )
			{
				Console.WriteLine( e.Message );
			}
		}
		
		public static object Deserialize( Type type, string path )
		{
			object obj = null;
			
			try
			{
				XmlSerializer serializer = new XmlSerializer( type );
				FileStream stream = new FileStream( path, FileMode.Open );
				XmlReader reader = new XmlTextReader( stream );
				obj = serializer.Deserialize( reader );
				stream.Close();
			}
			
			catch( Exception e )
			{
				Console.WriteLine( e.Message );
			}
			
			return obj;
		}
	}
}
