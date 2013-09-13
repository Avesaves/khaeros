using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;

namespace Server.Gumps
{
	public class NPCCustomGump : Gump
	{
        public int y = 0;
        public int x = 0;
		public bool isbeardone = false;

        public static int mustache = 8257;
		public static int thinmustache = 12720;
		public static int goatee = 8256;
		public static int thingoatee = 12721;
		public static int vandyke = 8269;
		public static int thinvandyke = 12722;
		public static int shortbeard = 8255;
		public static int shortbeardmustache = 8267;
		public static int thinbeard = 12727;
		public static int thinbeardmustache = 12728;
		public static int longbeard = 8254;
		public static int longbeardmustache = 8268;
		public static int fullbeard = 12725;
		public static int fullbeardmustache = 12726;
		public static int braidedbeard = 12729;
		public static int extralongbeard = 12730;
		public static int verylongbeard = 12731;
		public static int ornatebeard = 12732;
		public static int twobraidsbeard = 12733;
		public static int midlongbeard = 12734;
		public static int ornatelongbeard = 12735;
		public static int ornateverylongbeard = 12736;
		public static int curlylongbeard = 12737;
		public static int nicegoatee = 12723;
        public static int wildbeard = 8464;
        public static int thinlongbeard = 8469;

        public static KeyValuePair<string, int> shorthair = new KeyValuePair<string, int>( "Short Hair", 8251 );
        public static KeyValuePair<string, int> longhair = new KeyValuePair<string, int>( "Long Hair", 8252 );
        public static KeyValuePair<string, int> ponytail = new KeyValuePair<string, int>( "Ponytail", 8253 );
        public static KeyValuePair<string, int> pigtails = new KeyValuePair<string, int>( "Pig Tails", 8265 );
        public static KeyValuePair<string, int> curlyshort = new KeyValuePair<string, int>( "Curly Short", 8465 );
        public static KeyValuePair<string, int> spikedhair = new KeyValuePair<string, int>( "Spiked Hair", 8505 );
        public static KeyValuePair<string, int> curlylong = new KeyValuePair<string, int>( "Curly Long", 12742 );
        public static KeyValuePair<string, int> thinlong = new KeyValuePair<string, int>( "Thin Long", 12744 );
        public static KeyValuePair<string, int> longponytail = new KeyValuePair<string, int>( "Long Ponytail", 12749 );
        public static KeyValuePair<string, int> lushcurly = new KeyValuePair<string, int>( "Lush Curly", 12751 );
        public static KeyValuePair<string, int> straightlong = new KeyValuePair<string, int>( "Straight Long", 12757 );

        public static KeyValuePair<string, int> verylong = new KeyValuePair<string, int>( "Very Long", 8466 );
        public static KeyValuePair<string, int> receeding = new KeyValuePair<string, int>( "Receeding", 8467 );
        public static KeyValuePair<string, int> wavyshort = new KeyValuePair<string, int>( "Wavy Short", 8471 );
        public static KeyValuePair<string, int> lushlong = new KeyValuePair<string, int>( "Lush Long", 8488 );
        public static KeyValuePair<string, int> delicatelong = new KeyValuePair<string, int>( "Delicate Long", 8504 );
        public static KeyValuePair<string, int> wildcurly = new KeyValuePair<string, int>( "Wild Curly", 12752 );

        public static KeyValuePair<string, int> mhordulhair = new KeyValuePair<string, int>( "Mhordul Hair", 8260 );
        public static KeyValuePair<string, int> pageboy = new KeyValuePair<string, int>( "Pageboy", 8261 );
        public static KeyValuePair<string, int> fancyshort = new KeyValuePair<string, int>( "Fancy Short", 8262 );
        public static KeyValuePair<string, int> thinshort = new KeyValuePair<string, int>( "Thin Short", 8264 );
        public static KeyValuePair<string, int> topknot = new KeyValuePair<string, int>( "Top Knot", 8266 );
        public static KeyValuePair<string, int> shouldershort = new KeyValuePair<string, int>( "Shoulder Short", 12738 );
        public static KeyValuePair<string, int> shortdreads = new KeyValuePair<string, int>( "Short Dreads", 12741 );
        public static KeyValuePair<string, int> longdreads = new KeyValuePair<string, int>( "Long Dreads", 12745 );
        public static KeyValuePair<string, int> longplaited = new KeyValuePair<string, int>( "Long Plaited", 12746 );
        public static KeyValuePair<string, int> longmessy = new KeyValuePair<string, int>( "Long Messy", 12747 );
        public static KeyValuePair<string, int> mediumdreads = new KeyValuePair<string, int>( "Medium Dreads", 12748 );
        public static KeyValuePair<string, int> longSouthern = new KeyValuePair<string, int>( "Long Southern", 12750 );

		public static int hairhue1;
		public static int hairhue2;
		public static int hairhue3;
		public static int hairhue4;
		public static int hairhue5;
		public static int hairhue6;
		public static int hairhue7;
		public static int hairhue8;
		public static int hairhue9;
		public static int hairhue10;

        public KeyValuePair<string, int> hair0 = new KeyValuePair<string, int>( "Bald", 0 );
        public KeyValuePair<string, int> hair1 = shorthair;
        public KeyValuePair<string, int> hair2 = longhair;
        public KeyValuePair<string, int> hair3 = ponytail;
        public KeyValuePair<string, int> hair4 = pigtails;
        public KeyValuePair<string, int> hair5 = curlyshort;
        public KeyValuePair<string, int> hair6 = spikedhair;
        public KeyValuePair<string, int> hair7 = curlylong;
        public KeyValuePair<string, int> hair8 = thinlong;
        public KeyValuePair<string, int> hair9 = longponytail;
        public KeyValuePair<string, int> hair10 = lushcurly;
        public KeyValuePair<string, int> hair11 = straightlong;
        public KeyValuePair<string, int> hair12 = new KeyValuePair<string, int>( null, 0 );
        public KeyValuePair<string, int> hair13 = new KeyValuePair<string, int>( null, 0 );
        public KeyValuePair<string, int> hair14 = new KeyValuePair<string, int>( null, 0 );
        public KeyValuePair<string, int> hair15 = new KeyValuePair<string, int>( null, 0 );
        public KeyValuePair<string, int> hair16 = new KeyValuePair<string, int>( null, 0 );
        public KeyValuePair<string, int> hair17 = new KeyValuePair<string, int>( null, 0 );
        public KeyValuePair<string, int> hair18 = new KeyValuePair<string, int>( null, 0 );
        public KeyValuePair<string, int> hair19 = new KeyValuePair<string, int>( null, 0 );
        public KeyValuePair<string, int> hair20 = new KeyValuePair<string, int>( null, 0 );
		
