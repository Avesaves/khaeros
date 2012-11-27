//================================================//
// Created by dracana				  //
// Desc: Crafted bottle of wine default.          //
//================================================//
using System; 
using Server; 

namespace Server.Items 
{ 	
	public class BottleOfWine : BaseCraftWine
	{
		public override Item EmptyItem{ get { return new EmptyWineBottle(); } }

		[Constructable]
        public BottleOfWine()
            : base( 0x99F )
		{
			this.Weight = 0.2;
			this.FillFactor = 4;
            Hue = 2950;
            Layer = Layer.FirstValid;
		}
		
		public BottleOfWine( Serial serial ) : base( serial )
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
