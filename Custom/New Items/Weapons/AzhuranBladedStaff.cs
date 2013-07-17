using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x26BD, 0x26C7 )]
	public class Glaive : BasePoleArm
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.Dismount; } }
		public override string NameType { get { return "glaive"; } }

		public override int AosStrengthReq{ get{ return 55; } }
		public override double OverheadPercentage{ get{ return 0.2; } }
		public override double SwingPercentage{ get{ return 0.3; } }
		public override double ThrustPercentage{ get{ return 0.5; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 17; } }
		public override int AosMaxDamage{ get{ return 17; } }
		public override double AosSpeed{ get{ return 4.25; } }

		public override int OldStrengthReq{ get{ return 40; } }
		public override int OldMinDamage{ get{ return 14; } }
		public override int OldMaxDamage{ get{ return 16; } }
		public override int OldSpeed{ get{ return 37; } }

		public override int InitMinHits{ get{ return 21; } }
		public override int InitMaxHits{ get{ return 110; } }

		public override SkillName DefSkill{ get{ return SkillName.Polearms; } }
		
		public override int DefHitSound{ get{ return 0x23C; } }
		public override int DefMissSound{ get{ return 0x238; } }
		
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce2H; } }

		[Constructable]
		public Glaive() : base( 0x26BD )
		{
			Weight = 8.0;
			MaxRange = 2;
			AosElementDamages.Slashing = 70;
			AosElementDamages.Piercing = 30;
			Name = "glaive";
		}

		public Glaive( Serial serial ) : base( serial )
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
