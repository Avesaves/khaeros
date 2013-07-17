using System;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x13bf, 0x13c4 )]
	public class ChainChest : BaseArmor
	{
        public override ArmourWeight ArmourType { get { return ArmourWeight.Medium; } }

        public override int BaseBluntResistance{ get{ return 12; } }
		public override int BaseSlashingResistance{ get{ return 20; } }
		public override int BasePiercingResistance{ get{ return 16; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 4; } }
		public override int BaseColdResistance{ get{ return 4; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 45; } }
		public override int InitMaxHits{ get{ return 60; } }

		public override int AosStrReq{ get{ return 60; } }
		public override int OldStrReq{ get{ return 20; } }

		public override int OldDexBonus{ get{ return -5; } }

		public override int ArmorBase{ get{ return 28; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Chainmail; } }

		[Constructable]
		public ChainChest() : base( 0x13BF )
		{
			Weight = 7.0;
		}

		public ChainChest( Serial serial ) : base( serial )
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
