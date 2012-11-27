using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Misc;
using Server.Mobiles;
using Server.Items;

namespace Server.Gumps
{
    public class CustomSpellScrollGump : Gump
    {
    	private CustomSpellScroll Scroll;
    	
        public CustomSpellScrollGump( PlayerMobile m, CustomSpellScroll scroll ) : base( 0, 0 )
        {
            this.Closable=false;
			this.Disposable=false;
			this.Dragable=true;
			this.Resizable=false;

			AddPage(0);
			AddBackground(149, 86, 390, 370, 5120);
			AddBackground(165, 127, 359, 313, 3500);
			
			AddButton(460, 97, 4014, 4016, 1, GumpButtonType.Reply, 0);
			AddButton(495, 97, 4017, 4019, 0, GumpButtonType.Reply, 0);
			AddLabel(281, 98, 2010, @"Custom Spell Scroll");
			
			if( m == null || m.Deleted || scroll == null || scroll.Deleted )
				return;
			
			Scroll = scroll;
			
			AddLabel(190, 150, 2983, @"Name:");
			AddLabel(190, 175, 2983, @"Damage:");
			AddLabel(190, 200, 2983, @"Range:");
			AddLabel(190, 225, 2983, @"Chained Targets:");
			AddLabel(190, 250, 2983, @"Chained Damage:");
			AddLabel(190, 275, 2983, @"Chained Range:");
			AddLabel(190, 300, 2983, @"Status Duration:");
			AddLabel(190, 325, 2983, @"Status Type:");
			AddLabel(190, 350, 2983, @"Repetitions:");
			AddLabel(190, 375, 2983, @"Repetition Delay:");
			AddLabel(190, 400, 2983, @"Repetition Damage:");
			
			AddLabel(352, 150, 2983, @"Mana Cost: " + scroll.Spell.ManaCost.ToString());
			AddLabel(352, 200, 2983, @"Explosion Damage:");
			AddLabel(352, 225, 2983, @"Explosion Area:");
			AddLabel(352, 250, 2983, @"Explosion Sound:");
			AddLabel(352, 275, 2983, @"Explosion Hue:");
			AddLabel(352, 300, 2983, @"Explosion ID:");
			AddLabel(352, 325, 2983, @"Effect Sound:");
			AddLabel(352, 350, 2983, @"Effect Hue:");
			AddLabel(352, 375, 2983, @"Effect ID:");
			AddLabel(352, 400, 2983, @"Icon ID:");
			
			
			AddTextEntry(232, 150, 115, 20, 0, 0, @"" + scroll.Spell.CustomName);
			AddTextEntry(244, 175, 101, 20, 0, 1, @"" + scroll.Spell.Damage.ToString());
			AddTextEntry(234, 200, 111, 20, 0, 2, @"" + scroll.Spell.Range.ToString());
			AddTextEntry(297, 225, 46, 20, 0, 3, @"" + scroll.Spell.ChainedTargets.ToString());
			AddTextEntry(294, 250, 48, 20, 0, 4, @"" + scroll.Spell.ChainedDamage.ToString());
			AddTextEntry(280, 275, 73, 20, 0, 19, @"" + scroll.Spell.ChainedRange.ToString());
			AddTextEntry(298, 300, 53, 20, 0, 5, @"" + scroll.Spell.StatusDuration.ToString());
			AddTextEntry(273, 325, 80, 20, 0, 6, @"" + scroll.Spell.StatusType.ToString());
			AddTextEntry(267, 350, 76, 20, 0, 7, @"" + scroll.Spell.Reps.ToString());
			AddTextEntry(298, 375, 45, 20, 0, 8, @"" + scroll.Spell.RepDelay.ToString());
			AddTextEntry(310, 400, 33, 20, 0, 9, @"" + scroll.Spell.RepDamage.ToString());
			
			AddTextEntry(465, 200, 39, 20, 0, 10, @"" + scroll.Spell.ExplosionDamage.ToString());
			AddTextEntry(450, 225, 56, 20, 0, 11, @"" + scroll.Spell.ExplosionArea.ToString());
			AddTextEntry(456, 250, 58, 20, 0, 12, @"" + scroll.Spell.ExplosionSound.ToString());
			AddTextEntry(442, 275, 73, 20, 0, 13, @"" + scroll.Spell.ExplosionHue.ToString());
			AddTextEntry(433, 300, 83, 20, 0, 14, @"" + scroll.Spell.ExplosionID.ToString());
			AddTextEntry(442, 325, 73, 20, 0, 15, @"" + scroll.Spell.EffectSound.ToString());
			AddTextEntry(429, 350, 87, 20, 0, 16, @"" + scroll.Spell.EffectHue.ToString());
			AddTextEntry(419, 375, 98, 20, 0, 17, @"" + scroll.Spell.EffectID.ToString());
			AddTextEntry(402, 400, 107, 20, 0, 18, @"" + scroll.Spell.IconID.ToString());
        }
        
