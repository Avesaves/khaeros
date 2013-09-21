using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;

namespace Server.Gumps
{
	public class RaceGump : Gump
	{
		public RaceGump( PlayerMobile from )
			: base( 0, 0 )
		{
			from.CloseGump( typeof( RaceGump ) );
			from.CloseGump( typeof( InitialStatsGump ) );
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			
			this.AddPage(0);
			this.AddBackground(54, 31, 400, 383, 9270);
			this.AddBackground(71, 192, 364, 202, 3500);
			this.AddImage(4, 10, 10440);
			this.AddImage(423, 10, 10441);
            this.AddImage(183, 50, 29);
            //this.AddImage(215, 80, 9000);
			this.AddLabel(186, 47, 2010, @"Welcome to Khaeros");
			this.AddLabel(126, 84, 1149, @"The Keepers of the Light");
			this.AddLabel(126, 121, 1149, @"The Free Peoples of Lurieth");
			this.AddLabel(126, 158, 1149, @"The Northern Protectorate");
/* 			this.AddLabel(317, 84, 1149, @"Unaffiliated"); */
			this.AddButton(404, 46, 1150, 1152, 0, GumpButtonType.Reply, 0);
			
			this.AddPage(1);
			this.AddHtml( 99, 219, 307, 147, @"Please choose your culture.", (bool)true, (bool)true);

            this.AddButton(90, 81, 9720, 9722, 1, GumpButtonType.Page, 2);
            this.AddButton(90, 118, 9720, 9722, 2, GumpButtonType.Page, 3);
            this.AddButton(90, 155, 9720, 9722, 3, GumpButtonType.Page, 4);
/*             this.AddButton(389, 81, 9720, 9722, 4, GumpButtonType.Page, 5); */

			
			this.AddPage(2);
			this.AddHtml( 99, 219, 307, 147, "<center>THE KEEPERS</center><br><br>Hailing from " +
			             "the Western wastes, little is truly known of these people. They " +
			             "spend their hours searching for lost knowledge and secrets, " +
			             "obsessively cataloging them and hiding them from others.  " +
			             "Although apparently without a city of their own, the Keepers " +
			             "are commonly seen tending the libraries of the North and South, " +
			             "dressed in the gold and black of their philosophy. Non-religious, " +
			             "they are known to preach moderation and temperance, with " +
			             "all truths involving a complex interplay between two opposing views. " +
			             "The Keepers are often associated with poison, stealth, and intrigue, " +
			             "although this tends to be more of a stereotype than anything real. " +
			             "These people are unenthusiastically accepted by both Southeners and" +
			             "Northerners, representing the closest thing to a neutral culture. " +
			             "Despite this, there are always dark rumours.... " + 
			             "<br><br><a href=\"http://www.khaeros.net/keepers.htm\">Further " +
			             "Information</a><br>", (bool)true, (bool)true);
			this.AddButton(371, 46, 1153, 1155, 7, GumpButtonType.Reply, 0);

            this.AddButton(90, 81, 9723, 9722, 1, GumpButtonType.Page, 2);
            this.AddButton(90, 118, 9720, 9722, 2, GumpButtonType.Page, 3);
            this.AddButton(90, 155, 9720, 9722, 3, GumpButtonType.Page, 4);
/*             this.AddButton(389, 81, 9720, 9722, 4, GumpButtonType.Page, 5); */

			
			this.AddPage(3);
			this.AddHtml( 99, 219, 307, 147, "<center>LURIETH</center><br><br>" +
			             "The Free city of Lurieth is home to many people, but  " +
			             "political maneuvering and a history of self-serving leaders, " +
			             "have marred the once-great Lurethian reputation. The people " +
			             "of the south are known to prize their individuality and freedom, " +
			             "often making disparaging remarks of the devoted peoples of other " +
			             "cultures. Some of history's greatest warriors, leaders, and " +
			             "artisans have hailed from the South, and this lineage is never " +
			             "forgotten to the people of Lurieth. To be one of these people " +
			             "means to see yourself as part of the most powerful culture " +
			             "in Khaeros - at least in terms of money and manpower.  " +
			             "Of course, some might say that the Lurethians lack in knowledge, " +
			             "or in values or faith... but not to their faces. Many who " +
			             "live here look at Khaeros, and dream of an Imperial world" +
			             "which may still return again..... " +
			             "<br><br>" +
			             "<a href=\"http://www.khaeros.net/south.htm\">Further Information</a><br>", (bool)true, (bool)true);
			this.AddButton(371, 46, 1153, 1155, 8, GumpButtonType.Reply, 0);

            this.AddButton(90, 81, 9720, 9722, 1, GumpButtonType.Page, 2);
            this.AddButton(90, 118, 9723, 9722, 2, GumpButtonType.Page, 3);
            this.AddButton(90, 155, 9720, 9722, 3, GumpButtonType.Page, 4);
/*             this.AddButton(389, 81, 9720, 9722, 4, GumpButtonType.Page, 5); */

			
			this.AddPage(4);
			this.AddHtml( 99, 219, 307, 147, "<center>THE NORTH</center><br><br>The people " +
			             "of the North are often seen by others to be zealots; while  " +
			             "the Westerners debate endlessly over trivial matters and " +
			             "the Southerners gain power, the Northerners only seek to live " +
			             "a pious life. Of course, if this piety leads to their religion " +
			             "spreading through Khaeros, or the subjugation of lesser peoples, " +
			             "these are simply the way the Mother and Father wish history to " +
			             "play out. Although not as cosmopolitan as the Southerners, they " +
			             "make up for it in arms. The Church of the Divine Union is dangerous " +
			             "to trifle with, even in times of peace. Their times of worship " +
			             "are only just more frequent than their times of military practice." +
			             "<br><br><a href=\"http://www.khaeros.net/north.htm\">Further Information</a><br>", (bool)true, (bool)true);
			this.AddButton(371, 46, 1153, 1155, 9, GumpButtonType.Reply, 0);

            this.AddButton(90, 81, 9720, 9722, 1, GumpButtonType.Page, 2);
            this.AddButton(90, 118, 9720, 9722, 2, GumpButtonType.Page, 3);
            this.AddButton(90, 155, 9723, 9722, 3, GumpButtonType.Page, 4);
/*             this.AddButton(389, 81, 9720, 9722, 4, GumpButtonType.Page, 5); */

			
/* 			this.AddPage(5);
			this.AddHtml( 99, 219, 307, 147, "<center>???? [locked]</center><br><br>" +
			             "Just by looking at you, it is hard to see where you come from. " +
			             "Perhaps, after the terrible events, your ancestors lived " +
			             "in caves, or perhaps you are descendant from bandits. " +
			             "Either way, it is unlikely that you have been raised in such " +
			             "a way as to bias you towards certain gods or ways of living, " +
			             "although this may change. Your past is a mystery. " +

			             "<br><br><a href=\"http://www.khaeros.net/khaeros.htm\">Further Information</a><br>", (bool)true, (bool)true); */
		//	this.AddButton(371, 46, 1153, 1155, 10, GumpButtonType.Reply, 0);

/*             this.AddButton(90, 81, 9720, 9722, 1, GumpButtonType.Page, 2);
            this.AddButton(90, 118, 9720, 9722, 2, GumpButtonType.Page, 3);
            this.AddButton(90, 155, 9720, 9722, 3, GumpButtonType.Page, 4);
            this.AddButton(389, 81, 9723, 9722, 4, GumpButtonType.Page, 5); */

			
		
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			PlayerMobile m = sender.Mobile as PlayerMobile;
			
			if ( m == null )
				return;

			switch ( info.ButtonID )
			{
				case 0: 
				{
		   			return;
				}
			
				case 7: 
				{
					m.Nation = Nation.Western;
					break;
				}
					
				case 8: 
				{
					m.Nation = Nation.Southern;
					break;
				}
				
				case 9: 
				{
					m.Nation = Nation.Northern;
					break;
				}
					
				case 10: 
				{
					m.Nation = Nation.Mhordul;
					break;
				}
					

				
			}
			
			if ( info.ButtonID != 0 )
			{
				m.SendGump( new InitialStatsGump(m) );
				m.SendMessage( 60, "You have chosen a culture." );
			}
			
			switch( m.Nation )
			{
				case Nation.Southern:
				{
					m.RPTitle = "of the South";
					m.Feats.SetFeatLevel( FeatList.SouthernLanguage, 3 );
					m.Height = 103;
					m.Weight = 99;
					
					if( m.Female )
						m.Weight = 89;
					
					break;
				}
					
				case Nation.Western:
				{
					m.RPTitle = "of the West";
					m.Feats.SetFeatLevel( FeatList.WesternLanguage, 3 );
					m.Height = 100;
					m.Weight = 95;
					
					if( m.Female )
						m.Weight = 88;
					
					break;
				}
					
				case Nation.Haluaroc:
				{
					m.RPTitle = "the Haluaroc";
					m.Feats.SetFeatLevel( FeatList.HaluarocLanguage, 3 );
					m.Height = 93;
					m.Weight = 88;
					
					if( m.Female )
						m.Weight = 76;
					
					break;
				}
					
				case Nation.Mhordul:
				{
					m.RPTitle = "the Stranger";
					m.Height = 103;
					m.Weight = 98;
					
					if( m.Female )
						m.Weight = 92;
					
					break;
				}
				
				case Nation.Tirebladd:
				{
					m.RPTitle = "the Tirebladd";
					m.Feats.SetFeatLevel( FeatList.TirebladdLanguage, 3 );
					m.Height = 106;
					m.Weight = 105;
					
					if( m.Female )
						m.Weight = 91;
					
					break;
				}
					
				case Nation.Northern:
				{
					m.RPTitle = "of the North";
					m.Feats.SetFeatLevel( FeatList.NorthernLanguage, 3 );
                    m.Height = 106;
                    m.Weight = 93;
					
					if( m.Female )
						m.Weight = 81;
					
					break;
				}
			}
			
			m.Hue = BaseKhaerosMobile.AssignRacialHue( m.Nation );
			m.HairItemID = BaseKhaerosMobile.AssignRacialHair( m.Nation, m.Female );
			int hairhue = BaseKhaerosMobile.AssignRacialHairHue( m.Nation );
			m.HairHue = hairhue;
			
			if( !m.Female )
			{
				m.FacialHairItemID = BaseKhaerosMobile.AssignRacialFacialHair( m.Nation );
				m.FacialHairHue = hairhue;
			}
		}
	}
}
