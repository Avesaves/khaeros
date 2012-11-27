using System;
using Server;

namespace Server.Items
{
	public class WoodenKiteShield : BaseShield
	{
		public override ArmourWeight ArmourType { get { return ArmourWeight.Heavy; } }
		
		public override int SheathedMaleBackID{ get{ return 15162; } }
		public override int SheathedFemaleBackID{ get{ return 15163; } }
		
		public override int BaseBluntResistance{ get{ return 7; } }
		public override int BaseSlashingResistance{ get{ return 3; } }
		public override int BasePiercingResistance{ get{ return 5; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 50; } }
		public override int InitMaxHits{ get{ return 65; } }

		public override int AosStrReq{ get{ return 20; } }

		public override int ArmorBase{ get{ return 12; } }

		[Constructable]
		public WoodenKiteShield() : base( 0x1B79 )
		{
			Weight = 5.0;
		}

		public WoodenKiteShield( Serial serial ) : base(serial)
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Weight == 7.0 )
				Weight = 5.0;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );//version
		}
	}
}
