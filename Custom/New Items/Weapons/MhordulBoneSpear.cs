using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class BoneSpear : BasePoleArm, IBoneArmour
	{
		public override string NameType{ get{ return "bone Spear"; } }
		
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }
		public override int SheathedMaleBackID{ get{ return 15206; } }
		public override int SheathedFemaleBackID{ get{ return 15207; } }

		public override int AosStrengthReq{ get{ return 35; } }
		public override double OverheadPercentage{ get{ return 0.1; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.5; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 14; } }
		public override int AosMaxDamage{ get{ return 14; } }
		public override double AosSpeed{ get{ return 3.5; } }

		public override int OldStrengthReq{ get{ return 30; } }
		public override int OldMinDamage{ get{ return 2; } }
		public override int OldMaxDamage{ get{ return 36; } }
		public override int OldSpeed{ get{ return 46; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 80; } }
		
		public override SkillName DefSkill{ get{ return SkillName.Polearms; } }
		
		public override int DefHitSound{ get{ return 0x23C; } }
		public override int DefMissSound{ get{ return 0x238; } }
		
		public override bool Throwable { get { return true; } }
		
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce2H; } }

		[Constructable]
		public BoneSpear() : base( 0x3DD2 )
		{
			Weight = 6.0;
			Name = "bone Spear";
			AosElementDamages.Piercing = 100;
			MaxRange = 2;
		}

		public BoneSpear( Serial serial ) : base( serial )
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