		public static int facialhair1;
		public static int facialhair2;
		public static int facialhair3;
		public static int facialhair4;
		public static int facialhair5;
		public static int facialhair6;
		public static int facialhair7;
		public static int facialhair8;
		public static int facialhair9;
		public static int facialhair10;
		public static int facialhair11;
		public static int facialhair12;
		public static int facialhair13;
		public static int facialhair14;
			
		public static int skinhue1;
		public static int skinhue2;
		public static int skinhue3;
		public static int skinhue4;
		public static int skinhue5;
		public static int skinhue6;
		public static int skinhue7;
		public static int skinhue8;
		public static int skinhue9;
		public static int skinhue10;
		private Mobile m_mob;
		private Nation m_nation;
		private bool m_IgnoreChecks;
        private bool m_HairDressing;

        public NPCCustomGump() : base( 0, 0 ) { }

		public NPCCustomGump( Mobile m, Nation nation, int page ) : this( m, nation, page, false )
		{
		}

        public NPCCustomGump( Mobile m, Nation nation, int page, bool ignoreChecks )
            : this( m, nation, page, ignoreChecks, false )
        {
        }
		
		public NPCCustomGump( Mobile m, Nation nation, int page, bool ignoreChecks, bool hairDressing )
			: base( 0, 0 )
		{
			m_IgnoreChecks = ignoreChecks;
            m_HairDressing = hairDressing;
			m_mob = m;
			m_nation = nation;
			isbeardone = false;
			RacialInfo( nation );
			m.CloseGump( typeof( NPCCustomGump ) );
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(54, 31, 400, 383, 9270);
			this.AddBackground(71, 192, 364, 202, 3500);
            this.AddImage(183, 50, 29);
			this.AddImage(4, 10, 10440);
			this.AddImage(423, 10, 10441);
			this.AddLabel(126, 87, 1149, @"Hair");
			this.AddLabel(126, 121, 1149, @"Beard I");
			this.AddLabel(126, 158, 1149, @"Beard II");
			this.AddLabel(327, 84, 1149, @"Hair Hue");
			this.AddLabel(317, 121, 1149, @"Beard Hue");

            if( ignoreChecks )
                this.AddLabel( 175, 46, 2010, @"Character Customization" );

            else if( !hairDressing )
                this.AddLabel( 190, 46, 2010, @"Disguise Others" );

            else
                this.AddLabel( 200, 46, 2010, @"Hair Styling" );

            if( !hairDressing )
                this.AddLabel( 327, 158, 1149, @"Skin Hue" );

			this.AddButton(404, 46, 1150, 1152, 0, GumpButtonType.Reply, 0);
            this.AddButton(90, 81, page == 2 ? 9723 : 9720, 9722, 1, GumpButtonType.Reply, 0);
            this.AddButton(90, 118, page == 3 ? 9723 : 9720, 9722, 2, GumpButtonType.Reply, 0);
            this.AddButton(90, 155, page == 4 ? 9723 : 9720, 9722, 3, GumpButtonType.Reply, 0);
            this.AddButton(389, 81, page == 5 ? 9723 : 9720, 9722, 4, GumpButtonType.Reply, 0);
            this.AddButton(389, 118, page == 6 ? 9723 : 9720, 9722, 5, GumpButtonType.Reply, 0);

            if( !hairDressing )
                this.AddButton(389, 155, page == 7 ? 9723 : 9720, 9722, 6, GumpButtonType.Reply, 0);
						
			switch ( page )
			{
				case 1:
				{
                    if( !hairDressing )
					    this.AddHtml( 99, 219, 307, 147, @"You may now edit the target's appearance based on the race you have previously chosen.", (bool)true, (bool)true);
					
                    else
					    this.AddHtml( 99, 219, 307, 147, @"You may now style the target's hair and facial hair based your race.", (bool)true, (bool)true);
                   
                    break;
				}
				
				case 2:
				{
					AddHairLabels( this, nation, m.Female );
					break;
				}		
			
				case 3:
				{
					if( !m.Female )
					{
						isbeardone = true;
						this.AddButton(97, 215, 1209, 1210, 101, GumpButtonType.Reply, 0);
						this.AddLabel(115, 214, 0, @"None");
						facialhair1 = 0;
						this.AddButton(97, 238, 1209, 1210, 102, GumpButtonType.Reply, 0);
						this.AddLabel(115, 237, 0, @"Mustache");
						facialhair2 = mustache;
						this.AddButton(97, 261, 1209, 1210, 103, GumpButtonType.Reply, 0);
						this.AddLabel(115, 260, 0, @"Thin Mustache");
						facialhair3 = thinmustache;
						this.AddButton(97, 284, 1209, 1210, 104, GumpButtonType.Reply, 0);
						this.AddLabel(115, 283, 0, @"Goatee");
						facialhair4 = goatee;
						this.AddButton(97, 307, 1209, 1210, 105, GumpButtonType.Reply, 0);
						this.AddLabel(115, 306, 0, @"Thin Goatee");
						facialhair5 = thingoatee;
						this.AddButton(97, 330, 1209, 1210, 106, GumpButtonType.Reply, 0);
						this.AddLabel(115, 329, 0, @"Vandyke");
						facialhair6 = vandyke;
						this.AddButton(97, 353, 1209, 1210, 107, GumpButtonType.Reply, 0);
						this.AddLabel(115, 352, 0, @"Thin Vandyke");
						facialhair7 = thinvandyke;
						
						
						this.AddButton(215, 215, 1209, 1210, 108, GumpButtonType.Reply, 0);
						this.AddLabel(233, 214, 0, @"Short Beard");
						facialhair8 = shortbeard;
						this.AddButton(215, 238, 1209, 1210, 109, GumpButtonType.Reply, 0);
						this.AddLabel(233, 237, 0, @"Short Beard & Mustache");
						facialhair9 = shortbeardmustache;
						this.AddButton(215, 261, 1209, 1210, 110, GumpButtonType.Reply, 0);
						this.AddLabel(233, 260, 0, @"Thin Beard");
						facialhair10 = thinbeard;
						this.AddButton(215, 284, 1209, 1210, 111, GumpButtonType.Reply, 0);
						this.AddLabel(233, 283, 0, @"Thin Beard & Mustache");
						facialhair11 = thinbeardmustache;
						
						if( nation != Nation.Western )
						{
							this.AddButton(215, 307, 1209, 1210, 112, GumpButtonType.Reply, 0);
							this.AddLabel(233, 306, 0, @"Full Beard");
							facialhair12 = fullbeard;
							this.AddButton(215, 330, 1209, 1210, 113, GumpButtonType.Reply, 0);
							this.AddLabel(233, 329, 0, @"Full Beard & Mustache");
							facialhair13 = fullbeardmustache;
							this.AddButton(215, 353, 1209, 1210, 114, GumpButtonType.Reply, 0);
							this.AddLabel(233, 352, 0, @"Curly Long Beard");
							facialhair14 = curlylongbeard;
						}
					}
					
					break;
				}
			
			
				case 4: 
				{
					if( !m.Female )
						AddFacialHairLabels( nation );
					
					isbeardone = false;
					break;
				}
			
				case 5:
				{
					this.AddButton(111, 253, 1209, 1210, 36, GumpButtonType.Reply, 0);
					this.AddButton(111, 311, 1209, 1210, 37, GumpButtonType.Reply, 0);
					this.AddButton(171, 253, 1209, 1210, 38, GumpButtonType.Reply, 0);
					this.AddButton(171, 311, 1209, 1210, 39, GumpButtonType.Reply, 0);
					this.AddButton(231, 253, 1209, 1210, 40, GumpButtonType.Reply, 0);
					this.AddButton(231, 311, 1209, 1210, 41, GumpButtonType.Reply, 0);
					this.AddButton(291, 253, 1209, 1210, 42, GumpButtonType.Reply, 0);
					this.AddButton(291, 311, 1209, 1210, 43, GumpButtonType.Reply, 0);
					this.AddButton(351, 253, 1209, 1210, 44, GumpButtonType.Reply, 0);
					this.AddButton(351, 311, 1209, 1210, 45, GumpButtonType.Reply, 0);
					
					this.AddImage(134, 245, 255, hairhue1 -1 );
					this.AddImage(134, 303, 255, hairhue2 -1 );
					this.AddImage(194, 245, 255, hairhue3 -1 );
					this.AddImage(194, 303, 255, hairhue4 -1 );
					this.AddImage(254, 245, 255, hairhue5 -1 );
					this.AddImage(254, 303, 255, hairhue6 -1 );
					this.AddImage(314, 245, 255, hairhue7 -1 );
					this.AddImage(314, 303, 255, hairhue8 -1 );
					this.AddImage(374, 245, 255, hairhue9 -1 );
					this.AddImage(374, 303, 255, hairhue10 -1 );
					break;
				}
				
				case 6:
				{
					this.AddButton(111, 253, 1209, 1210, 46, GumpButtonType.Reply, 0);
					this.AddButton(111, 311, 1209, 1210, 47, GumpButtonType.Reply, 0);
					this.AddButton(171, 253, 1209, 1210, 48, GumpButtonType.Reply, 0);
					this.AddButton(171, 311, 1209, 1210, 49, GumpButtonType.Reply, 0);
					this.AddButton(231, 253, 1209, 1210, 50, GumpButtonType.Reply, 0);
					this.AddButton(231, 311, 1209, 1210, 51, GumpButtonType.Reply, 0);
					this.AddButton(291, 253, 1209, 1210, 52, GumpButtonType.Reply, 0);
					this.AddButton(291, 311, 1209, 1210, 53, GumpButtonType.Reply, 0);
					this.AddButton(351, 253, 1209, 1210, 54, GumpButtonType.Reply, 0);
					this.AddButton(351, 311, 1209, 1210, 55, GumpButtonType.Reply, 0);
					
					this.AddImage(134, 245, 255, hairhue1 -1 );
					this.AddImage(134, 303, 255, hairhue2 -1 );
					this.AddImage(194, 245, 255, hairhue3 -1 );
					this.AddImage(194, 303, 255, hairhue4 -1 );
					this.AddImage(254, 245, 255, hairhue5 -1 );
					this.AddImage(254, 303, 255, hairhue6 -1 );
					this.AddImage(314, 245, 255, hairhue7 -1 );
					this.AddImage(314, 303, 255, hairhue8 -1 );
					this.AddImage(374, 245, 255, hairhue9 -1 );
					this.AddImage(374, 303, 255, hairhue10 -1 );
					break;
				}
				
				case 7:
				{
					this.AddButton(111, 253, 1209, 1210, 56, GumpButtonType.Reply, 0);
					this.AddButton(111, 311, 1209, 1210, 57, GumpButtonType.Reply, 0);
					this.AddButton(171, 253, 1209, 1210, 58, GumpButtonType.Reply, 0);
					this.AddButton(171, 311, 1209, 1210, 59, GumpButtonType.Reply, 0);
					this.AddButton(231, 253, 1209, 1210, 60, GumpButtonType.Reply, 0);
					this.AddButton(231, 311, 1209, 1210, 61, GumpButtonType.Reply, 0);
					this.AddButton(291, 253, 1209, 1210, 62, GumpButtonType.Reply, 0);
					this.AddButton(291, 311, 1209, 1210, 63, GumpButtonType.Reply, 0);
					this.AddButton(351, 253, 1209, 1210, 64, GumpButtonType.Reply, 0);
					this.AddButton(351, 311, 1209, 1210, 65, GumpButtonType.Reply, 0);
					
					this.AddImage(134, 245, 255, skinhue1 -1 );
					this.AddImage(134, 303, 255, skinhue2 -1 );
					this.AddImage(194, 245, 255, skinhue3 -1 );
					this.AddImage(194, 303, 255, skinhue4 -1 );
					this.AddImage(254, 245, 255, skinhue5 -1 );
					this.AddImage(254, 303, 255, skinhue6 -1 );
					this.AddImage(314, 245, 255, skinhue7 -1 );
					this.AddImage(314, 303, 255, skinhue8 -1 );
					this.AddImage(374, 245, 255, skinhue9 -1 );
					this.AddImage(374, 303, 255, skinhue10 -1 );
					break;
				}
					
			}

		}
		
