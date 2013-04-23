using System; 
using Server.Items; 
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
	public class HeightWeightStone : Item 
	{ 
	[Constructable] 
		public HeightWeightStone() : base( 0xED4 ) 
		{ 
			Movable = false; 
        	Hue = 1237; 
        	Name = "Height & Weight"; 
		} 

		public override void OnDoubleClick( Mobile m ) 
		{
			PlayerMobile from = m as PlayerMobile;
			
			if( from.Nation != Nation.None )
				from.SendGump( new HeightWeightGump(from, true, Convert.ToDouble(from.Height), Convert.ToDouble(from.Weight)) );
			
			else
				from.SendMessage( "You must choose a culture first." );
        } 

      	public HeightWeightStone( Serial serial ) : base( serial ) 
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
