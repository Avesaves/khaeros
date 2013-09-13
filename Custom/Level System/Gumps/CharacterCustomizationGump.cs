using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;

namespace Server.Gumps
{
	public class CharCustomGump : NPCCustomGump
	{		
		public CharCustomGump( PlayerMobile m, int page )
		{
			isbeardone = false;
			RacialInfo( m.Nation );
			m.CloseGump( typeof( CharCustomGump ) );
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(54, 31, 400, 383, 9270);
			this.AddBackground(71, 192, 364, 202, 3500);
            this.AddImage(183, 50, 29);
			
			if( !m.Reforging || (m.Reforging && m.OldMapChar) )
			{
				this.AddButton(224, 161, 5600, 5604, 501, GumpButtonType.Reply, 0);
				this.AddButton(203, 161, 9764, 9765, 502, GumpButtonType.Reply, 0);
			}
			
			this.AddLabel(244, 158, 1149, @"Age: " + m.Age );
            
			this.AddImage(4, 10, 10440);
			this.AddImage(423, 10, 10441);
			this.AddLabel(126, 87, 1149, @"Hair");
			this.AddLabel(126, 121, 1149, @"Beard I");
			this.AddLabel(126, 158, 1149, @"Beard II");
			this.AddLabel(327, 84, 1149, @"Hair Hue");
			this.AddLabel(317, 121, 1149, @"Beard Hue");
			this.AddLabel(327, 158, 1149, @"Skin Hue");
			this.AddLabel(175, 46, 2010, @"Character Customization");
			this.AddButton(404, 46, 1150, 1152, 0, GumpButtonType.Reply, 0);
            this.AddButton(90, 81, page == 2 ? 9723 : 9720, 9722, 1, GumpButtonType.Reply, 0);
            this.AddButton(90, 118, page == 3 ? 9723 : 9720, 9722, 2, GumpButtonType.Reply, 0);
            this.AddButton(90, 155, page == 4 ? 9723 : 9720, 9722, 3, GumpButtonType.Reply, 0);
            this.AddButton(389, 81, page == 5 ? 9723 : 9720, 9722, 4, GumpButtonType.Reply, 0);
            this.AddButton(389, 118, page == 6 ? 9723 : 9720, 9722, 5, GumpButtonType.Reply, 0);
            this.AddButton(389, 155, page == 7 ? 9723 : 9720, 9722, 6, GumpButtonType.Reply, 0);
						
			switch ( page )
			{
				case 1:
				{
					this.AddHtml( 99, 219, 307, 147, @"You may now edit your character's appearance based on the race you have previously chosen.", (bool)true, (bool)true);
					break;
				}
				
				case 2:
				{
                    AddHairLabels( this, m.Nation, m.Female );
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
					
					break;
				}
			
			
				case 4: 
				{
					if( !m.Female )
						AddFacialHairLabels( m.Nation );
					
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
					this.AddLabel(115, 352, 0, @"Ornate Very Long Beard");
					facialhair7 = ornateverylongbeard;
					this.AddButton(269, 215, 1209, 1210, 108, GumpButtonType.Reply, 0);
					this.AddLabel(287, 214, 0, @"Mid-Long Beard");
					facialhair8 = midlongbeard;
					this.AddButton(269, 238, 1209, 1210, 109, GumpButtonType.Reply, 0);
					this.AddLabel(287, 237, 0, @"Ornate Beard");
					facialhair9 = ornatebeard;
					this.AddButton(97, 353, 1209, 1210, 107, GumpButtonType.Reply, 0);
					this.AddLabel(115, 352, 0, @"Braided Beard");
					facialhair7 = braidedbeard;
					this.AddButton(97, 215, 1209, 1210, 101, GumpButtonType.Reply, 0);
					this.AddLabel(115, 214, 0, @"Nice Goatee");
					facialhair1 = nicegoatee;
					
					break;
				}
					
				case Nation.Western:
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
					this.AddButton(97, 353, 1209, 1210, 107, GumpButtonType.Reply, 0);
					this.AddLabel(115, 352, 0, @"Braided Beard");
					facialhair7 = braidedbeard;
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
					this.AddButton(97, 353, 1209, 1210, 107, GumpButtonType.Reply, 0);
					this.AddLabel(115, 352, 0, @"Braided Beard");
					facialhair7 = braidedbeard;
					this.AddButton(97, 215, 1209, 1210, 101, GumpButtonType.Reply, 0);
					this.AddLabel(115, 214, 0, @"Nice Goatee");
					facialhair1 = nicegoatee;
					
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
					this.AddButton(97, 353, 1209, 1210, 107, GumpButtonType.Reply, 0);
					this.AddLabel(115, 352, 0, @"Braided Beard");
					facialhair7 = braidedbeard;
					this.AddButton(97, 215, 1209, 1210, 101, GumpButtonType.Reply, 0);
					this.AddLabel(115, 214, 0, @"Nice Goatee");
					facialhair1 = nicegoatee;					
					break;
				}
			}
		}
		
