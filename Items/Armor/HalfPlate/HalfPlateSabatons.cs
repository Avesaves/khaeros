using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x2B12, 0x2B13 )]
	public class HalfPlateSabatons : BaseArmor
	{
        public override ArmourWeight ArmourType { get { return ArmourWeight.Heavy; } }

        public override int BaseBluntResistance{ get{ return 5; } }
		public override int BasePiercingResistance{ get{ return 4; } }
		public override int BaseSlashingResistance{ get{ return 2; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 3; } }
		public override int BaseColdResistance{ get{ return 2; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 50; } }
		public override int InitMaxHits{ get{ return 65; } }

		public override int AosStrReq{ get{ return 95; } }
		public override int OldStrReq{ get{ return 45; } }

		public override int OldDexBonus{ get{ return -5; } }

		public override int ArmorBase{ get{ return 30; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

		[Constructable]
		public HalfPlateSabatons() : base( 0x2B12 )
		{
			Weight = 2.0;
			Name = "half-plate Sabatons";
		}

		public HalfPlateSabatons( Serial serial ) : base( serial )
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
				Weight = 4.0;
		}
	}
}
