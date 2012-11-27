using System;
using Server.Items;

namespace Server.Items
{
	public class SkullCapHelmet : BaseArmor
	{
        public override ArmourWeight ArmourType { get { return ArmourWeight.Light; } }

        public override int BaseBluntResistance{ get{ return 3; } }
		public override int BaseSlashingResistance{ get{ return 2; } }
		public override int BasePiercingResistance{ get{ return 2; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 2; } }
		public override int BaseColdResistance{ get{ return 2; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 55; } }
		public override int InitMaxHits{ get{ return 60; } }

		public override int AosStrReq{ get{ return 55; } }
		public override int OldStrReq{ get{ return 55; } }

		public override int ArmorBase{ get{ return 4; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

		[Constructable]
		public SkullCapHelmet() : base( 0x3BE8 )
		{
			Weight = 5.0;
			Name = "Skull Cap Helmet";
		}

		public SkullCapHelmet( Serial serial ) : base( serial )
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
