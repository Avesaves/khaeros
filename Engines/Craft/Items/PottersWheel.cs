using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	public class PottersWheel : BaseTool
	{
        public override CraftSystem CraftSystem { get { return DefPottery.CraftSystem; } }

		[Constructable]
		public PottersWheel() : base( 0x2DDB )
		{
			Weight = 50.0;
            Name = "Potter's Wheel";
		}

		public PottersWheel( Serial serial ) : base( serial )
		{
		}
		
		public override void OnDoubleClick( Mobile from )
		{
			CraftSystem system = this.CraftSystem;

			int num = system.CanCraft( from, this, null );

			if ( num > 0 )
			{
				from.SendLocalizedMessage( num );
			}
			else
			{
				CraftContext context = system.GetContext( from );

				from.SendGump( new CraftGump( from, system, this, null ) );
			}
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