		private void AddFacialHairLabels( Nation nation )
		{
			switch( nation )
			{
				case Nation.Southern:
				{
					this.AddButton(97, 215, 1209, 1210, 101, GumpButtonType.Reply, 0);
					this.AddLabel(115, 214, 0, @"Long Beard");
					facialhair1 = longbeard;
					this.AddButton(97, 238, 1209, 1210, 102, GumpButtonType.Reply, 0);
					this.AddLabel(115, 237, 0, @"Long Beard & Mustache");
					facialhair2 = longbeardmustache;
					this.AddButton(97, 261, 1209, 1210, 103, GumpButtonType.Reply, 0);
					this.AddLabel(115, 260, 0, @"Very Long Beard");
					facialhair3 = verylongbeard;
					this.AddButton(97, 284, 1209, 1210, 104, GumpButtonType.Reply, 0);
					this.AddLabel(115, 283, 0, @"Extra Long Beard");
					facialhair4 = extralongbeard;
					this.AddButton(97, 307, 1209, 1210, 105, GumpButtonType.Reply, 0);
					this.AddLabel(115, 306, 0, @"Two Braids Beard");
					facialhair5 = twobraidsbeard;
					this.AddButton(97, 330, 1209, 1210, 106, GumpButtonType.Reply, 0);
					this.AddLabel(115, 329, 0, @"Ornate Long Beard");
					facialhair6 = ornatelongbeard;
					this.AddButton(97, 353, 1209, 1210, 107, GumpButtonType.Reply, 0);
					this.AddLabel(115, 352, 0, @"Braided Beard");
					facialhair7 = braidedbeard;
					
					break;
				}
					
				case Nation.Haluaroc:
				{
					this.AddButton(97, 215, 1209, 1210, 101, GumpButtonType.Reply, 0);
					this.AddLabel(115, 214, 0, @"Nice Goatee");
					facialhair1 = nicegoatee;
					
					break;
				}
					
				case Nation.Mhordul:
				{
					this.AddButton(97, 215, 1209, 1210, 101, GumpButtonType.Reply, 0);
					this.AddLabel(115, 214, 0, @"Long Beard");
					facialhair1 = longbeard;
					this.AddButton(97, 238, 1209, 1210, 102, GumpButtonType.Reply, 0);
					this.AddLabel(115, 237, 0, @"Long Beard & Mustache");
					facialhair2 = longbeardmustache;
					this.AddButton(97, 261, 1209, 1210, 103, GumpButtonType.Reply, 0);
					this.AddLabel(115, 260, 0, @"Very Long Beard");
					facialhair3 = verylongbeard;
					this.AddButton(97, 284, 1209, 1210, 104, GumpButtonType.Reply, 0);
					this.AddLabel(115, 283, 0, @"Extra Long Beard");
					facialhair4 = extralongbeard;
					
					break;
				}
					
				case Nation.Tirebladd:
				{
					this.AddButton(97, 215, 1209, 1210, 101, GumpButtonType.Reply, 0);
					this.AddLabel(115, 214, 0, @"Long Beard");
					facialhair1 = longbeard;
					this.AddButton(97, 238, 1209, 1210, 102, GumpButtonType.Reply, 0);
					this.AddLabel(115, 237, 0, @"Long Beard & Mustache");
					facialhair2 = longbeardmustache;
					this.AddButton(97, 261, 1209, 1210, 103, GumpButtonType.Reply, 0);
					this.AddLabel(115, 260, 0, @"Very Long Beard");
					facialhair3 = verylongbeard;
					this.AddButton(97, 284, 1209, 1210, 104, GumpButtonType.Reply, 0);
					this.AddLabel(115, 283, 0, @"Extra Long Beard");
					facialhair4 = extralongbeard;
					this.AddButton(97, 307, 1209, 1210, 105, GumpButtonType.Reply, 0);
					this.AddLabel(115, 306, 0, @"Two Braids Beard");
					facialhair5 = twobraidsbeard;
					this.AddButton(97, 330, 1209, 1210, 106, GumpButtonType.Reply, 0);
					this.AddLabel(115, 329, 0, @"Ornate Long Beard");
					facialhair6 = ornatelongbeard;
					this.AddButton(97, 353, 1209, 1210, 107, GumpButtonType.Reply, 0);
					this.AddLabel(115, 352, 0, @"Ornate Very Long Beard");
					facialhair7 = ornateverylongbeard;
					this.AddButton(269, 215, 1209, 1210, 108, GumpButtonType.Reply, 0);
					this.AddLabel(287, 214, 0, @"Mid-Long Beard");
					facialhair8 = midlongbeard;
					this.AddButton(269, 238, 1209, 1210, 109, GumpButtonType.Reply, 0);
					this.AddLabel(287, 237, 0, @"Ornate Beard");
					facialhair9 = ornatebeard;
					
					break;
				}
					
				case Nation.Northern:
				{
					this.AddButton(97, 215, 1209, 1210, 101, GumpButtonType.Reply, 0);
					this.AddLabel(115, 214, 0, @"Long Beard");
					facialhair1 = longbeard;
					this.AddButton(97, 238, 1209, 1210, 102, GumpButtonType.Reply, 0);
					this.AddLabel(115, 237, 0, @"Long Beard & Mustache");
					facialhair2 = longbeardmustache;
					this.AddButton(97, 261, 1209, 1210, 103, GumpButtonType.Reply, 0);
					this.AddLabel(115, 260, 0, @"Very Long Beard");
					facialhair3 = verylongbeard;
					this.AddButton(97, 284, 1209, 1210, 104, GumpButtonType.Reply, 0);
					this.AddLabel(115, 283, 0, @"Extra Long Beard");
					facialhair4 = extralongbeard;
					
					break;
				}
			}
		}
		
