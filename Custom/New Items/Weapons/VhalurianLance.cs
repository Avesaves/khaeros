using System;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	[FlipableAttribute( 0x26C0, 0x26CA )]
	public class Lance : BasePoleArm
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.Dismount; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ConcussionBlow; } }
		public override string NameType { get { return "lance"; } }

		public override int AosStrengthReq{ get{ return 75; } }
		public override double OverheadPercentage{ get{ return 0.1; } }
		public override double SwingPercentage{ get{ return 0.1; } }
		public override double ThrustPercentage{ get{ return 0.8; } }
		public override double RangedPercentage{ get{ return 0; } }
		public override int AosMinDamage{ get{ return 20; } }
		public override int AosMaxDamage{ get{ return 20; } }
		public override double AosSpeed{ get{ return 5; } }
		
		public override bool CanThrustOnMount{ get{ return false; } }
		public override bool CanUseDefensiveFormation{ get{ return false; } }
		public override bool CannotUseOnFoot{ get{ return true; } }
		public override bool ChargeOnly{ get{ return true; } }

		public override int OldStrengthReq{ get{ return 95; } }
		public override int OldMinDamage{ get{ return 17; } }
		public override int OldMaxDamage{ get{ return 18; } }
		public override int OldSpeed{ get{ return 24; } }

		public override int DefHitSound{ get{ return 0x23C; } }
		public override int DefMissSound{ get{ return 0x238; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 110; } }

		public override SkillName DefSkill{ get{ return SkillName.Polearms; } }
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public Lance() : base( 0x26C0 )
		{
			Weight = 10.0;
			AosElementDamages.Piercing = 70;
			AosElementDamages.Blunt = 30;
			MaxRange = 2;
			Name = "lance";
		}

		public Lance( Serial serial ) : base( serial )
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
