using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Misc;

namespace Server.Gumps
{
	public class StatPointsGump : Gump
	{
		public StatPointsGump( PlayerMobile m ) : base( 0, 0 )
		{
			m.CloseGump( typeof(RaceGump) );
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
			this.AddButton(395, 85, 5600, 5604, 4, GumpButtonType.Reply, 0);
			this.AddButton(395, 120, 5600, 5604, 5, GumpButtonType.Reply, 0);
			this.AddButton(395, 155, 5600, 5604, 6, GumpButtonType.Reply, 0);
			this.AddHtml( 99, 219, 307, 147, @"You may now spend your Stat Bonus Points as you wish. Raising a stat above the cap costs 10 stat points.", (bool)true, (bool)true);
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
			
			int strbonus = 0;
			
			if( m.GetBackgroundLevel(BackgroundList.Strong) > 0 )
				strbonus += 5;
			
			if( m.GetBackgroundLevel(BackgroundList.Weak) > 0 )
				strbonus -= 5;
			
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
			int dexcap = 100 + ( m.GetBackgroundLevel(BackgroundList.Quick) * 5 ) - ( m.GetBackgroundLevel(BackgroundList.Clumsy) * 5 ) - m.TotalPenalty;
			int intcap = 100 + ( m.GetBackgroundLevel(BackgroundList.Smart) * 5 ) - ( m.GetBackgroundLevel(BackgroundList.Feebleminded) * 5 );
			int hitscap = 100 + ( m.GetBackgroundLevel(BackgroundList.Tough) * 5 ) - ( m.GetBackgroundLevel(BackgroundList.Frail) * 5 );
			int stamcap = 100 + ( m.GetBackgroundLevel(BackgroundList.Fit) * 5 ) - ( m.GetBackgroundLevel(BackgroundList.Unenergetic) * 5 ) - m.TotalPenalty;
			int manacap = 100 + ( m.GetBackgroundLevel(BackgroundList.IronWilled) * 5 ) - ( m.GetBackgroundLevel(BackgroundList.WeakWilled) * 5 );
			manacap += manaFromFeats;
			
			switch( m.Nation )
			{
				case Nation.Southern: strcap += 50; dexcap += 50; intcap += 50; hitscap += 50; stamcap += 50; manacap += 50; break;
				case Nation.Western: strcap += 50; dexcap += 50; intcap += 50; hitscap += 50; stamcap += 50; manacap += 50; break;
				case Nation.Haluaroc: strcap += 50; dexcap += 50; intcap += 50; hitscap += 50; stamcap += 50; manacap += 50; break;
				case Nation.Mhordul: strcap += 50; dexcap += 50; intcap += 50; hitscap += 50; stamcap += 50; manacap += 50; break;
				case Nation.Tirebladd: strcap += 50; dexcap += 50; intcap += 50; hitscap += 50; stamcap += 50; manacap += 50; break;
				case Nation.Northern: strcap += 50; dexcap += 50; intcap += 50; hitscap += 50; stamcap += 50; manacap += 50; break;
			}
			
			if (m == null)
				return;

			switch ( info.ButtonID )
			{	
				case 0:	break;
					
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
			}
			
			if( m.HasGump( typeof( CharInfoGump ) ) )
				m.SendGump( new CharInfoGump(m) );
			
			if ( info.ButtonID != 0 )
				m.SendGump( new StatPointsGump(m) );
		}
	}
}
