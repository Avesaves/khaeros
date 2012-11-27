
using System; 
using Server; 

namespace Server.Items 
{ 	
	public class BottleOfMead : BaseCraftMead
	{
		public override Item EmptyItem{ get { return new EmptyMeadBottle(); } }
		
		[Constructable]
		public BottleOfMead() : base( 0x99F )
		{
			this.Weight = 0.2;
			this.FillFactor = 3;
			Layer = Layer.OneHanded;
		}
		
		public BottleOfMead( Serial serial ) : base( serial )
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
