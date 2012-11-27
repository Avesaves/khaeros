using System;
using Server;
using Server.Engines.Harvest;

namespace Server.Items
{
	public class ClayHarvestingShovel : BaseHarvestTool
	{
		public override HarvestSystem HarvestSystem{ get{ return ClayHarvesting.System; } }

		[Constructable]
		public ClayHarvestingShovel() : this( 100 )
		{
		}

		[Constructable]
		public ClayHarvestingShovel( int uses ) : base( uses, 0xF39 )
		{
			Weight = 5.0;
            Name = "Clay Harvesting Shovel";
		}

        public ClayHarvestingShovel(Serial serial)
            : base( serial )
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
