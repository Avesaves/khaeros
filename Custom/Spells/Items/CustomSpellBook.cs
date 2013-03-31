using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items; 
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
    public class CustomSpellBook : Item, IEasyCraft
	{
		private List<CustomMageSpell> m_Spells = new List<CustomMageSpell>();
		
		[CommandProperty( AccessLevel.GameMaster )]
		public List<CustomMageSpell> Spells
		{
			get
			{
				if( m_Spells == null )
					return new List<CustomMageSpell>();
				
				return m_Spells;
			}
			
			set{ m_Spells = value; }
		}
		
		[Constructable]
		public CustomSpellBook() : base( 3834 ) 
		{
        	Hue = 1899; 
        	Name = "A spell book";
        	Layer = Layer.FirstValid;
		}
		
		public CustomMageSpell GetSpellByName( string name )
		{
			foreach( CustomMageSpell spell in Spells )
			{
				if( spell.CustomName.Trim().ToLower() == name.Trim().ToLower() )
					return spell;
			}
			
			return null;
		}
		
		public bool ContainsSpell( string name )
		{
			foreach( CustomMageSpell spell in Spells )
			{
				if( spell.CustomName.Trim().ToLower() == name.Trim().ToLower() )
					return true;
			}
			
			return false;
		}

		public override void OnDoubleClick( Mobile m ) 
		{
			if( !this.IsChildOf( m.Backpack ) )
			   return;
			
			((PlayerMobile)m).SpellBook = this;
			
			if( Spells.Count < 1 )
				m.SendMessage( "This spell book is empty." );
			
			else
				m.SendGump( new CustomSpellBookGump( (PlayerMobile)m, this, 0 ) );
        }
		
  		public CustomSpellBook( Serial serial ) : base( serial ) 
  		{ 
 		} 

 		public override void Serialize( GenericWriter writer ) 
  		{
 			base.Serialize( writer );
     		writer.Write( (int) 0 ); // version
     		writer.Write( (int) Spells.Count );
     		
     		foreach( CustomMageSpell spell in Spells )
     			CustomSpellScroll.SerializeSpell( writer, spell );
  		} 

  		public override void Deserialize( GenericReader reader ) 
  		{ 			
  			base.Deserialize( reader );
     		int version = reader.ReadInt();
     		int count = reader.ReadInt();
     		
     		for( int i = 0; i < count; i++ )
     			Spells.Add( CustomSpellScroll.DeserializeSpell(reader) );
  		} 
   	} 
} 
