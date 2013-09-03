using System;
using Server;
using System.IO;
using System.Text;
using Server.Network;
using Server.Mobiles;
using Server.Misc;
using Server.Gumps;
using Server.Engines.XmlSpawner2;
using System.Collections;
using System.Collections.Generic;

namespace Server.BackgroundInfo
{	
	public abstract class BaseBackground : object
	{
		public static string WikiFolder = Server.Misc.StatusPage.LiveServer == true ? @"\Wiki\data\pages\backgrounds\" : @"\Backgrounds\";
		
		private int m_Level;
		public int Level{ get{ return m_Level; } set{ m_Level = value; } }
		
		public abstract int Cost{ get; }
		public abstract string Name{ get; }
		public abstract BackgroundList ListName{ get; }
		public abstract string Description{ get; }
		public abstract string FullDescription{ get; }
		public static bool OldPages = false;
		
		public virtual bool MeetsOurRequirements( PlayerMobile m )
		{
            XmlBackground.CleanUp(m, ListName);
			return MeetsOurRequirements( m, false );
		}
		
		public virtual bool MeetsOurRequirements( PlayerMobile m, bool message )
		{
            XmlBackground.CleanUp(m, ListName);
			return true;
		}
		
		public bool TestBackgroundForPurchase( PlayerMobile m, BackgroundList opposite )
		{
            XmlBackground.CleanUp(m, opposite);
			return TestBackgroundForPurchase( m, opposite, false );
		}
		
		public bool TestBackgroundForPurchase( PlayerMobile m, BackgroundList opposite, bool message )
		{
            XmlBackground.CleanUp(m, opposite);
			if( m.Backgrounds.BackgroundDictionary[opposite].Level > 0 )
			{
				if( message )
					m.SendMessage( "You already have the " + Backgrounds.Catalogue[opposite].Name + " background. " +
					              "Remove it first if you wish to acquire the " + Name + " background." );
				
				return false;
			}
			
			return true;
		}
		
		public bool HasAnotherLookBackground( PlayerMobile m )
		{
			return HasAnotherLookBackground( m, false );
		}
		
		public bool HasAnotherLookBackground( PlayerMobile m, bool message )
		{
            XmlBackground.CleanUp(m, ListName);
            
            bool warned = false;
			
			if( ListName != BackgroundList.Attractive )
				warned = !TestBackgroundForPurchase( m, BackgroundList.Attractive, message );
			
			if( !warned && ListName != BackgroundList.GoodLooking )
				warned = !TestBackgroundForPurchase( m, BackgroundList.GoodLooking, message );
			
			if( !warned && ListName != BackgroundList.Gorgeous )
				warned = !TestBackgroundForPurchase( m, BackgroundList.Gorgeous, message );
			
			if( !warned && ListName != BackgroundList.Homely )
				warned = !TestBackgroundForPurchase( m, BackgroundList.Homely, message );
			
			if( !warned &&  ListName != BackgroundList.Ugly )
				warned = !TestBackgroundForPurchase( m, BackgroundList.Ugly, message );
			
			if( !warned && ListName != BackgroundList.Hideous )
				warned = !TestBackgroundForPurchase( m, BackgroundList.Hideous, message );
			
			return warned;
		}
		
		public bool HasThisBackground( PlayerMobile m )
		{
			return (m.Backgrounds.BackgroundDictionary[ListName].Level > 0);
		}
		
		public void AttemptPurchase( PlayerMobile m )
		{
            XmlBackground.CleanUp(m, ListName);
            
            if( !MeetsOurRequirements(m, true) )
				return;
			
			if( !HasThisBackground(m) && CanAcquireThisBackground(m) )
			{
				
				if( (175000 + m.ExtraCPRewards + m.CPCapOffset) < (m.CPSpent + Cost) )
				{
					m.SendMessage( "Adding this background would lower your CP cap beyond the amount of CP you have already spent." );
					return;
				}
				
				m.CPCapOffset -= Cost;
				m.FeatSlots += Cost;
				Level = 1;
				m.SendMessage( "You have acquired the " + Name + " background." );
				OnAddedTo( m );
				
				if( m.HasGump( typeof(CharInfoGump) ) )
					m.SendGump( new CharInfoGump(m) );
			}

            else if( HasThisBackground(m) && CanRemoveThisBackground(m) )
			{
				if( (175000 + m.ExtraCPRewards + m.CPCapOffset + Cost) < (m.CPSpent) )
				{
					m.SendMessage( "Removing this background would lower your CP cap beyond the amount of CP you have already spent." );
					return;
				}
				
				m.CPCapOffset += Cost;
				m.FeatSlots -= Cost;
				Level = 0;
				m.SendMessage( "You have removed the " + Name + " background." );
				OnRemovedFrom( m );
				
				if( m.HasGump( typeof(CharInfoGump) ) )
					m.SendGump( new CharInfoGump(m) );
			}
		}

        public virtual bool CanAcquireThisBackground( PlayerMobile m )
        {
            XmlBackground.CleanUp(m, ListName);
            return true;
        }

        public virtual bool CanRemoveThisBackground( PlayerMobile m )
        {
            XmlBackground.CleanUp(m, ListName);
            return true;
        }
		
		public virtual void OnAddedTo( PlayerMobile m )
		{
		}
		
		public virtual void OnRemovedFrom( PlayerMobile m )
		{
		}
		
		public static string GetFullDescription( BaseBackground background )
		{
			string description = "<b>" + background.Name + "</b><br><br>" +
				"<i>Description:</i> " +
				background.Description + "<br><br>" +
				"<i>CP Cap Offset:</i> " + background.Cost.ToString();
			
			return description;
		}
		
		public static string GetWebpageDescription( BaseBackground background )
		{		
			string description = "<p id=\"featName\">" + background.Name + "</p>" +
				"<span id=\"boldTopic\">Description:</span> " +
				background.Description + "<br><br>" +
				"<span id=\"boldTopic\">CP Cap Offset:</span> " + background.Cost.ToString();
			
			return description;
		}
		
		public static void PublishWikiPage( BaseBackground background, StreamWriter op )
		{
			FeatInfo.BaseFeat.HandleString( "====== " + background.Name + " ======", op );
			FeatInfo.BaseFeat.HandleString( "**Description:** ", op );
			FeatInfo.BaseFeat.HandleString( background.Description + "<br><br>", op );
			FeatInfo.BaseFeat.HandleString( "**CP Cap Offset:** " + background.Cost.ToString(), op );
		}

		public static void WriteWebpage( BaseBackground background )
		{
			string fileName = OldPages == true ? background.Name.Replace(" ", "") : background.Name.Replace(" ", "_").ToLower();
			string extension = OldPages == true ? ".htm" : ".txt";
			
			using ( StreamWriter op = new StreamWriter( Misc.StatusPage.WebFolder + WikiFolder + fileName + extension ) )
			{
				if( OldPages )
				{
					op.WriteLine( "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" );
					op.WriteLine( "<html xmlns=\"http://www.w3.org/1999/xhtml\">" );
					op.WriteLine( "<head>" );
					op.WriteLine( "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />" );
					op.WriteLine( "<title>" + background.Name + "</title>" );
					op.WriteLine( "<link href=\"styles.css\" rel=\"stylesheet\" type=\"text/css\" />" );
					op.WriteLine( "<style type=\"text/css\"></style></head><body><div id=\"main\">" );
					op.WriteLine( GetWebpageDescription(background) );
					op.WriteLine( "</div></body></html>" );
				}
				
				else
					PublishWikiPage( background, op );
			}
		}
	}
}
