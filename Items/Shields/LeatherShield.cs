using System;
using Server;

namespace Server.Items
{
	public class LeatherShield : BaseShield
	{
		public override int SheathedMaleBackID{ get{ return 15158; } }
		public override int SheathedFemaleBackID{ get{ return 15159; } }
		
		public override int BaseBluntResistance{ get{ return 5; } }
		public override int BaseSlashingResistance{ get{ return 3; } }
		public override int BasePiercingResistance{ get{ return 4; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 1; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 40; } }
		public override int InitMaxHits{ get{ return 50; } }

		public override int AosStrReq{ get{ return 20; } }

		public override int ArmorBase{ get{ return 7; } }

		[Constructable]
		public LeatherShield() : base( 0x3D45 )
		{
			Weight = 5.0;
			Name = "Leather Shield";
		}

		public LeatherShield( Serial serial ) : base(serial)
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
