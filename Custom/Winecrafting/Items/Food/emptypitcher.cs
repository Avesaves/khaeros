using System;

namespace Server.Items
{
	public class EmptyPitcher : Item
	{
		[Constructable]
		public EmptyPitcher() : this( 1 )
		{
		}
		
		[Constructable]
        public EmptyPitcher(int amount) : base(0xFF6)
		{
			Stackable = true;
			Weight = 1.0;
			Name = "Glass Pitcher";
			Amount = amount;
		}

		public EmptyPitcher( Serial serial ) : base( serial )
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
