using System;
using Server;

namespace Server.Items
{
	public class KiteShield : BaseShield, IDyable
	{
		public override ArmourWeight ArmourType { get { return ArmourWeight.Heavy; } }
		
		public override int SheathedMaleBackID{ get{ return 15164; } }
		public override int SheathedFemaleBackID{ get{ return 15165; } }
		
		public override int BaseBluntResistance{ get{ return 8; } }
		public override int BaseSlashingResistance{ get{ return 3; } }
		public override int BasePiercingResistance{ get{ return 5; } }
		public override int BasePhysicalResistance{ get{ return 0; } }
		public override int BaseFireResistance{ get{ return 0; } }
		public override int BaseColdResistance{ get{ return 0; } }
		public override int BasePoisonResistance{ get{ return 0; } }
		public override int BaseEnergyResistance{ get{ return 0; } }

		public override int InitMinHits{ get{ return 45; } }
		public override int InitMaxHits{ get{ return 60; } }

		public override int AosStrReq{ get{ return 45; } }

		public override int ArmorBase{ get{ return 16; } }

		[Constructable]
		public KiteShield() : base( 0x1B74 )
		{
			Weight = 7.0;
			Name = "kite shield";
		}

		public KiteShield( Serial serial ) : base(serial)
		{
		}

		public bool Dye( Mobile from, DyeTub sender )
		{
			if ( Deleted )
				return false;

			Hue = sender.DyedHue;

			return true;
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Weight == 5.0 )
				Weight = 7.0;
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 );//version
		}
	}
}