        public static int GetNumbersFromString( string text )
        {
        	if( text == null || text.Length < 1 )
        		return 0;
        	
        	int result = 0;
        	
        	if( int.TryParse( text, out result ) )
        		return result;
        	
        	char[] list = text.ToCharArray();
        	text = "";
        	
        	for( int i = 0; i < list.Length; i++ )
        	{
        		char c = (char)list[i];
        		
        		if( c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' )
        			text += c.ToString();
        	}
        	
        	if( int.TryParse( text, out result ) )
        		return result;
        	
        	return 0;
        }

        public override void OnResponse( NetState sender, RelayInfo info )
        {
            PlayerMobile from = sender.Mobile as PlayerMobile;
            
            if( from == null || from.Deleted )
            	return;
            
            if( info.ButtonID == 0 )
            	return;
            
            if( Scroll == null || Scroll.Deleted || !Scroll.IsChildOf(from.Backpack) )
            {
            	from.SendMessage( "The scroll must be in your backpack for you to edit it" );
            	from.SendGump( new CustomSpellScrollGump( from, Scroll ) );
            	return;
            }
            
            CustomMageSpell spell = new CustomMageSpell( null, 1 );
            spell.CustomName = info.GetTextEntry(0).Text;
            spell.Damage = GetNumbersFromString( info.GetTextEntry(1).Text );
            spell.Range = GetNumbersFromString( info.GetTextEntry(2).Text );
            spell.ChainedTargets = GetNumbersFromString( info.GetTextEntry(3).Text );
            spell.ChainedDamage = GetNumbersFromString( info.GetTextEntry(4).Text );
            spell.StatusDuration = GetNumbersFromString( info.GetTextEntry(5).Text );
            spell.StatusType = GetNumbersFromString( info.GetTextEntry(6).Text );
            spell.Reps = GetNumbersFromString( info.GetTextEntry(7).Text );
            spell.RepDelay = GetNumbersFromString( info.GetTextEntry(8).Text );
            spell.RepDamage = GetNumbersFromString( info.GetTextEntry(9).Text );
            spell.ExplosionDamage = GetNumbersFromString( info.GetTextEntry(10).Text );
            spell.ExplosionArea = GetNumbersFromString( info.GetTextEntry(11).Text );
            spell.ExplosionSound = GetNumbersFromString( info.GetTextEntry(12).Text );
            spell.ExplosionHue = GetNumbersFromString( info.GetTextEntry(13).Text );
            spell.ExplosionID= GetNumbersFromString( info.GetTextEntry(14).Text );
            spell.EffectSound = GetNumbersFromString( info.GetTextEntry(15).Text );
            spell.EffectHue = GetNumbersFromString( info.GetTextEntry(16).Text );
            spell.EffectID = GetNumbersFromString( info.GetTextEntry(17).Text );
            spell.IconID = GetNumbersFromString( info.GetTextEntry(18).Text );
            spell.ChainedRange = GetNumbersFromString( info.GetTextEntry(19).Text );

            if( CustomSpellScroll.TryToEditSpell(from, spell) )
            {
            	from.SendMessage( "You have successfully edited the scroll." );
            	Scroll.Spell = spell;
            	Scroll.InvalidateProperties();
            	return;
            }
            
            from.SendGump( new CustomSpellScrollGump( from, Scroll ) );
        }
    }
}