		public void SetHairID( Mobile m, int id )
		{
            if( m is PlayerMobile && !m_IgnoreChecks && !m_HairDressing )
                ( (PlayerMobile)m ).Disguise.HairItemID = id;
            else
                m.HairItemID = id;

            m.Delta( MobileDelta.Hue );
            m.Delta( MobileDelta.Hair );
            m.Delta( MobileDelta.FacialHair );
		}
		
		public void SetFacialHairID( Mobile m, int id )
		{
            if( m is PlayerMobile && !m_IgnoreChecks && !m_HairDressing )
                ( (PlayerMobile)m ).Disguise.FacialHairItemID = id;
            else
                m.FacialHairItemID = id;

            m.Delta( MobileDelta.Hue );
            m.Delta( MobileDelta.Hair );
            m.Delta( MobileDelta.FacialHair );
		}

        public void SetFacialHairHue( Mobile m, int id )
        {
            if( m is PlayerMobile && !m_IgnoreChecks && !m_HairDressing )
                ( (PlayerMobile)m ).Disguise.FacialHairHue = id;
            else
                m.FacialHairHue = id;

            m.Delta( MobileDelta.Hue );
            m.Delta( MobileDelta.Hair );
            m.Delta( MobileDelta.FacialHair );
        }

        public void SetHairHue( Mobile m, int id )
        {
            if( m is PlayerMobile && !m_IgnoreChecks && !m_HairDressing )
                ( (PlayerMobile)m ).Disguise.HairHue = id;
            else
                m.HairHue = id;

            m.Delta( MobileDelta.Hue );
            m.Delta( MobileDelta.Hair );
            m.Delta( MobileDelta.FacialHair );
        }

