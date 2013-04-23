using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Misc;

namespace Server.Gumps
{
	public class CharInfoGump : Gump
	{
		public CharInfoGump( PlayerMobile m )
			: base( 0, 0 )
		{
			m.CloseGump( typeof( CharInfoGump ) ); 
			int cpcap = 175000 + m.ExtraCPRewards;
			
			int totalxpneeded = m.Level * 1000;
			int thislevelsxp = m.XP - ( m.NextLevel - ( m.Level * 1000 ) );
		
			int divisor = totalxpneeded / 110;
			int offset = 0;
			
			if( divisor > 0 )
				offset = thislevelsxp / divisor;
			
			if( m.Level > 49 )
				offset = 0;
			
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage( 0 );
			this.AddBackground( 117, 114, 246, 279, 9270 );
			this.AddBackground( 129, 126, 223, 255, 3000 );
			this.AddLabel( 199, 131, 52, @"Character Info" );
			this.AddLabel( 140, 155, 0, @"" + m.Name );
			//this.AddLabel( 140, 175, 0, @"" + charclass );
			this.AddLabel( 140, 195, 0, @"Level: " + m.Level );
			this.AddLabel( 140, 215, 0, @"Age: " + m.Age );
			this.AddLabel( 140, 235, 0, @"CPs Available:" );
			this.AddLabel( 140, 255, 0, @"Total CPs spent:" );
			this.AddLabel( 140, 275, 0, @"Total CPs Cap:" );
			this.AddLabel( 260, 195, 0, @"Height: " + m.Height );
			this.AddLabel( 260, 215, 0, @"Weight: " + m.Weight );
			this.AddLabel( 260, 295, 0, @"Armour Class");
			this.AddLabel( 260, 315, 0, @"Blunt: " + m.BluntResistance );
			this.AddLabel( 260, 335, 0, @"Slashing: " + m.SlashingResistance );
			this.AddLabel( 260, 355, 0, @"Piercing: " + m.PiercingResistance );
			//this.AddLabel( 140, 295, 0, @"Life Points: " + m.Lives );
			//this.AddLabel( 140, 315, 0, @"Thirst: " + m.Thirst );
			this.AddLabel( 140, 335, 0, @"XP Meter");
			this.AddLabel( 260, 155, 0, @"" + m.RPTitle );
			this.AddButton( 327, 134, 3, 4, 0, GumpButtonType.Reply, 0 );
			this.AddImage( 137, 336, 93 );
			this.AddImage( 150, 151, 96 );
			//this.AddImage( 135, 358, 10006 ); level meter at 0%
			//this.AddImage( 245, 358, 10006 ); level meter at 100%
			this.AddImage( ( 135 + offset ), 358, 10006 );
			this.AddLabel( 260, 235, 0, @"" + m.CP );
			this.AddLabel( 260, 255, 0, @"" + m.CPSpent );
			this.AddLabel( 260, 275, 0, @"" + Convert.ToString( cpcap + m.CPCapOffset ) );
		}
		
		public class CharInfoTimer : Timer
        {
            private PlayerMobile pm;

            public CharInfoTimer( PlayerMobile m )
            	: base( TimeSpan.FromMilliseconds( 100 ) )
            {
                pm = m;
            }

            protected override void OnTick()
            {
                if( pm.HasGump( typeof( CharInfoGump ) ) )
					pm.SendGump( new CharInfoGump( pm ) );
                
                pm.m_CharInfoTimer = null;
            }
        }
	}
}
