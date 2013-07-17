using System;
using Server.Items;

namespace Server.Items
{
	public class OrnatePlateChest : BaseArmor
	{
        public override ArmourWeight ArmourType { get { return ArmourWeight.Heavy; } }

        public override int BaseBluntResistance{ get{ return 13; } }
		public override int BasePiercingResistance{ get{ return 19; } }
		public override int BaseSlashingResistance{ get{ return 22; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 3; } }
		public override int BaseColdResistance{ get{ return 2; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 50; } }
		public override int InitMaxHits{ get{ return 65; } }

		public override int AosStrReq{ get{ return 95; } }
		public override int OldStrReq{ get{ return 60; } }

		public override int OldDexBonus{ get{ return -8; } }

		public override int ArmorBase{ get{ return 40; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

		[Constructable]
		public OrnatePlateChest() : base( 0x3B1A )
		{
			Weight = 10.0;
			Name = "ornate Plate Chest";
		}

		public OrnatePlateChest( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( Weight == 1.0 )
				Weight = 10.0;
		}
	}
}
