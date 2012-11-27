using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Misc;
using Server.Targeting;

namespace Server.Misc
{		
	public class CustomSpellScrollCopyTarget : Target
    {
		private CustomMageSpell spell;
		
        public CustomSpellScrollCopyTarget( PlayerMobile m, CustomMageSpell Spell )
            : base( 8, false, TargetFlags.None )
        {
        	spell = Spell;
        	
            m.SendMessage( "Choose the scroll you wish to copy this spell to." );
        }

        protected override void OnTarget( Mobile m, object obj )
        {
        	if( m == null || m.Deleted || spell == null)
        		return;
        	
			PlayerMobile pm = m as PlayerMobile;
        	CustomSpellScroll scroll = obj as CustomSpellScroll;
        	
        	if( obj != null && obj is CustomSpellScroll && scroll.IsChildOf(m.Backpack) )
            {
        		if ( pm.IsApprentice )
        		{
        			m.SendMessage( "Despite your magical training, you can't seem to figure out how to do that properly." );
        			return;
        		}
				
				else if( scroll.Spell.CustomName != null )
        		{
        			m.SendMessage( "That scroll has already been used." );
        			return;
        		}

                else if( spell.CustomScripted )
                {
                    scroll.Delete();
                    Item newScroll = (Item)Activator.CreateInstance( spell.ScrollType );
                    m.Backpack.DropItem( newScroll );
                    m.SendMessage( "You copy the spell onto the scroll." );
                    return;
                }

                else
                {
                    m.SendMessage( "You copy the spell onto the scroll." );
                    scroll.Spell = CustomSpellScroll.DupeCustomMageSpell( spell );
                    scroll.InvalidateProperties();
                    return;
                }
            }
            
            m.SendMessage( "Invalid target." );
    	}
    }
}