		public static void SetHairID( PlayerMobile m, int id )
		{
			int hue = 0;
			hue = m.HairHue;
			m.HairItemID = 0;
			m.HairItemID = id;
			m.HairHue = hue;
		}
		
		public static void SetFacialHairID( PlayerMobile m, int id )
		{
			int hue = 0;
			hue = m.FacialHairHue;
			m.FacialHairItemID = 0;
			m.FacialHairItemID = id;
			m.FacialHairHue = hue;
		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			PlayerMobile m = sender.Mobile as PlayerMobile;
			int oldhair = m.HairItemID;
				
			RacialInfo( m.Nation );
			
			if ( m == null )
				return;

			switch ( info.ButtonID )
			{
				case 0: 
				{
		   			break;
				}
					
				case 1: 
				{
					m.SendGump( new CharCustomGump( m, 2 ) );
		   			break;
				}
					
				case 2: 
				{
					m.SendGump( new CharCustomGump( m, 3 ) );	
		   			break;
				}
					
				case 3: 
				{
					m.SendGump( new CharCustomGump( m, 4 ) );
		   			break;
				}
					
				case 4: 
				{
					m.SendGump( new CharCustomGump( m, 5 ) );
		   			break;
				}
					
				case 5: 
				{
					m.SendGump( new CharCustomGump( m, 6 ) );
		   			break;
				}
					
				case 6: 
				{
					m.SendGump( new CharCustomGump( m, 7 ) );
		   			break;
				}
					
#region Hair Styles

				case 7: // Short Hair
				{
					SetHairID( m, hair0.Value );
					break;
				}
					
				case 8: // Long Hair
				{
					SetHairID( m, hair1.Value );
					break;
				}
					
				case 9: // Pony Tail
				{
					SetHairID( m, hair2.Value );
					break;
				}
				
				case 10: // Buns
				{
					SetHairID( m, hair3.Value );
					break;
				}
				
				case 11: // Pig Tails
				{
					SetHairID( m, hair4.Value );
					break;
				}
					
				case 12: // Topknot
				{
					SetHairID( m, hair5.Value );
					break;
				}
					
				case 13: // Mid-Long
				{
					SetHairID( m, hair6.Value );
					break;
				}
					
				case 14: // Mohawk
				{
					SetHairID( m, hair7.Value );
					break;
				}
					
				case 15: // Afro
				{
					SetHairID( m, hair8.Value );
					break;
				}
					
				case 16: // Receeding
				{
					SetHairID( m, hair9.Value );
					break;
				}
					
				case 17: // Pageboy
				{
					SetHairID( m, hair10.Value );
					break;
				}
					
				case 18: // Spiked
				{
					SetHairID( m, hair11.Value );
					break;
				}
					
				case 19: // Mullet
				{
					SetHairID( m, hair12.Value );
					break;
				}
					
				case 20: // Curly
				{
					SetHairID( m, hair13.Value );
					break;
				}
					
				case 21: // Flower
				{
					SetHairID( m, hair14.Value );
					break;
				}
					
				case 22: // Big Knob
				{
					SetHairID( m, hair15.Value );
					break;
				}
					
				case 23: // Big Bun
				{
					SetHairID( m, hair16.Value );
					break;
				}
					
				case 24: // Big Braid
				{
					SetHairID( m, hair17.Value );
					break;
				}
					
				case 25: // Fancy Short
				{
					SetHairID( m, hair18.Value );
					break;
				}
					
				case 26: // Feather
				{
					SetHairID( m, hair19.Value );
					break;
				}
					
				case 27: // Bald
				{
					SetHairID( m, hair20.Value );
					break;
				}
				
#endregion	

#region Facial Hair Styles
				
				case 101: 
				{
					SetFacialHairID( m, facialhair1 );
					break;
				}
					
				case 102: 
				{
					SetFacialHairID( m, facialhair2 );
					break;
				}
					
				case 103: 
				{
					SetFacialHairID( m, facialhair3 );
					break;
				}
					
				case 104: 
				{
					SetFacialHairID( m, facialhair4 );
					break;
				}
					
				case 105: 
				{
					SetFacialHairID( m, facialhair5 );
					break;
				}
					
				case 106: 
				{
					SetFacialHairID( m, facialhair6 );
					break;
				}
					
				case 107: 
				{
					SetFacialHairID( m, facialhair7 );
					break;
				}
				
				case 108:
				{
					SetFacialHairID( m, facialhair8 );
					break;
				}
				
				case 109: 
				{
					SetFacialHairID( m, facialhair9 );
					break;
				}
				
				case 110:
				{
					SetFacialHairID( m, facialhair10 );
					break;
				}
				
				case 111: 
				{
					SetFacialHairID( m, facialhair11 );
					break;
				}
				
				case 112:
				{
					SetFacialHairID( m, facialhair12 );
					break;
				}
				
				case 113: 
				{
					SetFacialHairID( m, facialhair13 );
					break;
				}
				
				case 114:
				{
					SetFacialHairID( m, facialhair14 );
					break;
				}
					
#endregion

#region Hair Hues
					
				case 36:
				{
					m.HairHue = hairhue1;
					break;
				}
					
				case 37:
				{
					m.HairHue = hairhue2;
					break;
				}
					
				case 38:
				{
					m.HairHue = hairhue3;
					break;
				}
					
				case 39:
				{
					m.HairHue = hairhue4;
					break;
				}
					
				case 40:
				{
					m.HairHue = hairhue5;
					break;
				}
					
				case 41:
				{
					m.HairHue = hairhue6;
					break;
				}
					
				case 42:
				{
					m.HairHue = hairhue7;
					break;
				}
					
				case 43:
				{
					m.HairHue = hairhue8;
					break;
				}
					
				case 44:
				{
					m.HairHue = hairhue9;
					break;
				}
					
				case 45:
				{
					m.HairHue = hairhue10;
					break;
				}
					
#endregion

#region Facial Hair Hues
					
				case 46:
				{
					m.FacialHairHue = hairhue1;
					break;
				}
					
				case 47:
				{
					m.FacialHairHue = hairhue2;
					break;
				}
					
				case 48:
				{
					m.FacialHairHue = hairhue3;
					break;
				}
					
				case 49:
				{
					m.FacialHairHue = hairhue4;
					break;
				}
					
				case 50:
				{
					m.FacialHairHue = hairhue5;
					break;
				}
					
				case 51:
				{
					m.FacialHairHue = hairhue6;
					break;
				}
					
				case 52:
				{
					m.FacialHairHue = hairhue7;
					break;
				}
					
				case 53:
				{
					m.FacialHairHue = hairhue8;
					break;
				}
					
				case 54:
				{
					m.FacialHairHue = hairhue9;
					break;
				}
					
				case 55:
				{
					m.FacialHairHue = hairhue10;
					break;
				}

#endregion

#region Skin Hues
					
				case 56:
				{
					m.Hue = skinhue1;
					m.HairItemID = 8261;
					m.HairHue += 500;
					m.HairItemID = oldhair;
					m.HairHue -= 500;
					break;
				}
					
				case 57:
				{
					m.Hue = skinhue2;
					m.HairItemID = 8261;
					m.HairHue += 500;
					m.HairItemID = oldhair;
					m.HairHue -= 500;
					break;
				}
					
				case 58:
				{
					m.Hue = skinhue3;
					m.HairItemID = 8261;
					m.HairHue += 500;
					m.HairItemID = oldhair;
					m.HairHue -= 500;
					break;
				}
					
				case 59:
				{
					m.Hue = skinhue4;
					m.HairItemID = 8261;
					m.HairHue += 500;
					m.HairItemID = oldhair;
					m.HairHue -= 500;
					break;
				}
					
				case 60:
				{
					m.Hue = skinhue5;
					m.HairItemID = 8261;
					m.HairHue += 500;
					m.HairItemID = oldhair;
					m.HairHue -= 500;
					break;
				}
					
				case 61:
				{
					m.Hue = skinhue6;
					m.HairItemID = 8261;
					m.HairHue += 500;
					m.HairItemID = oldhair;
					m.HairHue -= 500;
					break;
				}
					
				case 62:
				{
					m.Hue = skinhue7;
					m.HairItemID = 8261;
					m.HairHue += 500;
					m.HairItemID = oldhair;
					m.HairHue -= 500;
					break;
				}
					
				case 63:
				{
					m.Hue = skinhue8;
					m.HairItemID = 8261;
					m.HairHue += 500;
					m.HairItemID = oldhair;
					m.HairHue -= 500;
					break;
				}
					
				case 64:
				{
					m.Hue = skinhue9;
					m.HairItemID = 8261;
					m.HairHue += 500;
					m.HairItemID = oldhair;
					m.HairHue -= 500;
					break;
				}
					
				case 65:
				{
					m.Hue = skinhue10;
					m.HairItemID = 8261;
					m.HairHue += 500;
					m.HairItemID = oldhair;
					m.HairHue -= 500;
					break;
				}
				
				case 501:
				{
					if( m.Age < 45 )
						m.Age++;
					
					m.SendGump( new CharCustomGump( m, 1 ) );
					
					return;							
				}
				
				case 502:
				{
					if( m.Age > 18 )
						m.Age--;
					
					m.SendGump( new CharCustomGump( m, 1 ) );
					
					return;							
				}
#endregion

			}
			
			if( info.ButtonID > 6 )
			{
				m.Hue++;
				m.Hue--;
			}
			
			if( m.HairItemID > 0 && m.HairHue < 1 )
				m.HairHue = BaseKhaerosMobile.AssignRacialHairHue( m.Nation );
			
			if( m.FacialHairItemID > 0 && m.FacialHairHue < 1 )
				m.FacialHairHue = BaseKhaerosMobile.AssignRacialHairHue( m.Nation );
			
			if( info.ButtonID > 99 )
			{
				if( isbeardone )
					m.SendGump( new CharCustomGump( m, 3 ) );
				
				else
					m.SendGump( new CharCustomGump( m, 4 ) );
			}
			
			if ( info.ButtonID > 6 && info.ButtonID < 28 )
			{
				m.SendGump( new CharCustomGump( m, 2 ) );
			}
			
			if ( info.ButtonID > 27 && info.ButtonID < 36 )
			{
				m.SendGump( new CharCustomGump( m, 3 ) );
			}
			
			if ( info.ButtonID > 35 && info.ButtonID < 46 )
			{
				m.SendGump( new CharCustomGump( m, 5 ) );
			}
			
			if ( info.ButtonID > 45 && info.ButtonID < 56 )
			{
				m.SendGump( new CharCustomGump( m, 6 ) );
			}
			
			if ( info.ButtonID > 55 && info.ButtonID < 99 )
			{
				m.SendGump( new CharCustomGump( m, 7 ) );
			}
		}
	}
}
