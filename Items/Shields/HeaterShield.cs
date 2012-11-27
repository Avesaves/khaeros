using System;
using Server;

namespace Server.Items
{
	public class HeaterShield : BaseShield
	{
		public override ArmourWeight ArmourType { get { return ArmourWeight.Heavy; } }
		
		public override int SheathedMaleBackID{ get{ return 15166; } }
		public override int SheathedFemaleBackID{ get{ return 15167; } }
		
		public override int BaseBluntResistance{ get{ return 5; } }
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
		public HeaterShield() : base( 0x1B76 )
		{
			Weight = 8.0;
		}

		public HeaterShield( Serial serial ) : base(serial)
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