        public void SetHue( Mobile m, int id )
        {
            if( m is PlayerMobile && !m_IgnoreChecks && !m_HairDressing )
                ( (PlayerMobile)m ).Disguise.Hue = id;
            else
                m.Hue = id;

            m.Delta( MobileDelta.Hue );
            m.Delta( MobileDelta.Hair );
            m.Delta( MobileDelta.FacialHair );
        }

        public static void AddLabelledButton( Gump gump, KeyValuePair<string, int> kvp, int index )
        {
            if( kvp.Key == null )
                return;

            gump.AddButton( gump.X, gump.Y, 1209, 1210, index, GumpButtonType.Reply, 0 );
            gump.AddLabel( gump.X + 18, gump.Y - 1, 0, kvp.Key );
            gump.Y += 23;
        }
		
		public static void AddHairLabels( NPCCustomGump gump, Nation nation, bool isfemale )
		{
            if( isfemale )
                gump.hair12 = verylong;
            else
                gump.hair12 = receeding;

            switch( nation )
            {
                case Nation.Southern:
                {
                    gump.hair13 = shouldershort;
                    gump.hair14 = longSouthern;
                    gump.hair15 = fancyshort;
                    gump.hair16 = thinshort;

                    if( isfemale )
                    {
                        gump.hair17 = wavyshort;
                        gump.hair18 = lushlong;
                        gump.hair19 = delicatelong;
                        gump.hair20 = wildcurly;
                    }

                    break;
                }

                case Nation.Western:
                {
                    gump.hair13 = topknot;
                    gump.hair14 = shouldershort;
                    gump.hair15 = shortdreads;
                    gump.hair16 = mediumdreads;
                    gump.hair17 = longdreads;

                    if( isfemale )
                    {
                        gump.hair18 = wildcurly;
                        gump.hair19 = wavyshort;
                    }

                    break;
                }

                case Nation.Haluaroc:
                {
                    gump.hair13 = longplaited;
                    gump.hair14 = shortdreads;
                    gump.hair15 = mediumdreads;
                    gump.hair16 = longdreads;
                    gump.hair17 = shouldershort;

                    if( isfemale )
                    {
                        gump.hair18 = wavyshort;
                        gump.hair19 = lushlong;
                        gump.hair20 = wildcurly;
                    }

                    break;
                }

                case Nation.Mhordul:
                {
                    gump.hair13 = mhordulhair;
                    gump.hair14 = longmessy;

                    if( isfemale )
                    {
                        gump.hair15 = wavyshort;
                        gump.hair16 = wildcurly;
                    }

                    break;
                }

                case Nation.Tirebladd:
                {
                    gump.hair13 = fancyshort;
                    gump.hair14 = thinshort;

                    if( isfemale )
                    {
                        gump.hair15 = wavyshort;
                        gump.hair16 = lushlong;
                        gump.hair17 = delicatelong;
                        gump.hair18 = wildcurly;
                    }

                    break;
                }

                case Nation.Northern:
                {
                    gump.hair13 = fancyshort;
                    gump.hair14 = thinshort;
                    gump.hair15 = pageboy;

                    if( isfemale )
                    {
                        gump.hair16 = wavyshort;
                        gump.hair17 = lushlong;
                        gump.hair18 = delicatelong;
                        gump.hair19 = wildcurly;
                    }

                    break;
                }
            }

            gump.X = 97;
            gump.Y = 215;
            AddLabelledButton( gump, gump.hair0, 7 );
            AddLabelledButton( gump, gump.hair1, 8 );
            AddLabelledButton( gump, gump.hair2, 9 );
            AddLabelledButton( gump, gump.hair3, 10 );
            AddLabelledButton( gump, gump.hair4, 11 );
            AddLabelledButton( gump, gump.hair5, 12 );
            AddLabelledButton( gump, gump.hair6, 13 );
            gump.X = 194;
            gump.Y = 215;
            AddLabelledButton( gump, gump.hair7, 14 );
            AddLabelledButton( gump, gump.hair8, 15 );
            AddLabelledButton( gump, gump.hair9, 16 );
            AddLabelledButton( gump, gump.hair10, 17 );
            AddLabelledButton( gump, gump.hair11, 18 );
            AddLabelledButton( gump, gump.hair12, 19 );
            AddLabelledButton( gump, gump.hair13, 20 );
            gump.X = 321;
            gump.Y = 215;
            AddLabelledButton( gump, gump.hair14, 21 );
            AddLabelledButton( gump, gump.hair15, 22 );
            AddLabelledButton( gump, gump.hair16, 23 );
            AddLabelledButton( gump, gump.hair17, 24 );
            AddLabelledButton( gump, gump.hair18, 25 );
            AddLabelledButton( gump, gump.hair19, 26 );
            AddLabelledButton( gump, gump.hair20, 27 );
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			PlayerMobile m = sender.Mobile as PlayerMobile;
			
			if( m == null || m_mob == null )
				return;
			
			int oldhair = m_mob.HairItemID;
            int featlevel = ( (PlayerMobile)m ).Feats.GetFeatLevel( FeatList.DisguiseKit );

            if( m_HairDressing )
                featlevel = ( (PlayerMobile)m ).Feats.GetFeatLevel( FeatList.Barbery );

            else if( m != m_mob )
                featlevel = ( (PlayerMobile)m ).Feats.GetFeatLevel( FeatList.DisguiseOthers );
			
			if( info.ButtonID > 6 && m.AccessLevel < AccessLevel.GameMaster && !m_IgnoreChecks )
			{
				Item apron = m_mob.FindItemOnLayer( Layer.MiddleTorso );
              
				if( apron == null || !( apron is HairStylingApron ) )
				{
					m.SendMessage( "Your target needs to be wearing a hair styling apron." );
					return;
				}	
				
				if( !m.CanSee( m_mob ) || !m.InRange( m_mob, 1 ) || !m.Alive || !m_mob.Alive || !m.InLOS( m_mob ) )
				{
					m.SendMessage( "Target out of reach." );
					return;
				}
				
				if( info.ButtonID > 27 && info.ButtonID < 100 )
				{
					Item dyetub = m.Backpack.FindItemByType( typeof( DyeTub ) ) as DyeTub;
					
					if( dyetub == null )
					{
						m.SendMessage( "You need to have a dye tub in your backpack in order to do that." );
						return;
					}
					
					if( !m_HairDressing && featlevel < 2 && info.ButtonID > 55 && info.ButtonID < 66 )
					{
						m.SendMessage( "You lack the appropriate feat level." );
						return;
					}
				}
				
				else
				{
					Item scissors = m.Backpack.FindItemByType( typeof( Scissors ) ) as Scissors;
					
					if( scissors == null )
					{
						m.SendMessage( "You need to have a pair of scissors in your backpack in order to do that." );
						return;
					}
					
					if( !m_HairDressing && featlevel < 2 )
					{
						m.SendMessage( "You lack the appropriate feat level." );
						return;
					}
				}
			}
				
			RacialInfo( m_nation );
			
			if ( m_mob == null )
				return;

			switch ( info.ButtonID )
			{
				case 0: 
				{
		   			break;
				}
					
				case 1: 
				{
                    if( !m_HairDressing || featlevel > 0 )
					    m.SendGump( new NPCCustomGump( m_mob, m_nation, 2, m_IgnoreChecks, m_HairDressing ) );

                    else
                    {
                        m.SendMessage( "You lack the appropriate feat level." );
                        m.SendGump( new NPCCustomGump( m_mob, m_nation, 0, m_IgnoreChecks, m_HairDressing ) );
                    }
		   			break;
				}
					
				case 2: 
				{
                    if( !m_HairDressing || featlevel > 1 )
                        m.SendGump( new NPCCustomGump( m_mob, m_nation, 3, m_IgnoreChecks, m_HairDressing ) );

                    else
                    {
                        m.SendMessage( "You lack the appropriate feat level." );
                        m.SendGump( new NPCCustomGump( m_mob, m_nation, 0, m_IgnoreChecks, m_HairDressing ) );
                    }
		   			break;
				}
					
				case 3: 
				{
                    if( !m_HairDressing || featlevel > 1 )
                        m.SendGump( new NPCCustomGump( m_mob, m_nation, 4, m_IgnoreChecks, m_HairDressing ) );

                    else
                    {
                        m.SendMessage( "You lack the appropriate feat level." );
                        m.SendGump( new NPCCustomGump( m_mob, m_nation, 0, m_IgnoreChecks, m_HairDressing ) );
                    }
		   			break;
				}
					
				case 4: 
				{
                    if( !m_HairDressing || featlevel > 2 )
                        m.SendGump( new NPCCustomGump( m_mob, m_nation, 5, m_IgnoreChecks, m_HairDressing ) );

                    else
                    {
                        m.SendMessage( "You lack the appropriate feat level." );
                        m.SendGump( new NPCCustomGump( m_mob, m_nation, 0, m_IgnoreChecks, m_HairDressing ) );
                    }
		   			break;
				}
					
				case 5: 
				{
                    if( !m_HairDressing || featlevel > 2 )
                        m.SendGump( new NPCCustomGump( m_mob, m_nation, 6, m_IgnoreChecks, m_HairDressing ) );

                    else
                    {
                        m.SendMessage( "You lack the appropriate feat level." );
                        m.SendGump( new NPCCustomGump( m_mob, m_nation, 0, m_IgnoreChecks, m_HairDressing ) );
                    }
		   			break;
				}
					
				case 6: 
				{
					m.SendGump( new NPCCustomGump( m_mob, m_nation, 7, m_IgnoreChecks, m_HairDressing ) );
		   			break;
				}
					
#region Hair Styles

				case 7: // Short Hair
				{
					SetHairID( m_mob, hair0.Value );
					break;
				}
					
				case 8: // Long Hair
				{
					SetHairID( m_mob, hair1.Value );
					break;
				}
					
				case 9: // Pony Tail
				{
					SetHairID( m_mob, hair2.Value );
					break;
				}
				
				case 10: // Buns
				{
					SetHairID( m_mob, hair3.Value );
					break;
				}
				
				case 11: // Pig Tails
				{
					SetHairID( m_mob, hair4.Value );
					break;
				}
					
				case 12: // Topknot
				{
					SetHairID( m_mob, hair5.Value );
					break;
				}
					
				case 13: // Mid-Long
				{
					SetHairID( m_mob, hair6.Value );
					break;
				}
					
				case 14: // Mohawk
				{
					SetHairID( m_mob, hair7.Value );
					break;
				}
					
				case 15: // Afro
				{
					SetHairID( m_mob, hair8.Value );
					break;
				}
					
				case 16: // Receeding
				{
					SetHairID( m_mob, hair9.Value );
					break;
				}
					
				case 17: // Pageboy
				{
					SetHairID( m_mob, hair10.Value );
					break;
				}
					
				case 18: // Spiked
				{
					SetHairID( m_mob, hair11.Value );
					break;
				}
					
				case 19: // Mullet
				{
					SetHairID( m_mob, hair12.Value );
					break;
				}
					
				case 20: // Curly
				{
					SetHairID( m_mob, hair13.Value );
					break;
				}
					
				case 21: // Flower
				{
					SetHairID( m_mob, hair14.Value );
					break;
				}
					
				case 22: // Big Knob
				{
					SetHairID( m_mob, hair15.Value );
					break;
				}
					
				case 23: // Big Bun
				{
					SetHairID( m_mob, hair16.Value );
					break;
				}
					
				case 24: // Big Braid
				{
					SetHairID( m_mob, hair17.Value );
					break;
				}
					
				case 25: // Fancy Short
				{
					SetHairID( m_mob, hair18.Value );
					break;
				}
					
				case 26: // Feather
				{
					SetHairID( m_mob, hair19.Value );
					break;
				}
					
				case 27: // Bald
				{
					SetHairID( m_mob, hair20.Value );
					break;
				}
				
#endregion	

#region Facial Hair Styles
				
				case 101: 
				{
					SetFacialHairID( m_mob, facialhair1 );
					break;
				}
					
				case 102: 
				{
					SetFacialHairID( m_mob, facialhair2 );
					break;
				}
					
				case 103: 
				{
					SetFacialHairID( m_mob, facialhair3 );
					break;
				}
					
				case 104: 
				{
					SetFacialHairID( m_mob,  facialhair4 );
					break;
				}
					
				case 105: 
				{
					SetFacialHairID( m_mob, facialhair5 );
					break;
				}
					
				case 106: 
				{
					SetFacialHairID( m_mob, facialhair6 );
					break;
				}
					
				case 107: 
				{
					SetFacialHairID( m_mob, facialhair7 );
					break;
				}
				
				case 108:
				{
					SetFacialHairID( m_mob, facialhair8 );
					break;
				}
				
				case 109: 
				{
					SetFacialHairID( m_mob, facialhair9 );
					break;
				}
				
				case 110:
				{
					SetFacialHairID( m_mob, facialhair10 );
					break;
				}
				
				case 111: 
				{
					SetFacialHairID( m_mob, facialhair11 );
					break;
				}
				
				case 112:
				{
					SetFacialHairID( m_mob, facialhair12 );
					break;
				}
				
				case 113: 
				{
					SetFacialHairID( m_mob, facialhair13 );
					break;
				}
				
				case 114:
				{
					SetFacialHairID( m_mob, facialhair14 );
					break;
				}
					
#endregion

#region Hair Hues
					
				case 36:
				{
                    SetHairHue( m_mob, hairhue1 );
					break;
				}
					
				case 37:
				{
                    SetHairHue( m_mob, hairhue2 );
					break;
				}
					
				case 38:
				{
                    SetHairHue( m_mob, hairhue3 );
					break;
				}
					
				case 39:
				{
                    SetHairHue( m_mob, hairhue4 );
					break;
				}
					
				case 40:
				{
                    SetHairHue( m_mob, hairhue5 );
					break;
				}
					
				case 41:
				{
                    SetHairHue( m_mob, hairhue6 );
					break;
				}
					
				case 42:
				{
                    SetHairHue( m_mob, hairhue7 );
					break;
				}
					
				case 43:
				{
                    SetHairHue( m_mob, hairhue8 );
					break;
				}
					
				case 44:
				{
                    SetHairHue( m_mob, hairhue9 );
					break;
				}
					
				case 45:
				{
                    SetHairHue( m_mob, hairhue10 );
					break;
				}
					
#endregion

#region Facial Hair Hues
					
				case 46:
				{
                    SetFacialHairHue( m_mob, hairhue1 );
					break;
				}
					
				case 47:
				{
                    SetFacialHairHue( m_mob, hairhue2 );
					break;
				}
					
				case 48:
				{
                    SetFacialHairHue( m_mob, hairhue3 );
					break;
				}
					
				case 49:
				{
                    SetFacialHairHue( m_mob, hairhue4 );
					break;
				}
					
				case 50:
				{
                    SetFacialHairHue( m_mob, hairhue5 );
					break;
				}
					
				case 51:
				{
                    SetFacialHairHue( m_mob, hairhue6 );
					break;
				}
					
				case 52:
				{
                    SetFacialHairHue( m_mob, hairhue7 );
					break;
				}
					
				case 53:
				{
                    SetFacialHairHue( m_mob, hairhue8 );
					break;
				}
					
				case 54:
				{
                    SetFacialHairHue( m_mob, hairhue9 );
					break;
				}
					
				case 55:
				{
                    SetFacialHairHue( m_mob, hairhue10 );
					break;
				}

#endregion

#region Skin Hues
					
				case 56:
				{
                    SetHue( m_mob, skinhue1 );
					break;
				}
					
				case 57:
				{
                    SetHue( m_mob, skinhue2 );
					break;
				}
					
				case 58:
				{
                    SetHue( m_mob, skinhue3 );
					break;
				}
					
				case 59:
				{
                    SetHue( m_mob, skinhue4 );
					break;
				}
					
				case 60:
				{
                    SetHue( m_mob, skinhue5 );
					break;
				}
					
				case 61:
				{
                    SetHue( m_mob, skinhue6 );
					break;
				}
					
				case 62:
				{
                    SetHue( m_mob, skinhue7 );
					break;
				}
					
				case 63:
				{
                    SetHue( m_mob, skinhue8 );
					break;
				}
					
				case 64:
				{
                    SetHue( m_mob, skinhue9 );
					break;
				}
					
				case 65:
				{
                    SetHue( m_mob, skinhue10 );
					break;
				}
				
#endregion

			}

			if( m_mob.HairItemID > 0 && m_mob.HairHue < 1 )
				m_mob.HairHue = BaseKhaerosMobile.AssignRacialHairHue( m_nation );
			
			if( m_mob.FacialHairItemID > 0 && m_mob.FacialHairHue < 1 )
				m_mob.FacialHairHue = BaseKhaerosMobile.AssignRacialHairHue( m_nation );
			
			if( info.ButtonID > 99 )
			{
				if( isbeardone )
					m.SendGump( new NPCCustomGump( m_mob, m_nation, 3, m_IgnoreChecks, m_HairDressing ) );
				
				else
                    m.SendGump( new NPCCustomGump( m_mob, m_nation, 4, m_IgnoreChecks, m_HairDressing ) );
			}
			
			if ( info.ButtonID > 6 && info.ButtonID < 28 )
			{
                m.SendGump( new NPCCustomGump( m_mob, m_nation, 2, m_IgnoreChecks, m_HairDressing ) );
			}
			
			if ( info.ButtonID > 27 && info.ButtonID < 36 )
			{
                m.SendGump( new NPCCustomGump( m_mob, m_nation, 3, m_IgnoreChecks, m_HairDressing ) );
			}
			
			if ( info.ButtonID > 35 && info.ButtonID < 46 )
			{
                m.SendGump( new NPCCustomGump( m_mob, m_nation, 5, m_IgnoreChecks, m_HairDressing ) );
			}
			
			if ( info.ButtonID > 45 && info.ButtonID < 56 )
			{
                m.SendGump( new NPCCustomGump( m_mob, m_nation, 6, m_IgnoreChecks, m_HairDressing ) );
			}
			
			if ( info.ButtonID > 55 && info.ButtonID < 99 )
			{
                m.SendGump( new NPCCustomGump( m_mob, m_nation, 7, m_IgnoreChecks, m_HairDressing ) );
			}
		}
		
