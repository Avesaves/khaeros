using System;
using Server;

namespace Server.Items
{
	public class MhordulHornedBoneHelm : BaseArmor, IBoneArmour
	{
		public override int BaseBluntResistance{ get{ return 5; } }
		public override int BasePiercingResistance{ get{ return 2; } }
		public override int BaseSlashingResistance{ get{ return 3; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 3; } }
		public override int BaseColdResistance{ get{ return 4; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 25; } }
		public override int InitMaxHits{ get{ return 30; } }

		public override int AosStrReq{ get{ return 20; } }
		public override int OldStrReq{ get{ return 40; } }

		public override int ArmorBase{ get{ return 30; } }

		public override ArmorMaterialType MaterialType{ get{ return ArmorMaterialType.Plate; } }

		[Constructable]
		public MhordulHornedBoneHelm() : base( 0x3BE2 )
		{
			Weight = 3.0;
			Name = "Mhordul Horned Bone Helm";
		}

		public MhordulHornedBoneHelm( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );

			if ( Weight == 1.0 )
				Weight = 3.0;
		}
		
		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}
