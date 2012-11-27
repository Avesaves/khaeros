using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xEC4, 0xEC5 )]
	public class SkinningKnife : BaseKnife
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ShadowStrike; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Disarm; } }
		
		public override int SheathedMaleWaistID{ get{ return 15212; } }
		public override int SheathedFemaleWaistID{ get{ return 15213; } }

		public override string NameType { get { return "Skinning Knife"; } }
		
		public override bool Critical{ get{ return true; } }
		public override bool CannotBlock{ get{ return true; } }
		public override bool CannotUseOnMount{ get{ return true; } }
		
		public override int AosStrengthReq{ get{ return 5; } }
		public override double OverheadPercentage{ get{ return 0.2; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.4; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 11; } }
		public override int AosMaxDamage{ get{ return 11; } }
		public override double AosSpeed{ get{ return 2.75; } }

		public override int OldStrengthReq{ get{ return 5; } }
		public override int OldMinDamage{ get{ return 1; } }
		public override int OldMaxDamage{ get{ return 10; } }
		public override int OldSpeed{ get{ return 40; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 40; } }
		
		public override SkillName DefSkill{ get{ return SkillName.Fencing; } }
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public SkinningKnife() : base( 0xEC4 )
		{
			Weight = 1.5;
			Layer = Layer.OneHanded;
			AosElementDamages.Piercing = 20;
            AosElementDamages.Slashing = 80;
		}

		public SkinningKnife( Serial serial ) : base( serial )
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
