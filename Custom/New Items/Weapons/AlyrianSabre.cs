using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x2D2F, 0x2D23 )]
	public class Falcata : BaseKnife
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.Disarm; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Bladeweave; } }
		public override int SheathedMaleWaistID{ get{ return 15214; } }
		public override int SheathedFemaleWaistID{ get{ return 15215; } }
		public override int SheathedMaleBackID{ get{ return 15197; } }
		public override int SheathedFemaleBackID{ get{ return 15198; } }

		public override string NameType { get { return "falcata"; } }
		
		public override int AosStrengthReq{ get{ return 35; } }
		public override double OverheadPercentage{ get{ return 0.4; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 15; } }
		public override int AosMaxDamage{ get{ return 15; } }
		public override double AosSpeed{ get{ return 3.75; } }

		public override int OldStrengthReq{ get{ return 15; } }
		public override int OldMinDamage{ get{ return 9; } }
		public override int OldMaxDamage{ get{ return 11; } }
		public override int OldSpeed{ get{ return 48; } }

		public override int DefHitSound{ get{ return 0x23B; } }
		public override int DefMissSound{ get{ return 0x239; } }

		public override int InitMinHits{ get{ return 30; } }
		public override int InitMaxHits{ get{ return 60; } }

		[Constructable]
		public Falcata() : base( 0x2D2F )
		{
			Weight = 6.0;
			Name = "falcata";
			AosElementDamages.Slashing = 100;
		}

		public Falcata( Serial serial ) : base( serial )
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
