using System;

namespace Server.Items
{
	public class DroxTesticle : Food
	{
		[Constructable]
		public DroxTesticle() : this( 1 )
		{
		}
		
		[Constructable]
		public DroxTesticle( int amount  ) : base( 0x2809 )
		{
			Weight = 1.0;
            Name = "drox testicle";
            this.FillFactor = 1;
            this.Stackable = false;
            Hue = 1140;
		}

		public DroxTesticle( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
