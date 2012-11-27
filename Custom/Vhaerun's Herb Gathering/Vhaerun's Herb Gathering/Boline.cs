//	Vhaerun's Herb Gathering 1.0	//

using System;
using Server.Network;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xEC5, 0xEC4 )]
	public class Boline : BaseBoline
	{
		//public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.InfectiousStrike; } }
		//public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ShadowStrike; } }

		public override int AosStrengthReq{ get{ return 25; } }
		public override int AosMinDamage{ get{ return 10; } }
		public override int AosMaxDamage{ get{ return 10; } }
		public override double AosSpeed{ get{ return 2.5; } }
		
		public override bool CannotBlock{ get{ return true; } }
		public override bool CannotUseOnMount{ get{ return true; } }
		
		public override double OverheadPercentage{ get{ return 0.2; } }
		public override double SwingPercentage{ get{ return 0.4; } }
		public override double ThrustPercentage{ get{ return 0.4; } }
		public override double RangedPercentage{ get{ return 0; } }

		public override int OldStrengthReq{ get{ return 25; } }
		public override int OldMinDamage{ get{ return 13; } }
		public override int OldMaxDamage{ get{ return 15; } }
		public override int OldSpeed{ get{ return 36; } }

		public override int DefHitSound{ get{ return 0x23B; } }
		public override int DefMissSound{ get{ return 0x23A; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 70; } }

		public override SkillName DefSkill{ get{ return SkillName.ExoticWeaponry; } }
		public override WeaponType DefType{ get{ return WeaponType.Piercing; } }
		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public Boline() : base( 0xEC5 )
		{
			Name = "boline";
			Hue = 0x36C;
			Weight = 3.0;
			AosElementDamages.Slashing = 90;
			AosElementDamages.Piercing = 10;
		}

		public Boline( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
			
			if( version < 1 )
			{
				StrRequirement = 25;
				MinDamage = 3;
				MaxDamage = 5;
				Speed = 35;
				HitSound = 0x23B;
				MissSound = 0x23A;
				Skill = SkillName.ExoticWeaponry;
				AosElementDamages.Slashing = 90;
				AosElementDamages.Piercing = 10;
				Weight = 3.0;
			}
		}
	}
}
