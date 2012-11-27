using System; 
using Server.Items; 
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
	public class FeatsStone : Item 
	{ 
	[Constructable] 
		public FeatsStone() : base( 0xED4 ) 
		{ 
			Movable = false; 
        	Hue = 1410; 
        	Name = "Initial Skills"; 
		} 

		public override void OnDoubleClick( Mobile m ) 
		{
			PlayerMobile from = m as PlayerMobile;
			from.SendGump( new UniversalFeatsGump(from) );
        } 

      	public FeatsStone( Serial serial ) : base( serial ) 
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
