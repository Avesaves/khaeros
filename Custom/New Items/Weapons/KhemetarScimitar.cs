using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x13B6, 0x13B5 )]
	public class Scimitar : BaseSword
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.DoubleStrike; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }
		public override int SheathedMaleBackID{ get{ return 15197; } }
		public override int SheathedFemaleBackID{ get{ return 15198; } }
		public override int SheathedMaleWaistID{ get{ return 15214; } }
		public override int SheathedFemaleWaistID{ get{ return 15215; } }
		
		
		public override string NameType { get { return "scimitar"; } }

		public override int AosStrengthReq{ get{ return 25; } }
		public override double OverheadPercentage{ get{ return 0.4; } }
		public override double SwingPercentage{ get{ return 0.5; } }
		public override double ThrustPercentage{ get{ return 0.1; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 14; } }
		public override int AosMaxDamage{ get{ return 14; } }
		public override double AosSpeed{ get{ return 3.5; } }

		public override int OldStrengthReq{ get{ return 10; } }
		public override int OldMinDamage{ get{ return 4; } }
		public override int OldMaxDamage{ get{ return 30; } }
		public override int OldSpeed{ get{ return 43; } }

		public override int DefHitSound{ get{ return 0x23B; } }
		public override int DefMissSound{ get{ return 0x23A; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 90; } }

		[Constructable]
		public Scimitar() : base( 0x13B6 )
		{
			Weight = 5.0;
			AosElementDamages.Slashing = 100;
			Name = "scimitar";
		}

		public Scimitar( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
