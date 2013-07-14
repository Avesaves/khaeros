using System; 
using Server.Items; 
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
	public class DeityStone : Item 
	{ 
	[Constructable] 
		public DeityStone() : base( 0xED4 ) 
		{ 
			Movable = false; 
        	Hue = 1310; 
        	Name = "Main Ambition"; 
		} 

		public override void OnDoubleClick( Mobile m ) 
		{
			PlayerMobile from = m as PlayerMobile;
			from.SendGump( new ChosenDeityGump(from) );
        } 

      	public DeityStone( Serial serial ) : base( serial ) 
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
