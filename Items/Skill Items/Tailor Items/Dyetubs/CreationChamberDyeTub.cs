using System;
using Server;
using Server.Multis;
using Server.Targeting;

namespace Server.Items
{
	public class CreationChamberDyeTub : DyeTub
	{
		[Constructable] 
		public CreationChamberDyeTub()
		{
		}

		public CreationChamberDyeTub( Serial serial ) : base( serial )
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
