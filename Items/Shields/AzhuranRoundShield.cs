using System;
using Server;

namespace Server.Items
{
	public class AzhuranRoundShield : BaseShield
	{
		public override int SheathedMaleBackID{ get{ return 15160; } }
		public override int SheathedFemaleBackID{ get{ return 15161; } }
		
		public override ArmourWeight ArmourType { get { return ArmourWeight.Medium; } }
		
		public override int BaseBluntResistance{ get{ return 4; } }
		public override int BaseSlashingResistance{ get{ return 5; } }
		public override int BasePiercingResistance{ get{ return 5; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 1; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 25; } }
		public override int InitMaxHits{ get{ return 30; } }

		public override int AosStrReq{ get{ return 35; } }

		public override int ArmorBase{ get{ return 10; } }

		[Constructable]
		public AzhuranRoundShield() : base( 0x3D2E )
		{
			Weight = 6.0;
			Name = "Azhuran Round Shield";
		}

		public AzhuranRoundShield( Serial serial ) : base(serial)
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
