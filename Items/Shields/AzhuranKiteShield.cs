using System;
using Server;

namespace Server.Items
{
	public class TallFaceShield : BaseShield
	{
		public override ArmourWeight ArmourType { get { return ArmourWeight.Medium; } }
		
		public override int SheathedMaleBackID{ get{ return 15162; } }
		public override int SheathedFemaleBackID{ get{ return 15163; } }
		
		public override int BaseBluntResistance{ get{ return 4; } }
		public override int BaseSlashingResistance{ get{ return 5; } }
		public override int BasePiercingResistance{ get{ return 5; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 1; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 50; } }
		public override int InitMaxHits{ get{ return 65; } }

		public override int AosStrReq{ get{ return 90; } }

		public override int ArmorBase{ get{ return 23; } }

		[Constructable]
		public TallFaceShield() : base( 0x3D2F )
		{
			Weight = 8.0;
			Name = "tall face shield";
		}

		public TallFaceShield( Serial serial ) : base(serial)
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
