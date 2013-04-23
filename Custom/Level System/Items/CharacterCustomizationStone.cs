using System; 
using Server.Items; 
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
	public class CharacterCustomizationStone : Item 
	{ 
	[Constructable] 
		public CharacterCustomizationStone() : base( 0xED4 ) 
		{ 
			Movable = false; 
        	Hue = 1446; 
        	Name = "a Character Customization Stone"; 
		} 

		public override void OnDoubleClick( Mobile m ) 
		{
			PlayerMobile from = m as PlayerMobile;
			
			if( from.Nation != Nation.None )
				from.SendGump( new CharCustomGump(from, 1) );
			
			else
				from.SendMessage( "You must choose a culture first." );
    	} 

  		public CharacterCustomizationStone( Serial serial ) : base( serial ) 
  		{ 
 		} 

 		public override void Serialize( GenericWriter writer ) 
  		{ 
     		base.Serialize( writer ); 

     		writer.Write( (int) 0 ); // version 
  		} 

  		public override void Deserialize( GenericReader reader ) 
  		{ 
     		base.Deserialize( reader ); 

     		int version = reader.ReadInt(); 
  		} 
   	} 
} 