		public static void RacialInfo( Nation nation )
		{
			switch ( nation )
			{		
				case Nation.Western:
				{
					hairhue1 = 2801;
					hairhue2 = 2411;
					hairhue3 = 1141;
					hairhue4 = 1109;
					hairhue5 = 2799;
					hairhue6 = 2406;
					hairhue7 = 1175;
					hairhue8 = 1881;
					hairhue9 = 1022;
					hairhue10 = 2992;
					
					skinhue1 = 1005;
					skinhue2 = 1850;
					skinhue3 = 1812;
					skinhue4 = 1849;
					skinhue5 = 1815;
					skinhue6 = 1148;
					skinhue7 = 1147;
					skinhue8 = 1146;
					skinhue9 = 1145;
					skinhue10 = 1144;
					break;
				}
			
				case Nation.Haluaroc:
				{
					hairhue1 = 2795;
					hairhue2 = 2801;
					hairhue3 = 2799;
					hairhue4 = 2990;
					hairhue5 = 1908;
					hairhue6 = 1141;
					hairhue7 = 1175;
					hairhue8 = 1133;
					hairhue9 = 1149;
					hairhue10 = 2992;
					
					skinhue1 = 1815;
					skinhue2 = 1814;
					skinhue3 = 1813;
					skinhue4 = 1812;
					skinhue5 = 1851;
					skinhue6 = 1850;
					skinhue7 = 1849;
					skinhue8 = 1822;
					skinhue9 = 1823;
					skinhue10 = 1824;
					break;
				}
					
				case Nation.Northern:
				{
					hairhue1 = 2990;
					hairhue2 = 1147;
					hairhue3 = 1058;
					hairhue4 = 1109;
					hairhue5 = 2418;
					hairhue6 = 1149;
					hairhue7 = 1145;
					hairhue8 = 1103;
					hairhue9 = 1120;
					hairhue10 = 1509;
					
					skinhue1 = 1018;
					skinhue2 = 1025;
					skinhue3 = 1851;
					skinhue4 = 1145;
					skinhue5 = 1005;
					skinhue6 = 1012;
					skinhue7 = 1019;
					skinhue8 = 1026;
					skinhue9 = 1031;
					skinhue10 = 1065;
					break;
				}
					
				case Nation.Tirebladd:
				{
					hairhue1 = 2418;
					hairhue2 = 2803;
					hairhue3 = 2218;
					hairhue4 = 2657;
					hairhue5 = 2796;
					hairhue6 = 1126;
					hairhue7 = 2413;
					hairhue8 = 1118;
					hairhue9 = 2601;
					hairhue10 = 1108;
					
					skinhue1 = 1002;
					skinhue2 = 1009;
					skinhue3 = 1016;
					skinhue4 = 1023;
					skinhue5 = 1003;
					skinhue6 = 1010;
					skinhue7 = 1017;
					skinhue8 = 1024;
					skinhue9 = 1004;
					skinhue10 = 1011;
					break;
				}
					
				case Nation.Southern:
				{
					hairhue1 = 2107;
					hairhue2 = 1149;
					hairhue3 = 2413;
					hairhue4 = 1022;
					hairhue5 = 2312;
					hairhue6 = 1627;
					hairhue7 = 1830;
					hairhue8 = 2303;
					hairhue9 = 1845;
					hairhue10 = 2206;
					
					skinhue1 = 1062;
					skinhue2 = 1061;
					skinhue3 = 1849;
					skinhue4 = 1064;
					skinhue5 = 1065;
					skinhue6 = 1066;
					skinhue7 = 1019;
					skinhue8 = 1003;
					skinhue9 = 1037;
					skinhue10 = 1144;
					break;
				}
					
				case Nation.Mhordul:
				{
					
					
					hairhue1 = 1149;
					hairhue2 = 1022;
					hairhue3 = 1141;
					hairhue4 = 2990;
					hairhue5 = 1109;
					hairhue6 = 1190;
					hairhue7 = 2799;
					hairhue8 = 2801;
					hairhue9 = 2992;
					hairhue10 = 2418;
					
					skinhue1 = 1849;
					skinhue2 = 1145;
					skinhue3 = 1062;
					skinhue4 = 1147;
					skinhue5 = 2426;
					skinhue6 = 1017;
					skinhue7 = 1011;
					skinhue8 = 2425;
					skinhue9 = 1813;
					skinhue10 = 1072;
					

					break;
				}
			}
		}
	}
}
