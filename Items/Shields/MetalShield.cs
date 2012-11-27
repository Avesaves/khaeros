using System;
using Server;

namespace Server.Items
{
	public class MetalShield : BaseShield
	{
		public override int SheathedMaleBackID{ get{ return 15160; } }
		public override int SheathedFemaleBackID{ get{ return 15161; } }
		
		public override ArmourWeight ArmourType { get { return ArmourWeight.Medium; } }
		
		public override int BaseBluntResistance{ get{ return 4; } }
		public override int BaseSlashingResistance{ get{ return 4; } }
		public override int BasePiercingResistance{ get{ return 5; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 1; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 50; } }
		public override int InitMaxHits{ get{ return 65; } }

		public override int AosStrReq{ get{ return 45; } }

		public override int ArmorBase{ get{ return 11; } }

		[Constructable]
		public MetalShield() : base( 0x1B7B )
		{
			Weight = 6.0;
		}

		public MetalShield( Serial serial ) : base(serial)
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );//version
		}
	}
}
