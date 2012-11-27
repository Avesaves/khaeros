using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x2B06, 0x2B07 )]
	public class TyreanHalfPlateLegs : BaseArmor
	{
        public override ArmourWeight ArmourType { get { return ArmourWeight.Heavy; } }

        public override int BaseBluntResistance{ get{ return 10; } }
		public override int BasePiercingResistance{ get{ return 7; } }
		public override int BaseSlashingResistance{ get{ return 5; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 3; } }
		public override int BaseColdResistance{ get{ return 2; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 50; } }
		public override int InitMaxHits{ get{ return 65; } }

		public override int AosStrReq{ get{ return 90; } }

		public override int OldStrReq{ get{ return 60; } }
		public override int OldDexBonus{ get{ return -6; } }

		public override int ArmorBase{ get{ return 40; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

		[Constructable]
		public TyreanHalfPlateLegs() : base( 0x2B06 )
		{
			Weight = 7.0;
			Name = "Tyrean Half-Plate Legs";
		}

		public TyreanHalfPlateLegs( Serial serial ) : base( serial )
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
		}
	}
}
