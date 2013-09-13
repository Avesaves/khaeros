using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;

namespace Server.Gumps
{
	public class InitialStatsGump : Gump
	{
		
		public InitialStatsGump( PlayerMobile m )
			: base( 0, 0 )
		{
			m.CloseGump( typeof( InitialStatsGump ) );
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
			this.AddLabel(194, 48, 2010, @"Stat Points Left: " + m.StatPoints + "");
			this.AddLabel(116, 82, 1149, @"Strength");
			this.AddLabel(116, 117, 1149, @"Dexterity");
			this.AddLabel(116, 152, 1149, @"Intelligence");
			this.AddLabel(324, 82, 1149, @"Hit Points");
			this.AddLabel(337, 117, 1149, @"Stamina");
			this.AddLabel(356, 152, 1149, @"Mana");
			
			this.AddButton(404, 46, 1150, 1152, 0, GumpButtonType.Reply, 0);
			
			this.AddButton(96, 85, 5600, 5604, 1, GumpButtonType.Reply, 0);
			this.AddButton(96, 120, 5600, 5604, 2, GumpButtonType.Reply, 0);
			this.AddButton(96, 155, 5600, 5604, 3, GumpButtonType.Reply, 0);
			this.AddButton(415, 85, 5600, 5604, 4, GumpButtonType.Reply, 0);
			this.AddButton(415, 120, 5600, 5604, 5, GumpButtonType.Reply, 0);
			this.AddButton(415, 155, 5600, 5604, 6, GumpButtonType.Reply, 0);
			this.AddButton(75, 85, 9764, 9765, 7, GumpButtonType.Reply, 0);
			this.AddButton(75, 120, 9764, 9765, 8, GumpButtonType.Reply, 0);
			this.AddButton(75, 155, 9764, 9765, 9, GumpButtonType.Reply, 0);
			this.AddButton(395, 85, 9764, 9765, 10, GumpButtonType.Reply, 0);
			this.AddButton(395, 120, 9764, 9765, 11, GumpButtonType.Reply, 0);
			this.AddButton(395, 155, 9764, 9765, 12, GumpButtonType.Reply, 0);
			this.AddHtml( 99, 219, 307, 147, @"You may now spend your initial Stat Bonus Points as you wish. While in the creation chamber, " +
			             " you can reopen this gump by using the .StatPoints command.", (bool)true, (bool)true);

		}
		public static int GetValue( int feat )
		{
			if( feat == 1 )
				return 1;
			
			if( feat == 2 )
				return 3;
			
			if( feat == 3 )
				return 6;
			
			return 0;
		}
		
