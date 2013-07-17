using System;
using Server.Items;

namespace Server.Items
{
	public class RingmailGorget : BaseArmor
	{
        public override ArmourWeight ArmourType { get { return ArmourWeight.Light; } }

        public override int BaseBluntResistance{ get{ return 5; } }
		public override int BasePiercingResistance{ get{ return 3; } }
		public override int BaseSlashingResistance{ get{ return 0; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 3; } }
		public override int BaseColdResistance{ get{ return 2; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 50; } }
		public override int InitMaxHits{ get{ return 65; } }

		public override int AosStrReq{ get{ return 45; } }
		public override int OldStrReq{ get{ return 30; } }

		public override int OldDexBonus{ get{ return -1; } }

		public override int ArmorBase{ get{ return 40; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Ringmail; } }

		[Constructable]
		public RingmailGorget() : base( 0x3BDC )
		{
			Weight = 2.0;
			Name = "ringmail gorget";
		}

		public RingmailGorget( Serial serial ) : base( serial )
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
