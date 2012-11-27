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
			this.AddLabel(126, 84, 1149, @"Azhuran");
			this.AddLabel(126, 121, 1149, @"Khemetar");
			this.AddLabel(126, 158, 1149, @"Alyrian");
			this.AddLabel(317, 84, 1149, @"Mhordul");
			this.AddLabel(317, 121, 1149, @"Tyrean");
			this.AddLabel(317, 158, 1149, @"Vhalurian");
			this.AddButton(404, 46, 1150, 1152, 0, GumpButtonType.Reply, 0);
			
			this.AddPage(1);
			this.AddHtml( 99, 219, 307, 147, @"Please choose your race.", (bool)true, (bool)true);

            this.AddButton(90, 81, 9720, 9722, 1, GumpButtonType.Page, 2);
            this.AddButton(90, 118, 9720, 9722, 2, GumpButtonType.Page, 3);
            this.AddButton(90, 155, 9720, 9722, 3, GumpButtonType.Page, 4);
            this.AddButton(389, 81, 9720, 9722, 4, GumpButtonType.Page, 5);
            this.AddButton(389, 118, 9720, 9722, 5, GumpButtonType.Page, 6);
            this.AddButton(389, 155, 9720, 9722, 6, GumpButtonType.Page, 7);
			
			this.AddPage(2);
			this.AddHtml( 99, 219, 307, 147, "<center>AZHURAN</center><br><br>The jungles " +
			             "of Azhur are a place of muted danger. Despite the lush foliage " +
			             "and the exotic chatter of its creatures, it is hardly a place " +
			             "for idle travel. Within Azhur there are many dangers including " +
			             "disease, horribly venomous creatures, and fearsome predators. " +
			             "Sharing the jungle with these dangers are a very resourceful " +
			             "people, the Azhuran. The diminutive yet exceedingly dangerous " +
			             "Azhurans have long mastered the dangers of the jungles for their " +
			             "own use. Their warriors move with incredible speed and strike " +
			             "with a ferocity unparalleled by any man, save the Mhordul. The sun " +
			             "is everything to these dark people, and their brutal rituals in " +
			             "the sun god Xipotec�s name reflect their dedication. Only the " +
			             "dumb or desperate would care to trespass in the lands of these " +
			             "xenophobic people.<br><br>Strength: 135<br>Dexterity: 150<br>" +
			             "Intelligence: 140<br><br>Hitpoints: 125<br>Stamina: 130<br>" +
			             "Mana: 145<br><br><a href=\"http://www.khaeros.com/alyrian.htm\">Further " +
			             "Information</a><br>", (bool)true, (bool)true);
			this.AddButton(371, 46, 1153, 1155, 7, GumpButtonType.Reply, 0);

            this.AddButton(90, 81, 9723, 9722, 1, GumpButtonType.Page, 2);
            this.AddButton(90, 118, 9720, 9722, 2, GumpButtonType.Page, 3);
            this.AddButton(90, 155, 9720, 9722, 3, GumpButtonType.Page, 4);
            this.AddButton(389, 81, 9720, 9722, 4, GumpButtonType.Page, 5);
            this.AddButton(389, 118, 9720, 9722, 5, GumpButtonType.Page, 6);
            this.AddButton(389, 155, 9720, 9722, 6, GumpButtonType.Page, 7);
			
			this.AddPage(3);
			this.AddHtml( 99, 219, 307, 147, "<center>KHEMETAR</center><br><br>" +
			             "A land of extremes, the desert of Khemet " +
			             "is no place for the unprepared to travel. Fierce sandstorms, " +
			             "blistering heat, and freezing nights are not all one can expect " +
			             "on the dry sea of sand. On the banks of a prominent river sits " +
			             "one of the most ancient cities in the world. Inside, the " +
			             "Khemetar lead a structured, orderly life of hard work, deep " +
			             "scholarly pursuits, and rigorous martial training. Keepers of a " +
			             "great knowledge, the Khemetar are rumored to know more about " +
			             "the wide world than any other nation in existence. They guard " +
			             "this information greedily. Visitation in Khemet is certainly " +
			             "something that should be carefully handled, especially in " +
			             "spiritual precincts, as no man wishes to be face to face with " +
			             "a powerful mummy guardian of one of the city�s many and " +
			             "imposing clergy. Slavery is common practice there, although its " +
			             "ranks are primarily filled with native Khemetar of low status " +
			             "or high offense. While many would argue the faults of defined " +
			             "social classes, the Khemetar cite it as integral to the " +
			             "stability and strength their city enjoys.<br><br>" +
			             "Strength: 130<br>Dexterity: 145<br>Intelligence: 150<br>" +
			             "<br>Hitpoints: 135<br>Stamina: 125<br>Mana: 140<br><br>" +
			             "<a href=\"http://www.khaeros.com/azhuran.htm\">Further Information</a><br>", (bool)true, (bool)true);
			this.AddButton(371, 46, 1153, 1155, 8, GumpButtonType.Reply, 0);

            this.AddButton(90, 81, 9720, 9722, 1, GumpButtonType.Page, 2);
            this.AddButton(90, 118, 9723, 9722, 2, GumpButtonType.Page, 3);
            this.AddButton(90, 155, 9720, 9722, 3, GumpButtonType.Page, 4);
            this.AddButton(389, 81, 9720, 9722, 4, GumpButtonType.Page, 5);
            this.AddButton(389, 118, 9720, 9722, 5, GumpButtonType.Page, 6);
            this.AddButton(389, 155, 9720, 9722, 6, GumpButtonType.Page, 7);
			
			this.AddPage(4);
			this.AddHtml( 99, 219, 307, 147, "<center>ALYRIAN</center><br><br>The people " +
			             "from the forests of Alyria are a tall, fair race of spiritual " +
			             "naturalists and graceful warriors. Alyrians, while being nearly " +
			             "lawless, are an orderly people dedicated to the good of the " +
			             "community and the lush paradise in which their city is nestled. " +
			             "Lead by the oral teachings and traditions of their druidic " +
			             "spiritual leaders, these people are uniformly devout. War is not " +
			             "a common event, but one they are quite capable of handling. " +
			             "Elegance is often a word attributed to the Alyrian people, as " +
			             "even their warriors fight with fluid, graceful movements. " +
			             "Likewise, the beauty and aesthetic detail of their crafters� " +
			             "works, especially wooden, are renowned all over the world.<br>" +
			             "<br>Strength: 125<br>Dexterity: 140<br>Intelligence: 145<br>" +
			             "<br>Hitpoints: 130<br>Stamina: 135<br>Mana: 150<br><br>" +
			             "<a href=\"http://www.khaeros.com/khemetar.htm\">Further Information</a><br>", (bool)true, (bool)true);
			this.AddButton(371, 46, 1153, 1155, 9, GumpButtonType.Reply, 0);

            this.AddButton(90, 81, 9720, 9722, 1, GumpButtonType.Page, 2);
            this.AddButton(90, 118, 9720, 9722, 2, GumpButtonType.Page, 3);
            this.AddButton(90, 155, 9723, 9722, 3, GumpButtonType.Page, 4);
            this.AddButton(389, 81, 9720, 9722, 4, GumpButtonType.Page, 5);
            this.AddButton(389, 118, 9720, 9722, 5, GumpButtonType.Page, 6);
            this.AddButton(389, 155, 9720, 9722, 6, GumpButtonType.Page, 7);
			
			this.AddPage(5);
			this.AddHtml( 99, 219, 307, 147, "<center>MHORDUL</center><br><br>" +
			             "K�dath-Mal is a cave complex whose location " +
			             "is unknown and its existence only whispered. This is an " +
			             "inconsequential detail, as the people it supposedly houses are " +
			             "far more important. With no territorial boundaries or respect " +
			             "for any that exist, the Mhordul are easily the most dangerous " +
			             "people in the world on a day to day basis. These cavedwellers make up " +
			             "a highly structured tribal society entirely centered around " +
			             "survival and strength as predators in the world. As a result, " +
			             "hunting is the encounter in which many unfortunates in the world " +
			             "get their first glimpse of these people. Little is known of " +
			             "their culture, while much is known of their prowess both in " +
			             "pitched battles and stealthily efficient ambushes. Despite their " +
			             "ferocity, a strange code of honor can be observed in their " +
			             "actions. A Mhordul charging into battle atop their large wolves " +
			             "is enough to put fear in the heart of even a Vhalurian knight." +
			             "<br><br>Strength: 150<br>Dexterity: 135<br>Intelligence: 125<br>" +
			             "<br>Hitpoints: 145<br>Stamina: 140<br>Mana: 130<br><br>" +
			             "<a href=\"http://www.khaeros.com/mhordul.htm\">Further Information</a><br>", (bool)true, (bool)true);
			this.AddButton(371, 46, 1153, 1155, 10, GumpButtonType.Reply, 0);

            this.AddButton(90, 81, 9720, 9722, 1, GumpButtonType.Page, 2);
            this.AddButton(90, 118, 9720, 9722, 2, GumpButtonType.Page, 3);
            this.AddButton(90, 155, 9720, 9722, 3, GumpButtonType.Page, 4);
            this.AddButton(389, 81, 9723, 9722, 4, GumpButtonType.Page, 5);
            this.AddButton(389, 118, 9720, 9722, 5, GumpButtonType.Page, 6);
            this.AddButton(389, 155, 9720, 9722, 6, GumpButtonType.Page, 7);
			
			this.AddPage(6);
			this.AddHtml( 99, 219, 307, 147, "<center>TYREAN</center><br><br>" +
			             "Far in the north lies an imposing mountain range and vast " +
			             "frozen tundra. A cold place nearly devoid of any real " +
			             "discernable life, one would expect the land of Tyris to be a " +
			             "frozen cousin of Khemet, incapable of supporting life. Here, " +
			             "however, lies home to some of the most fierce creatures in the " +
			             "land. Living beside them are the Tyrean people. A large, rugged " +
			             "people, Tyreans are among some of the largest people in the " +
			             "land. With an affinity for mead, most the world view them as " +
			             "brutish drunkards and nothing more. This is far from the truth. " +
			             "Tyreans possess some of the most stalwart warriors around. " +
			             "In addition, they boast the most supreme metalwork in the world. " +
			             "Few people are as well equipped to handle the steep slopes of " +
			             "their mountainous land or the harsh permafrost of the tundra. " +
			             "It would take a fool to try and conquer this world of " +
			             "harsh climate and ferocious warriors, yet all stand from afar, " +
			             "covetous of Tyris� steely might." +
			             "<br><br>Strength: 140<br>Dexterity: 125<br>Intelligence: 130<br>" +
			             "<br>Hitpoints: 150<br>Stamina: 145<br>Mana: 135<br><br>" +
			             "<a href=\"http://www.khaeros.com/tyrean.htm\">Further Information</a><br>", (bool)true, (bool)true);
			this.AddButton(371, 46, 1153, 1155, 11, GumpButtonType.Reply, 0);

            this.AddButton(90, 81, 9720, 9722, 1, GumpButtonType.Page, 2);
            this.AddButton(90, 118, 9720, 9722, 2, GumpButtonType.Page, 3);
            this.AddButton(90, 155, 9720, 9722, 3, GumpButtonType.Page, 4);
            this.AddButton(389, 81, 9720, 9722, 4, GumpButtonType.Page, 5);
            this.AddButton(389, 118, 9723, 9722, 5, GumpButtonType.Page, 6);
            this.AddButton(389, 155, 9720, 9722, 6, GumpButtonType.Page, 7);
			
			this.AddPage(7);
			this.AddHtml( 99, 219, 307, 147, "<center>VHALURIAN</center><br><br>" +
			             "The plains of Vhaluran are an ordinary, mild place dotted " +
			             "with a few quaint forests. The most remarkable aspects of this " +
			             "area central to the world are not natural. Scattered across " +
			             "the plains are imposing fortresses surrounding one central " +
			             "city. Living and working in this city are the Vhalurian people. " +
			             "A society of budding academics and pious folk with a strong sense " +
			             "of civil loyalty, Vhalurians live their lives around the throne " +
			             "of their King. Despite this deep feeling of loyalty, Vhalurians " +
			             "are quite individualistic. Vhaluran has learned to make good " +
			             "use of the local terrain with an army made up of some of the " +
			             "best cavalry in the world, as well as legions of heavy infantry " +
			             "suited to open field phalanx warfare. Religion and morality " +
			             "play a prominent role in this surprisingly open city, where " +
			             "the features of several peoples of the world may be seen " +
			             "combined in a single man. A relatively young nation with a " +
			             "tumultuous, enterprising history, Vhaluran is a shadow of its " +
			             "former power due to its aforementioned past." +
			             "<br><br>Strength: 145<br>Dexterity: 130<br>Intelligence: 135<br>" +
			             "<br>Hitpoints: 140<br>Stamina: 150<br>Mana: 125<br><br>" +
			             "<a href=\"http://www.khaeros.com/vhalurian.htm\">Further Information</a><br>", (bool)true, (bool)true);
			this.AddButton(371, 46, 1153, 1155, 12, GumpButtonType.Reply, 0);

            this.AddButton(90, 81, 9720, 9722, 1, GumpButtonType.Page, 2);
            this.AddButton(90, 118, 9720, 9722, 2, GumpButtonType.Page, 3);
            this.AddButton(90, 155, 9720, 9722, 3, GumpButtonType.Page, 4);
            this.AddButton(389, 81, 9720, 9722, 4, GumpButtonType.Page, 5);
            this.AddButton(389, 118, 9720, 9722, 5, GumpButtonType.Page, 6);
            this.AddButton(389, 155, 9723, 9722, 6, GumpButtonType.Page, 7);
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
					m.Nation = Nation.Azhuran;
					break;
				}
					
				case 8: 
				{
					m.Nation = Nation.Khemetar;
					break;
				}
				
				case 9: 
				{
					m.Nation = Nation.Alyrian;
					break;
				}
					
				case 10: 
				{
					m.Nation = Nation.Mhordul;
					break;
				}
					
				case 11: 
				{
					m.Nation = Nation.Tyrean;
					break;
				}
					
				case 12: 
				{
					m.Nation = Nation.Vhalurian;
					break;
				}
			}
			
			if ( info.ButtonID != 0 )
			{
				m.SendGump( new InitialStatsGump(m) );
				m.SendMessage( 60, "You have chosen to be " + m.Nation + "." );
			}
			
			switch( m.Nation )
			{
				case Nation.Alyrian:
				{
					m.RPTitle = "the Alyrian";
					m.Feats.SetFeatLevel( FeatList.AlyrianLanguage, 3 );
					m.Height = 103;
					m.Weight = 99;
					
					if( m.Female )
						m.Weight = 86;
					
					break;
				}
					
				case Nation.Azhuran:
				{
					m.RPTitle = "the Azhuran";
					m.Feats.SetFeatLevel( FeatList.AzhuranLanguage, 3 );
					m.Height = 90;
					m.Weight = 82;
					
					if( m.Female )
						m.Weight = 72;
					
					break;
				}
					
				case Nation.Khemetar:
				{
					m.RPTitle = "the Khemetar";
					m.Feats.SetFeatLevel( FeatList.KhemetarLanguage, 3 );
					m.Height = 93;
					m.Weight = 88;
					
					if( m.Female )
						m.Weight = 76;
					
					break;
				}
					
				case Nation.Mhordul:
				{
					m.RPTitle = "the Mhordul";
					m.Feats.SetFeatLevel( FeatList.MhordulLanguage, 3 );
					m.Height = 109;
					m.Weight = 111;
					
					if( m.Female )
						m.Weight = 96;
					
					break;
				}
				
				case Nation.Tyrean:
				{
					m.RPTitle = "the Tyrean";
					m.Feats.SetFeatLevel( FeatList.TyreanLanguage, 3 );
					m.Height = 106;
					m.Weight = 105;
					
					if( m.Female )
						m.Weight = 91;
					
					break;
				}
					
				case Nation.Vhalurian:
				{
					m.RPTitle = "the Vhalurian";
					m.Feats.SetFeatLevel( FeatList.VhalurianLanguage, 3 );
                    m.Height = 100;
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
