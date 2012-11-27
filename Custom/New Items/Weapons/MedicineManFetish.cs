using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class MedicineManFetish : BaseStaff, IBoneArmour
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.Block; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ForceOfNature; } }

		public override string NameType { get { return "Medicine Man's Fetish"; } }
		
		public override int AosStrengthReq{ get{ return 20; } }
		public override double OverheadPercentage{ get{ return 0.5; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 13; } }
		public override int AosMaxDamage{ get{ return 13; } }
		public override double AosSpeed{ get{ return 3.25; } }

		public override int OldStrengthReq{ get{ return 15; } }
		public override int OldMinDamage{ get{ return 10; } }
		public override int OldMaxDamage{ get{ return 12; } }
		public override int OldSpeed{ get{ return 48; } }

		public override int InitMinHits{ get{ return 30; } }
		public override int InitMaxHits{ get{ return 60; } }

		[Constructable]
		public MedicineManFetish() : base( 0x3DC4 )
		{
			Weight = 4.0;
			AosElementDamages.Blunt = 100;
			Name = "Medicine Man's Fetish";
		}

		public MedicineManFetish( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}
