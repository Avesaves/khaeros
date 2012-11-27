using System;
using Server.Network;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xF52, 0xF51 )]
	public class Dagger : BaseKnife
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.InfectiousStrike; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ShadowStrike; } }
		public override string NameType { get { return "Dagger"; } }
		
		public override int SheathedMaleWaistID{ get{ return 15212; } }
		public override int SheathedFemaleWaistID{ get{ return 15213; } }
		
		public override bool Critical{ get{ return true; } }
		public override bool CannotBlock{ get{ return true; } }
		public override bool CannotUseOnMount{ get{ return true; } }

		public override int AosStrengthReq{ get{ return 10; } }
		public override double OverheadPercentage{ get{ return 0.2; } }
		public override double SwingPercentage{ get{ return 0.3; } }
		public override double ThrustPercentage{ get{ return 0.5; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 10; } }
		public override int AosMaxDamage{ get{ return 10; } }
		public override double AosSpeed{ get{ return 2.5; } }

		public override int OldStrengthReq{ get{ return 1; } }
		public override int OldMinDamage{ get{ return 3; } }
		public override int OldMaxDamage{ get{ return 15; } }
		public override int OldSpeed{ get{ return 55; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 40; } }

		public override SkillName DefSkill{ get{ return SkillName.Fencing; } }
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public Dagger() : base( 0xF52 )
		{
			Weight = 1.5;
            AosElementDamages.Piercing = 100;
		}

		public Dagger( Serial serial ) : base( serial )
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