		public override void OnResponse(NetState sender, RelayInfo info)
		{
			PlayerMobile m = sender.Mobile as PlayerMobile;

            if( m == null )
                return;
			
			int strbonus = 0;
            int dexbonus = 0;
            int intbonus = 0;
            int hitsbonus = 0;
            int stambonus = 0;
            int manabonus = 0;
			
			if( m.GetBackgroundLevel(BackgroundList.Strong) > 0 )
				strbonus += 5;
			
			if( m.GetBackgroundLevel(BackgroundList.Weak) > 0 )
				strbonus -= 5;

            if( m.GetBackgroundLevel(BackgroundList.Quick) > 0 )
                dexbonus += 5;

            if( m.GetBackgroundLevel(BackgroundList.Clumsy) > 0 )
                dexbonus -= 5;

            if( m.GetBackgroundLevel(BackgroundList.Smart) > 0 )
                intbonus += 5;

            if( m.GetBackgroundLevel(BackgroundList.Feebleminded) > 0 )
                intbonus -= 5;

            if( m.GetBackgroundLevel(BackgroundList.Tough) > 0 )
                hitsbonus += 5;

            if( m.GetBackgroundLevel(BackgroundList.Frail) > 0 )
                hitsbonus -= 5;

            if( m.GetBackgroundLevel(BackgroundList.Fit) > 0 )
                stambonus += 5;

            if( m.GetBackgroundLevel(BackgroundList.Unenergetic) > 0 )
                stambonus -= 5;

            if( m.GetBackgroundLevel(BackgroundList.IronWilled) > 0 )
                manabonus += 5;

            if( m.GetBackgroundLevel(BackgroundList.WeakWilled) > 0 )
                manabonus -= 5;
			
			int manaFromFeats = GetValue(m.Feats.GetFeatLevel(FeatList.LifeII)) + 
				GetValue(m.Feats.GetFeatLevel(FeatList.DeathI)) + 
				GetValue(m.Feats.GetFeatLevel(FeatList.MatterII)) + 
				GetValue(m.Feats.GetFeatLevel(FeatList.MindII)) + 
				GetValue(m.Feats.GetFeatLevel(FeatList.TimeII)) + 
				GetValue(m.Feats.GetFeatLevel(FeatList.SpaceII)) + 
				GetValue(m.Feats.GetFeatLevel(FeatList.FateII)) + 
				GetValue(m.Feats.GetFeatLevel(FeatList.ForcesII)) + 
				GetValue(m.Feats.GetFeatLevel(FeatList.PrimeII)) + 
				GetValue(m.Feats.GetFeatLevel(FeatList.SpiritII));

			int strcap = 100 + strbonus;
			int dexcap = 100 + dexbonus - m.TotalPenalty;
			int intcap = 100 + intbonus;
			int hitscap = 100 + hitsbonus;
			int stamcap = 100 + stambonus - m.TotalPenalty;
			int manacap = 100 + manabonus + manaFromFeats;
			
			switch( m.Nation )
			{
				case Nation.Southern: strcap += 50; dexcap += 50; intcap += 50; hitscap += 50; stamcap += 50; manacap += 50; break;
				case Nation.Western: strcap += 50; dexcap += 50; intcap += 50; hitscap += 50; stamcap += 50; manacap += 50; break;
				case Nation.Haluaroc: strcap += 50; dexcap += 50; intcap += 50; hitscap += 50; stamcap += 50; manacap += 50; break;
				case Nation.Mhordul: strcap += 50; dexcap += 50; intcap += 50; hitscap += 50; stamcap += 50; manacap += 50; break;
				case Nation.Tirebladd: strcap += 50; dexcap += 50; intcap += 50; hitscap += 50; stamcap += 50; manacap += 50; break;
				case Nation.Northern: strcap += 50; dexcap += 50; intcap += 50; hitscap += 50; stamcap += 50; manacap += 50; break;
			}

			switch ( info.ButtonID )
			{
					
				case 0:
				{
					break;
				}
					
				case 1: 
				{
					if ( m.StatPoints > 4 && strcap > m.RawStr )
					{
						m.RawStr += 5;
						m.StatPoints -= 5;
					}
					
					else if ( m.StatPoints > 9 )
					{
						m.RawStr += 5;
						m.StatPoints -= 10;
					}
					
					else if ( m.StatPoints > 4 )
						m.SendMessage( "It costs 10 stat points to raise a stat above the cap." );
					
					break;
				}

				case 2: 
				{
					if ( m.StatPoints > 4 && dexcap > (m.RawDex + m.TotalPenalty) )
					{
						m.RawDex += 5;
						m.StatPoints -= 5;
					}
					
					else if ( m.StatPoints > 9 )
					{
						m.RawDex += 5;
						m.StatPoints -= 10;
					}
					
					else if ( m.StatPoints > 4 )
						m.SendMessage( "It costs 10 stat points to raise a stat above the cap." );
					
					break;
				}

				case 3: 
				{
					if ( m.StatPoints > 4 && intcap > m.RawInt )
					{
						m.RawInt += 5;
						m.StatPoints -= 5;
						m.CPCapOffset += 1000;
						m.FeatSlots -= 1000;
					}
					
					else if ( m.StatPoints > 9 )
					{
						m.RawInt += 5;
						m.StatPoints -= 10;
						m.CPCapOffset += 1000;
						m.FeatSlots -= 1000;
					}
					
					else if ( m.StatPoints > 4 )
						m.SendMessage( "It costs 10 stat points to raise a stat above the cap." );
					
					break;
				}
					
				case 4: 
				{
					if ( m.StatPoints > 4 && hitscap > m.RawHits )
					{
						m.RawHits += 5;
						m.StatPoints -= 5;
					}
					
					else if ( m.StatPoints > 9 )
					{
						m.RawHits += 5;
						m.StatPoints -= 10;
					}
					
					else if ( m.StatPoints > 4 )
						m.SendMessage( "It costs 10 stat points to raise a stat above the cap." );
					
					break;
				}
					
				case 5: 
				{
					if ( m.StatPoints > 4 && stamcap > (m.RawStam + m.TotalPenalty) )
					{
						m.RawStam += 5;
						m.StatPoints -= 5;
					}
					
					else if ( m.StatPoints > 9 )
					{
						m.RawStam += 5;
						m.StatPoints -= 10;
					}
					
					else if ( m.StatPoints > 4 )
						m.SendMessage( "It costs 10 stat points to raise a stat by 5 above the cap." );
					
					break;
				}
					
				case 6: 
				{
					if ( m.StatPoints > 4 && manacap > m.RawMana )
					{
						m.RawMana += 5;
						m.StatPoints -= 5;
					}
					
					else if ( m.StatPoints > 9 )
					{
						m.RawMana += 5;
						m.StatPoints -= 10;
					}
					
					else if ( m.StatPoints > 4 )
						m.SendMessage( "It costs 10 stat points to raise a stat by 5 above the cap." );
					
					break;
				}
					
				case 7: 
				{
					if ( m.RawStr > 10 )
					{
						if( m.RawStr > strcap )
							m.StatPoints += 5;
						
						m.RawStr -= 5;
						m.StatPoints += 5;
					}
					
					break;
				}
					
				case 8: 
				{
					if ( m.RawDex > 10 )
					{
						if( m.RawDex > dexcap )
							m.StatPoints += 5;
						
						m.RawDex -= 5;
						m.StatPoints += 5;
					}
					
					break;
				}
					
				case 9: 
				{
					if ( m.RawInt > 10 )
					{
						int max = 175000 + m.ExtraCPRewards + m.CPCapOffset - 1000;
						
						if( m.CPSpent > max )
						{
							m.SendMessage( "That would set your CP Cap below the amount of CP you have already spent." );
							break;
						}
						
						if( m.RawInt > intcap )
							m.StatPoints += 5;
						
						m.RawInt -= 5;
						m.StatPoints += 5;
						m.CPCapOffset -= 1000;
						m.FeatSlots += 1000;
					}
					
					break;
				}
					
				case 10: 
				{
					if ( m.RawHits > 10 )
					{
						if( m.RawHits > hitscap )
							m.StatPoints += 5;
						
						m.Hits -= 5;
						m.RawHits -= 5;
						m.StatPoints += 5;
					}
					
					break;
				}
					
				case 11: 
				{
					if ( m.RawStam > 10 )
					{
						if( m.RawStam > stamcap )
							m.StatPoints += 5;
						
						m.Stam -= 5;
						m.RawStam -= 5;
						m.StatPoints += 5;
					}
					
					break;
				}
					
				case 12: 
				{
					if ( m.RawMana > 10 )
					{
						if( m.RawMana > manacap )
							m.StatPoints += 5;
						
						m.Mana -= 5;
						m.RawMana -= 5;
						m.StatPoints += 5;
					}
					
					break;
				}
			}
			
			if( m.HasGump( typeof( CharInfoGump ) ) )
				m.SendGump( new CharInfoGump(m) );
			
			if ( info.ButtonID != 0 )
			{
				m.SendGump( new InitialStatsGump( m ) );	
			}
		}
	}
}
