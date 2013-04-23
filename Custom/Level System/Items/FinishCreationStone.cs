using System; 
using Server.Items; 
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
	public class FinishCreationStone : Item 
	{ 
	[Constructable] 
		public FinishCreationStone() : base( 0xED4 ) 
		{ 
			Movable = false; 
        	Hue = 1510; 
        	Name = "Finish Character Creation"; 
		} 

		public override void OnDoubleClick( Mobile m ) 
		{
			//bool finished = true;
			PlayerMobile from = m as PlayerMobile;
			
			if( from.Nation == Nation.None )
			{
				from.SendMessage( 60, "You must choose a culture first." );
				return;
			}
			
			from.SendGump( new BackgroundsGump(from, BackgroundList.None, 0, true) );
    	} 

  		public FinishCreationStone( Serial serial ) : base( serial ) 
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
